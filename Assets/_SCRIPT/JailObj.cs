using System;
using System.Collections.Generic;
using Lean.Pool;
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
    [SerializeField] private GameObject fxGroupLock;

    public event Action<JailObj> OnJailUnlock; // Instance event

    public bool IsLocked => currentState == JailState.Locked; // Provide a public read-only property

    private Tween catJumpTween; // Store the initial jump tween
    private float originalJumpDuration = 1f;
    private float originalJumpHeight = 1.5f;
    private Vector3 originalPosition; // Store the original position

    private void Start()
    {
        InitializeCatJump();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && IsLocked)
        {
            UnlockJail();
        }
    }

    private void UnlockJail()
    {
        SetJailUnlockedState();
        PlayUnlockVFX();
        AnimateCatOnUnlock();
    }

    private void InitializeCatJump()
    {
        if (CatModel != null)
        {
            originalPosition = CatModel.transform.position; // Store the original position
            catJumpTween = CreateJumpTween(originalPosition, originalJumpHeight, originalJumpDuration);
        }
    }

    private Tween CreateJumpTween(Vector3 position, float jumpHeight, float duration)
    {
        return CatModel.transform.DOJump(position, jumpHeight, 1, duration).SetLoops(-1, LoopType.Yoyo);
    }

    private void SetJailUnlockedState()
    {
        currentState = JailState.Unlocked;
        Door.SetActive(false);
        
        fxGroupLock.SetActive(false);

        OnJailUnlock?.Invoke(this); // Notify the GameManager
    }

    private void PlayUnlockVFX()
    {
        if (fxSlash != null && fxUnlock != null)
        {
            Vector3 objPosition = transform.position;
            objPosition.y += 2f;

            Quaternion invertedRotation = Quaternion.LookRotation(-transform.forward); // Invert the facing direction of this object

            SpawnAndDespawnEffect(fxUnlock, objPosition, invertedRotation, 5f);
            SpawnAndDespawnEffect(fxSlash, objPosition, invertedRotation, 3f);
        }
    }

    private void SpawnAndDespawnEffect(ParticleSystem effect, Vector3 position, Quaternion rotation, float despawnTime)
    {
        ParticleSystem spawnedEffect = LeanPool.Spawn(effect, position, rotation);
        LeanPool.Despawn(spawnedEffect, despawnTime);
    }

    private void AnimateCatOnUnlock()
    {
        if (CatModel == null) return;

        catJumpTween.Pause(); // Pause the current jump tween

        Sequence unlockSequence = DOTween.Sequence()
            .Append(CatModel.transform.DOJump(CatModel.transform.position, originalJumpHeight * 1.5f, 1, 0.5f).SetLoops(6, LoopType.Restart)) // Faster jump with increased height
            .Join(CatModel.transform.DORotate(new Vector3(0, 180, 0), 0.5f).SetLoops(6, LoopType.Yoyo)) // Rotate right to left
            .OnComplete(ResetCatAnimation);

        unlockSequence.Play();
    }

    private void ResetCatAnimation()
    {
        CatModel.transform.DOKill(); // Kill the unlock sequence tweens
        catJumpTween = CreateJumpTween(originalPosition, originalJumpHeight, originalJumpDuration); // Restore and resume the original jump tween
        catJumpTween.Play(); // Ensure it starts playing
    }
}
