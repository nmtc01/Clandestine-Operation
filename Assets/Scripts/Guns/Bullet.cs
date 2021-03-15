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

    public void SetLayer(string ownerName)
    {
        gameObject.layer = LayerMask.NameToLayer(ownerName + "Bullet");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Damage player - caused by enemy shooting
            Player.GetInstanceHealth()?.Damage(damage);

            Score.IncreaseScore(ScoreValues.playerShot);
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Armour armour = collision.gameObject.GetComponent<Armour>();
            
            if (armour != null && armour.IsActive())
            {
                // Damage armour - caused by player shooting
                armour?.Damage(damage);

                Score.IncreaseScore(ScoreValues.armouredEnemyShot);
            }
            else if(collision.gameObject.CompareTag("Head"))
            {
                EnemyHead enemyHead = collision.gameObject.GetComponent<EnemyHead>();

                // Kill enemy with 1 shot
                enemyHead?.KillEnemy();

                Score.IncreaseScore(ScoreValues.enemyHeadshot);
            } 
            else if(collision.gameObject.CompareTag("HeadBoss"))
            {
                BossHead enemyHead = collision.gameObject.GetComponent<BossHead>();
                
                // Damage enemy - caused by player shooting
                enemyHead?.DamageHead(damage*2);

                Score.IncreaseScore(ScoreValues.enemyShot);
            } 
            else
            {
                Health enemyHealth = collision.gameObject.GetComponent<Health>();

                // Damage enemy - caused by player shooting
                enemyHealth?.Damage(damage);

                Score.IncreaseScore(ScoreValues.enemyShot);
            }
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Cover"))
        {
            Health coverHealth = collision.gameObject.GetComponent<Health>();

            // Damage cover - caused by enemy shooting
            coverHealth?.Damage(damage);
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
