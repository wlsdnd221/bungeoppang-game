using TMPro;
using UnityEngine;

public class HUDUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text timerText;

    private void Update()
    {
        if (DayManager.Instance == null)
            return;

        UpdateDayText();
        UpdateTimerText();
    }

    private void UpdateDayText()
    {
        dayText.text = $"DAY {DayManager.Instance.CurrentDay}";
    }

    private void UpdateTimerText()
    {
        float remainingTime = DayManager.Instance.RemainingTime;

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.CeilToInt(remainingTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}