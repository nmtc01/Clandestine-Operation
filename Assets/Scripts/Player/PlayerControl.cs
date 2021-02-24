using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private bool isAiming = false;
    private bool isWalking = false;
    private float oppositeDir = 0f;
    [SerializeField]
    private GameObject skeleton = null;
    [SerializeField]
    private Animator animator;

    public bool IsAiming()
    {
        return isAiming;
    }

    public void SetIsAiming(bool aiming)
    {
        isAiming = aiming;
        animator.SetBool("isAiming", aiming);
    }

    public void SetIsWalking(bool walking)
    {
        isWalking = walking;
        animator.SetBool("isWalking", walking);
    }

    public void SetOppositeDir(float oppositeDir)
    {
        this.oppositeDir = oppositeDir;
        animator.SetFloat("oppositeDir", oppositeDir, 0.1f, Time.deltaTime);
    }

    public void RotateSkeleton(bool rotate) 
    {
        skeleton.transform.rotation = Quaternion.Euler(0, rotate ? -90 : 90, 0);
    }

    public Vector3 getSkeletonDirection()
    {
        return skeleton.transform.forward;
    }
}
