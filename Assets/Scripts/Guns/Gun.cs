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

    // Update is called once per frame
    void Update()
    {
        if(canShoot)
        {
            /*Draw ray? */

            if (Input.GetButtonDown("Fire"))
            {
                Debug.Log("Fire");

                Vector3 shootDirection = bulletSpawner.transform.forward;

                RaycastHit hit;
                if (Physics.Raycast(bulletSpawner.transform.position, bulletSpawner.transform.forward, out hit, range))
                {
                    /* 
                     * TODO 
                     *  Change Raycast to BoxCast and detect collisions with objects even if they are not on the same z coordinate
                     *      Physics.BoxCast(
                     *          bulletSpawner.transform.position, 
                     *          new Vector3(.1f, .1f, 10f), 
                     *          bulletSpawner.transform.forward, 
                     *          out hit
                     *      )
                     */
                    shootDirection = (hit.point - bulletSpawner.transform.position).normalized;
                }

                Shoot(shootDirection);
            }
        }
    }

    private void Shoot(Vector3 dir)
    {
        GameObject instBullet = Instantiate(bullet, bulletSpawner.transform.position, bulletSpawner.transform.rotation);
        instBullet.GetComponent<Rigidbody>().AddForce(dir * shootForce, ForceMode.Impulse);
    }

    public void SetCanShoot(bool shoot)
    {
        canShoot = shoot;
    }

    public Transform GetBulletSpawnerTransform()
    {
        return bulletSpawner.transform;
    }
}
