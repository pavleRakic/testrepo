using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireballFired : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 flightDirection;
    public float lifeTime = 3f;
    public float speed = 10;
    public float creationTime;
    [SerializeField] public float damage = 10f;
    void Start()
    {
        creationTime = Time.time;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(creationTime + lifeTime > Time.time)
            transform.position += flightDirection*Time.deltaTime;
    }

  /*  private void OnCollisionEnter(Collision collision) 
    {
        // Check if the hit object implements IDamageable
        Debug.Log("Enemy entered!");
        if (collision.gameObject.TryGetComponent(out IDamagable damageable))
        {
            damageable.TakeDamage(damage);
        }
        Destroy(gameObject); // Destroy fireball on hit
    }*/


    private void OnTriggerEnter(Collider other) // Note: Collider, not Collision
    {


    if (other.TryGetComponent(out IDamagable damageable))
    {
        damageable.TakeDamage(damage);

    }
        Destroy(gameObject);
    }
}
