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
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
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
        } 
        else
        {
            //TODO If the player isn't in the enemy FOV, the enemy continues its path
        }

        if (canWalk)
        {
            agent.SetDestination(enemyTargetPositions[currentTargetPosition]);

            // Face the target
            FaceTarget(Quaternion.Euler(agent.steeringTarget));

            if (agent.remainingDistance == 0)
            {
                canWalk = false;
                StartCoroutine(WaitOnPosition());
            }
        }
    }

    private IEnumerator WaitOnPosition()
    {
        yield return new WaitForSeconds(timeWaitingInEachPosition);

        currentTargetPosition = (currentTargetPosition + 1) % enemyTargetPositions.Count;

        canWalk = true;

        yield return null;
    }

    private void FaceTarget(Quaternion lookRotation)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
