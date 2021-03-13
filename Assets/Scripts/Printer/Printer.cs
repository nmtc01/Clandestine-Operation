using System.Collections;
using UnityEngine;

public class Printer : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Health health = Player.GetInstance().GetComponent<Health>();
            health.Kill();
            StopAllCoroutines();
            animator.SetFloat("stop", 0f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {       
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5.0f));
            audioSource.Play();
            animator.SetTrigger("launch");
        }
    }
}
