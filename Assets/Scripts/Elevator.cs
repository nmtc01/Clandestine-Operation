using System;
using System.Collections.Generic;
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
    [SerializeField]
    private List<GameObject> interactButtons = new List<GameObject>();
    private bool canInteract = false;

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
        if(canInteract && Input.GetButtonDown("Interact"))
        {
            // Player enter door, changes state, player exits other door
        }
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

    private void OnTriggerEnter(Collider other)
    {
        DetectCollisionWithPlayer(other.gameObject, true);
    }

    private void OnTriggerExit(Collider other)
    {
        DetectCollisionWithPlayer(other.gameObject, false);
    }

    private void DetectCollisionWithPlayer(GameObject obj, bool active)
    {
        if (obj.layer == LayerMask.NameToLayer("Player"))
        {
            canInteract = active;
            SetButtonsActive(active);
        }
    }

    private void SetButtonsActive(bool active)
    {
        for (int i = 0; i < interactButtons.Count; ++i)
        {
            interactButtons[i].SetActive(active);
        }
    }

}
