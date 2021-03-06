using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float minXValue = -100f, maxXValue = 100f;
    public Transform target;
    private Vector3 offset;
    private Vector3 coveringOffset;
    private PlayerControl playerControl;
    private bool rotate = true;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 4, -10);
        coveringOffset = new Vector3(4, 0.8f, -10);
        playerControl = Player.GetInstance().GetComponent<PlayerControl>();
    }

    private void LateUpdate() 
    {
        Follow();
    }

    private void Follow()
    {
        if (playerControl.IsCovering() && rotate) 
        {
            Cover(90, -1);
        }
        else if (!playerControl.IsCovering() && !rotate)
        {
            Stand();
        }
        Vector3 targetPosition = target.transform.position + offset;
        targetPosition.x = Mathf.Clamp(targetPosition.x, minXValue, maxXValue);
        targetPosition.z = offset.z;
        transform.position = targetPosition;
    }

    private void Cover(int radius, int factor)
    {
        target.position = Player.GetInstance().transform.position;
        this.transform.rotation = Quaternion.Euler(0, radius, 0);
        offset += factor * coveringOffset; 
        rotate = !rotate;
    }

    private void Stand()
    {
        Cover(0, 1);
    }
}
