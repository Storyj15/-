using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CoordinatesEffectAni : MonoBehaviour
{
    public static bool EnemyPassiveDeBuff;
    public static bool isBuffSkill;
    public static int HitAniIndex;
    public static int BuffAniIndex;
    public int[] Coordinates = new int[2];
    public AnimatorOverrideController[] PlayerHitAnimation;
    public AnimatorOverrideController[] EnemyHitAnimation;
    public AnimatorOverrideController[] BuffAnimation;
    public AudioClip[] HitSound;

    private Image image;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        EnemyPassiveDeBuff = false;
        isBuffSkill = false;
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        image.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (LocationManager.showEnemyPassiveSkillZone)
        {
            var enemyPassiveSkillZone = EnemyManager.GetInstance().EnemyATKZone.Any(arr => arr.GetType() == Coordinates.GetType() && arr.SequenceEqual(Coordinates));
            if (enemyPassiveSkillZone)
            {
                if (EnemyPassiveDeBuff)
                {
                    animator.runtimeAnimatorController = BuffAnimation[1];
                }
                else
                {
                    animator.runtimeAnimatorController = BuffAnimation[0];
                }
                animator.SetBool("Hit", true);
                image.color = new Color32(255, 255, 255, 255);
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }
        }
        if (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.AttackResult)
        {
            if (!LocationManager.showPlayerATKZone && !LocationManager.showPlayerSkillZone && !LocationManager.showEnemyATKZone)
            {
                image.enabled = false;
            }
            else
            {
                if (LocationManager.showPlayerATKZone)
                {
                    var playerATKZone = PlayerUIManager.GetInstance().PlayerData.ATKHitZone.Any(arr => arr.GetType() == Coordinates.GetType() && arr.SequenceEqual(Coordinates));
                    if (playerATKZone)
                    {
                        animator.runtimeAnimatorController = PlayerHitAnimation[HitAniIndex];
                        animator.SetBool("Hit", true);
                        image.color = new Color32(255, 255, 255, 255);
                        image.enabled = true;
                    }
                    else
                    {
                        image.enabled = false;
                    }
                }
                if (LocationManager.showPlayerSkillZone)
                {
                    var playerSkillZone = PlayerUIManager.GetInstance().PlayerData.SkillZones.Any(arr => arr.GetType() == Coordinates.GetType() && arr.SequenceEqual(Coordinates));
                    if (playerSkillZone)
                    {
                        if (isBuffSkill)
                        {
                            animator.runtimeAnimatorController = BuffAnimation[0];
                        }
                        else
                        {
                            animator.runtimeAnimatorController = PlayerHitAnimation[0];
                        }
                        animator.SetBool("Hit", true);
                        image.color = new Color32(255, 255, 255, 255);
                        image.enabled = true;
                    }
                    else
                    {
                        image.enabled = false;
                    }
                }
                if (LocationManager.showEnemyATKZone)
                {
                    var enemyATKZone = EnemyManager.GetInstance().EnemyATKZone.Any(arr => arr.GetType() == Coordinates.GetType() && arr.SequenceEqual(Coordinates));
                    if (enemyATKZone)
                    {
                        animator.runtimeAnimatorController = EnemyHitAnimation[HitAniIndex];
                        animator.SetBool("Hit", true);
                        image.color = new Color32(255, 255, 255, 255);
                        image.enabled = true;
                    }
                    else
                    {
                        image.enabled = false;
                    }
                }
            }
        }
    }
    public void HitAnimationEnd()
    {
        LocationManager.showPlayerATKZone = false;
        LocationManager.showPlayerSkillZone = false;
        LocationManager.showEnemyATKZone = false;
        LocationManager.showEnemyPassiveSkillZone = false;
        animator.SetBool("Hit", false);
    }
    public void HitSoundPlay(int hitindex)
    {
        GameManager.gameManager_instance.audioManager.PlayClip(2, HitSound[hitindex], false);
    }
}
