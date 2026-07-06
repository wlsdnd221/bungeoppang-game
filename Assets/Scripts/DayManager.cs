using UnityEngine;

public enum DayState
{
    Ready,
    Open,
    Result
}

[System.Serializable]
public class DayResult
{
    public int day;
    public int revenue;
    public int customersServed;
    public int satisfiedCustomers;
    public int sellCount;
    public int burnedCount;
}

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    // DAY 관련
    public int CurrentDay => currentDay;
    public float RemainingTime => remainingTime;
    public DayState CurrentState => currentState;

    [Header("Day Settings")]
    [SerializeField] private int currentDay = 1;
    [SerializeField] private float dayDuration = 60f;

    [Header("Runtime")]
    [SerializeField] private DayState currentState = DayState.Ready;
    [SerializeField] private float remainingTime;

    [SerializeField] private ResultUI resultUI;

    private DayResult currentResult;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SetReadyState();
        StartDay(); // 임시시작
    }

    private void Update()
    {
        if (currentState != DayState.Open)
            return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            EndDay();
        }
    }

    public void StartDay()
    {
        if (currentState != DayState.Ready)
            return;

        remainingTime = dayDuration;

        currentResult = new DayResult
        {
            day = currentDay,
            revenue = 0,
            customersServed = 0,
            satisfiedCustomers = 0,
            sellCount = 0,
            burnedCount = 0
        };

        currentState = DayState.Open;

        Debug.Log($"[DayManager] Day {currentDay} Open");
    }

    public void EndDay()
    {
        if (currentState != DayState.Open)
            return;

        currentState = DayState.Result;

        // 임시 결과 콘솔
        Debug.Log($"[DayManager] Day {currentDay} End");
        Debug.Log($"[DayManager] Revenue: {currentResult.revenue}");
        Debug.Log($"[DayManager] Customers: {currentResult.customersServed}");
        Debug.Log($"[DayManager] Satisfied: {currentResult.satisfiedCustomers}");
        Debug.Log($"[DayManager] Burned: {currentResult.burnedCount}");

        // 결과창 표시
        resultUI.Show(currentResult);
    }

    public void NextDay()
    {
        if (currentState != DayState.Result)
            return;

        currentDay++;

        resultUI.Hide();

        SetReadyState();

        StartDay();
    }

    public void RecordSale(int price, bool isSatisfied)
    {
        if (currentState != DayState.Open)
            return;

        currentResult.revenue += price;
        currentResult.customersServed++;
        currentResult.sellCount++;

        if (isSatisfied)
        {
            currentResult.satisfiedCustomers++;
        }
    }

    public void RecordBurned()
    {
        if (currentState != DayState.Open)
            return;

        currentResult.burnedCount++;
    }

    private void SetReadyState()
    {
        currentState = DayState.Ready;
        remainingTime = dayDuration;

        Debug.Log($"[DayManager] Day {currentDay} Ready");
    }

    // 오픈상태확인용
    public bool IsOpen()
    {
        return currentState == DayState.Open;
    }
}