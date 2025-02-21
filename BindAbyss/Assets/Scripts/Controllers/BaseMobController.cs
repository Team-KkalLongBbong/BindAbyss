using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class BaseMobController : MonoBehaviour
{
    [SerializeField]
    protected Define.MobState   state;
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

        state =     Define.MobState.Move;
        target =    GameObject.FindGameObjectWithTag("Player");
    }

    protected abstract void Idle();

    protected abstract void Move();

    protected abstract void Attack();

    protected void Damaged()
    {

    }

    protected virtual void Death()
    {
        
    }
}
