using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IHealthController
{
    [SerializeField]
    private float lookRadius = 10f;

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

    #region FOV Variables

    [SerializeField]
    private float maxAngle = 45f;
    [SerializeField]
    private float maxRadius = 20f;
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

    
    [SerializeField]
    private TimerCountDown timer;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timer = GetComponent<TimerCountDown>();
        SetAgentNewDestination();
        shootingBehaviour = AimAndShoot();
    }

    // Update is called once per frame
    void Update()
    {
        // Walk Animation
        SetIsWalking(canWalk);
        
        if(FOVDetection.InFOV(transform, Player.GetInstance().transform, maxAngle, maxRadius))
        {
            // Starts timer countdown
            timer.StartCounting();

            // Stopping agent from moving
            canWalk = false;
            agent.ResetPath();

            // Enemy looks to the player
            SetIsAiming(true);
            Vector3 direction = (Player.GetInstance().transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
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
        animator.SetBool("isAiming", aiming);
    }

    public void SetIsDead(bool dead)
    {
        if (dead) DestroyEnemyPhysics();
        animator.SetBool("isDead", dead);
    }
    #endregion

    private void DestroyEnemyPhysics()
    {
        GetComponent<Collider>().enabled = false;
        headCollider.enabled = false;
        enabled = false;
    }
}
