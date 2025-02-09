using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{

    //변수선언=================================================================================
    Stat _stat;

    [SerializeField]
    float _scanRange = 10;
    [SerializeField]
    float _atkRange = 2;


    //Init=================================================================================
    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Monster;
        _stat = gameObject.GetComponent<Stat>();

        if(gameObject.GetComponentInChildren<UI_HpBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HpBar>(transform);
    }


    //대기모션 =================================================================================
    protected override void UpdateIdle()
    {
        GameObject player = Managers.Game.GetPlayer();
        if (player == null)
            return;

        //극한 최적화를 위해서는 sqrMagnitude 사용
        float distance = (player.transform.position - transform.position).magnitude;

        //기존상태 >> 이동
        if(distance <= _scanRange)
        {
            _lockTarget = player;
            State = Define.State.Moving;
            return;
        }
    }


    //이동모션 =================================================================================
    protected override void UpdateMoving()
    {
        //기존상태 >> 공격
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= _atkRange)
            {
                NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
                nma.SetDestination(transform.position);

                State = Define.State.Skill;
                return;
            }
        }

        //float끼리의 뺄샘에서는 항상 오차범위가 있기 때문에 극소값으로 계산
        Vector3 dir = _destPos - transform.position;

        //기존상태 >> 정지
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            nma.SetDestination(_destPos);
            nma.speed = _stat.MoveSpeed;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }


    //스킬모션 세부조정 =================================================================================
    protected override void UpdateSkill()
    {
        Debug.Log("Monster UpdateSkill");

        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    //애니메이션 이벤트: 타격시 발동 =================================================================================
    void OnHitEvent()
    {
        Debug.Log("Monster OnHitEvent");

        if (_lockTarget != null)
        {
            //체력
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);

            if(targetStat.Hp > 0)
            {
                float distance = (_lockTarget.transform.position - transform.position).magnitude;
                if (distance <= _atkRange)
                    State = Define.State.Skill;
                else
                    State = Define.State.Moving;
            }
            else
                State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Idle;
        }
    }
}
