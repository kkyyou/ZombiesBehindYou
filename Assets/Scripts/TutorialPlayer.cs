﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : MonoBehaviour
{
    private Animator animator;
    private bool isRight = true; // 히어로 보는 방향 오른쪽 체크.
    private TutoRightTargetZone rightTargetZone; // 히어로 오른쪽 공격 범위.
    private TutoLeftTargetZone leftTargetZone;   // 히어로 왼쪽 공격 범위.
    public LayerMask zombieLayerMask;
    public TutorialEnemyManager tutoEnemyManager;
    public TutorialManager tutoManager;

    // Start is called before the first frame update
    void Start()
    {
        rightTargetZone = FindObjectOfType<TutoRightTargetZone>();
        leftTargetZone = FindObjectOfType<TutoLeftTargetZone>();

        animator = transform.Find("Character").GetComponent<Animator>();
    }

    public void TurnButton()
    {
        isRight = !isRight;
        Flip();

        animator.SetTrigger("AttackTrigger");

        AudioManager.instance.PlayRandomAttackSound();

        // 플래그 변경하여 Zone에서 Zombie가 날아가게 한다.
        if (isRight)
        {
            Vector2 playerVector = transform.position;
            playerVector = new Vector2(playerVector.x + 1, playerVector.y);
            Collider2D collision = CheckCollision(playerVector);

            if (collision)
            {
                rightTargetZone.ThrowZombie(collision);
         
            }

            tutoEnemyManager.GoNextTurn();
        }
        else
        {
            Vector2 playerVector = transform.position;
            playerVector = new Vector2(playerVector.x - 1, playerVector.y);
            Collider2D collision = CheckCollision(playerVector);

            if (collision)
            {
                leftTargetZone.ThrowZombie(collision);
            }

            tutoEnemyManager.GoNextTurn();
        }

        tutoManager.SetTutoStep(tutoManager.GetTutoStep() + 1);
        tutoManager.nextTuto = true;

        StartCoroutine(AttackCoroutine());
    }

    public void AttackButton()
    {
        animator.SetTrigger("AttackTrigger");

        // Attack Sound 랜덤 재생.
        AudioManager.instance.PlayRandomAttackSound();

        Collider2D collision;
        Vector2 playerVector = transform.position;

        if (isRight)
        {
            playerVector = new Vector2(playerVector.x + 1, playerVector.y);
        }
        else
        {
            playerVector = new Vector2(playerVector.x - 1, playerVector.y);
        }

        collision = CheckCollision(playerVector);

        if (collision)
        {
            if (isRight)
                rightTargetZone.ThrowZombie(collision);
            else
                leftTargetZone.ThrowZombie(collision);
        }

        tutoEnemyManager.GoNextTurn();

        tutoManager.SetTutoStep(tutoManager.GetTutoStep() + 1);
        tutoManager.nextTuto = true;

        StartCoroutine(AttackCoroutine());


    }

    public void LeftRightAttackButton()
    {
        animator.SetTrigger("LRAttackTrigger");

        AudioManager.instance.PlayRandomAttackSound();

        // Right
        Vector2 vector1 = transform.position;
        vector1 = new Vector2(vector1.x + 1, vector1.y);
        Collider2D collision1 = CheckCollision(vector1);

        if (collision1)
        {
            rightTargetZone.ThrowZombie(collision1);
        }

        // Left
        Vector2 vector2 = transform.position;
        vector2 = new Vector2(vector2.x - 1, vector2.y);
        Collider2D collision2 = CheckCollision(vector2);

        if (collision2)
        {
            leftTargetZone.ThrowZombie(collision2);
        }

        // 점수 및 HP 업데이트.
        if (collision1 && collision2)
        {
            tutoEnemyManager.GoNextTurn();
        }
        else if (collision1)
        {
            tutoEnemyManager.GoNextTurn();
        }
        else if (collision2)
        {
            tutoEnemyManager.GoNextTurn();
        }

        tutoManager.SetTutoStep(tutoManager.GetTutoStep() + 1);
        tutoManager.nextTuto = true;

        StartCoroutine(LeftRightAttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.02f);
        animator.SetBool("Attack", false);
    }

    IEnumerator LeftRightAttackCoroutine()
    {
        yield return new WaitForSeconds(0.02f);
        animator.SetBool("LeftRightAttack", false);
    }

    public void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale = new Vector3(theScale.x * -1, theScale.y, theScale.z);
        transform.localScale = theScale;
    }

    public Collider2D CheckCollision(Vector2 end)
    {
        RaycastHit2D hit;

        hit = Physics2D.Linecast(transform.position, end, zombieLayerMask);

        if (hit.transform != null)
            return hit.collider;

        return null;
    }
}
