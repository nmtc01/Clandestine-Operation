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
    private LineRenderer lr = new LineRenderer();
    [SerializeField]
    private GameObject bulletSpawner = null;

    private void Start()
    {
        lr.SetPosition(0, bulletSpawner.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Aim"))
        {
            if (Input.GetButtonDown("Fire"))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, range))
                {
                    lr.SetPosition(1, hit.point);
                } 
                else
                {
                    //DO SOMETHING
                }

                Shoot();
            }
        }
    }

    private void Shoot()
    {
    }
}
