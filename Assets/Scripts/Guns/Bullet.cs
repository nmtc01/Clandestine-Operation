using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float timeBeforeDestroying = 5f;

    private float damage = 0f;

    private void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    public void SetDamage(float gunBulletDamage)
    {
        damage = gunBulletDamage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Damage player or enemy

        // Destroy Bullet
        Destroy(gameObject);
    }

    private IEnumerator DestroyBullet()
    {
        // If the bullet doesn't collide with any object it will be destroyed after some time
        yield return new WaitForSeconds(timeBeforeDestroying);

        Destroy(gameObject);
    }
}
