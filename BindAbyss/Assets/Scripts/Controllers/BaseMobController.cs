using UnityEngine;

public abstract class BaseMobController : MonoBehaviour
{
    protected MonsterStat _stat;
    protected Define.MobState state;

    private void Start()
    {
        _stat = gameObject.GetOrAddComponent<MonsterStat>();
        state = Define.MobState.Move;
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
