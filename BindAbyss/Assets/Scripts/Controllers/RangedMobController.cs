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

        //���� ����ȭ�� ���ؼ��� sqrMagnitude ���
        float distance = (target.transform.position - transform.position).magnitude;

        //�������� >> �̵�
        if (distance <= stat.DetectionRange)
        {
            state = Define.MobState.Move;
            return;
        }
    }

    protected override void Move()
    {
        state = Define.MobState.Move;

        //�������� >> ����
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

        //float������ ���������� �׻� ���������� �ֱ� ������ �ؼҰ����� ���
        Vector3 dir = _destPos - transform.position;

        //�������� >> ����
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
            //ü��
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
