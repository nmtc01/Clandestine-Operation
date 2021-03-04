using System;
using UnityEngine;

public enum ElevatorDoorState
{
    CLOSE_DOOR,
    OPEN_DOOR,
}

[Serializable]
public struct ElevatorDoor
{
    public Animator animator;
    public ElevatorDoorState state;

}

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private ElevatorDoor upDoor, downDoor;

    // Start is called before the first frame update
    void Start()
    {
        if (upDoor.state != ElevatorDoorState.CLOSE_DOOR) 
            SetDoorState(upDoor, downDoor.state);
        if (downDoor.state != ElevatorDoorState.CLOSE_DOOR)
            SetDoorState(downDoor, downDoor.state);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeDoorState(ElevatorDoor door)
    {
        if(door.state == ElevatorDoorState.CLOSE_DOOR)
        {
            SetDoorState(door, ElevatorDoorState.OPEN_DOOR);
        }
        else if(door.state == ElevatorDoorState.OPEN_DOOR)
        {
            SetDoorState(door, ElevatorDoorState.CLOSE_DOOR);
        }
    }

    private void SetDoorState(ElevatorDoor door, ElevatorDoorState state)
    {
        door.state = state;
        door.animator.SetTrigger(GetStateTrigger(state));
    }

    private string GetStateTrigger(ElevatorDoorState state)
    {
        if (state == ElevatorDoorState.CLOSE_DOOR) return "close";
        if (state == ElevatorDoorState.OPEN_DOOR) return "open";

        return "";
    }

}
