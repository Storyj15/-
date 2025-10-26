using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public static bool isMove;
    public static bool playerdie;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        playerdie = false;
        isMove = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            isMove = false;
            animator.SetBool("Move", true);
        }
        if (playerdie)
        {
            playerdie = false;
            animator.SetBool("Die",true);
            Invoke(nameof(PlayerDie), 0.4f);
        }
    }
    public void MoveStop()
    {
        animator.SetBool("Move", false);
    }
    public void PlayerDie()
    {
        animator.SetBool("Die", false);
        gameObject.SetActive(false);
    }
}
