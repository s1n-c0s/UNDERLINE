using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBox : MonoBehaviour
{
    private HealthSystem _HealthSystem;

    private void Start()
    {
        _HealthSystem = GetComponent<HealthSystem>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.rigidbody)
        {
            _HealthSystem.TakeDamage(1);
        }
    }
}