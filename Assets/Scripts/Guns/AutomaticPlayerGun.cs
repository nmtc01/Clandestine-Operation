using UnityEngine;

public class AutomaticPlayerGun : PlayerGun
{
    protected override bool GunCanShoot()
    {
        return Input.GetButtonDown("Fire");
    }
}
