using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    float _bulletSpeed = 0.5f;

    private void Start()
    {
        _bulletSpeed = 5f;
    }

    private void Update()
    {
        //Vector3 movement = transform.forward * _bulletSpeed * Time.deltaTime;

        transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");

        if (collision.collider.gameObject == target)
        {
            MonsterStat stat = GetComponentInParent<MonsterStat>();

            if (target != null)
            {
                Test targetStat = target.GetComponent<Test>();
                targetStat.TestDamage(stat);

                Destroy(gameObject);
            }
        }
    }
}
