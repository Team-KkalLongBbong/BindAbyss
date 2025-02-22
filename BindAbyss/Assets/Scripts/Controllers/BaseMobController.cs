using System.Collections;
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

    BaseMobController           _isRangedMob;
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

        _isRangedMob = gameObject.GetComponent<RangedMobController>();
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

    //Make AttackSpeed With Coroutine
    #region Coroutine
    private void StartAttackCoroutine()
    {
        if (_attackCoroutine == null)
        {
            _attackCoroutine = StartCoroutine(RangedMobAtkCoroutine());
        }
    }

    private void StopAttackCoroutine()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
    }

    private IEnumerator RangedMobAtkCoroutine()
    {
        while (true)
        {
            RangedMobAtk(_isRangedMob);
            yield return new WaitForSeconds(stat.AtkSpeed);
        }
    }
    #endregion

    void RangedMobAtk(BaseMobController isRangedMob)
    {
        if (isRangedMob != null) //It is RangedMob
        {
            _destPos = target.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= stat.DetectionRange)
            {
                anim.CrossFade("Attack2", 0.1f, -1, 0);
            }
            else
            {
                int skillGatcha = Random.Range(1, 11);
                if (skillGatcha <= 8)
                    anim.CrossFade("Attack1", 0.1f, -1, 0);
                else
                    anim.CrossFade("SpAtk", 0.1f, -1, 0);
            }
        }
    }


    protected virtual void Move()
    {
        State = Define.MobState.Move;

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

    protected abstract void Idle();

    protected abstract void Attack();
}
