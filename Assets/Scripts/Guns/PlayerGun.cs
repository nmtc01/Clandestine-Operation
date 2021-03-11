using System.Collections;
using UnityEngine;

public class PlayerGun : Gun
{
    [SerializeField]
    private float shotCooldown = .15f;
    private float timeSinceLastShot = 0f;

    [SerializeField]
    protected int clipMaxSize = 10;
    protected int clipCurrentSize;

    [SerializeField]
    private float reloadingTime = 1f;
    private bool isReloading = false;

    [SerializeField]
    private Vector3 gunLocalPosition = Vector3.zero;

    #region Gun UI
    [SerializeField]
    private string imagePath = "";
    #endregion

    private bool isCurrentGun = false;

    public override void Start()
    {
        base.Start();

        clipCurrentSize = clipMaxSize;
        PlayerGunUI.instance.SetClipProperties(clipMaxSize, clipCurrentSize);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (isCurrentGun && Player.GetInstanceControl().IsAiming() && timeSinceLastShot >= shotCooldown && !isReloading && GunCanShoot())
        {
            Shoot();
        }
    }

    /**
     * Function that detects if the gun can shoot. It should be overrided by this class' children so that other behaviours, such as automatic guns, can be achieved
     */
    protected virtual bool GunCanShoot()
    {
        return Input.GetButtonDown("Fire");
    }

    public override void Shoot()
    {
        base.Shoot();

        timeSinceLastShot = 0;
        clipCurrentSize--;

        PlayerGunUI.instance.SetClipCurrentSize(clipCurrentSize);

        if (clipCurrentSize == 0)
            HandleEmptyClip();
    }

    public void SetCurrentGun(bool currentGun)
    {
        isCurrentGun = currentGun;

        if (currentGun) SetPlayerGunUI();
    }

    protected virtual void HandleEmptyClip()
    {
        StartCoroutine(Reload());
    }

    protected IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadingTime);

        clipCurrentSize = clipMaxSize;
        PlayerGunUI.instance.SetClipProperties(clipMaxSize, clipCurrentSize);
        isReloading = false;
        yield return null;
    }

    public void SetHandPosition()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        transform.localPosition = gunLocalPosition;
        transform.localRotation = Quaternion.identity;
    }

    protected virtual void SetPlayerGunUI()
    {
        SetUIImage();
        PlayerGunUI.instance.InitSlider();
        PlayerGunUI.instance.SetClipProperties(clipMaxSize, clipCurrentSize);
    }

    protected void SetUIImage()
    {
        Sprite spr = Resources.Load<Sprite>(imagePath);
        PlayerGunUI.instance.SetImage(spr);
    }
}
