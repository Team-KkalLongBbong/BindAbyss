using UnityEngine;
using UnityEngine.AI;

public class RangedMobController : BaseMobController
{

    private void Update()
    {
        ActionControl();

        Debug.Log($"{state}");
    }

    protected void ActionControl()
    {
        switch (state)
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

    protected void AnimControl()
    {
        switch (state)
        {
            case Define.MobState.Default:
                anim.CrossFade("Idle", 0.1f);
                break;
            case Define.MobState.Attack:
                break;
            case Define.MobState.Move:
                anim.CrossFade("Fly", 0.1f);
                break;
            case Define.MobState.Damaged:
                break;
            case Define.MobState.Death:
                break;
        }
    }

    protected override void Idle()
    {
        if (target == null)
            return;

        //극한 최적화를 위해서는 sqrMagnitude 사용
        float distance = (target.transform.position - transform.position).magnitude;

        //기존상태 >> 이동
        if (distance <= stat.DetectionRange)
        {
            state = Define.MobState.Move;
            return;
        }
    }

    protected override void Move()
    {
        state = Define.MobState.Move;

        //기존상태 >> 공격
        if (target != null)
        {
            _destPos = target.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= stat.AtkRange)
            {
                NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
                nma.SetDestination(transform.position);

                state = Define.MobState.Attack;
                return;
            }
        }

        //float끼리의 뺄샘에서는 항상 오차범위가 있기 때문에 극소값으로 계산
        Vector3 dir = _destPos - transform.position;

        //기존상태 >> 정지
        if (dir.magnitude < 0.1f)
        {
            state = Define.MobState.Default;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            nma.SetDestination(_destPos);
            nma.speed = stat.MoveSpeed;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    protected override void Attack()
    {
        if (target != null)
        {
            //체력
            Test targetStat = target.GetComponent<Test>();
            targetStat.TestDamage(stat);

            if (targetStat.hp > 0)
            {
                float distance = (target.transform.position - transform.position).magnitude;
                if (distance <= stat.AtkRange)
                    state = Define.MobState.Attack;
                else
                    state = Define.MobState.Move;
            }
            else
                state = Define.MobState.Default;
        }
        else
        {
            state = Define.MobState.Default;
        }
    }





}
