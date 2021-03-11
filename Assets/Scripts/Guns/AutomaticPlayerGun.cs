using UnityEngine;

public class AutomaticPlayerGun : PlayerGun
{
    [SerializeField]
    private int maxAmmo = 100;
    private int currentAmmo;

    public override void Start()
    {
        base.Start();
        currentAmmo = maxAmmo;
    }

    protected override bool GunCanShoot()
    {
        return Input.GetButton("Fire");
    }

    public override void Shoot()
    {
        currentAmmo--;
        PlayerGunUI.instance.SetSliderValue(currentAmmo);
        
        base.Shoot();
    }

    protected override void HandleEmptyClip()
    {
        maxAmmo -= clipMaxSize;

        if(maxAmmo <= 0)
        {
            // Resets the current player gun and destroys this gun
            Player.GetInstanceShoot().ResetGun();
        } 
        else
        {
            if(maxAmmo < clipMaxSize) clipMaxSize = maxAmmo;
            PlayerGunUI.instance.SetClipProperties(clipMaxSize, clipCurrentSize);

            StartCoroutine(Reload());
        }
    }

    public override void SetPlayerGunUI()
    {
        SetUIImage();
        PlayerGunUI.instance.InitSlider(maxAmmo, currentAmmo);
        PlayerGunUI.instance.SetClipProperties(clipMaxSize, clipCurrentSize);
    }

}
