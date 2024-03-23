using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : EnemyBase
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        OnPlayerSpotted += Attack;
        OnPlayerOutOfRange += StopAttack;
    }

    // Update is called once per frame
    void Update()
    {
        FacePlayer();
    }

    protected override void FacePlayer()
    {
        if (isAttacking)
            base.FacePlayer();
    }

    protected override void Attack()
    {
        base.Attack();
        if (!isAttacking)
        {
            isAttacking = true;
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
}
