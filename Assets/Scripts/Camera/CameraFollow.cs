using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float minXValue = -100f, maxXValue = 100f;

    public Transform target;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 4, -10);
    }

    private void LateUpdate() 
    {
        Follow();
    }

    private void Follow()
    {
        Vector3 targetPosition = target.transform.position + offset;
        targetPosition.x = Mathf.Clamp(targetPosition.x, minXValue, maxXValue);
        targetPosition.z = offset.z;
        transform.position = targetPosition;
    }
}
