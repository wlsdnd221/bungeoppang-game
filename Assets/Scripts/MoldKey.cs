using UnityEngine;
using UnityEngine.UI;

public class MoldKey : MonoBehaviour {

    public GameAreaManager areaManager;         // 활성화 구역
    public InventoryManager inventoryManager;   // 수거 된 붕어빵 인벤토리

    // 이 붕어틀이 담당하는 키
    // Q 틀인지, W 틀인지 구분
    public KeyCode keyCode;

    // 붕어틀 UI 이미지
    // 색상 변경할 때 사용
    public Image image;

    // 현재 선택된 재료를 가져오기 위한 참조
    public IngredientSelector ingredientSelector;

    // 붕어틀 상태
    public enum MoldState {
        Empty,
        BatterOnly,
        CookingFront,
        AlmostReadyFront,
        NeedFlip,
        CookingBack,
        AlmostReadyBack,
        Done,
        Burnt
    }

    /********** 붕어빵 데이터 **********/
    private bool hasBatter;                     // 반죽 투입 여부
    private IngredientData filling;             // 속재료("RedBean", "Custard" 등)
    private MoldState state = MoldState.Empty;  // 현재 붕어틀 상태

    /********** 시간 관련 **********/
    private float timer;                    // 현재 상태에서 흐른 시간
    private float cookTime = 3f;            // 완전히 익는데 걸리는 시간
    private float almostReadyTime = 2.5f;   // 완성 직전 노랑 상태 진입 시점
    private float actionLimitTime = 1.5f;   // 플레이어가 반응할 수 있는 제한 시간 (뒤집기 또는 수거 안 하면 탐)

    /********** 깜빡임 관련 **********/
    private float blinkTimer;
    private bool blinkOn = true;

    /********** 초기화 **********/
    void Awake()
    {
        image = GetComponent<Image>();

        // 시작은 빈틀
        image.color = Color.gray;
    }

    // ----------------------------
    // 매 프레임 실행
    // ----------------------------
    void Update()
    {
        // 담당 키 입력 확인
        if (Input.GetKeyDown(keyCode))
        {
            HandleKeyPress();
        }

        // 굽기 로직
        UpdateCooking();

        // 깜빡임 로직
        UpdateBlink();
    }


    /********** 키 입력 처리 **********/
    void HandleKeyPress() {
        if (areaManager.currentArea == GameAreaManager.Area.Ingredient) {
            switch (state) {
                case MoldState.Empty:
                case MoldState.BatterOnly:
                    AddSelectedIngredient();
                    break;
            }

            return;
        }

        if (areaManager.currentArea == GameAreaManager.Area.Mold) {
            switch (state) {
                case MoldState.NeedFlip:
                    Flip();
                    break;

                case MoldState.Done:
                    Collect();
                    break;

                case MoldState.Burnt:
                    ResetMold();
                    break;
            }

            return;
        }
    }

    // ----------------------------
    // 재료 투입
    // ----------------------------
    void AddSelectedIngredient() {
        IngredientData ingredient = ingredientSelector.GetSelectedIngredient();

        // 반죽이 아직 없는 상태

        if (!hasBatter) {
            // 첫 재료는 반드시 반죽
            if (ingredient.id != "Batter") {
                return;
            }

            hasBatter = true;
            state = MoldState.BatterOnly;
            image.color = Color.yellow;
            Debug.Log($"{keyCode} 반죽 투입");

            return;
        }

        // 반죽은 있는데 속재료는 없음

        if (filling == null) {
            // 반죽 또 넣는 거 방지
            if (ingredient.id == "Batter")
                return;

            filling = ingredient;

            StartFrontCooking();

            Debug.Log($"{keyCode} {ingredient} 투입");
        }
    }

    // ----------------------------
    // 앞면 굽기 시작
    // ----------------------------
    void StartFrontCooking() {
        state = MoldState.CookingFront;
        timer = 0f;

        image.color = new Color(1f, 0.5f, 0f);

        Debug.Log($"{keyCode} 앞면 굽기 시작");
    }

    // ----------------------------
    // 뒤집기
    // ----------------------------
    void Flip() {
        state = MoldState.CookingBack;
        timer = 0f;

        image.color = new Color(1f, 0.5f, 0f);

        Debug.Log($"{keyCode} 뒤집음");
    }

    // ----------------------------
    // 완성품 수거
    // ----------------------------
    void Collect()
    {
        Debug.Log($"{keyCode} 붕어빵 수거 / 속재료: {filling}");

        inventoryManager.AddBungeoppang(filling.id);

        ResetMold();
    }

    // ----------------------------
    // 붕어틀 초기화
    // ----------------------------
    void ResetMold() {
        hasBatter = false;
        filling = null;
        timer = 0f;
        blinkTimer = 0f;
        blinkOn = true;
        state = MoldState.Empty;
        image.color = Color.gray;
    }

    // ----------------------------
    // 굽기 진행
    // ----------------------------
    void UpdateCooking()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case MoldState.CookingFront:
                if (timer >= almostReadyTime)
                {
                    state = MoldState.AlmostReadyFront;
                    image.color = Color.yellow;
                }
                break;

            case MoldState.AlmostReadyFront:
                if (timer >= cookTime)
                {
                    state = MoldState.NeedFlip;
                    timer = 0f;
                }
                break;

            case MoldState.NeedFlip:
                if (timer >= actionLimitTime)
                {
                    Burn();
                }
                break;

            case MoldState.CookingBack:
                if (timer >= almostReadyTime)
                {
                    state = MoldState.AlmostReadyBack;
                    image.color = Color.yellow;
                }
                break;

            case MoldState.AlmostReadyBack:
                if (timer >= cookTime)
                {
                    state = MoldState.Done;
                    timer = 0f;
                }
                break;

            case MoldState.Done:
                if (timer >= actionLimitTime)
                {
                    Burn();
                }
                break;
        }
    }

    // ----------------------------
    // 깜빡임 처리
    // ----------------------------
    void UpdateBlink() {
        // 파랑 / 초록 상태만 깜빡임

        if (state != MoldState.NeedFlip && state != MoldState.Done) {
            return;
        }


        blinkTimer += Time.deltaTime;

        if (blinkTimer >= 0.25f) {

            blinkTimer = 0f;
            blinkOn = !blinkOn;

            if (state == MoldState.NeedFlip) {
                image.color =
                    blinkOn ? Color.blue : Color.white;
            }

            if (state == MoldState.Done) {
                image.color = blinkOn ? Color.green : Color.white;
            }
        }
    }


    /********** 탄 붕어빵 처리 **********/
    void Burn() {
        state = MoldState.Burnt;
        timer = 0f;
        image.color = Color.red;
        Debug.Log($"{keyCode} 탐");
    }

}
