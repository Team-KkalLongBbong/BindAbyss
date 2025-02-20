using UnityEngine;

public class RangedMobController : BaseMobController
{
    private void Update()
    {
        ActionControl();
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

    protected override void Idle()
    {
        throw new System.NotImplementedException();
    }

    protected override void Move()
    {
        
    }

    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }





}
