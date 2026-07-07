using UnityEngine;
using UnityEngine.UI;

public class MoldKey : MonoBehaviour {

//    public GameAreaManager areaManager;         // 활성화 구역
//    public InventoryManager inventoryManager;   // 수거 된 붕어빵 인벤토리

    // 이 붕어틀이 담당하는 키
    // Q 틀인지, W 틀인지 구분
    public KeyCode keyCode;

    // 붕어틀 UI 이미지
    // 색상 변경할 때 사용
//    public Image image;

    [SerializeField] private Color readyOutlineColor = Color.yellow;
    [SerializeField] private Color completedOutlineColor = Color.green;

    [Header("MoldImage")]
    public Image fishImage;
    public Outline fishOutline;
//    public GameObject yellowOutline;
//    public GameObject greenOutline;


    [Header("Sprites")]
    public Sprite emptySprite;
    public Sprite batterSprite;
    public Sprite frontStage1Sprite;
    public Sprite frontStage2Sprite;
    public Sprite frontStage3Sprite;
    public Sprite backStage1Sprite;
    public Sprite backStage2Sprite;
    public Sprite backStage3Sprite;
    public Sprite completedSprite;
    public Sprite burntSprite;

    // 붕어틀 상태
    public enum MoldState
    {
        Empty,          // 빈 틀
        Batter,         // 반죽 투입
        BakingFront,    // 앞면 굽기
        ReadyToFlip,    // 뒤집기 표기
        BakingBack,     // 뒷면 굽기
        Completed,      // 완성(회수)
        Burnt           // 태움
    }

    /********** 붕어빵 데이터 **********/
//    private bool hasBatter;                     // 반죽 투입 여부
//    private IngredientData filling;             // 속재료("RedBean", "Custard" 등)
    private MoldState state = MoldState.Empty;  // 현재 붕어틀 상태

    /********** 시간 관련 **********/
    private float timer;                    // 현재 상태에서 흐른 시간
//    private float cookTime = 3f;            // 완전히 익는데 걸리는 시간
//    private float almostReadyTime = 2.5f;   // 완성 직전 노랑 상태 진입 시점
//    private float actionLimitTime = 1.5f;   // 플레이어가 반응할 수 있는 제한 시간 (뒤집기 또는 수거 안 하면 탐)

[SerializeField] private float bakeTime = 5f;
[SerializeField] private float flipStartRatio = 0.6f;
[SerializeField] private float flipEndRatio = 0.9f;
[SerializeField] private float burntRatio = 1.1f;

    /********** 초기화 **********/
    void Awake()
    {
        fishOutline.enabled = true;

        fishOutline.effectColor = Color.red;
        fishOutline.effectDistance = new Vector2(8, 8);

        fishImage.sprite = completedSprite;
//        fishOutline.effectDistance = new Vector2(8, -8);
//
//        if (fishOutline != null)
//        {
//            fishOutline.enabled = false;
//        }
//
//        if (fishImage != null && emptySprite != null)
//        {
//            fishImage.sprite = emptySprite;
//        }
    }

    // ----------------------------
    // 매 프레임 실행
    // ----------------------------
    void Update()
    {
        // 이미지는 항상 갱신
        UpdateVisual();

        if (DayManager.Instance == null || !DayManager.Instance.IsOpen())
        {
            return;
        }
        
         // 증강 선택창이 열려 있으면 입력/굽기 진행 중지
        if (AugmentManager.Instance != null && AugmentManager.Instance.IsAugmentPanelOpen)
        {
            return;
        }

        if (Input.GetKeyDown(keyCode))
        {
            HandleKeyPress();
        }

        UpdateCooking();
    }

    /********** 키 입력 처리 **********/
    void HandleKeyPress()
    {
        switch (state)
        {
            case MoldState.Empty:
                StartFrontCooking();
                break;

            case MoldState.ReadyToFlip:
                Flip();
                break;

            case MoldState.Completed:
                Collect();
                break;

            case MoldState.Burnt:
                ResetMold();
                break;
        }
    }

    // ----------------------------
    // 재료 투입
    // ----------------------------
//    void AddSelectedIngredient() {
//        IngredientData ingredient = ingredientSelector.GetSelectedIngredient();
//
//        // 반죽이 아직 없는 상태
//
//        if (!hasBatter) {
//            // 첫 재료는 반드시 반죽
//            if (ingredient.id != "Batter") {
//                return;
//            }
//
//            hasBatter = true;
//            state = MoldState.BatterOnly;
//            image.color = Color.yellow;
//            Debug.Log($"{keyCode} 반죽 투입");
//
//            return;
//        }
//
//        // 반죽은 있는데 속재료는 없음
//
//        if (filling == null) {
//            // 반죽 또 넣는 거 방지
//            if (ingredient.id == "Batter")
//                return;
//
//            filling = ingredient;
//
//            StartFrontCooking();
//
//            Debug.Log($"{keyCode} {ingredient} 투입");
//        }
//    }

    // ----------------------------
    // 앞면 굽기 시작
    // ----------------------------
    void StartFrontCooking()
    {
        state = MoldState.Batter;
        timer = 0f;

        Debug.Log($"{keyCode} 반죽 투입");
    }

    // ----------------------------
    // 뒤집기
    // ----------------------------
    void Flip()
    {
        state = MoldState.BakingBack;
        timer = 0f;

        Debug.Log($"{keyCode} 뒤집음");
    }

    // ----------------------------
    // 가판대로 이동
    // ----------------------------
    void Collect()
    {
        Debug.Log($"{keyCode} 가판대로 이동");
        
        // 가판대로 이동
        StandManager.Instance.AddBungeoppang();
        // 경험치 획득
        ExperienceManager.Instance.AddExp();

        // 증강 선택시 사용
//        ExperienceManager.Instance.AddExpBonus(1);
//        ExperienceManager.Instance.AddExpMultiplier(0.2f);

        ResetMold();
    }

    // ----------------------------
    // 붕어틀 초기화
    // ----------------------------
    public void ResetMold()
    {
        timer = 0f;
        state = MoldState.Empty;
    }

    // ----------------------------
    // 굽기 진행
    // ----------------------------
    void UpdateCooking()
    {
        if (state == MoldState.Empty || state == MoldState.Burnt)
            return;

        float cookTimeMultiplier = 1f;

        if (PlayerStats.Instance != null)
        {
            cookTimeMultiplier = PlayerStats.Instance.cookTimeMultiplier;
        }

        timer += Time.deltaTime / cookTimeMultiplier;

        switch (state)
        {
            case MoldState.Batter:
                if (timer >= 0.3f)
                {
                    state = MoldState.BakingFront;
                    timer = 0f;
                }
                break;

            case MoldState.BakingFront:
                if (IsBurnt())
                {
                    Burn();
                    return;
                }

                if (IsFlipWindow())
                {
                    state = MoldState.ReadyToFlip;
                }
                break;

            case MoldState.ReadyToFlip:
                if (IsBurnt())
                {
                    Burn();
                }
                break;

            case MoldState.BakingBack:
                if (IsBurnt())
                {
                    Burn();
                    return;
                }

                if (IsFlipWindow())
                {
                    state = MoldState.Completed;
                    timer = 0f;
                }
                break;

            case MoldState.Completed:
                if (timer >= bakeTime * 0.5f)
                {
                    Burn();
                }
                break;
        }
    }

    /* 판정함수 */
    private float GetBakeRatio()
    {
        return timer / bakeTime;
    }

    private bool IsFlipWindow()
    {
        float ratio = GetBakeRatio();
        return ratio >= flipStartRatio && ratio <= flipEndRatio;
    }

    private bool IsBurnt()
    {
        return GetBakeRatio() >= burntRatio;
    }

    /********** 탄 붕어빵 처리 **********/
    void Burn()
    {
        state = MoldState.Burnt;
        timer = 0f;

        DayManager.Instance.RecordBurned();

        Debug.Log($"{keyCode} 탐");
    }

    private void UpdateVisual()
    {
        fishOutline.enabled = false;

        switch (state)
        {
            case MoldState.Empty:
                fishImage.sprite = emptySprite;
                break;

            case MoldState.Batter:
                fishImage.sprite = batterSprite;
                break;

            case MoldState.BakingFront:
                fishImage.sprite = GetFrontBakeSprite();
                break;

            case MoldState.ReadyToFlip:
                fishImage.sprite = frontStage3Sprite;
                fishOutline.enabled = true;
                fishOutline.effectColor = readyOutlineColor;
                break;

            case MoldState.BakingBack:
                fishImage.sprite = GetBackBakeSprite();
                break;

            case MoldState.Completed:
                fishImage.sprite = backStage3Sprite;

                fishOutline.enabled = true;
                fishOutline.effectColor = completedOutlineColor;

              break;
//            case MoldState.Completed:
//                fishImage.sprite = completedSprite;
//                break;

            case MoldState.Burnt:
                fishImage.sprite = burntSprite;
                break;
        }
    }

    private Sprite GetFrontBakeSprite()
    {
        float ratio = GetBakeRatio();

        if (ratio < 0.2f)
            return frontStage1Sprite;

        if (ratio < 0.4f)
            return frontStage2Sprite;

        return frontStage3Sprite;
    }

    private Sprite GetBackBakeSprite()
    {
        float ratio = timer / bakeTime;

        if (ratio < 0.2f)
            return backStage1Sprite;

        if (ratio < 0.4f)
            return backStage2Sprite;

        return backStage3Sprite;
    }

}
