using UnityEngine;

public class Cover : MonoBehaviour, IHealthController
{
    [SerializeField]
    private GameObject key = null;
    private float range = 3.2f;
    private float slack = 3f;
    private bool isCovering = false;
    
    [SerializeField]
    private GameObject playerSkeleton = null;
    [SerializeField]
    private GameObject healthUI = null;

    // Update is called once per frame
    void Update()
    {
        Transform player = Player.GetInstance().transform;
        if (Mathf.Abs(this.transform.position.x - player.position.x) <= range)
        {
            // Show key
            if (key) key.SetActive(true);

            // Interact
            if (Input.GetButtonDown("Interact") && Player.GetInstanceControl().IsAlive())
            {
                Interact();
            }
        }
        else Deactivate();
    }

    private void HandlePlayerColliders()
    {
        GameObject player = Player.GetInstance();

        // Freeze positions
        Rigidbody rigidbody = player.GetComponent<Rigidbody>();
        if (isCovering)
        {
            Player.EnablePhysics(false);
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            Player.EnablePhysics(true);
            rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
    }

    private void Interact()
    {
        isCovering = !isCovering;

        // Activate covering features
        GameObject fourthWall = FourthWall.GetInstance();
        if (fourthWall) fourthWall.SetActive(isCovering);
        Player.GetInstanceControl().SetIsCovering(isCovering);
        if (healthUI) healthUI.SetActive(isCovering);

        // Change player position and rotation
        Transform player = Player.GetInstance().transform;
        player.position = new Vector3(transform.position.x - slack, player.position.y, player.position.z);
        if (playerSkeleton && playerSkeleton.transform.right.z > 0) playerSkeleton.transform.right = -1 * playerSkeleton.transform.right;

        // Change player colliders to fit new position
        HandlePlayerColliders();
    }


    private void Deactivate()
    {
        if (key) key.SetActive(false);
    }

    public void SetIsDead(bool dead)
    {
        if(isCovering && dead) Interact();
    }
}
