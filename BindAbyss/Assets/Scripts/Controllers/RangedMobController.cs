using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class RangedMobController : BaseMobController
{
    private void Update()
    {
        Debug.Log($"{_state}");

        ActionControl();
    }

    //Maybe Don't Use
    protected override void Match()
    {
        base.Match();

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

    //Attack
    protected override void Attack()
    {
        _destPos = target.transform.position;
        float distance = (_destPos - transform.position).magnitude;
        if (distance <= stat.DetectionRange)
        {
            anim.CrossFade("Attack2", 0.1f, -1, 0);
            Debug.Log("Meele Atk");
        }
        else
        {
            int skillGatcha = Random.Range(1, 11);
            if (skillGatcha <= 8)
            {
                Debug.Log("Normal Atk");
                anim.CrossFade("Attack1", 0.1f, -1, 0);
            }
            else
            {
                Debug.Log("Special Atk");
                anim.CrossFade("SpAtk", 0.1f, -1, 0);
            }
        }
    }

    //Watching Player & Choose Next Pattern
    protected override void AttackAI()
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
