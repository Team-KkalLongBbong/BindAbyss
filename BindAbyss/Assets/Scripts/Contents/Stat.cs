using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    //���ȼ���=================================================================================
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


    //���� ������Ƽ ����=================================================================================
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public float Attack { get { return _attack; } set { _attack = value; } }
    public float AtkSpeed { get { return _atkSpeed; } set { _atkSpeed = value; } }
    public float Defense { get { return _denfense; } set { _denfense = value; } }
    public float MoveSpeed { get { return _movespeed; } set { _movespeed = value; } }


    //Start�� ���� �ʱ�ȭ=================================================================================
    public void Start()
    {
        _hp = 100;
        _maxHp = 100;
        _attack = 10;
        _denfense = 5;
        _movespeed = 2.0f;
    }


    //������ ������ ȣ��=================================================================================
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


    //��� ȣ��=================================================================================
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
