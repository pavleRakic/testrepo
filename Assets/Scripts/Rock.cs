using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour, IDamagable
{
    // Start is called before the first frame update

 [SerializeField] private float health = 50f;

    // Implement the interface method
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemy hit!");
        if (health <= 0) 
        {
            Destroy(gameObject); // Rock breaks when health ≤ 0
        }
    }
}
