using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRefactored : EnemyBase
{
    private Coroutine AttackCR;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        OnPlayerSpotted += ChangeToChase;
        //OnPlayerOutOfRange += StopAttack;
    }

    // Update is called once per frame
    void Update()
    {
        FacePlayer();
        if (currentState == EnemyState.Chase && agent.remainingDistance <= attackDist && !GameManager.Instance.isDead)
            Attack();
    }

    protected override void FacePlayer()
    {
        if (isAttacking)
            base.FacePlayer();
    }

    protected override void ChangeToChase()
    {
        base.ChangeToChase();
    }

    protected override void Attack()
    {
        if (!isAttacking)
        {
            if (AttackCR == null)
                AttackCR = StartCoroutine(AttackSequence(3));
        }
    }

    protected override void StopAttack()
    {
        base.StopAttack();
        if (isAttacking)
        {
            isAttacking = false;
        }
    }

    IEnumerator AttackSequence(float delay)
    {
        isAttacking = true;
        agent.stoppingDistance = 100;
        anim.SetFloat("Speed", 0);
        anim.SetTrigger("Kick");

        yield return new WaitForSeconds(delay);

        isAttacking = false;
        agent.stoppingDistance = 0;
        anim.SetFloat("Speed", moveInputModifier);
        AttackCR = null;
    }
}
