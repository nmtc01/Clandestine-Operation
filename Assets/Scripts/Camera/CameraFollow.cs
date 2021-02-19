using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    private void LateUpdate() 
    {
        Follow();
    }

    private void Follow()
    {
        Vector3 playerPosition = player.transform.position + offset;
        transform.position = playerPosition;
    }
}
