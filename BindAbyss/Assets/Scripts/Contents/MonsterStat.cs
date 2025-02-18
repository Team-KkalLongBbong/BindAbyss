using UnityEngine;

public class MonsterStat : MonoBehaviour
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private int _hp;
    [SerializeField]
    private int _maxHp;
    [SerializeField]
    private float _atk;
    [SerializeField]
    private float _def;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _atkSpeed;
    [SerializeField]
    private float _resist;
    [SerializeField]
    private float _detectionRange;

    public string Name { get { return _name; } set { _name = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public float Atk { get { return _atk; } set { _atk = value; } }
    public float Def { get { return _def; } set { _def = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public float AtkSpeed { get { return _atkSpeed; } set { _atkSpeed = value; } }
    public float Resist { get { return _resist; } set { _resist = value; } }
    public float DetectionRange { get { return _detectionRange; } set { _detectionRange = value; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _name = "Orc";
        _hp = 100;
        _maxHp = 100;
        _atk = 10;
        _def = 5;
        _moveSpeed = 5.0f;
        _atkSpeed = 1.0f;
        _resist = 1.0f;
        _detectionRange = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
