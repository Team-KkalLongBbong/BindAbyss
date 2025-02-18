using UnityEngine;

public abstract class BaseMobController : MonoBehaviour
{
    Test _stat;
    Define.MobState state;

    private void Start()
    {
        _stat = gameObject.GetOrAddComponent<Test>();
        state = Define.MobState.Default;
    }

    private void Update()
    {
        ActionControl();
    }

    protected void ActionControl()
    {
        switch (state)
        {
            case Define.MobState.Default:
                NonDetectedMove();
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

    protected void NonDetectedMove()
    {

    }

    protected abstract void Move();

    protected abstract void Attack();

    protected void Damaged()
    {

    }

    protected virtual void Death()
    {
        
    }
}
