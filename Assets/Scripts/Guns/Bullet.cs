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
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();

            // Damage player - caused by enemy shooting
            playerHealth?.Damage(damage);
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {

            if(collision.gameObject.CompareTag("Head"))
            {
                EnemyHead enemyHead = collision.gameObject.GetComponent<EnemyHead>();
                // Kill enemy with 1 shot
                enemyHead?.KillEnemy();
            } 
            else
            {
                Health enemyHealth = collision.gameObject.GetComponent<Health>();

                // Damage enemy - caused by player shooting
                enemyHealth?.Damage(damage);
            }
        }

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
