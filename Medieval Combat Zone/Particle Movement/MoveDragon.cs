using UnityEngine;

public class MoveDragon : MonoBehaviour
{

    public DragonRage rage;

    [SerializeField] private float moveSpeed = 10f;

    private void Start()
    {
        
        rage = GetComponent<DragonRage>();

    }

    void Update()
    {

        rage.LifeSpan();

        transform.position = transform.position + transform.forward * Time.deltaTime * moveSpeed;

    }

}
