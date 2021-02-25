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

    protected bool canShoot = false;

    private Vector3 shootDirection;

    private void Start()
    {
        shootDirection = bulletSpawner.transform.forward;
    }

    public void Shoot()
    {
        GameObject instBullet = Instantiate(bullet, (Vector2)bulletSpawner.transform.position, Quaternion.Euler(bulletSpawner.transform.forward));
        instBullet.GetComponent<Rigidbody>().AddForce(shootDirection * shootForce, ForceMode.Impulse);
        instBullet.GetComponent<Bullet>().SetDamage(damage);
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
