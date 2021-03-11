using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed;
    public Transform cameraTarget;
    public float aheadAmount;
    public float aheadSpeed;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        playerSpeed = 4f;
        aheadAmount = 4f;
        aheadSpeed = 2f;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Can't walk with player input if entering on elevator or if is covering
        if (Player.GetInstanceControl().IsInElevator() || Player.GetInstanceControl().IsCovering() || !Player.GetInstanceControl().IsAlive()) return;

        Move();
    }

    // Player Horizontal Movement
    void Move() 
    {
        float movement = Input.GetAxis("Horizontal");
        if (movement != 0)
        {
            // Player is walking
            Player.GetInstanceControl().SetIsWalking(true);
            
            int camDelta = 1;
            transform.position += new Vector3(playerSpeed * movement * Time.deltaTime, 0f, 0f);

            if (!Player.GetInstanceControl().IsAiming())
            {
                // Turn
                if (!Mathf.Approximately(0, movement)) 
                    Player.GetInstanceControl().RotateSkeleton(movement < 0);
            }
            else
            {
                // Player is aiming
                // Player moving while aiming in opposite directions
                if (movement * Player.GetInstanceControl().getSkeletonDirection().x < 0) 
                {
                    camDelta = -1;
                    Player.GetInstanceControl().SetOppositeDir(1f);
                }
                else Player.GetInstanceControl().SetOppositeDir(0f);
            }

            // Camera follow movement
            cameraTarget.localPosition = new Vector3(Mathf.Lerp(cameraTarget.localPosition.x, aheadAmount * camDelta * movement, aheadSpeed * Time.deltaTime), cameraTarget.localPosition.y, cameraTarget.localPosition.z);
        }
        else Player.GetInstanceControl().SetIsWalking(false);
    }

    public void WalkToElevatorDoor(Vector3 doorPosition, Elevator elevator)
    {
        PlayerControl playerControl = Player.GetInstanceControl();
        playerControl.SetInElevator(true);
        playerControl.SetIsWalking(true);
        playerControl.SetIsAiming(false);

        StartCoroutine(MoveToElevatorDoor(doorPosition, elevator));
    }

    private IEnumerator MoveToElevatorDoor(Vector3 doorPosition, Elevator elevator)
    {
        //Quaternion lookToDoor = Quaternion.Euler(0, -90, 0);
        float maxTimeToRotate = .2f;
        PlayerControl playerControl = Player.GetInstanceControl();
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
        Player.GetInstanceControl().SetIsWalking(false);
        yield return null;
    }

    public void UpdatePlayerPosOnElevator(Vector3 doorPosition)
    {
        Player.GetInstanceControl().RotateSkeleton(180);
        transform.position = doorPosition;
    }

    public void WalkFromElevatorDoor(Vector2 doorPosition, Elevator elevator)
    {
        Player.GetInstanceControl().SetIsWalking(true);

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

        PlayerControl playerControl = Player.GetInstanceControl();
        playerControl.SetIsWalking(false);
        playerControl.SetInElevator(false);
        elevator.PlayerCanInteract();
        yield return null;
    }
}
