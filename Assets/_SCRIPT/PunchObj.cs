using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PunchObj : MonoBehaviour
{
   [SerializeField] private float duration = 0.75f;
   
   public void startBounce()
   {
      transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), duration, 10, 1);
   }
}
