using System.Collections;
using UnityEngine;

public class BossGun : Gun
{
    [SerializeField]
    protected float shotCooldown = 10f;
    protected float timeSinceLastShot = 0f;

    [SerializeField]
    protected int clipMaxSize = 100;
    protected int clipCurrentSize;

    [SerializeField]
    private float reloadingTime = 0.1f;
    protected bool isReloading = false;

    [SerializeField]
    private Vector3 gunLocalPosition = Vector3.zero;

    [SerializeField]
    private AudioClip reloadAudioClip = null;

    protected bool gunIsShooting = false;

    public override void Start()
    {
        base.Start();

        clipCurrentSize = clipMaxSize;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        gunIsShooting = timeSinceLastShot >= shotCooldown && !isReloading && BossController.GetInstance().CanShoot();
        
        if(gunIsShooting)
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        base.Shoot();

        timeSinceLastShot = 0;
        clipCurrentSize--;

        if (clipCurrentSize == 0)
            HandleReload();
    }

    protected virtual void HandleReload()
    {
        StartCoroutine(Reload());
    }

    protected IEnumerator Reload()
    {
        // Plays the reload clip
        audioSource.Stop();
        audioSource.clip = reloadAudioClip;
        audioSource.Play();

        isReloading = true;
        yield return new WaitForSeconds(reloadingTime);

        clipCurrentSize = clipMaxSize;
        isReloading = false;

        // Resets the default clip
        audioSource.clip = shootAudioClip;
        yield return null;
    }
}
