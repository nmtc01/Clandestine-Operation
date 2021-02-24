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
            // Player is walking
            playerControl.SetIsWalking(true);
            
            int camDelta = 1;
            transform.position += new Vector3(playerSpeed*movement*Time.deltaTime, 0f, 0f);

            if (!playerControl.IsAiming())
            {
                // Turn
                if (!Mathf.Approximately(0, movement)) 
                    playerControl.RotateSkeleton(movement < 0);
            }
            else
            {
                // Player is aiming
                // Player moving while aiming in opposite directions
                if (movement * playerControl.getSkeletonDirection().x < 0) 
                    playerControl.SetOppositeDir(1f);
                else playerControl.SetOppositeDir(0f);
            }

            // Camera follow movement
            cameraTarget.localPosition = new Vector3(Mathf.Lerp(cameraTarget.localPosition.x, aheadAmount * camDelta * movement, aheadSpeed * Time.deltaTime), cameraTarget.localPosition.y, cameraTarget.localPosition.z);
        }
        else playerControl.SetIsWalking(false);
    }
}
