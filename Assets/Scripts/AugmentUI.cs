using UnityEngine;

public class AugmentUI : MonoBehaviour
{
    public static AugmentUI Instance { get; private set; }

    [SerializeField] private GameObject panel;

    private void Awake()
    {
        Instance = this;
    }

    public void Show()
    {
        panel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Hide()
    {
        panel.SetActive(false);

        Time.timeScale = 1f;
    }
}