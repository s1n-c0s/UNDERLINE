using TMPro;
using UnityEngine;

public class ISkillCooldown : MonoBehaviour
{
    [SerializeField] private SkillTurnSystem _skillTurnSystem; // Reference to SkillTurnSystem
    private TextMeshProUGUI textMeshPro; // Reference to TextMeshProUGUI component
    private TextAnimator textAnimator; // Reference to TextAnimator component

    private void Start()
    {
        gameObject.AddComponent<TextAnimator>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        textAnimator = GetComponent<TextAnimator>();
        UpdateText(); // Initialize text on start
    }

    private void Update()
    {
        int currentTurn = _skillTurnSystem.GetCurrentValue();

        if (textMeshPro.text != currentTurn.ToString())
        {
            textAnimator.UpdateTextWithScaleEffect(currentTurn.ToString());
        }
    }

    // Directly update text without tween
    private void UpdateText()
    {
        textMeshPro.text = _skillTurnSystem.GetCurrentValue().ToString();
    }
}