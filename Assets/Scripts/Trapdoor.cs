using System.Collections;
using UnityEngine;

public class Trapdoor : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(TrapdoorAnimation());
    }

    private IEnumerator TrapdoorAnimation()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(4f, 5.5f));
            animator.SetTrigger("open");
            yield return new WaitForSeconds(.5f);
            animator.SetTrigger("close");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<Health>().Kill();
        }
    }

}
