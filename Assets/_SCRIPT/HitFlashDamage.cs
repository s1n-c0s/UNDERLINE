using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlashDamage : MonoBehaviour
{
    [SerializeField] private GameObject _model;
    private MeshRenderer _meshRenderer;
    private Color originColor;
    public Color flashColor;
    public float timeFade = 0.15f;
    
    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = _model.GetComponent<MeshRenderer>();
        originColor = _meshRenderer.material.color;
    }

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FlashStart();
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            FlashStart();
        }
    }

    void FlashStart()
    {
        _meshRenderer.material.color = flashColor;
        Invoke("FlashStop", timeFade);
    }

    void FlashStop()
    {
        _meshRenderer.material.color = originColor;
    }
}
