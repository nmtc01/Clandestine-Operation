using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject spine = null;

    [SerializeField]
    private float maxRotation = 45f;

    [SerializeField]
    private Vector3 initalRotation = Vector3.zero;

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
        isAiming = Input.GetButton("Aim");

        if (isAiming)
        {
            RotateSpine();
            SetAimingLinePositions();
        } 
        else
        {
            spine.transform.localRotation = Quaternion.Euler(initalRotation);
            ResetAimingLine();
        }

        playerControl.SetIsAiming(isAiming);
        currentGun.SetCanShoot(isAiming);
    }

    private void RotateSpine()
    {
        Vector2 vpMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 vpSpinePos = Camera.main.WorldToViewportPoint(spine.transform.position);

        Vector2 lookToMouseVec = (vpMousePos - vpSpinePos).normalized;

        float rot = Mathf.Rad2Deg * Mathf.Acos(Mathf.Clamp(Vector2.Dot(lookToMouseVec, playerControl.getSkeletonDirection()),-1f,1f));

        if(vpMousePos.x < vpSpinePos.x)
        {
            rot %= 180;
        } 
        playerControl.RotateSkeleton(vpMousePos.x < vpSpinePos.x);

        if (rot >= maxRotation) rot = maxRotation;

        if (vpMousePos.y > vpSpinePos.y) rot *= -1;

        spine.transform.localRotation = Quaternion.Euler(new Vector3(rot, initalRotation.y, initalRotation.z));
    }

    private void SetAimingLinePositions()
    {
        Transform bulletSpawnerTransform = currentGun.GetBulletSpawnerTransform();

        /* 
         * TODO 
         *  Change Raycast to BoxCast and detect collisions with objects even if they are not on the same z coordinate
         *    Physics.BoxCast(
         *          bulletSpawnerTransform.position, 
         *          new Vector3(5f, .0001f, 3f), 
         *          bulletSpawnerTransform.forward,
         *          out hit, 
         *          Quaternion.identity, 
         *          aimMaxLength, 
         *          aimingIgnoredColliders
         *    )
         */
        Vector3 endWorldPoint; 
        
        RaycastHit hit;
        
        Vector2 rayDirection = bulletSpawnerTransform.forward;
        Vector2 startPoint = bulletSpawnerTransform.position;

        if(Physics.Raycast(startPoint, rayDirection, out hit, aimMaxLength, aimingIgnoredColliders))
        {
            endWorldPoint = hit.point;
        }   
        else
        {
            endWorldPoint = bulletSpawnerTransform.position + bulletSpawnerTransform.forward * aimMaxLength;
        }

        aimingLine.gameObject.SetActive(true);
        aimingLine.SetPosition(0, bulletSpawnerTransform.position);
        aimingLine.SetPosition(1, endWorldPoint);

        currentGun.SetShootingDirection((endWorldPoint - bulletSpawnerTransform.position).normalized);
    }

    private void ResetAimingLine()
    {
        aimingLine.gameObject.SetActive(false);
    }
}
