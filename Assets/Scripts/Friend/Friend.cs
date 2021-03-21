using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public void StartParty()
    {
        transform.position = new Vector3(transform.position.x * 1.1f, transform.position.y, transform.position.z);
        animator.SetBool("isDancing", true);
    }
}
