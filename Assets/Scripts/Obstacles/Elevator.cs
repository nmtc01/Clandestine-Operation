using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElevatorDoorState
{
    CLOSE_DOOR,
    OPEN_DOOR,
}

[Serializable]
public class ElevatorDoor
{
    public Animator animator;
    public ElevatorDoorState state;
    public Vector3 position;
}

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private ElevatorDoor upDoor, downDoor;
    [SerializeField]
    private List<GameObject> interactButtons = new List<GameObject>();
    [SerializeField]
    private int currentFloor = 0;

    private bool canInteract = false;
    private bool moving = false;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        switch (currentFloor)
        {
            case 0:
                SetDoorState(downDoor, ElevatorDoorState.OPEN_DOOR);
                upDoor.state = ElevatorDoorState.CLOSE_DOOR;
                break;
            case 1:
                SetDoorState(upDoor, ElevatorDoorState.OPEN_DOOR);
                downDoor.state = ElevatorDoorState.CLOSE_DOOR;
                break;
            default:
                break;
        }

        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(canInteract && Input.GetButtonDown("Interact"))
        {
            // Player enter door, changes state, player exits other door
            SetButtonsActive(false);

            Player.GetInstanceMovement().WalkToElevatorDoor(CurrentDoor().position, this);
            canInteract = false;
            moving = true;
        }
    }

    public void PlayerEnteredDoor()
    {
        StartCoroutine(SimulateElevatorMovement());
    }

    private IEnumerator SimulateElevatorMovement()
    {
        ChangeDoorState(CurrentDoor());

        yield return new WaitForSeconds(1);

        currentFloor = currentFloor == 0 ? 1 : 0;

        Player.GetInstanceMovement().UpdatePlayerPosOnElevator(CurrentDoor().position);

        ChangeDoorState(CurrentDoor());

        yield return new WaitForSeconds(1);

        Player.GetInstanceMovement().WalkFromElevatorDoor(CurrentDoor().position, this);

        yield return null;
    }

    private void ChangeDoorState(ElevatorDoor door)
    {
        audioSource.Play();

        if (door.state == ElevatorDoorState.CLOSE_DOOR)
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
        if (!moving && obj.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerCanInteract(active);
        }
    }

    private void SetButtonsActive(bool active)
    {
        for (int i = 0; i < interactButtons.Count; ++i)
        {
            interactButtons[i].SetActive(active);
        }
    }

    private ElevatorDoor CurrentDoor()
    {
        return currentFloor == 0 ? downDoor : upDoor;
    }

    public void PlayerCanInteract(bool interact = true)
    {
        canInteract = interact;
        moving = false;
        SetButtonsActive(interact);
    }
}
