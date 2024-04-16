using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetMovementAnimationTrigger()
    {
        anim.SetTrigger("movementAnimationTrigger");
    }

    public void SetAttackAnimation(bool isAttacking)
    {
        anim.SetBool("attackPlayer", isAttacking);
    }

        public void SetIdleAnimation(bool cantSee)
    {
        Debug.Log(anim.GetBool("CantSee"));
        anim.SetBool("CantSee", cantSee);
                Debug.Log(anim.GetBool("CantSee"));

    }

    public void SetZeroHealthAnimation()
    {
        anim.SetBool("0Health", true);
    }
}
