using UnityEngine;

public class Doors : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoors()
    {
        animator.SetTrigger("open");
    }
}
