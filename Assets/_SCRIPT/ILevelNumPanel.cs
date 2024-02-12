using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ILevelPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelText;

    private void Awake()
    { 
        SetLevelText();
    }

    public void SetLevelText()
    {
        levelText.text = "Lv. " + SceneManager.GetActiveScene().buildIndex.ToString();
    }
}
