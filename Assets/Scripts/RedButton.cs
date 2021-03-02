using UnityEngine;

public class RedButton : MonoBehaviour
{
    [SerializeField]
    private Doors doors = null;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            doors.OpenDoors();
        }
    }
}
