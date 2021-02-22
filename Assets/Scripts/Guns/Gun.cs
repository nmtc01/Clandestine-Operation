using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    private float range = 100f;
    [SerializeField]
    private float shootForce = 30f;
    [SerializeField]
    private GameObject bulletSpawner = null;

    [SerializeField]
    private GameObject bullet = null;

    private bool canShoot = false;

    private Vector3 shootDirection;

    private void Start()
    {
        shootDirection = bulletSpawner.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if(canShoot && Input.GetButtonDown("Fire"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject instBullet = Instantiate(bullet, bulletSpawner.transform.position, Quaternion.Euler(bulletSpawner.transform.forward));
        instBullet.GetComponent<Rigidbody>().AddForce(shootDirection * shootForce, ForceMode.Impulse);
    }

    public void SetCanShoot(bool shoot)
    {
        canShoot = shoot;
    }

    public void SetShootingDirection(Vector3 direction)
    {
        shootDirection = direction;
    }

    public Transform GetBulletSpawnerTransform()
    {
        return bulletSpawner.transform;
    }
}
