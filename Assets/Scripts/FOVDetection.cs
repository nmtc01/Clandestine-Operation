using UnityEngine;

public class FOVDetection : MonoBehaviour
{
    public static bool InFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        // Target is in FOV if isn't more than maxRadius from the checking object
        if ((target.position - checkingObject.position).magnitude > maxRadius) 
        {
            return false;
        }

        Vector3 targetDirection = (target.position - checkingObject.position).normalized;

        float angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(targetDirection, checkingObject.forward));
        
        if(Mathf.Abs(angle) <= maxAngle) // Target is in FOV
        {
            return true;
        }

        /*Collider[] overlaps = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);
        
        for(int i = 0; i < count; i++)
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
        }*/

        return false;
    }
}
