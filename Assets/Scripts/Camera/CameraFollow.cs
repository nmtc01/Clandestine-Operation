using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 1, -10);
    }

    private void LateUpdate() 
    {
        Follow();
    }

    private void Follow()
    {
        Vector3 targetPosition = target.transform.position + offset;
        transform.position = targetPosition;
    }
}
