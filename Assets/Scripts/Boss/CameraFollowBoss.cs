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

        if (Mathf.Abs(target.position.x - this.transform.position.x) <= range)
        {
            BossController.GetInstance().Turn();
            if (Input.GetButton("Return"))
            {
                this.transform.gameObject.SetActive(false);
                BossController.GetInstance().GrabGun(true); 
            }
        }
    }

    private void Follow()
    {
        Vector3 targetPosition = target.transform.position + offset;
        targetPosition.x = Mathf.Lerp(this.transform.position.x,Mathf.Clamp(targetPosition.x, minXValue, maxXValue), Time.deltaTime*0.3f);
        targetPosition.z = offset.z;
        transform.position = targetPosition;
    }
}
