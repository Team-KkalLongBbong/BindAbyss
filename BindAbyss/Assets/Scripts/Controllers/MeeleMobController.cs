using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MeeleMobController : BaseMobController
{
    private void Start()
    {
        base.Init();
    }

    private void Update()
    {
        Debug.Log($"{_state}");

        ActionControl();
    }

    protected override void Attack()
    {
        int skillGatcha = Random.Range(0, 9);
        bool hasSpAtk = true;

        if (hasSpAtk)
        {
            if (skillGatcha >= 0 && skillGatcha <= 3)
                anim.CrossFade("Attack1", 0.1f, -1, 0);
            else if (skillGatcha > 3 && skillGatcha <= 7)
                anim.CrossFade("Attack2", 0.1f, -1, 0);
            else
                anim.CrossFade("SpAtk", 0.1f, -1, 0);
        }
        else
        {
            if (skillGatcha >= 0 && skillGatcha <= 4)
                anim.CrossFade("Attack1", 0.1f, -1, 0);
            else
                anim.CrossFade("Attack2", 0.1f, -1, 0);
        }
    }

    protected override void AttackAI()
    {
        Test targetStat = target.GetComponent<Test>();

        if (target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);

            if (targetStat.hp > 0)
            {
                float distance = (target.transform.position - transform.position).magnitude;
                if (distance <= stat.AtkRange)
                    State = Define.MobState.Attack;
                else
                    State = Define.MobState.Move;
            }
            else
                State = Define.MobState.Default;
        }
        else
        {
            State = Define.MobState.Default;
        }
    }

    protected override void Match()
    {
        base.Match();


    }
}