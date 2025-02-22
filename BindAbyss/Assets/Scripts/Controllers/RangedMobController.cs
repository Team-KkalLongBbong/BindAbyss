using UnityEngine;
using UnityEngine.AI;

public class RangedMobController : BaseMobController
{

    private void Update()
    {
        ActionControl();

        Debug.Log($"{_state}");
    }

    protected void ActionControl()
    {
        switch (State)
        {
            case Define.MobState.Default:
                Idle();
                break;
            case Define.MobState.Attack:
                Attack();
                break;
            case Define.MobState.Move:
                Move();
                break;
            case Define.MobState.Damaged:
                Damaged();
                break;
            case Define.MobState.Death:
                Death();
                break;
        }
    }

    protected override void Idle()
    {
        if (target == null)
            return;

        float distance = (target.transform.position - transform.position).magnitude;

        //Current Status >> Move
        if (distance <= stat.DetectionRange)
        {
            State = Define.MobState.Move;
            return;
        }
    }

    protected override void Attack()
    {
        if (target != null)
        {
            Test targetStat = target.GetComponent<Test>();
            //targetStat.TestDamage(stat);

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


    private void DebuggingGizmo()
    {
        Gizmos.color = Color.red;

        Vector3 center = new Vector3(transform.position.x, transform.position.y+2f, transform.position.z);

        Gizmos.DrawWireSphere(center, 50f);
    }


}
