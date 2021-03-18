using UnityEngine;

public class CameraFollowBoss : MonoBehaviour
{
    [SerializeField]
    private float minXValue = -100f, maxXValue = 100f;

    public Transform target;
    private Vector3 offset;
    private float range = 6f;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 4, 0);
    }

    private void LateUpdate() 
    {
        Follow();
    }

    private void Follow()
    {
        Vector3 targetPosition = target.transform.position + offset;
        targetPosition.x = Mathf.Lerp(transform.position.x,Mathf.Clamp(targetPosition.x, minXValue, maxXValue), Time.deltaTime*0.3f);
        targetPosition.z = offset.z;
        transform.position = targetPosition;
    }

    public void ActivateCamera()
    {
        GameObject fourthWall = FourthWall.GetInstance();
        if (fourthWall) fourthWall.SetActive(true);
        PlayerControl playerControl = Player.GetInstanceControl();
        playerControl.SetInTransition(true);
        playerControl.ResetPlayerMovements();
        transform.gameObject.SetActive(true);
    }

    public void DeactivateCamera()
    {
        GameObject fourthWall = FourthWall.GetInstance();
        if (fourthWall) fourthWall.SetActive(false);
        Player.GetInstanceControl().SetInTransition(false);
        transform.gameObject.SetActive(false);
    }

    public bool IsInRange()
    {
        return Mathf.Abs(target.position.x - transform.position.x) <= range;
    }
}
