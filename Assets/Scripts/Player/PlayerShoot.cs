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

    // Start is called before the first frame update
    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        currentGun = defaultGun;
    }

    // Update is called once per frame
    void Update()
    {
        bool isAiming = Input.GetButton("Aim");

        if (isAiming)
        {
            RotateSpine();
        } 
        else
        {
            spine.transform.localRotation = Quaternion.Euler(initalRotation);
        }

        playerControl.SetIsAiming(isAiming);
        currentGun.SetCanShoot(isAiming);
    }

    private void RotateSpine()
    {
        Vector2 vpMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 vpSpinePos = Camera.main.WorldToViewportPoint(spine.transform.position);

        Vector2 lookToMouseVec = (vpMousePos - vpSpinePos).normalized;

        float rot = Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(lookToMouseVec, playerControl.getSkeletonDirection()));

        if(vpMousePos.x < vpSpinePos.x)
        {
            rot %= 180;
        } 
        playerControl.RotateSkeleton(vpMousePos.x < vpSpinePos.x);

        if (rot >= maxRotation) rot = maxRotation;

        if (vpMousePos.y > vpSpinePos.y) rot *= -1;

        spine.transform.localRotation = Quaternion.Euler(new Vector3(rot, initalRotation.y, initalRotation.z));
    }
}
