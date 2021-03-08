using UnityEngine;

public class CameraBoundary : MonoBehaviour
{
    [SerializeField]
    private bool stopOnExitRight = true;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CameraFollow.SetNewVal(stopOnExitRight);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CameraFollow.ResetValues();
        }
    }
}
