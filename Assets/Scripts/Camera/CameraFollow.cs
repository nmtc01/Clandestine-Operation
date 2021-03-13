using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Singleton
    private static CameraFollow instance = null;

    private void Awake()
    {
        if (instance != null && instance != gameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    [SerializeField]
    private float minXValue = -100f, maxXValue = 100f;
    private float currentMinXVal = -100f, currentMaxXVal = 100f;

    public Transform target;
    private Vector3 offset;
    private Vector3 coveringOffset;
    private bool rotate = true;

    private static bool canFollow = true;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 4, -10);
        coveringOffset = new Vector3(4, 0.8f, -10);

        ResetXValues();
    }

    private void LateUpdate() 
    {
        if(canFollow)
            Follow();
    }

    private void Follow()
    {
        PlayerControl playerControl = Player.GetInstanceControl();
        if (playerControl.IsCovering() && rotate) 
        {
            Cover(90, -1);
        }
        else if (!playerControl.IsCovering() && !rotate)
        {
            Stand();
        }
        Vector3 targetPosition = target.transform.position + offset;
        targetPosition.x = Mathf.Clamp(targetPosition.x, currentMinXVal, currentMaxXVal);
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

    private void SetNewXValue(bool max)
    {
        if (max)
            currentMaxXVal = instance.transform.position.x;
        else
            currentMinXVal = instance.transform.position.x;
    }

    public static void SetNewVal(bool max)
    {
        instance.SetNewXValue(max);
    }

    private void ResetXValues()
    {
        currentMinXVal = minXValue;
        currentMaxXVal = maxXValue;

    }

    public static void ResetValues()
    {
        instance.ResetXValues();
    }

}
