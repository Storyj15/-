using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public static bool ActionPlay;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        ActionPlay = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ActionPlay)
        {
            animator.SetBool("TrapAction", true);
        }
        else
        {
            animator.SetBool("TrapAction", false);
        }
    }
}
