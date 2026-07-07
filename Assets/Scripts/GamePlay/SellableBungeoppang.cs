using UnityEngine;
using UnityEngine.EventSystems;

public class SellableBungeoppang : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int price = 1000;

    public int Price => price;

    public void OnPointerClick(PointerEventData eventData)
    {
        StandManager.Instance.Sell(this);
    }
}