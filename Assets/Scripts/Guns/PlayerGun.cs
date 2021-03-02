using System.Collections;
using UnityEngine;

public class PlayerGun : Gun
{
    [SerializeField]
    private float shotCooldown = .15f;
    private float timeSinceLastShot = 0f;

    [SerializeField]
    private int clipMaxSize = 10;
    private int clipCurrentSize;

    [SerializeField]
    private float reloadingTime = 1f;
    private bool isReloading = false;

    public override void Start()
    {
        base.Start();

        clipCurrentSize = clipMaxSize;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (canShoot && timeSinceLastShot >= shotCooldown && !isReloading && Input.GetButtonDown("Fire"))
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        base.Shoot();

        timeSinceLastShot = 0;
        clipCurrentSize--;

        if (clipCurrentSize == 0) StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadingTime);

        clipCurrentSize = clipMaxSize;
        isReloading = false;
        yield return null;
    }
}
