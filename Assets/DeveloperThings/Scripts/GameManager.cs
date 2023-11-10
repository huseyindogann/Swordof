using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Linq;

public class GameManager : UnitySingleton<GameManager>
{

    public enum Winner { Player, Enemy }
    public static GameData gameData;
    public static event Action SetGameData;
    public static event Action LoadGameData;
    public Winner gameWinner;
    private float playerMoney;
    private int difficultyTier;
    [SerializeField]private int playerLevel=0;
    private int lastSceneIndex;
    private int playerGoblet;
    private bool isGameOver = false;
    private bool isGameGoing = false; 
    private int mergedEquipment = 0;
    private Vector3 camOriginalPos;
    private Camera cam;
    public float shakeAmount = 0.7f;
    [SerializeField] private List<MergeArea> mergeSlots= new List<MergeArea>();
    [SerializeField] private List<FortStats> fortStats= new List<FortStats>();
    [SerializeField] private GameObject[] allItemTypes;
    [SerializeField] private int soldierEquipped = 0;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text gobletText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text winPanelGobletText;
    [SerializeField] private Transform moneyIconTransform;
    [SerializeField] private Transform gobletIconTransform;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject failPanel;
    [SerializeField] private GameObject playerPathObject;
    [SerializeField] private GameObject enemyPathObject;
    [SerializeField] private GameObject playerFortObject;
    [SerializeField] private GameObject enemyFortObject;
   




    private void OnEnable()
    {
        cam = Camera.main;
        camOriginalPos = cam.transform.position;
        LoadGameData += LoadGameDatas;
        SetGameData += SetGameDatas;
    }
    private void Awake()
    {
        CreateListsToSave();
        LoadJSONDatas();
        
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
       lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
        gameData = new GameData();
        gameData = SaveSystem.Load(gameData);
        LoadGameData?.Invoke();
        //gameData.lastSceneIndex = lastSceneIndex;
        Debug.Log(lastSceneIndex);
       
        if (playerMoney < 40)
        {
            playerMoney = 40;
        }

        SetDifficultyTier();
        
        isGameOver = false;
        isGameGoing = false;
        YsoCorp.GameUtils.YCManager.instance.OnGameStarted(playerLevel);
    }

    private void SetGameDatas()
    {
        gameData.playerMoney = playerMoney;
        gameData.playerLevel = playerLevel;
        gameData.playerGoblet = playerGoblet;
        gameData.lastSceneIndex = lastSceneIndex;


    }
    private void LoadGameDatas()
    {
        playerMoney = gameData.playerMoney;
        playerLevel = gameData.playerLevel;
        playerGoblet = gameData.playerGoblet;
        lastSceneIndex=gameData.lastSceneIndex;
    }

    private void Update()
    {
        if(isGameGoing)
        {
            SetGameData?.Invoke();
            SaveSystem.Save(gameData);
            SavaJSONDatas();
            moneyText.text = Mathf.RoundToInt(playerMoney).ToString();
            levelText.text = "LEVEL" + playerLevel.ToString();

        }
    }


    public void FinishGame()
    {
        isGameOver = true;
        if (gameWinner == Winner.Player)
        {
            winPanel.SetActive(true);
            winPanelGobletText.text = (100 * playerLevel).ToString();
            WinRewardManager.Instance.StartRewardingGoblet(100 * playerLevel);
            YsoCorp.GameUtils.YCManager.instance.OnGameFinished(true);
        }
        else
        {
            YsoCorp.GameUtils.YCManager.instance.OnGameFinished(false);
            failPanel.SetActive(true);

        } 
    }
    private void SetDifficultyTier()
    {
        if (playerLevel < 5) difficultyTier = 1;
        else if (playerLevel >= 5 && playerLevel < 11) difficultyTier = 2;
        else if (playerLevel > 10) difficultyTier = 3;
    }
    IEnumerator EarnMoneyAnimated(float value)
    {
        Color color;
        color = moneyText.color;
        moneyText.color = Color.green;
        for (int i = 0; i < 50; i++)
        {
            playerMoney += value / 50;
            yield return new WaitForSeconds(0.01f);

        }
        moneyText.color = color;
        yield return null;
    }
    public void ShakeCamera()
    {
        StartCoroutine("ShakeCam");

    }
    private void SavaJSONDatas()
    {
        SaveSystem.SaveToJSON(mergeSlots, "mergeSlots.json");
        SaveSystem.SaveToJSON(fortStats, "fortStats.json");

    }
    private void LoadJSONDatas()
    {
        
        if(SaveSystem.ReadListFromJSON<MergeArea>("mergeSlots.json")!=null)
        {

            mergeSlots = SaveSystem.ReadListFromJSON<MergeArea>("mergeSlots.json");
            fortStats = SaveSystem.ReadListFromJSON<FortStats>("fortStats.json");

        }
        else 
        {
            Debug.Log("First Open");
        }
        

    }
    IEnumerator ShakeCam()
    {
        float shakeDuration = 0.3f;
        float shakeAmount = 0.1f;

        while (shakeDuration > 0)
        {
            cam.transform.localPosition = camOriginalPos + UnityEngine.Random.insideUnitSphere * shakeAmount;
            shakeDuration -= 0.1f;
            yield return new WaitForSeconds(0.1f);


        }
        cam.transform.localPosition = camOriginalPos;
        yield return null;


    }
    public int GetDifficultyTier() => difficultyTier;
    public Transform GetMoneyIconTransform() => moneyIconTransform;
    public Transform GetGobletIconTransform() => gobletIconTransform;
    public bool IsGameOver() => isGameOver;
    public bool IsGameGoing() => isGameGoing;
    public MergeArea GetMergeAreas(int id)
    {
        for(int i = 0;i<mergeSlots.Count;i++) 
        {
            if(mergeSlots.ElementAt(i).id == id) return mergeSlots.ElementAt(i);
            
            
        }
        return null;
    

    }
    public FortStats GetFortStats(int id)
    {
        for (int i = 0; i < fortStats.Count; i++)
        {
            if (fortStats.ElementAt(i).id == id) return fortStats.ElementAt(i);


        }
        return null;


    }
    public void SetIntoMergeAreas(MergeArea mergeArea)
    {
        Debug.Log(mergeArea.id);
        bool isAlreadyhave=false;
        for(int i = 0;i<mergeSlots.Count;i++) 
        {

            if (mergeSlots.ElementAt(i).id == mergeArea.id)
            {
                mergeSlots[i] = mergeArea;
                isAlreadyhave = true;
            } 
        }

        if (!isAlreadyhave)
        {
            mergeSlots.Add(mergeArea);
        }
    }
    public void SetIntoFortStatsTheFort(FortStats fortStat)
    {
        bool isAlreadyhave = false;
        for (int i = 0; i < fortStats.Count; i++)
        {
            
            if (fortStats.ElementAt(i).id == fortStat.id)
            {
                fortStats[i] = fortStat;
                isAlreadyhave = true;
            }
            
        }

        if (!isAlreadyhave)
        {
            fortStats.Add(fortStat);
        }
    }
    public Winner GetGameWinner() => gameWinner;
    public void SetGameState(bool value) => isGameGoing = value;
    public void IncreaseLevel() => playerLevel++;
    public void SpendMoney(float value) => playerMoney -= value;
    public int GetLastSceneIndex() => lastSceneIndex;
    public float GetMoneyValue() => playerMoney;
    public void EarnMoney(float value) => playerMoney += value;
    public void EarnMoneyAnim(float value) => StartCoroutine("EarnMoneyAnimated", value);
    public int GetPlayerLevel() => playerLevel;
    public void IncreaseMergedEquipment() => mergedEquipment++;
    public void IncreaseEquippedSoldier() => soldierEquipped++;
    public int GetMergedEquipment() => mergedEquipment;
    public int GetEquippedSoldier() => soldierEquipped;
    public GameObject GetPlayerPathObject() => playerPathObject;
    public GameObject GetEnemyPathObject() => enemyPathObject;
    public GameObject GetPlayerFortObject() => playerFortObject;
    public GameObject GetEnemyFortObject() => enemyFortObject;
    public GameObject[] GetAllItemTypes() => allItemTypes;
    private void CreateListsToSave()
    {
        //mergeSlots = new List<MergeArea>();
        for (int i = 1; i < 7; i++)
        {
         
            MergeArea mergeArea = new MergeArea(i);
            mergeSlots.Add(mergeArea);
        }
        //fortStats = new List<FortStats>();
        for (int i = 1; i < 3; i++)
        {
            FortStats fortStat = new FortStats(i);
            fortStats.Add(fortStat);
        }

    }
    public void unlimitedmoney()
    {
        playerMoney = 999999999;
    }
    public void limitedmoney()
    {
        playerMoney = 500;
    }


}
