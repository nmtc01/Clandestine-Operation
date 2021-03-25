using System.Collections;
using UnityEngine;

public class PlayerGun : Gun
{
    [SerializeField]
    protected float shotCooldown = .15f;
    protected float timeSinceLastShot = 0f;

    [SerializeField]
    protected int clipMaxSize = 10;
    protected int clipCurrentSize;

    [SerializeField]
    private float reloadingTime = 1f;
    protected bool isReloading = false;

    [SerializeField]
    private Vector3 gunLocalPosition = Vector3.zero;

    #region Gun UI
    [SerializeField]
    private string imagePath = "";
    #endregion

    [SerializeField]
    private AudioClip reloadAudioClip = null;

    private bool isCurrentGun = false;
    protected bool gunIsShooting = false;

    public override void Start()
    {
        base.Start();

        clipCurrentSize = clipMaxSize;

        if(isCurrentGun)
            PlayerGunUI.instance.SetClipProperties(clipMaxSize, clipCurrentSize);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isCurrentGun) return; // These operations can only be executed if the gun is the current player gun

        if(!isReloading && clipCurrentSize < clipMaxSize && Input.GetButtonDown("Reload"))
        {
            HandleReload();
        }

        timeSinceLastShot += Time.deltaTime;
        gunIsShooting = Player.GetInstanceControl().IsAiming() && timeSinceLastShot >= shotCooldown && !isReloading && GunCanShoot();
        
        if(gunIsShooting)
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
            HandleReload();
    }

    public void SetCurrentGun(bool currentGun)
    {
        isCurrentGun = currentGun;

        if (currentGun)
        {
            SetPlayerGunUI();
        }
    }

    protected virtual void HandleReload()
    {
        StartCoroutine(Reload());
    }

    protected IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(audioSource.clip.length - audioSource.time);

        // Plays the reload clip
        audioSource.Stop();
        audioSource.clip = reloadAudioClip;
        audioSource.Play();

        yield return new WaitForSeconds(reloadingTime);

        clipCurrentSize = clipMaxSize;
        PlayerGunUI.instance.SetClipProperties(clipMaxSize, clipCurrentSize);
        isReloading = false;

        // Resets the default clip
        audioSource.clip = shootAudioClip;
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
