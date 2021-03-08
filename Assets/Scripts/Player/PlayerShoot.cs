using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject spine = null;

    [SerializeField]
    private float maxRotation = 45f;

    [SerializeField]
    private Vector3 initialRotation = Vector3.zero;

    private PlayerControl playerControl;

    [SerializeField]
    private Gun defaultGun = null;
    private Gun currentGun;
    [SerializeField]
    private GameObject gunPosition = null;

    [SerializeField]
    private float aimMaxLength = 15f;
    private bool isAiming = false;
    [SerializeField]
    private LayerMask aimingIgnoredColliders = 0;
    [SerializeField]
    private LineRenderer aimingLine = null;
    const float spineHeight = 0.075f;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        currentGun = defaultGun;

        aimingIgnoredColliders = ~aimingIgnoredColliders;
    }

    // Update is called once per frame
    void Update()
    {
        // Can't aim and shoot if entering on elevator
        if (playerControl.IsInElevator()) return;

        isAiming = Input.GetButton("Aim");

        playerControl.SetIsAiming(isAiming);
        currentGun.SetCanShoot(isAiming);
    }

    private void LateUpdate()
    {
        if (isAiming)
        {
            RotateSpine();
            SetAimingLinePositions();
        }
        else
        {
            spine.transform.localRotation = Quaternion.Euler(initialRotation);
            ResetAimingLine();
        }
    }

    private void RotateSpine()
    {
        Vector2 vpMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 vpSpinePos = Camera.main.WorldToViewportPoint(spine.transform.position) + new Vector3(0, spineHeight, 0);

        Vector2 lookToMouseVec = (vpMousePos - vpSpinePos).normalized;

        float rot = Mathf.Rad2Deg * Mathf.Acos(Mathf.Clamp(Vector2.Dot(lookToMouseVec, playerControl.getSkeletonDirection()), -1f, 1f));

        if (vpMousePos.x < vpSpinePos.x)
        {
            rot %= 180;
        }
        playerControl.RotateSkeleton(vpMousePos.x < vpSpinePos.x);

        if (rot >= maxRotation) rot = maxRotation;

        if (vpMousePos.y > vpSpinePos.y) rot *= -1;

        spine.transform.localRotation = Quaternion.Euler(new Vector3(rot, initialRotation.y, initialRotation.z));
    }

    private void SetAimingLinePositions()
    {
        Transform bulletSpawnerTransform = currentGun.GetBulletSpawnerTransform();

        Vector3 endWorldPoint;

        RaycastHit hit;

        Vector2 rayDirection = bulletSpawnerTransform.forward;
        Vector2 startPoint = bulletSpawnerTransform.position;

        if (Physics.Raycast(startPoint, rayDirection, out hit, aimMaxLength, aimingIgnoredColliders))
        {
            endWorldPoint = hit.point;
        }
        else
        {
            endWorldPoint = bulletSpawnerTransform.position + bulletSpawnerTransform.forward * aimMaxLength;
        }

        aimingLine.gameObject.SetActive(true);
        aimingLine.SetPosition(0, startPoint);
        aimingLine.SetPosition(1, endWorldPoint);

        currentGun.SetShootingDirection((endWorldPoint - bulletSpawnerTransform.position).normalized);
    }

    private void ResetAimingLine()
    {
        aimingLine.gameObject.SetActive(false);
    }

    public void SetNewGun(PlayerGun gun)
    {
        if(currentGun == defaultGun)
        {
            currentGun.gameObject.SetActive(false);
        } 
        else
        {
            Destroy(currentGun);
        }

        currentGun = gun;
        gun.transform.parent = gunPosition.transform;
        gun.SetHandPosition();
    }

    public void ResetGun()
    {
        Destroy(currentGun);
        currentGun = defaultGun;
        currentGun.gameObject.SetActive(true);
    }
}
