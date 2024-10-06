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
    public Image waveBar; // ���̺� �� �̹��� �߰�

    [Header("�ϴ� ��ư")]
    public GameObject characterName;
    public GameObject characterX;
    public GameObject petName;
    public GameObject petX;
    public GameObject shopName;
    public GameObject shopX;

    [Header("��ȭ")]
    public double gold;
    public int gem;

    [Header("���� & ���̺�")]
    public int stageNumber;
    public int roundNumber;
    private int wave; // ���� ���̺긦 �����ϴ� ���� �߰�
    private int maxWave; // �ִ� ���̺� ��

    [Header("�˾� â bool ��")]
    public bool isOpenShop = false;
    public bool isOpenGacha = false;
    public bool isOpenCharacter = false;
    public bool isOpenClothes = false;
    public bool isOpenWeapon = false;
    public bool isOpenArmor = false;
    public bool isOpenPet = false;
    public bool isOpenOption = false;

    [Header("Ÿ�� ����")]
    public Transform playerTransform;
    public Coroutine decreaseWaveCoroutine;
    public GameObject skullObject;
    public GameObject TimeObject;
    void Start()
    {
        Initialize();
    }
    
    // �ʱ�ȭ �Լ�
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

        // ������ 60���� ����
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

    // ��� ������ �ʱ�ȭ �Լ�
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
        // ��� ������ ��Ȱ��ȭ
        DeactivateAllPages();

        // ���ǿ� ���� ������ Ȱ��ȭ
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
        // ���� ����
        roundNumber++;
        if (roundNumber > 10)
        {
            roundNumber = 1;
            stageNumber++;
        }

        // ����Ʈ �������� ����
        QuestManager.instance.UpdateQuestProgress(1, "�������� Ŭ����");
        QuestManager.instance.UpdateQuestProgress(1, "���� Ŭ����");
        Debug.Log($"����Ʈ �������� ����");

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
        // DecreaseWave �ڷ�ƾ�� ����
        decreaseWaveCoroutine = StartCoroutine(DecreaseWaveCoroutine());
    }

    public IEnumerator DecreaseWaveCoroutine()
    {
        // �ʱ� ���̺� �� ����
        float startWave = wave;
        float targetWave = wave - 100;

        skullObject.SetActive(false);
        TimeObject.SetActive(true);
        waveBar.color = Color.red;


        yield return new WaitForSeconds(1.4f); // 2�� ���

        // 10�� ���� ���ҽ�ų ���̹Ƿ� ��� �ð��� ����
        float duration = 10f;
        float elapsedTime = 0f;

        // 10�� ���� ����
        while (elapsedTime < duration)
        {
            // ��� �ð� ������ ��� (0���� 1����)
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // wave ���� ���������� ���� (�ڿ������� ����)
            wave = (int)Mathf.Lerp(startWave, targetWave, t); // ����� ����ȯ

            // UI ������Ʈ
            UpdateWaveBar();

            // ���� �����ӱ��� ���
            yield return null;
        }

        // ���������� ��Ȯ�ϰ� targetWave ������ ����
        wave = (int)targetWave;
        UpdateWaveBar();

        // wave�� 0 ���ϰ� �Ǿ��� �� �÷��̾� ��� ó��
        if (wave <= 0)
        {
            PlayerController player = playerTransform.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die(); // �÷��̾� ���
            }
        }
    }

    public void StopDecreaseWaveAndReset()
    {
        // ���� ���� �ڷ�ƾ�� ������ ����
        if (decreaseWaveCoroutine != null)
        {
            StopCoroutine(decreaseWaveCoroutine);
            decreaseWaveCoroutine = null;
        }


        // wave ���� 0���� �����ϰ� UI ������Ʈ
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

    // ��� ������ ���� �ʱ�ȭ �Լ�
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
