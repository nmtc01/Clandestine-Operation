using UnityEngine;

public class PlayerGun : Gun
{
    // Update is called once per frame
    void Update()
    {
        if (canShoot && Input.GetButtonDown("Fire"))
        {
            Shoot();
        }
    }
}
