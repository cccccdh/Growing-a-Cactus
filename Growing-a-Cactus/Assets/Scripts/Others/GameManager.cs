using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI StageText;
    public TextMeshProUGUI GemText;
    public GameObject ShopPage;
    public GameObject GachaPage;
    public GameObject CharacterPage;
    public GameObject ClothesPage;
    public GameObject WeaponPage;
    public GameObject ArmorPage;
    public GameObject PetPage;
    public GameObject OptionPage;
    public Image waveBar; // 웨이브 바 이미지 추가

    [Header("하단 버튼")]
    public GameObject characterName;
    public GameObject characterX;
    public GameObject petName;
    public GameObject petX;
    public GameObject shopName;
    public GameObject shopX;

    [Header("재화")]
    public double gold;
    public int gem;

    [Header("라운드 & 웨이브")]
    public int stageNumber;
    public int roundNumber;
    private int wave; // 현재 웨이브를 추적하는 변수 추가
    private int maxWave; // 최대 웨이브 값

    [Header("팝업 창 bool 값")]
    public bool isOpenShop = false;
    public bool isOpenGacha = false;
    public bool isOpenCharacter = false;
    public bool isOpenClothes = false;
    public bool isOpenWeapon = false;
    public bool isOpenArmor = false;
    public bool isOpenPet = false;
    public bool isOpenOption = false;

    [Header("타임 어택")]
    public Transform playerTransform;
    public Coroutine decreaseWaveCoroutine;
    public GameObject skullObject;
    public GameObject TimeObject;
    void Start()
    {
        Initialize();
    }
    
    // 초기화 함수
    public void Initialize()
    {
        gold = 0;
        gem = 0;
        stageNumber = 1;
        roundNumber = 1;
        wave = 0;
        maxWave = 100;

        UpdateGoldText();
        UpdateGemText();
        UpdateStageText();
        UpdateWaveBar();

        DeactivateAllPages();
        ResetAllFlags();        

        // 프레임 60으로 고정
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isOpenGacha)
            {
                isOpenGacha = false;
            }
            if (isOpenWeapon)
            {
                isOpenWeapon = false;
            }
        }
    }

    // 모든 페이지 초기화 함수
    private void DeactivateAllPages()
    {
        ShopPage.SetActive(false);
        GachaPage.SetActive(false);
        CharacterPage.SetActive(false);
        ClothesPage.SetActive(false);
        WeaponPage.SetActive(false);
        ArmorPage.SetActive(false);
        PetPage.SetActive(false);
        OptionPage.SetActive(false);
    }

    private void PageControl()
    {
        // 모든 페이지 비활성화
        DeactivateAllPages();

        // 조건에 따라 페이지 활성화
        if (isOpenCharacter && !isOpenShop && !isOpenPet)
        {
            CharacterPage.SetActive(true);
            ClothesPage.SetActive(isOpenClothes);
            WeaponPage.SetActive(isOpenWeapon);
            ArmorPage.SetActive(isOpenArmor);
        }
        else if (isOpenPet && !isOpenCharacter && !isOpenShop)
        {
            PetPage.SetActive(true);
        }
        else if (isOpenShop && !isOpenCharacter && !isOpenPet)
        {
            ShopPage.SetActive(true);
            GachaPage.SetActive(isOpenGacha);
        }
        else if (isOpenOption)
        {
            OptionPage.SetActive(true);
        }
    }

    private void UpdateGoldText()
    {
        GoldText.text = TextFormatter.FormatText(gold);
    }

    public void UpdateGemText()
    {
        GemText.text = gem.ToString();
    }

    public void UpdateStageText()
    {
        StageText.text = stageNumber.ToString() + "-" + roundNumber.ToString();
    }

    void UpdateWaveBar()
    {
        waveBar.fillAmount = (float)wave / maxWave;
    }

    public double Gold
    {
        get { return gold; }
    }

    public void IncreaseGold(double amount)
    {
        gold += amount;
        UpdateGoldText();
    }

    public void DecreaseGold(double amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateGoldText();
        }
    }

    public void IncreaseGem(int amount)
    {
        gem += amount;
        UpdateGemText();
    }

    public void DecreaseGem(int amount)
    {
        if(gem >= amount)
        {
            gem -= amount;
            OpenRandomPick();
            UpdateGemText();
        }
    }

    public void IncreaseStage()
    {
        // 라운드 증가
        roundNumber++;
        if (roundNumber > 10)
        {
            roundNumber = 1;
            stageNumber++;
        }

        // 퀘스트 스테이지 갱신
        QuestManager.instance.UpdateQuestProgress(1, "스테이지 클리어");
        QuestManager.instance.UpdateQuestProgress(1, "라운드 클리어");
        Debug.Log($"퀘스트 스테이지 갱신");

        UpdateStageText();

        EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager != null)
        {
            enemyManager.SetStageInfo(stageNumber, roundNumber);
        }
    }

    public void IncreaseWave(int amount)
    {
        if (wave < 100)
        {
            wave += amount;
            if (wave > maxWave)
            {
                wave = maxWave;
            }
            UpdateWaveBar();
            if (wave == 100)
            {
                DecreaseWave();
            }
        }
    }

    public void DecreaseWave()
    {
        // DecreaseWave 코루틴을 시작
        decreaseWaveCoroutine = StartCoroutine(DecreaseWaveCoroutine());
    }

    public IEnumerator DecreaseWaveCoroutine()
    {
        // 초기 웨이브 값 저장
        float startWave = wave;
        float targetWave = wave - 100;

        skullObject.SetActive(false);
        TimeObject.SetActive(true);
        waveBar.color = Color.red;


        yield return new WaitForSeconds(1.4f); // 2초 대기

        // 10초 동안 감소시킬 것이므로 경과 시간을 추적
        float duration = 10f;
        float elapsedTime = 0f;

        // 10초 동안 진행
        while (elapsedTime < duration)
        {
            // 경과 시간 비율을 계산 (0에서 1까지)
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // wave 값을 선형적으로 감소 (자연스러운 감소)
            wave = (int)Mathf.Lerp(startWave, targetWave, t); // 명시적 형변환

            // UI 업데이트
            UpdateWaveBar();

            // 다음 프레임까지 대기
            yield return null;
        }

        // 마지막으로 정확하게 targetWave 값으로 설정
        wave = (int)targetWave;
        UpdateWaveBar();

        // wave가 0 이하가 되었을 때 플레이어 사망 처리
        if (wave <= 0)
        {
            PlayerController player = playerTransform.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die(); // 플레이어 사망
            }
        }
    }

    public void StopDecreaseWaveAndReset()
    {
        // 실행 중인 코루틴이 있으면 중지
        if (decreaseWaveCoroutine != null)
        {
            StopCoroutine(decreaseWaveCoroutine);
            decreaseWaveCoroutine = null;
        }


        // wave 값을 0으로 설정하고 UI 업데이트
        wave = 0;
        UpdateWaveBar();
        skullObject.SetActive(true);
        TimeObject.SetActive(false);
        waveBar.color = Color.yellow;


    }

    public void ResetWave()
    {
        wave = 0;
        skullObject.SetActive(true);
        TimeObject.SetActive(false);
        waveBar.color = Color.yellow;
        UpdateWaveBar();
    }

    // 모든 페이지 상태 초기화 함수
    private void ResetAllFlags()
    {
        isOpenShop = false;
        isOpenGacha = false;
        isOpenCharacter = false;
        isOpenClothes = false;
        isOpenWeapon = false;
        isOpenArmor = false;
        isOpenPet = false;
        isOpenOption = false;
        characterX.SetActive(false);
        petX.SetActive(false);
        shopX.SetActive(false);
        characterName.SetActive(true);
        petName.SetActive(true);
        shopName.SetActive(true);
    }

    public void OpenShop()
    {
        bool newState = !isOpenShop;
        ResetAllFlags();
        isOpenShop = newState;

        PageControl();

        shopName.SetActive(!isOpenShop);
        shopX.SetActive(isOpenShop);
    }

    public void OpenRandomPick()
    {
        isOpenGacha = !isOpenGacha;
        PageControl();
    }

    public void OpenCharacter()
    {
        bool newState = !isOpenCharacter;
        ResetAllFlags();
        isOpenCharacter = newState;

        PageControl();

        characterName.SetActive(!isOpenCharacter);
        characterX.SetActive(isOpenCharacter);
    }

    public void OpenPet()
    {
        bool newState = !isOpenPet;
        ResetAllFlags();
        isOpenPet = newState;

        PageControl();

        petName.SetActive(!isOpenPet);
        petX.SetActive(isOpenPet);
    }

    public void OpenOption()
    {
        bool newState = !isOpenOption;
        ResetAllFlags();
        isOpenOption = newState;

        PageControl();
    }

    public void OpenClothes()
    {
        isOpenClothes = !isOpenClothes;

        PageControl();
    }

    public void OpenWeapon()
    {
        isOpenWeapon = !isOpenWeapon;
        isOpenArmor = false;

        PageControl();
    }

    public void OpenArmor()
    {
        isOpenArmor = !isOpenArmor;
        isOpenWeapon = false;

        PageControl();
    }
}
