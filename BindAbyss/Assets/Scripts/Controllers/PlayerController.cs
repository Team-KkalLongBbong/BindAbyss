using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{

    //변수선언=================================================================================
    int _mouseMask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    Stat  _stat;

    [SerializeField]
    bool _stopSkill = false;


    //Init=================================================================================
    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _stat = gameObject.GetComponent<Stat>();
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        if (gameObject.GetComponentInChildren<UI_HpBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HpBar>(transform);
    }


    /*
    State패턴.
    현재 플레이어의 상태를 열거형으로 정리해
    그 각 열거형 형태마다 함수로 캡슐화 해서 한번에 하나의 애니메이션 가동
    한번에 한 상태밖에 줄수 없다는 단점이 있지만 소형 플젝에선 사용하기 용이함.
    단점이 왜 문제가 되냐면, 움직이면서 주문질할때는 moving, skill 두개의 상태가 필요하기 때문.
     */


    //이동=================================================================================
    protected override void UpdateMoving()
    {
        //기존상태 >> 공격
        if(_lockTarget != null)
        {
            float distance = (_destPos - transform.position).magnitude;
            if(distance <= 1.8f)
            {
                State = Define.State.Skill;
                return;
            }
        }

        Vector3 dir = _destPos - transform.position;
        dir.y = 0;

        //float끼리의 뺄샘에서는 항상 오차범위가 있기 때문에 극소값으로 계산
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            Debug.DrawRay(transform.position + Vector3.up, dir.normalized, Color.green);
            if(Physics.Raycast(transform.position + Vector3.up, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if(Input.GetMouseButton(0) == false)
                    State = Define.State.Idle;
                return;
            }

            /*  normalize를 하지 않으면 벡터값(방향과 속도 등을 담고있음)에 미묘한 오차가 생김.
                normalizs로 벡터값을 1로 바꿔서 속도는 일정하게 하고 방향값만 가져와야함.
                Clamp는 첫번째 변수값의 최댓값을 무조건 두번째와 세번째 값 사이의 값으로 만드는 기능.  */

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }


    //공격=================================================================================
    protected override void UpdateSkill()
    {
        if(_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    //애니메이션 이벤트 : 타격시 발동
    void OnHitEvent()
    {
        Debug.Log("player skill on");

        if(_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            Stat myStat = gameObject.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
        }

        if (_stopSkill)
        {
            Debug.Log("stopskill true");
            State = Define.State.Idle;
        }
        else
        {
            Debug.Log("stopskill false");
            State = Define.State.Skill;
        }

    }


    //마우스 조작 처리=================================================================================
    void OnMouseEvent(Define.MouseEvent evt)
   {
        switch (State)
        {
            case Define.State.Die:
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
                {
                    if (evt == Define.MouseEvent.PointerUp)
                        _stopSkill = true;
                }
                break;
        }
    }


    //마우스 조작처리 2 =================================================================================
    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mouseMask);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        _destPos = hit.point;
                        State = Define.State.Moving;
                        _stopSkill = false;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            _lockTarget = hit.collider.gameObject;
                        else
                        {
                            _lockTarget = null;
                        }
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                        if (_lockTarget == null && raycastHit)
                        _destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }
    }
}
