using UnityEngine;

public class PlayerGun : Gun
{
    [SerializeField]
    private float shotCooldown = .15f;
    private float timeSinceLastShot = 0f;

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (canShoot && timeSinceLastShot >= shotCooldown && Input.GetButtonDown("Fire"))
        {
            Shoot();
            timeSinceLastShot = 0;
        }
    }
}
