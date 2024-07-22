using System;
using System.Collections.Generic;
using Lean.Pool;
using Unity.Mathematics;
using UnityEngine;

public class JailObj : MonoBehaviour
{
    [SerializeField] private bool isLock = true;
    [SerializeField] private GameObject Door;

    [Header("VFX")]
    [SerializeField] private ParticleSystem fxUnlock;
    [SerializeField] private ParticleSystem fxSlash;
    [SerializeField] private List<GameObject> fx_groupLock;

    public event Action<JailObj> OnJailUnlock; // Instance event

    public bool IsLocked => isLock; // Provide a public read-only property
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isLock)
        {
            isLock = false;
            Door.SetActive(false);
            foreach (var fx_lock in fx_groupLock)
            {
                fx_lock.SetActive(false);
            }

            OnJailUnlock?.Invoke(this); // Notify the GameManager

            if (fxSlash != null && fxUnlock != null)
            {
                Vector3 objPosition = transform.position;
                objPosition.y += 2f;

                Quaternion backwardRotation = Quaternion.Euler(0, 180, 0); // Rotate 180 degrees around the Y-axis

                ParticleSystem _fxUnlock = LeanPool.Spawn(fxUnlock, objPosition, backwardRotation);
                LeanPool.Despawn(_fxUnlock, 3f);
                ParticleSystem fxinit = LeanPool.Spawn(fxSlash, objPosition, Quaternion.identity);
                LeanPool.Despawn(fxinit, 3f);
            }
        }
    }
}