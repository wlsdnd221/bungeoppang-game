using UnityEngine;

[CreateAssetMenu(
    fileName = "NewIngredient",
    menuName = "Bungeoppang/Ingredient Data"
)]
public class IngredientData : ScriptableObject
{
    public string id;

    public string koreanName;
    public string englishName;

    public int price;

    public Sprite icon;
}