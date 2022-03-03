using System;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Serializable]
    private struct MinMaxValues
    {
        public float min;
        public float max;
    }

    [SerializeField] private GameObject spine = null;

    private float maxRotation = 45f;
    [SerializeField] private MinMaxValues legRotY;

    private Vector3 initialRotation = Vector3.zero;

    [Header("Gun")]
    [SerializeField] private Gun defaultGun = null;
    private Gun currentGun;
    [SerializeField] private GameObject gunPosition = null;

    [Header("Aiming Line")]
    [SerializeField] private float aimMaxLength = 15f;
    [SerializeField] private LayerMask depthAimingIgnoredColliders = 0, sideAimingIgnoredColliders = 0;
    [SerializeField] private LineRenderer aimingLine = null;
    const float spineHeight = 0.075f;

    [Header("Crosshair")]
    [SerializeField] private GameObject crosshair = null;
    [SerializeField] private MinMaxValues vpCrshrX;
    [SerializeField] private MinMaxValues vpCrshrY;

    [Header("Aiming Box")]
    [SerializeField] private float boxCastZSize = 1.75f;
    private float boxCastYSize = .1f;

    // Start is called before the first frame update
    private void Start()
    {
        currentGun = defaultGun;

        // Set Gun UI
        ((PlayerGun)currentGun).SetCurrentGun(true);

        depthAimingIgnoredColliders = ~depthAimingIgnoredColliders;
        sideAimingIgnoredColliders = ~sideAimingIgnoredColliders;
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerControl playerControl = Player.GetInstanceControl();

        // Can't aim and shoot if in a cinematic transition
        if (playerControl.IsInTransition()) return;

        bool isAiming = Input.GetButton("Aim") && playerControl.IsAlive();

        playerControl.SetIsAiming(isAiming);
    }

    private void LateUpdate()
    {
        PlayerControl playerControl = Player.GetInstanceControl();
        if (playerControl.IsAiming())
        {
            if (playerControl.IsCovering())
            {
                Player.EnablePhysics(true);
                Vector3 crosshairPos = AimCrosshair();
                RotateSpineCrosshair(crosshairPos);
                SetAimingCrossHairLinePositions(crosshairPos);
            }
            else
            {
                RotateSpine();
                SetAimingLinePositions();
                crosshair.SetActive(false);
            }
        }
        else
        {
            if (playerControl.IsCovering())
            {
                Player.EnablePhysics(false);
                ResetRotationSpineCrosshair();
            }
            else
            {
                spine.transform.localRotation = Quaternion.Euler(initialRotation);
            }
            ResetAimingLine();
            crosshair.SetActive(false);
        }
    }

    private void RotateSpine()
    {
        Vector2 vpMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 vpSpinePos = Camera.main.WorldToViewportPoint(spine.transform.position) + new Vector3(0, spineHeight, 0);

        Vector2 lookToMouseVec = (vpMousePos - vpSpinePos).normalized;

        float rot = Mathf.Rad2Deg * Mathf.Acos(Mathf.Clamp(Vector2.Dot(lookToMouseVec, Player.GetInstanceControl().GetSkeletonDirection()), -1f, 1f));

        if (vpMousePos.x < vpSpinePos.x) rot %= 180;

        Player.GetInstanceControl().RotateSkeleton(vpMousePos.x < vpSpinePos.x);

        if (rot >= maxRotation) rot = maxRotation;

        if (vpMousePos.y > vpSpinePos.y) rot *= -1;

        spine.transform.localRotation = Quaternion.Euler(new Vector3(rot, initialRotation.y, initialRotation.z));
    }

    private void SetAimingLinePositions()
    {
        Transform bulletSpawnerTransform = currentGun.GetBulletSpawnerTransform();

        Vector2 rayDirection = bulletSpawnerTransform.forward;
        Vector3 startPoint = bulletSpawnerTransform.position;

        Vector3 endWorldPoint = GetAimingLineFinalPos(new Ray(startPoint, rayDirection), false);

        DrawAimingLine(startPoint, endWorldPoint, (endWorldPoint - startPoint).normalized);
    }

    private void DrawAimingLine(Vector3 startPoint, Vector3 endPoint, Vector3 shootingDirection)
    {
        aimingLine.gameObject.SetActive(true);
        aimingLine.SetPosition(0, startPoint);
        aimingLine.SetPosition(1, endPoint);

        currentGun.SetShootingDirection(shootingDirection);
    }

    private Vector3 GetAimingLineFinalPos(Ray ray, bool raycast = false)
    {
        RaycastHit hit;

        if (GetTarget(ray, raycast, out hit)) return hit.point;

        return ray.origin + ray.direction * aimMaxLength;
    }

    private bool GetTarget(Ray ray, bool raycast, out RaycastHit hit)
    {
        if (raycast) return Physics.Raycast(ray, out hit, aimMaxLength, depthAimingIgnoredColliders, QueryTriggerInteraction.Ignore);

        ExtDebug.DrawBoxCastBox(ray.origin, new Vector3(1, boxCastYSize, boxCastZSize), Quaternion.identity, ray.direction, aimMaxLength, Color.yellow);

        return Physics.BoxCast(ray.origin, new Vector3(1, boxCastYSize, boxCastZSize), ray.direction, out hit, Quaternion.identity, aimMaxLength, sideAimingIgnoredColliders, QueryTriggerInteraction.Ignore);
    }

    private void ResetAimingLine() => aimingLine.gameObject.SetActive(false);

    public void SetNewGun(PlayerGun gun)
    {
        if (currentGun == defaultGun)
        {
            ((PlayerGun)currentGun).SetCurrentGun(false);
            currentGun.gameObject.SetActive(false);
        }
        else
        {
            Destroy(currentGun);
        }

        currentGun = gun;
        gun.SetCurrentGun(true);
        gun.transform.parent = gunPosition.transform;
        gun.SetHandPosition();
    }

    public void ResetGun()
    {
        Destroy(currentGun.gameObject);
        currentGun = defaultGun;
        currentGun.gameObject.SetActive(true);

        // Reset Player Gun UI
        ((PlayerGun)currentGun).SetCurrentGun(true);
    }

    private Vector3 AimCrosshair()
    {
        crosshair.SetActive(true);

        Vector2 vpMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        vpMousePos.x = Mathf.Clamp(vpMousePos.x, vpCrshrX.min, vpCrshrX.max);
        vpMousePos.y = Mathf.Clamp(vpMousePos.y, vpCrshrY.min, vpCrshrY.max);

        Ray ray = Camera.main.ViewportPointToRay(vpMousePos);

        Vector3 crosshairPos = GetAimingLineFinalPos(ray, true);

        crosshair.transform.position = Camera.main.WorldToScreenPoint(crosshairPos);

        return crosshairPos;
    }

    private void SetAimingCrossHairLinePositions(Vector3 crosshairPos)
    {
        Transform bulletSpawnerTransform = currentGun.GetBulletSpawnerTransform();

        DrawAimingLine(bulletSpawnerTransform.position, crosshairPos, (crosshairPos - bulletSpawnerTransform.position).normalized);
    }

    private void RotateSpineCrosshair(Vector3 crosshairPos)
    {
        float dirY = Mathf.Clamp(Quaternion.LookRotation((crosshairPos - Player.GetArmatureTransform().position)).eulerAngles.y, legRotY.min, legRotY.max);

        Player.GetInstanceControl().RotateSkeleton(dirY);

        spine.transform.LookAt(crosshairPos);
    }

    private void ResetRotationSpineCrosshair() => Player.GetInstanceControl().RotateSkeleton(new Vector3(1, 0, 0));

    private void OnDisable()
    {
        ResetAimingLine();
        currentGun.enabled = false;
    }
}