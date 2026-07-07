using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AugmentSlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    private AugmentData augmentData;
    private int slotIndex;

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }

    public void SetData(AugmentData data)
    {
        augmentData = data;

        nameText.text = data.augmentName;
        descriptionText.text = data.description;
        icon.sprite = data.icon;
    }

    public void OnClick()
    {
        if (augmentData == null)
            return;

        AugmentManager.Instance.SelectAugment(augmentData);
    }

    public void OnClickReroll()
    {
        AugmentManager.Instance.RerollSlot(slotIndex);
    }
}