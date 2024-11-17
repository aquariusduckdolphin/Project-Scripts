using UnityEngine;
using CombatZone.Damage;


public class DragonRage : MonoBehaviour
{

    [SerializeField] private float damageAmount = 100f;

    [SerializeField] private float lifeSpan = 100f;

    [SerializeField] private Transform parent;

    [SerializeField] private float moveSpeed = 10f;

    private void Start()
    {
        
        parent = transform.root;

    }

    private void Update()
    {

        parent.position = parent.position + parent.forward * Time.deltaTime * moveSpeed;

    }

    private void OnTriggerEnter(Collider other)
    {

        KillEnemies(other);

        Destroy(gameObject, lifeSpan);

    }

    private void KillEnemies(Collider other)
    {

        IDamage hurtEnemies = other.GetComponent<IDamage>();

        hurtEnemies?.TakeDamage(damageAmount);

    }

    public void LifeSpan()
    {

        lifeSpan -= Time.deltaTime;

        if (lifeSpan <= 0) { Destroy(gameObject); }

    }

}
