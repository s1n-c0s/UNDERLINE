using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingArea : MonoBehaviour
{
    public int falldamage = 3;
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           other.gameObject.GetComponent<HealthSystem>().TakeDamage(falldamage);
           other.gameObject.GetComponent<PlayerController>().Respawn();
        }
        /*else
        {
            other.gameObject.GetComponent<HealthSystem>().TakeDamage(50);
        }*/
        
        if(other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<HealthSystem>().TakeDamage(50);
        }
    }
}
