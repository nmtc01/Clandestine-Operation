using UnityEngine;

public class Doors : MonoBehaviour
{
    private Animator animator;
    private bool closed = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoors()
    {
        if(closed)
        {
            GetComponent<AudioSource>().Play();
            closed = false;
        }
        animator.SetTrigger("open");
    }
}
