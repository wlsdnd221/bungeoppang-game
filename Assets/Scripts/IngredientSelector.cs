using UnityEngine;
using UnityEngine.UI;

public class IngredientSelector : MonoBehaviour
{
    public GameAreaManager areaManager;

    public Image[] ingredientSlots;

    private int selectedIndex = 0;

    private Color normalColor = Color.white;
    private Color selectedColor = Color.yellow;

    void Start()
    {
        RefreshUI();
    }

    void Update()
    {
        if (areaManager.currentArea != GameAreaManager.Area.Ingredient)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedIndex--;

            if (selectedIndex < 0)
                selectedIndex = ingredientSlots.Length - 1;

            RefreshUI();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedIndex++;

            if (selectedIndex >= ingredientSlots.Length)
                selectedIndex = 0;

            RefreshUI();
        }
    }

    void RefreshUI()
    {
        for (int i = 0; i < ingredientSlots.Length; i++)
        {
            ingredientSlots[i].color =
                i == selectedIndex
                ? selectedColor
                : normalColor;
        }
    }

    public IngredientData[] ingredients;

    public IngredientData GetSelectedIngredient()
    {
        return ingredients[selectedIndex];
    }
}