using UnityEngine;

public class PlayerControl : MonoBehaviour, IHealthController
{
    private bool isAiming = false;
    private bool inTransition = false;
    private bool isCovering = false;
    private bool isAlive = true;

    [SerializeField]
    private GameObject skeleton = null;
    [SerializeField]
    private Animator animator;

    public bool IsAiming() => isAiming;

    public void SetIsAiming(bool aiming)
    {
        isAiming = aiming;
        animator.SetBool("isAiming", aiming);
    }

    public void SetIsWalking(bool walking) => animator.SetBool("isWalking", walking);
    public void SetIsRunning(bool running) => animator.SetBool("isRunning", running);
    public void SetOppositeDir(float oppositeDir) => animator.SetFloat("oppositeDir", oppositeDir, 0.1f, Time.deltaTime);

    public void SetIsDead(bool dead)
    {
        animator.SetBool("isDead", dead);
        isAlive = false;
    }

    public bool IsAlive() => isAlive;

    public void RotateSkeleton(bool rotate) => skeleton.transform.rotation = Quaternion.Euler(0, rotate ? -90 : 90, 0);
    public void RotateSkeleton(Vector3 direction) => skeleton.transform.forward = direction;
    public void RotateSkeleton(float yRot) => skeleton.transform.rotation = Quaternion.Euler(0, yRot, 0);
    public Vector3 GetSkeletonDirection() => skeleton.transform.forward;

    public void SetInTransition(bool trans) => inTransition = trans;

    public bool IsInTransition() => inTransition;

    public void SetIsCovering(bool covering)
    {
        isCovering = covering;
        animator.SetBool("isCovering", covering);
    }

    public bool IsCovering() => isCovering;
    public bool IsInvisible() => isCovering && !isAiming;

    public void ResetPlayerMovements()
    {
        SetIsAiming(false);
        SetIsWalking(false);
    }

    public void SetIsDancing(bool dancing) => animator.SetBool("isDancing", dancing);
    public void SetIsRolling(bool rolling) => animator.SetBool("isRolling", rolling);

    public bool IsRolling() => animator.GetBool("isRolling");
}