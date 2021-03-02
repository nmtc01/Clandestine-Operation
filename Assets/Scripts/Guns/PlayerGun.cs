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

    public override void Start()
    {
        base.Start();

        clipCurrentSize = clipMaxSize;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (canShoot && timeSinceLastShot >= shotCooldown && !IsReloading() && Input.GetButtonDown("Fire"))
        {
            Shoot();
        }
    }

    private bool IsReloading()
    {
        return clipCurrentSize <= 0;
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
        clipCurrentSize = clipMaxSize;
        Debug.Log("Reloading");
        yield return null;
    }
}
