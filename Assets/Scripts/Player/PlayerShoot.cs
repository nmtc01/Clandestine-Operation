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

    // Start is called before the first frame update
    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Aim"))
        {
            RotateSpine();

            playerControl.SetIsAiming(true);

            /*if(Input.GetButtonDown("Fire"))
            {
                Debug.Log("Fire");
            }*/
        } 
        else
        {
            spine.transform.localRotation = Quaternion.Euler(initalRotation);

            playerControl.SetIsAiming(false);
        }
    }

    private void RotateSpine()
    {
        Vector2 vpMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 vpSpinePos = Camera.main.WorldToViewportPoint(spine.transform.position);

        Vector2 lookToMouseVec = vpMousePos - vpSpinePos;

        float rot = Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(lookToMouseVec, transform.forward) / lookToMouseVec.magnitude);

        if(vpMousePos.x < vpSpinePos.x)
        {
            rot %= 180;
            transform.rotation = Quaternion.Euler(0, -90, 0);
        } 
        else
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        if (rot >= maxRotation) rot = maxRotation;

        if (vpMousePos.y > vpSpinePos.y) rot *= -1;

        spine.transform.localRotation = Quaternion.Euler(new Vector3(rot, initalRotation.y, initalRotation.z));
    }
}
