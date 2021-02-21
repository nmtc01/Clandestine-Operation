using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
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

    #region FOV Variables

    [SerializeField]
    private Transform playerTransform = null;
    [SerializeField]
    private float maxAngle = 45f;
    [SerializeField]
    private float maxRadius = 20f;
    private bool wasInFOV = false;
    #endregion
    private Animator animator;
    [SerializeField]
    private float stoppingDistanceError = .5f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetAgentNewDestination();
    }

    // Update is called once per frame
    void Update()
    {
        // Walk Animation
        SetIsWalking(canWalk);
        
        if(FOVDetection.InFOV(transform, playerTransform, maxAngle, maxRadius))
        {
            // Stopping agent from moving
            canWalk = false;
            StopAllCoroutines();
            agent.ResetPath();

            // Enemy looks to the player
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            FaceTarget(lookRotation);

            wasInFOV = true;
        } 
        else if(wasInFOV)
        {
            //If the player isn't in the enemy FOV, the enemy continues its path
            canWalk = true;
            wasInFOV = false;
            SetAgentNewDestination();
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

    public void SetIsWalking(bool walking)
    {
        animator.SetBool("isWalking", walking);
    }
}
