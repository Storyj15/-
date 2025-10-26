using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAniController : MonoBehaviour
{
    private Animator animator;
    public AnimatorOverrideController[] SkillAnimations;
    public AudioClip[] SkillClip;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        if (GameManager.gameManager_instance.PlayerJob != 3)
        {
            animator.runtimeAnimatorController = SkillAnimations[GameManager.gameManager_instance.PlayerJob];
        }
        else
        {
            if (PlayerUIManager.GetInstance().isFirstATK)
            {
                animator.runtimeAnimatorController = SkillAnimations[6];
            }
            else
            {
                animator.runtimeAnimatorController = SkillAnimations[7];
            }
        }
    }
    public void SkillSound(int skillindex)
    {
        GameManager.gameManager_instance.audioManager.PlayClip(2, SkillClip[skillindex], false);
    }
}
