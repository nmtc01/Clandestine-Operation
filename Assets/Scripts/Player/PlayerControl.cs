using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private bool isAiming = false;
    [SerializeField]
    private GameObject skeleton = null;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
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

    public void RotateSkeleton(bool rotate) 
    {
        skeleton.transform.rotation = Quaternion.Euler(0, rotate ? -90 : 90, 0);
    }

    public Vector3 getSkeletonDirection()
    {
        return skeleton.transform.forward;
    }
}
