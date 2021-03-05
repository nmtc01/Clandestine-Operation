using UnityEngine;

public class Cover : MonoBehaviour
{
    [SerializeField]
    private GameObject key = null;
    private int range = 5;
    private PlayerControl playerControl;
    private bool isCovering = false;

    void Start()
    {
        playerControl = Player.GetInstance().GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform player = Player.GetInstance().transform;
        if ((player.position - this.transform.position).magnitude < range)
        {
            key.SetActive(true);
            if (Input.GetButtonDown("Interact"))
            {
                isCovering = !isCovering;
                playerControl.SetIsCovering(isCovering);
                player.SetParent(this.transform);
            }
        }
        else 
        {
            key.SetActive(false);
        }
    }
}
