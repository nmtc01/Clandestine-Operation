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
    private static float currentMinXVal = -100f, currentMaxXVal = 100f;

    public Transform target;
    private Vector3 offset;

    private static bool canFollow = true;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 4, -10);

        ResetXValues();
    }

    private void LateUpdate() 
    {
        if(canFollow)
            Follow();
    }

    private void Follow()
    {
        Vector3 targetPosition = target.transform.position + offset;
        targetPosition.x = Mathf.Clamp(targetPosition.x, currentMinXVal, currentMaxXVal);
        targetPosition.z = offset.z;
        transform.position = targetPosition;
    }

    public static void SetNewVal(bool max)
    {
        if (max)
            currentMaxXVal = instance.transform.position.x;
        else
            currentMinXVal = instance.transform.position.x;
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
