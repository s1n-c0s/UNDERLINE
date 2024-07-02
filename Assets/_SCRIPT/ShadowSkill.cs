using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class ShadowSkill : MonoBehaviour
{
    private ShadowLife _shadowLife;
    private List<GameObject> shadowClone;

    private void Start()
    {
        _shadowLife = GetComponent<ShadowLife>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Player"))
        {
            foreach (var _shadowBullet in shadowClone)
            {
                
            }
        }
    }
}
