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

        return false;
    }
}
