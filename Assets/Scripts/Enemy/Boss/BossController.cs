using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour, IHealthController, IEnemyController
{
    private static BossController instance = null;
    private Animator animator;
    private bool canShoot = false;

    [SerializeField]
    private BossGun gun = null;
    private bool wasInFOV = false;
    private IEnumerator shootingBehaviour = null;
    [SerializeField]
    private float timeToShoot = 1f, timeBetweenShots = 0.1f;
    [SerializeField]
    private GameObject healthUI = null;
    private bool isAlive = true;
    [SerializeField]
    private CameraFollowBoss secondaryCamera = null;
    [SerializeField]
    private BossSpeech speech = null;
    private bool secondaryCameraActive = false;
    private bool firstTimeTurn = true;
    private bool firstTimeGrabbingGun = true;
    private Health health;

    #region party
    [SerializeField]
    private GameObject cage = null;
    [SerializeField]
    private int endSpot = 75;
    [SerializeField]
    private Friend[] friends;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        shootingBehaviour = AimAndShoot();

        health = GetComponent<Health>();
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if (canShoot && isAlive)
        {
            // Enemy looks to the player
            Vector3 direction = (Player.GetArmatureTransform().position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(-1*direction.x, direction.y, -1*direction.z));
            FaceTarget(lookRotation);

            gun.SetShootingDirection(direction);
            if (!wasInFOV)
            {
                StopAllCoroutines();
                StartCoroutine(shootingBehaviour);
            }
            wasInFOV = true;
        }

        if (secondaryCamera.IsInRange() && secondaryCameraActive)
        {
            speech.ActivateDialogManager();
            if (firstTimeTurn && speech.EndedFirstSentence())
            {
                firstTimeTurn = false;
                Turn();
            }

            if (Input.GetButton("Return"))
            {
                DeactivateBossCamera();
                speech.DeactivateDialogManager();
                if (firstTimeTurn)
                {
                    firstTimeTurn = false;
                    Turn();
                } 
                if (isAlive) ShowUI();
                else HandleParty();
            }
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

    private void Turn()
    {
        animator.SetTrigger("turn_right");
    }

    private void ShowUI()
    {
        healthUI.SetActive(true);
    }

    public void GrabGun(bool grabbing)
    {
        animator.SetBool("grabbing_gun", grabbing);
        if (firstTimeGrabbingGun)
        {
            firstTimeGrabbingGun = false;
            gun.transform.SetParent(transform);
        }
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
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    public void SetIsDead(bool dead)
    {
        isAlive = !dead;
        animator.SetBool("is_dead", dead);
        if (dead)
        {
            canShoot = false;
            DestroyBossPhysics();
            Player.GetInstanceControl().SetIsCovering(false);
        }
    }

    public void DamageEnemy(float damage)
    {
        // Damage boss - caused by player shooting
        health.Damage(damage);
    }

    public void ActivateBossCamera()
    {
        GrabGun(false);
        StopAllCoroutines();
        gun.StopAudio();
        secondaryCameraActive = true;
        secondaryCamera.ActivateCamera();
    }

    public void DeactivateBossCamera()
    {
        secondaryCamera.DeactivateCamera();
        secondaryCameraActive = false;
        GrabGun(true);
        wasInFOV = false;
        if (isAlive)
        {
            gun.PlayAudio();
        }
    }

    private void DestroyBossPhysics()
    {
        GetComponent<Collider>().enabled = false;
        StopAllCoroutines();
    }

    private void HandleParty()
    {
        if (cage) cage.transform.Rotate(new Vector3(0,0,180));
        GameObject player = Player.GetInstance();
        player.transform.position = new Vector3(endSpot, player.transform.position.y, player.transform.position.z);
        GameManager.DestroyObject();
        GameWin.ShowGameWinScreen();
        
        for (int i = 0; i < friends.Length; i++)
        {
            friends[i].StartParty();
        }
        Player.GetInstanceControl().SetIsDancing(true);
    }
}
