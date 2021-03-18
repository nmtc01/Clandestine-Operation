using System.Collections;
using UnityEngine;

public class BossGun : Gun
{
    protected float timeSinceLastShot = 0f;

    [SerializeField]
    protected int clipMaxSize = 25;
    protected int clipCurrentSize;

    [SerializeField]
    private float reloadingTime = 5f;
    protected bool isReloading = false;

    [SerializeField]
    private Vector3 gunLocalPosition = Vector3.zero;

    [SerializeField]
    private AudioClip reloadAudioClip = null;

    private bool firstTimeShooting = true;

    public override void Start()
    {
        base.Start();

        clipCurrentSize = clipMaxSize;
    }

    public override void Shoot()
    {
        if (isReloading) return;
        if (firstTimeShooting) 
        {
            audioSource.Play();
            firstTimeShooting = false;
        }
        base.ShootBullet();

        timeSinceLastShot = 0;
        clipCurrentSize--;

        if (clipCurrentSize == 0)
            HandleReload();
    }

    protected void HandleReload()
    {
        StartCoroutine(Reload());
    }

    protected IEnumerator Reload()
    {
        isReloading = true;

        // Plays the reload clip
        ReloadAudio();

        yield return new WaitForSeconds(reloadingTime);

        clipCurrentSize = clipMaxSize;
        isReloading = false;

        DefaultAudio();
        firstTimeShooting = true;
        yield return null;
    }

    private void ReloadAudio()
    {
        // Plays the reload clip
        StopAudio();
        audioSource.clip = reloadAudioClip;
        audioSource.loop = false;
        PlayAudio();
    }

    public void DefaultAudio()
    {
        // Resets the default clip
        StopAudio();
        audioSource.clip = shootAudioClip;
        audioSource.loop = true;
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }

    public void PlayAudio()
    {
        audioSource.Play();
    }
}
