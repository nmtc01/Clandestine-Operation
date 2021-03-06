using UnityEngine;

public class Cover : MonoBehaviour
{
    [SerializeField]
    private GameObject key = null;
    private float range = 3f;
    private bool isCovering = false;
    
    [SerializeField]
    private GameObject playerSkeleton = null;
    [SerializeField]
    private GameObject crosshair = null;
    private PlayerControl playerControl;

    void Start()
    {
        playerControl = Player.GetInstance().GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform player = Player.GetInstance().transform;
        if (Mathf.Abs(this.transform.position.x - player.position.x) <= range)
        {
            key.SetActive(true);
            if (Input.GetButtonDown("Interact"))
            {
                isCovering = !isCovering;
                playerControl.SetIsCovering(isCovering);
                crosshair.SetActive(isCovering);
                player.position = new Vector3(transform.position.x - range, player.position.y, player.position.z);
                if (playerSkeleton.transform.right.z > 0) playerSkeleton.transform.right = -1 * playerSkeleton.transform.right;
            }
        }
        else 
        {
            crosshair.SetActive(false);
            key.SetActive(false);
        }
    }
}
