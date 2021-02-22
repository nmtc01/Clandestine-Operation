using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed;
    public Transform cameraTarget;
    public float aheadAmount;
    public float aheadSpeed;

    private PlayerControl playerControl;

    // Start is called before the first frame update
    void Start()
    {
        playerSpeed = 10f;
        aheadAmount = 4f;
        aheadSpeed = 2f;
        playerControl = GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    // Player Horizontal Movement
    void Move() 
    {
        float movement = Input.GetAxis("Horizontal");
        if (movement != 0)
        {  
            int camDelta = 1;
            transform.position += new Vector3(playerSpeed*movement*Time.deltaTime, 0f, 0f);

            if (!playerControl.IsAiming())
            {
                // Turn
                if (!Mathf.Approximately(0, movement)) 
                    playerControl.RotateSkeleton(movement < 0);
            }
            else camDelta = playerControl.getSkeletonDirection().x * movement < 0 ? -1 : 1;
            Debug.Log(playerControl.getSkeletonDirection().x > 0);
            Debug.Log(movement > 0);
            Debug.Log(camDelta);

            // Camera follow movement
            cameraTarget.localPosition = new Vector3(cameraTarget.localPosition.x, cameraTarget.localPosition.y, Mathf.Lerp(cameraTarget.localPosition.z, aheadAmount * camDelta * movement, aheadSpeed * Time.deltaTime));
        }
    }
}
