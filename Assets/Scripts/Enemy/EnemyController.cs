using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float lookRadius = 10f;

    [SerializeField]
    private GameObject player = null;

    [SerializeField]
    private List<Vector3> enemyTargetPositions = new List<Vector3>();
    private int currentTargetPosition = 0;
    [SerializeField]
    private float timeWaitingInEachPosition = 5f;
    private bool canWalk = true;

    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canWalk)
        {
            agent.SetDestination(enemyTargetPositions[currentTargetPosition]);
            Debug.Log("Set Destination: " + currentTargetPosition);
            // Face the target
            FaceTarget();

            if (agent.remainingDistance == 0)
            {
                Debug.Log("Can't walk.");
                canWalk = false;
                StartCoroutine(WaitOnPosition());
            }
        }
    }

    private IEnumerator WaitOnPosition()
    {
        yield return new WaitForSeconds(timeWaitingInEachPosition);

        currentTargetPosition = (currentTargetPosition + 1) % enemyTargetPositions.Count;

        Debug.Log("New Target: " + currentTargetPosition);

        canWalk = true;

        yield return null;
    }

    private void FaceTarget()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(agent.steeringTarget), Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
