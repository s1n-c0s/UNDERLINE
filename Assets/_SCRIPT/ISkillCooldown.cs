using System.Collections;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class ISkillCooldown : MonoBehaviour
{
    [SerializeField] private SkillTurnSystem _skillTurnSystem; // Reference to SkillTurnSystem
    private TextMeshProUGUI textMeshPro; // Reference to TextMeshProUGUI component

    private bool isScaling = false;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        int currentTurn = _skillTurnSystem.GetCurrentSkillCooldown();

        if (textMeshPro.text != currentTurn.ToString())
        {
            if (!isScaling)
            {
                StartCoroutine(UpdateTextWithScaleEffect(currentTurn.ToString()));
            }
        }
    }

    private IEnumerator UpdateTextWithScaleEffect(string newText)
    {
        isScaling = true;

        // Punch scale effect
        textMeshPro.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 0, 1f);

        yield return new WaitForSeconds(0.3f); // Wait for the punch scale effect to finish

        // Update text
        textMeshPro.text = newText;

        // Scale back to original size
        textMeshPro.transform.DOScale(Vector3.one, 0.3f);

        yield return new WaitForSeconds(0.3f); // Wait for the scale back effect to finish

        isScaling = false;
    }
}