using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IHealthController, IEnemyController
{
    #region Walking Variables
    [SerializeField]
    private List<Vector3> enemyTargetPositions = new List<Vector3>();
    private int currentTargetPosition = 0;
    [SerializeField]
    private float timeWaitingInEachPosition = 5f;
    private bool canWalk = true;
    #endregion

    private NavMeshAgent agent;

    [SerializeField]
    private BoxCollider headCollider = null;
    private bool isAiming = false;

    #region FOV Variables

    [SerializeField]
    private float maxAngle = 45f;
    [SerializeField]
    private float lookRadius = 15f;
    private bool wasInFOV = false;
    #endregion
    private Animator animator;
    [SerializeField]
    private float stoppingDistanceError = .5f;

    #region Aim and Shoot Variables
    [SerializeField]
    private Gun gun = null;
    private IEnumerator shootingBehaviour = null;
    [SerializeField]
    private float timeToShoot = 1f, timeBetweenShots = 1.5f;
    #endregion

    #region Enemy Alert 
    private bool alertedBefore = false;
    private AudioSource audioSource;
    #endregion


    private EnemyHealth health;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetAgentNewDestination();
        shootingBehaviour = AimAndShoot();

        audioSource = GetComponent<AudioSource>();

        health = GetComponent<EnemyHealth>();
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        // Walk Animation
        SetIsWalking(canWalk);
        
        if((!Player.GetInstanceControl().IsInvisible() && FOVDetection.InFOV(transform, Player.GetArmatureTransform(), maxAngle, lookRadius)) || alertedBefore)
        {
            AlertEnemy();

            // Stopping agent from moving
            canWalk = false;
            agent.ResetPath();

            // Enemy looks to the player
            SetIsAiming(true);
            Vector3 direction = (Player.GetArmatureTransform().position - transform.position).normalized;
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
        else if(wasInFOV)
        {
            //If the player isn't in the enemy FOV, the enemy continues its path
            canWalk = true;
            wasInFOV = false;
            SetIsAiming(false);
            SetAgentNewDestination();
            StopCoroutine(shootingBehaviour);
        }

        if (canWalk && Vector3.Distance(enemyTargetPositions[currentTargetPosition], transform.position) <= stoppingDistanceError && agent.remainingDistance == 0)
        {
            canWalk = false;
            StartCoroutine(WaitOnPosition());
        }
    }

    private void Alert()
    {
        TimerCountDown.IncrementEnemiesAlerted();
        alertedBefore = true;
        audioSource.Play();
    }

    public void AlertEnemy()
    {
        if (!alertedBefore && !isDead)
        {
            Alert();
        }
    }

    private IEnumerator WaitOnPosition()
    {
        yield return new WaitForSeconds(timeWaitingInEachPosition);

        currentTargetPosition = (currentTargetPosition + 1) % enemyTargetPositions.Count;

        canWalk = true;
        
        SetAgentNewDestination();

        yield return null;
    }

    private void SetAgentNewDestination()
    {
        agent.SetDestination(enemyTargetPositions[currentTargetPosition]);

        // Face the target
        FaceTarget(Quaternion.Euler(agent.steeringTarget));
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

    #region Animator Related Functions
    public void SetIsWalking(bool walking)
    {
        animator.SetBool("isWalking", walking);
    }

    public void SetIsAiming(bool aiming)
    {
        isAiming = aiming;
        animator.SetBool("isAiming", aiming);
    }

    public bool IsAiming()
    {
        return isAiming;
    }

    public void SetIsDead(bool dead)
    {
        isDead = dead;
        if (dead) 
        {
            if (alertedBefore) TimerCountDown.DecrementEnemiesAlerted();
            DestroyEnemyPhysics();

            Score.IncreaseScore(ScoreValues.enemyKill);
        }
        animator.SetBool("isDead", dead);
    }

    public void SetKillPlayer(bool kill)
    {
        animator.SetBool("foundPlayer", kill);
    }
    #endregion

    private void DestroyEnemyPhysics()
    {
        GetComponent<Collider>().enabled = false;
        StopAllCoroutines();
        agent.enabled = false;
        headCollider.enabled = false;
        enabled = false;
    }

    public void DamageEnemy(float damage)
    {
        // Damage enemy - caused by player shooting
        health.Damage(damage);

        health.ShowUI();

        AlertEnemy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetInstanceID() == Player.GetInstance().GetInstanceID())
        {
            AlertEnemy();
        }
    }
}
