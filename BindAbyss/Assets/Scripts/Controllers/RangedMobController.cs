using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class RangedMobController : BaseMobController
{
    private void Update()
    {
        ActionControl();

        Debug.Log($"{_state}");
    }

    protected void ActionControl()
    {
        switch (State)
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

    //Maybe Don't Use
    protected override void Idle()
    {
        if (target == null)
            return;

        float distance = (target.transform.position - transform.position).magnitude;

        //Current Status >> Move
        if (distance <= stat.DetectionRange)
        {
            State = Define.MobState.Move;
            return;
        }
    }

    //Watching Player & Choose Next Pattern
    protected override void Attack()
    {
        Test targetStat = target.GetComponent<Test>();

        if (target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);

            if (targetStat.hp > 0)
            {
                float distance = (target.transform.position - transform.position).magnitude;
                if (distance <= stat.AtkRange)
                    State = Define.MobState.Attack;
                else
                    State = Define.MobState.Move;
            }
            else
                State = Define.MobState.Default;
        }
        else
        {
            State = Define.MobState.Default;
        }
    }

    public void CreateBullet()
    {
        Managers.Resource.Instantiate($"{gameObject.name}Bullet", gameObject.transform);
    }

    //For Range Debugging
    private void DebuggingGizmo()
    {
        Vector3 center = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        
        Gizmos.color = Color.red;
        for (int i = 0; i < 5; i++)
        {
            Gizmos.DrawWireSphere(center, stat.AtkRange);
        }

        Gizmos.color = Color.magenta;
        for (int i = 0; i < 5; i++)
        {
            Gizmos.DrawWireSphere(center, stat.DetectionRange);
        }
    }


}
