using UnityEngine;
using UnityEngine.UI;

public class GameAreaManager : MonoBehaviour
{
    public enum Area
    {
        Customer,
        Mold,
        Ingredient
    }

    public Area currentArea = Area.Mold;

    public Image moldAreaImage;
    public Image ingredientAreaImage;

    private Color normalColor = new Color(0.55f, 0.58f, 0.65f);
    private Color selectedColor = new Color(1f, 0.85f, 0.2f);

    void Start()
    {
        UpdateAreaUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        if (currentArea == Area.Ingredient)
            currentArea = Area.Mold;

        UpdateAreaUI();
        Debug.Log($"현재 구역 : {currentArea}");
    }

    void MoveDown()
    {
        if (currentArea == Area.Mold)
            currentArea = Area.Ingredient;

        UpdateAreaUI();
        Debug.Log($"현재 구역 : {currentArea}");
    }

    void UpdateAreaUI()
    {
        if (moldAreaImage != null)
            moldAreaImage.color =
                currentArea == Area.Mold ? selectedColor : normalColor;

        if (ingredientAreaImage != null)
            ingredientAreaImage.color =
                currentArea == Area.Ingredient ? selectedColor : normalColor;
    }
}