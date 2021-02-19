using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Aim"))
        {
            if(Input.GetButtonDown("Fire"))
            {
                Debug.Log("Fire");
            }
        }
    }
}
