using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject spine = null;

    [SerializeField]
    private float maxRotation = 45f;
    [SerializeField]
    private float minVPCrshrX = .25f, maxVPCrshrX = .75f;
    [SerializeField]
    private float minVPCrshrY = .2f, maxVPCrshrY = .8f;
    [SerializeField]
    private float minLegRotY = 80f, maxLegRotY = 100f;

    [SerializeField]
    private Vector3 initialRotation = Vector3.zero;

    [SerializeField]
    private Gun defaultGun = null;
    private Gun currentGun;
    [SerializeField]
    private GameObject gunPosition = null;

    [SerializeField]
    private float aimMaxLength = 15f;
    [SerializeField]
    private LayerMask aimingIgnoredColliders = 0;
    [SerializeField]
    private LineRenderer aimingLine = null;
    const float spineHeight = 0.075f;
    [SerializeField]
    private GameObject crosshair = null;
    private float coverAimingLineRange = 30f;

    // Start is called before the first frame update
    void Start()
    {
        currentGun = defaultGun;

        // Set Gun UI
        ((PlayerGun)currentGun).SetCurrentGun(true);

        aimingIgnoredColliders = ~aimingIgnoredColliders;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerControl playerControl = Player.GetInstanceControl();

        // Can't aim and shoot if in a cinematic transition
        if (playerControl.IsInTransition())
        {
            return;
        }

        bool isAiming = Input.GetButton("Aim") && playerControl.IsAlive();

        playerControl.SetIsAiming(isAiming);
    }

    private void LateUpdate()
    {
        PlayerControl playerControl = Player.GetInstanceControl();
        if (playerControl.IsAiming())
        {
            if(playerControl.IsCovering())
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

        if (vpMousePos.x < vpSpinePos.x)
        {
            rot %= 180;
        }
        Player.GetInstanceControl().RotateSkeleton(vpMousePos.x < vpSpinePos.x);

        if (rot >= maxRotation) rot = maxRotation;

        if (vpMousePos.y > vpSpinePos.y) rot *= -1;

        spine.transform.localRotation = Quaternion.Euler(new Vector3(rot, initialRotation.y, initialRotation.z));
    }

    private void SetAimingLinePositions()
    {
        Transform bulletSpawnerTransform = currentGun.GetBulletSpawnerTransform();


        Vector2 rayDirection = bulletSpawnerTransform.forward;
        Vector2 startPoint = bulletSpawnerTransform.position;

        Vector3 endWorldPoint = GetAimingLineFinalPos(new Ray(startPoint, rayDirection), aimMaxLength);

        DrawAimingLine(startPoint, endWorldPoint, (endWorldPoint - bulletSpawnerTransform.position).normalized);
    }

    private void DrawAimingLine(Vector3 startPoint, Vector3 endPoint, Vector3 shootingDirection)
    {
        aimingLine.gameObject.SetActive(true);
        aimingLine.SetPosition(0, startPoint);
        aimingLine.SetPosition(1, endPoint);

        currentGun.SetShootingDirection(shootingDirection);
    }

    private Vector3 GetAimingLineFinalPos(Ray ray, float maxLength)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, aimMaxLength, aimingIgnoredColliders))
        {
            return hit.point;
        }

        return ray.origin + ray.direction * aimMaxLength;
    }

    private void ResetAimingLine()
    {
        aimingLine.gameObject.SetActive(false);
    }

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
        vpMousePos.x = Mathf.Clamp(vpMousePos.x, minVPCrshrX, maxVPCrshrX);
        vpMousePos.y = Mathf.Clamp(vpMousePos.y, minVPCrshrY, maxVPCrshrY);

        Ray ray = Camera.main.ViewportPointToRay(vpMousePos);
        
        Vector3 crosshairPos = GetAimingLineFinalPos(ray, coverAimingLineRange);
        
        crosshair.transform.position = crosshairPos;

        return crosshairPos;
    }

    private void SetAimingCrossHairLinePositions(Vector3 crosshairPos)
    {
        Transform bulletSpawnerTransform = currentGun.GetBulletSpawnerTransform();

        DrawAimingLine(bulletSpawnerTransform.position, crosshairPos, (crosshairPos - bulletSpawnerTransform.position).normalized);
    }

    private void RotateSpineCrosshair(Vector3 crosshairPos)
    {
        float dirY = Mathf.Clamp(Quaternion.LookRotation((crosshairPos - Player.GetArmatureTransform().position)).eulerAngles.y, minLegRotY, maxLegRotY);

        Player.GetInstanceControl().RotateSkeleton(dirY);

        spine.transform.LookAt(crosshairPos);
    }

    private void ResetRotationSpineCrosshair()
    {
        Player.GetInstanceControl().RotateSkeleton(new Vector3(1, 0, 0));
    }

    private void OnDisable()
    {
        ResetAimingLine();
        currentGun.enabled = false;
    }
}
