using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed;

    private PlayerControl playerControl;

    // Start is called before the first frame update
    void Start()
    {
        playerSpeed = 10f;

        playerControl = GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    // Player Horizontal Movement
    void Move() 
    {
        float movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(playerSpeed*movement*Time.deltaTime, 0f, 0f);

        // Turn
        if (!Mathf.Approximately(0, movement) && !playerControl.IsAiming())
            transform.rotation = movement > 0 ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0);
    }
}
