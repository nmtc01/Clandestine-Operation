using UnityEngine;

public class PlayerControl : MonoBehaviour, IHealthController
{
    private bool isAiming = false;
    private bool inElevator = false;
    private bool isCovering = false;
    private bool isAlive = true;

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
        animator.SetBool("isWalking", walking);
    }

    public void SetOppositeDir(float oppositeDir)
    {
        animator.SetFloat("oppositeDir", oppositeDir, 0.1f, Time.deltaTime);
    }

    public void SetIsDead(bool dead)
    {
        animator.SetBool("isDead", dead);
        isAlive = false;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void RotateSkeleton(bool rotate) 
    {
        skeleton.transform.rotation = Quaternion.Euler(0, rotate ? -90 : 90, 0);
    }

    public void RotateSkeleton(Vector3 direction) 
    {
        skeleton.transform.forward = direction;
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

    public void SetIsCovering(bool covering)
    {
        isCovering = covering;
        animator.SetBool("isCovering", covering);
    }

    public bool IsCovering()
    {
        return isCovering;
    }

    public bool IsInvisible()
    {
        return isCovering && !isAiming;
    }

    public void SetIsFallingBack(bool falling)
    {
        isAlive = false;
        animator.SetBool("isFallingBack", falling);
        Player.GetInstanceHealth().Kill();
    }
}
