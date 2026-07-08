using TMPro;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text revenueText;
    [SerializeField] private TMP_Text customerText;
    [SerializeField] private TMP_Text satisfactionText;
    [SerializeField] private TMP_Text burnedText;

    public void Show(DayResult result)
    {
        gameObject.SetActive(true);

        titleText.text = $"Day {result.day} End";
        revenueText.text = $"Revenue : {result.revenue}";
        customerText.text = $"Customers : {result.customersServed}";
        satisfactionText.text = $"Satisfied : {result.satisfiedCustomers}";
        burnedText.text = $"Burned : {result.burnedCount}";
    }

    public void OpenSkillTree()
    {
        Hide();
        SkillTreeUI.Instance.Open();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}