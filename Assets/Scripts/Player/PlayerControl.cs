using UnityEngine;

public class PlayerControl : MonoBehaviour, IHealthController
{
    private bool isAiming = false;
    private bool isWalking = false;
    private bool inElevator = false;
    private float oppositeDir = 0f;
    [SerializeField]
    private GameObject skeleton = null;
    [SerializeField]
    private Animator animator;
    
    void Update()
    {
        if (TimerCountDown.IsFinished())
            SetIsDead(true);
    }

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

    public void SetIsDead(bool dead)
    {
        animator.SetBool("isDead", dead);
    }

    public void RotateSkeleton(bool rotate) 
    {
        skeleton.transform.rotation = Quaternion.Euler(0, rotate ? -90 : 90, 0);
    }

    public void RotateSkeleton(float yRot)
    {
        skeleton.transform.rotation = Quaternion.Euler(0, yRot, 0);
    }

    public Vector3 getSkeletonDirection()
    {
        return skeleton.transform.forward;
    }

    public void SetInElevator(bool elev)
    {
        inElevator = elev;
    }

    public bool IsInElevator()
    {
        return inElevator;
    }
}
