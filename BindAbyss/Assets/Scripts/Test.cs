using UnityEngine;

public class Test : MonoBehaviour
{
    //Made for Covering UnCompleted Features.

    public int hp;

    private void Start()
    {
        hp = 100;
    }

    public void TestDamage(MonsterStat attacker)
    {
        hp -= (int)attacker.Atk;
    }
}
