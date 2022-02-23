using UnityEngine;
using UnityEngine.VFX;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    private float shootForce = 30f;
    [SerializeField]
    private GameObject bulletSpawner = null;

    [SerializeField]
    private GameObject bullet = null;

    private Vector3 shootDirection;

    [SerializeField]
    protected AudioClip shootAudioClip = null;
    protected AudioSource audioSource;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem shootEffect = null;

    public virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = shootAudioClip;

        shootDirection = bulletSpawner.transform.forward;
    }

    public virtual void Shoot()
    {
        //audioSource.Stop();
        audioSource.Play(); // Play shoot sound

        shootEffect?.Play();

        ShootBullet();
    }

    public virtual void ShootBullet()
    {
        GameObject instBullet = Instantiate(bullet, bulletSpawner.transform.position, Quaternion.Euler(bulletSpawner.transform.forward));
        instBullet.GetComponent<Rigidbody>().AddForce(shootDirection * shootForce, ForceMode.Impulse);

        Bullet blt = instBullet.GetComponent<Bullet>();
        blt.SetDamage(damage);
        blt.SetLayer(LayerMask.LayerToName(gameObject.layer));
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
