using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed;
    public Transform cameraTarget;
    public float aheadAmount;
    public float aheadSpeed;

    private PlayerControl playerControl;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        playerSpeed = 4f;
        aheadAmount = 4f;
        aheadSpeed = 2f;
        playerControl = GetComponent<PlayerControl>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Can't walk with player input if entering on elevator
        if (playerControl.IsInElevator()) return;

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
            transform.position += new Vector3(playerSpeed * movement * Time.deltaTime, 0f, 0f);

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
                {
                    camDelta = -1;
                    playerControl.SetOppositeDir(1f);
                }
                else playerControl.SetOppositeDir(0f);
            }

            // Camera follow movement
            cameraTarget.localPosition = new Vector3(Mathf.Lerp(cameraTarget.localPosition.x, aheadAmount * camDelta * movement, aheadSpeed * Time.deltaTime), cameraTarget.localPosition.y, cameraTarget.localPosition.z);
        }
        else playerControl.SetIsWalking(false);
    }

    public void WalkToElevatorDoor(Vector3 doorPosition, Elevator elevator)
    {
        playerControl.SetInElevator(true);
        playerControl.SetIsWalking(true);

        StartCoroutine(MoveToElevatorDoor(doorPosition, elevator));
    }

    private IEnumerator MoveToElevatorDoor(Vector3 doorPosition, Elevator elevator)
    {
        //Quaternion lookToDoor = Quaternion.Euler(0, -90, 0);
        float maxTimeToRotate = .2f;
        float initY = playerControl.getSkeletonDirection().y;
        for (float t = 0; t <= maxTimeToRotate; t += Time.deltaTime)
        {
            playerControl.RotateSkeleton(Mathf.Lerp(initY, 0, t / maxTimeToRotate));
            yield return null;
        }

        Vector3 initVal = transform.position;
        for (float t = 0; t <= 3; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(initVal, doorPosition, t / 3f);
            yield return null;
        }

        elevator.PlayerEnteredDoor();
        playerControl.SetIsWalking(false);
        yield return null;
    }

    public void UpdatePlayerPosOnElevator(Vector3 doorPosition)
    {
        playerControl.RotateSkeleton(180);
        transform.position = doorPosition;
    }

    public void WalkFromElevatorDoor(Vector2 doorPosition, Elevator elevator)
    {
        playerControl.SetIsWalking(true);

        StartCoroutine(MoveFromElevatorDoor(doorPosition, elevator));
    }

    private IEnumerator MoveFromElevatorDoor(Vector2 doorPosition, Elevator elevator)
    {
        Vector3 initVal = transform.position;
        for (float t = 0; t <= 3; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(initVal, doorPosition, t / 3f);
            yield return null;
        }

        playerControl.SetIsWalking(false);
        playerControl.SetInElevator(false);
        elevator.PlayerCanInteract();
        yield return null;
    }
}
