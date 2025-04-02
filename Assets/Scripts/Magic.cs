using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class Magic : MonoBehaviour
{

    [SerializeField] private GameObject fireballPrefab; // Assign your orb prefab in Inspector
    [SerializeField] private float fireballSpeed = 10f; // Adjust speed as needed
    [SerializeField] private Transform magicSpawnLocation;
    private float cooldown = 3f;
    private bool isOnCooldown = false;
    private float nextFireTime = 0f;
    private Vector3 flightDirection;
    private float fireballTime = 3f;
    private bool isSent = false;
    private bool isHeld = false;
    private float maxHold = 3;
    private float startHoldTime = 0;
    private bool isStartHoldTimeLocked = false;
    private float damage = 10;
    private float size = 0.6f;
    private bool switching = false;
    private bool switchingTwo = false;

    private bool switchingThree = false;
    private Vector3 fireballPositionStartVector;
    [SerializeField] private float baseOffset = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.Alpha1) && Time.time >= nextFireTime)
        {
            isHeld = true;
            if(!isStartHoldTimeLocked)
                startHoldTime = Time.time;
            isStartHoldTimeLocked =true;

            if ( startHoldTime + 1 >= Time.time)
            {
                size = 0.6f;
                damage = 10;

                if(!switching)
                {
                    Debug.Log("one");
                    switching = true;
                }
           
            }
            else if (startHoldTime + 2 >= Time.time)
            {
                size = 1.2f;
                damage = 25;
                if(!switchingTwo)
                {
                     Debug.Log("two");

                    switchingTwo = true;
                }

            }
            else if((startHoldTime + 3 >= Time.time))
            {
                size = 2f;
                damage = 50;
                if(!switchingThree)
                {
                    Debug.Log("three");


                    switchingThree = true;
                }
            }

        }
        else
        {
            isStartHoldTimeLocked =false;
            isHeld = false;
            switching = false;
            switchingTwo = false;
            switchingThree = false;


        }

        if(Input.GetKeyUp(KeyCode.Alpha1) && Time.time >= nextFireTime)
        {
            isOnCooldown = true;
            flightDirection = magicSpawnLocation.forward;

            Vector3 spawnOffset = 
            magicSpawnLocation.forward * (baseOffset + size) + 
            magicSpawnLocation.up * (baseOffset + size);
        // Create the fireball at the player's position + forward offset
            GameObject fireball = Instantiate(
                fireballPrefab,
               magicSpawnLocation.position + spawnOffset,
                transform.rotation
                
            );

            float targetSize = size;
            float prefabScaleX = fireballPrefab.transform.localScale.x;
            fireball.transform.localScale = Vector3.one * (targetSize / prefabScaleX);

           // fireball.transform.localScale = new Vector3(2f, 2f, 2f);

            

             FireballFired fireballScript = fireball.GetComponent<FireballFired>();
            if (fireballScript != null)
            {
                fireballScript.flightDirection = magicSpawnLocation.forward * fireballSpeed;
                fireballScript.lifeTime = fireballTime;
                fireballScript.speed = fireballSpeed; // Optional if you're using flightDirection
                fireballScript.damage = damage;

                Debug.Log("DAMAGE IS "+ fireballScript.damage);
            }
            else
            {
                Debug.LogWarning("Fireball prefab has no FireballFired script!");
            }

        
            Destroy(fireball, 3f); 
            nextFireTime = Time.time + cooldown;
            size = 0.6f;
            damage = 10;
        }

      /*  if(Input.GetKeyDown(KeyCode.Alpha1) && Time.time >= nextFireTime)
        {
            isOnCooldown = true;
            flightDirection = magicSpawnLocation.forward;
        // Create the fireball at the player's position + forward offset
            GameObject fireball = Instantiate(
                fireballPrefab,
                magicSpawnLocation.position, // Spawn slightly ahead
                
                transform.rotation
                
            );

             FireballFired fireballScript = fireball.GetComponent<FireballFired>();
            if (fireballScript != null)
            {
                fireballScript.flightDirection = magicSpawnLocation.forward * fireballSpeed;
                fireballScript.lifeTime = fireballTime;
                fireballScript.speed = fireballSpeed; // Optional if you're using flightDirection
            }
            else
            {
                Debug.LogWarning("Fireball prefab has no FireballFired script!");
            }

        
            Destroy(fireball, 3f); 
            nextFireTime = Time.time + cooldown;
        }*/

    }
}
