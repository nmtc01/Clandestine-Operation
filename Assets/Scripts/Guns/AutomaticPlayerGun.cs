using UnityEngine;

public class AutomaticPlayerGun : PlayerGun
{
    [SerializeField]
    private int maxAmmo = 100;

    protected override bool GunCanShoot()
    {
        return Input.GetButton("Fire");
    }

    protected override void HandleEmptyClip()
    {
        maxAmmo -= clipMaxSize;

        if(maxAmmo <= 0)
        {
            PlayerShoot playerShoot = Player.GetInstance().GetComponent<PlayerShoot>();
            playerShoot.ResetGun();
            Destroy(gameObject);
        } 
        else
        {
            if(maxAmmo < clipMaxSize) clipMaxSize = maxAmmo;

            StartCoroutine(Reload());
        }
    }
}
