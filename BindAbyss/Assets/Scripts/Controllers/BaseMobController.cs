using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public abstract class BaseMobController : MonoBehaviour
{
    [SerializeField]
    protected Define.MobState   _state;
    [SerializeField]
    protected GameObject        target;
    [SerializeField]
    protected Vector3           _destPos;
    [SerializeField]
    protected MonsterStat       stat;
    [SerializeField]
    protected Animator          anim;
    [SerializeField]
    protected bool              _atkActive = false;
    Coroutine                   _attackCoroutine;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        stat = gameObject.GetOrAddComponent<MonsterStat>();
        anim = GetComponent<Animator>();

        _state = Define.MobState.Move;
        target = GameObject.FindGameObjectWithTag("Player");
    }

    public virtual Define.MobState State
    {
        get { return _state; }
        set
        {
            _state = value;

            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case Define.MobState.Default:
                    anim.CrossFade("Idle", 0.1f);
                    StopAttackCoroutine();
                    break;
                case Define.MobState.Move:
                    anim.CrossFade("Move", 0.1f);
                    StopAttackCoroutine();
                    break;
                case Define.MobState.Attack:
                    StartAttackCoroutine();
                    break;
            }
        }
    }

    protected virtual void ActionControl()
    {
        switch (State)
        {
            case Define.MobState.Default:
                Match();
                break;
            case Define.MobState.Attack:
                AttackAI();
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

    //Make AttackSpeed With Coroutine
    #region Coroutine
    protected virtual void StartAttackCoroutine()
    {
        if (_attackCoroutine == null)
        {
            _attackCoroutine = StartCoroutine(AtkCoroutine());
        }
    }

    protected virtual void StopAttackCoroutine()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
    }

    protected virtual IEnumerator AtkCoroutine()
    {
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(stat.AtkSpeed);
        }
    }
    #endregion

    protected virtual void Move()
    {
        //Current Statue >> Attack
        if (target != null)
        {
            _destPos = target.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= stat.AtkRange)
            {
                NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
                nma.SetDestination(transform.position);

                State = Define.MobState.Attack;
                return;
            }
        }

        Vector3 dir = _destPos - transform.position;

        //Current Status >> Pause
        if (dir.magnitude < 0.1f)
        {
            State = Define.MobState.Default;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            nma.SetDestination(_destPos);
            nma.speed = stat.MoveSpeed;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    protected void Damaged()
    {

    }

    protected virtual void Death()
    {
        
    }

    protected virtual void Match()
    {

    }

    protected abstract void Attack();
    protected abstract void AttackAI();
}
