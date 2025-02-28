using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    float               _bulletSpeed = 0.5f;
    Rigidbody           _rb;

    public MonsterStat  parentStat;

    private void Start()
    {
        _bulletSpeed = 5f;
        _rb = GetComponent<Rigidbody>();
        _rb.linearVelocity = transform.forward * _bulletSpeed;
        MonsterStat stat = GetComponentInParent<MonsterStat>();
        gameObject.transform.parent = null;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");

        if (collision.collider.gameObject == target)
        {
            if (target != null)
            {
                Test targetStat = target.GetComponent<Test>();
                targetStat.TestDamage(parentStat);

                Destroy(gameObject);
            }
        }
    }
}
