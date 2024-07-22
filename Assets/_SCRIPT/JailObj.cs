using System;
using System.Collections.Generic;
using Lean.Pool;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening; // Import DoTween namespace

public class JailObj : MonoBehaviour
{
    public enum JailState
    {
        Locked,
        Unlocked
    }

    [SerializeField] private JailState currentState = JailState.Locked;
    [SerializeField] private GameObject Door;
    [SerializeField] private GameObject CatModel; // Add a reference for the cat model

    [Header("VFX")]
    [SerializeField] private ParticleSystem fxUnlock;
    [SerializeField] private ParticleSystem fxSlash;
    [SerializeField] private List<GameObject> fx_groupLock;

    public event Action<JailObj> OnJailUnlock; // Instance event

    public bool IsLocked => currentState == JailState.Locked; // Provide a public read-only property

    private Tween catJumpTween; // Store the initial jump tween
    private float originalJumpDuration = 1f;
    private float originalJumpHeight = 1.5f;
    private Vector3 originalPosition; // Store the original position

    private void Start()
    {
        // Initialize the cat model jump parameters
        if (CatModel != null)
        {
            originalJumpDuration = 1f; // Original jump duration
            originalJumpHeight = 1.5f; // Original jump height
            originalPosition = CatModel.transform.position; // Store the original position

            // Set up the initial jump tween
            catJumpTween = CatModel.transform.DOJump(originalPosition, originalJumpHeight, 1, originalJumpDuration).SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentState == JailState.Locked)
        {
            UnlockJail();
        }
    }

    private void UnlockJail()
    {
        currentState = JailState.Unlocked;
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
            LeanPool.Despawn(_fxUnlock, 5f);
            ParticleSystem fxinit = LeanPool.Spawn(fxSlash, objPosition, Quaternion.identity);
            LeanPool.Despawn(fxinit, 3f);
        }

        // Modify the cat's animation to jump faster and rotate from right to left for 3 seconds
        if (CatModel != null)
        {
            catJumpTween.Pause(); // Pause the current jump tween

            Sequence unlockSequence = DOTween.Sequence()
                .Append(CatModel.transform.DOJump(CatModel.transform.position, originalJumpHeight * 1.5f, 1, 0.5f).SetLoops(6, LoopType.Restart)) // Faster jump with increased height
                .Join(CatModel.transform.DORotate(new Vector3(0, 180, 0), 0.5f).SetLoops(6, LoopType.Yoyo)) // Rotate right to left
                .OnComplete(() =>
                {
                    // Restore the original jump behavior
                    CatModel.transform.DOKill(); // Kill the unlock sequence tweens
                    // Create and start the original jump tween
                    catJumpTween = CatModel.transform.DOJump(originalPosition, originalJumpHeight, 1, originalJumpDuration).SetLoops(-1, LoopType.Yoyo);
                    catJumpTween.Play(); // Ensure it starts playing
                });

            unlockSequence.Play();
        }
    }
}
