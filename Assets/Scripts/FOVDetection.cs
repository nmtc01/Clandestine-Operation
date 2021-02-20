using UnityEngine;

public class FOVDetection : MonoBehaviour
{
    #region EDITOR FOV TEST
#if UNITY_EDITOR
    [SerializeField]
    private Transform player = null;

    [SerializeField]
    private float maxAngle = 45f;

    [SerializeField]
    private float maxRadius = 5f;

    private bool isInFov = false;

    private void OnDrawGizmos()
    {
        if (isInFov)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);
    }

    private void Update()
    {
        isInFov = InFOV(transform, player.transform, maxAngle, maxRadius);
    }
#endif
    #endregion

    public static bool InFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        Collider[] overlaps = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);
        
        for(int i = 0; i < count + 1; i++)
        {
            if(overlaps[i] != null && overlaps[i].transform == target)
            {
                Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                directionBetween.y *= 0;

                float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                if(angle <= maxAngle) //View angle
                {
                    Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                    RaycastHit hit;

                    if(Physics.Raycast(ray, out hit, maxRadius) && hit.transform == target)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
