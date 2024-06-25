using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using Unity.Mathematics;
using UnityEngine;

public class JailObj : MonoBehaviour
{
    [SerializeField] private bool isLock = true;
    [SerializeField] private GameObject Door;

    [Header("VFX")] [SerializeField] private ParticleSystem fxUnlock;
    [SerializeField] private ParticleSystem fxSlash;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isLock)
        {
            isLock = false;
            Door.SetActive(false);

            if (fxSlash != null && fxUnlock != null) 
            {
                ParticleSystem _fxUnlock = LeanPool.Spawn(fxSlash, transform.forward, quaternion.identity);
                LeanPool.Despawn(_fxUnlock, 3f);
                ParticleSystem fxinit = LeanPool.Spawn(fxSlash, transform.position, Quaternion.identity);
                LeanPool.Despawn(fxinit, 3f);
            }
        }
    }
}
