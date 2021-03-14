using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private static BossController instance = null;
    private Animator animator;
    private bool canShoot = false;

    [SerializeField]
    private Gun gun = null;
    private bool wasInFOV = false;
    private IEnumerator shootingBehaviour = null;
    [SerializeField]
    private float timeToShoot = 1f, timeToReload = 2f;
    [SerializeField]
    private GameObject healthUI = null;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        shootingBehaviour = AimAndShoot();
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if (canShoot)
        {
            // Enemy looks to the player
            Vector3 direction = (Player.GetInstance().transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
            FaceTarget(lookRotation);

            gun.SetShootingDirection(direction);
            if (!wasInFOV)
            {
                StopAllCoroutines();
                StartCoroutine(shootingBehaviour);
            }
            wasInFOV = true;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public static BossController GetInstance()
    {
        return instance;
    }

    public void Turn()
    {
        animator.SetTrigger("turn_right");
    }

    public void GrabGun(bool grabbing)
    {
        animator.SetBool("grabbing_gun", grabbing);
        healthUI.SetActive(true);
        canShoot = grabbing;
    }

    public bool CanShoot()
    {
        return canShoot;
    }

    private void FaceTarget(Quaternion lookRotation)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
    }

    private IEnumerator AimAndShoot()
    {
        yield return new WaitForSeconds(timeToShoot);

        while(true)
        {
            gun.Shoot();
            yield return new WaitForSeconds(timeToReload);
        }
    }
}
