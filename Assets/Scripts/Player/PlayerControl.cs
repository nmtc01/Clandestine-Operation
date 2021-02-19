using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private bool isAiming = false;

    public bool IsAiming()
    {
        return isAiming;
    }

    public void SetIsAiming(bool aiming)
    {
        isAiming = aiming;
    }
}
