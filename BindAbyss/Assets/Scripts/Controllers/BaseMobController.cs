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

    private void Start()
    {
        stat =      gameObject.GetOrAddComponent<MonsterStat>();
        anim =      GetComponent<Animator>();

        _state =     Define.MobState.Move;
        target =    GameObject.FindGameObjectWithTag("Player");
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
                    break;
                case Define.MobState.Move:
                    anim.CrossFade("Fly", 0.1f);
                    break;
                case Define.MobState.Attack:
                   // anim.CrossFade("ATK", 0.1f, -1, 0);
                    break;
            }
        }
    }

    protected abstract void Idle();

    protected abstract void Attack();

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
}
