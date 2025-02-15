using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    //스탯선언=================================================================================
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _maxHp;
    [SerializeField]
    protected float _attack;
    [SerializeField]
    protected float _atkSpeed;
    [SerializeField]
    protected float _denfense;
    [SerializeField]
    protected float _movespeed;


    //스탯 프로퍼티 생성=================================================================================
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public float Attack { get { return _attack; } set { _attack = value; } }
    public float AtkSpeed { get { return _atkSpeed; } set { _atkSpeed = value; } }
    public float Defense { get { return _denfense; } set { _denfense = value; } }
    public float MoveSpeed { get { return _movespeed; } set { _movespeed = value; } }


    //Start문 스탯 초기화=================================================================================
    public void Start()
    {
        _hp = 100;
        _maxHp = 100;
        _attack = 10;
        _denfense = 5;
        _movespeed = 2.0f;
    }


    //데미지 페이즈 호출=================================================================================
    public virtual void OnAttacked(Stat attacker)
    {
        int damage = (int)Mathf.Max(0, attacker.Attack - Defense);
        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }


    //사망 호출=================================================================================
    protected virtual void OnDead(Stat attacker)
    {
        /*
        PlayerStat playerStat = attacker as PlayerStat;
        if (playerStat != null)
        {
            playerStat.Exp += 5;
        }
        */
        Managers.Game.Despawn(gameObject);
    }
}
