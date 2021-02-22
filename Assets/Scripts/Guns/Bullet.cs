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
            // Damage player - caused by enemy shooting
            Debug.Log("Damage Player");
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(collision.gameObject.CompareTag("Head"))
            {
                // Kill enemy with 1 shot
                Debug.Log("Kill Enemy");
            } 
            else
            {
                // Damage enemy - caused by player shooting
                Debug.Log("Damage Enemy");
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
