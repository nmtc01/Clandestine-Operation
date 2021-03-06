using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private GameObject crosshair = null;
    private float speed = 4f;
    private PlayerControl playerControl;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = Player.GetInstance().GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        // Player cannot move crosshair if is not covering
        if (!playerControl.IsCovering()) return;

        Move();
    }

    void Move() 
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            Vector3 crosshairPos = crosshair.transform.position;
            crosshairPos.y = Mathf.Clamp(crosshairPos.y + speed * vertical * Time.deltaTime, 7, 13);
            crosshairPos.z = Mathf.Clamp(crosshairPos.z - speed * horizontal * Time.deltaTime, -5, 3);
            crosshair.transform.position = crosshairPos;
        }
    }
}
