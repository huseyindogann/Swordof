using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using RayFire;
using TMPro;

public class FortController : MonoBehaviour
{
    private GameObject defaultBlueVikingSoldier;
    [SerializeField] private GameObject defaultBlueVikingSoldierPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] queuePoints;
    [SerializeField] private GameObject[] soldiersInQueue;
    [SerializeField] private GameObject[] rayFireElements;
    [SerializeField] private Transform upgradeEffectTransform;
    private TMP_Text upgradeCostText;
    private GameObject upgradeMoneyIcon;
    private Button upgradeButton;
    private float changeColorDuration = 0.5f;
    private int maxQueueSize;
    private int queueSize;
    private float upgradeCost=0;
    private float healthUpgradeAmount = 60;
    private float coolDownUpgradeAmount = 1f;
    public Fort fort;
    private FortStats stats;
    [SerializeField] private int level;
    private int maxLevel = 3;

    [SerializeField]private float health;
    [SerializeField]private float coolDown;
    public Image healthBar;
    public Image coolDownBar;
    private int equippedCounter;
    private int equippedCounterMax;


    private void Start()
    {
        stats = new FortStats(fort.id);
        FortStats _stats = GameManager.Instance.GetFortStats(fort.id);
        if (stats != null) stats = _stats;
        level=stats.level;
        upgradeCost = SetUpgradeCost();
        if (transform.CompareTag("PlayerFort"))
        {
            upgradeEffectTransform = transform.GetChild(6).transform;
            upgradeCostText = transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            upgradeButton = transform.GetChild(5).GetChild(0).GetComponent<Button>();
            upgradeCostText.text=upgradeCost.ToString();
            upgradeMoneyIcon = transform.GetChild(5).GetChild(1).gameObject;
            if (level == maxLevel)
            {
                upgradeCostText.text = "MAX LEVEL";
                upgradeButton.enabled = false;
                upgradeMoneyIcon.SetActive(false);
            }
        } 
        SetDifficultyCounter();
        transform.GetComponent<MeshRenderer>().enabled = false;
        coolDown = SetCoolDown();
        health = SetHealth();
        if(GameManager.Instance.GetPlayerLevel()>10&&transform.CompareTag("EnemyFort"))
        {
            coolDown = 8;
            health=300;
        }
        healthBar.fillAmount = health / fort.health;
        maxQueueSize = transform.GetChild(0).childCount;
        soldiersInQueue = new GameObject[maxQueueSize];
        queuePoints = new Transform[maxQueueSize];
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            queuePoints[i] = transform.GetChild(0).GetChild(i);
        }
        queueSize = 0;
        if (GameManager.Instance.GetPlayerLevel() > 1)
        {


            if (transform.CompareTag("EnemyFort"))
            {

                StartCoroutine("EnemyFortSpawner");

            }
            if (transform.CompareTag("PlayerFort"))
            {
                StartCoroutine("PlayerFortBehaviour");
            }
        }
        else
        {
            
            if (transform.CompareTag("EnemyFort"))
            {
                StartCoroutine("EnemyFortTutorial");

            }
            if (transform.CompareTag("PlayerFort"))
            {
                Debug.Log("PlayerFortTutorialStarting..");
                StartCoroutine("PlayerFortTutorial");
            }
        }
        GameManager.Instance.SetGameState(true);
    }

    public void UpgradeFort()
    {
        if(GameManager.Instance.GetMoneyValue()>=upgradeCost && level<maxLevel)
        {
            level += 1;
            stats.level = level;
            UpgradeFortEffects();
            GameManager.Instance.SpendMoney(upgradeCost);
            upgradeCost = SetUpgradeCost();
            upgradeCostText.text = upgradeCost.ToString();
            if(level==maxLevel)
            {
                upgradeCostText.text = "MAX LEVEL";
                upgradeButton.enabled = false;
                upgradeMoneyIcon.SetActive(false);
            }
            coolDown = SetCoolDown();
            health = SetHealth();
            healthBar.fillAmount = health / fort.health;
        }
        else
        {
            for (int i = 0; i < transform.GetChild(5).childCount; i++)
            {
                if (transform.GetChild(5).GetChild(i).GetComponent<Image>() != null)
                {
                    Image image;
                    image = transform.GetChild(5).GetChild(i).GetComponent<Image>();
                    image.DOColor(Color.red, changeColorDuration).OnComplete(() => { image.DOColor(Color.white, changeColorDuration); });


                }
                if (transform.GetChild(5).GetChild(i).GetComponent<TMP_Text>() != null)
                {
                    TMP_Text text;
                    text = transform.GetChild(5).GetChild(i).GetComponent<TMP_Text>();
                    text.DOColor(Color.red, changeColorDuration).OnComplete(() => { text.DOColor(Color.white, changeColorDuration); });

                }
            }
        }

    }
    private float SetUpgradeCost()=> (level * 1000) + 1000;
    private float SetCoolDown()=> fort.coolDown - level * coolDownUpgradeAmount;
    private float SetHealth()=> fort.health + level * healthUpgradeAmount;

    private void UpgradeFortEffects()
    {
        var upgradeEffect = ObjectPooler.Instance.GetUpgradeFortParticlesFromPool();
        if (upgradeEffect != null)
        {
            upgradeEffect.transform.position = upgradeEffectTransform.position;
            upgradeEffect.SetActive(true);
        }
        var upgradePopUp = ObjectPooler.Instance.GetFortUpgradePopUp();
        if (upgradePopUp != null)
        {
            upgradePopUp.transform.position = new Vector3(upgradeEffectTransform.position.x, upgradeEffectTransform.position.y + 8, upgradeEffectTransform.position.z);
            upgradePopUp.SetActive(true);
            Vector3 targetPos = new Vector3(upgradePopUp.transform.position.x, upgradePopUp.transform.position.y + 5, upgradePopUp.transform.position.z);
            upgradePopUp.transform.DOMove(targetPos, 0.7f).OnComplete(() =>
            {
                upgradePopUp.SetActive(false);
            });
        }

    }

    public void BuyVikingSoldier()
    {
        if (queueSize < maxQueueSize)
        {
            defaultBlueVikingSoldier = Instantiate(defaultBlueVikingSoldierPrefab, spawnPoint.position, spawnPoint.rotation);
            defaultBlueVikingSoldier.transform.parent = transform;
            soldiersInQueue[maxQueueSize - queueSize - 1] = defaultBlueVikingSoldier;
            defaultBlueVikingSoldier.GetComponent<SoldierController>().SetQueueNumber(maxQueueSize - queueSize - 1);
            queueSize++;

        }
    }
    public int GetQueueAmount() => queueSize;
    public Transform GetQueuePoint(int index) => queuePoints[index];
    public void EquipSoldier(Item _item)
    {
        GameObject firstSoldier = soldiersInQueue[maxQueueSize - 1];
        if (firstSoldier != null && _item!=null)
        {
            firstSoldier.GetComponent<SoldierController>().TakeUpArms(_item);
            MoveTheQueue();

        }
        if (GameManager.Instance.GetPlayerLevel() == 1 && transform.CompareTag("PlayerFort"))
        {
            GameManager.Instance.IncreaseEquippedSoldier();
        }

    }

    private void MoveTheQueue()
    {

        for (int i = 0; i < maxQueueSize; i++)
        {
            if (soldiersInQueue[i] != null && i != maxQueueSize - 1)
            {
                soldiersInQueue[i + 1] = soldiersInQueue[i];
                soldiersInQueue[i].GetComponent<SoldierController>().IncreaseQueueNumber();
                soldiersInQueue[i] = null;

            }
            else if (queueSize == 1) soldiersInQueue[i] = null;

        }
        queueSize--;
    }
    public bool CheckQueue()
    {
        if (queueSize > 0) return true;
        else return false;
    }
    private void SetDifficultyCounter()
    {
        int difficultyTier = GameManager.Instance.GetDifficultyTier();
        if (difficultyTier == 1)
        {
            equippedCounter = 6;
            equippedCounterMax = equippedCounter;
        }
        if (difficultyTier == 2)
        {
            equippedCounter = 6;
            equippedCounterMax = equippedCounter;
        }
        if (difficultyTier == 3)
        {
            equippedCounter = 3;
            equippedCounterMax = equippedCounter;
        }

    }

    IEnumerator EnemyEquiper()
    {

        GameObject[] items = GameManager.Instance.GetAllItemTypes();
        int difficultyTier = GameManager.Instance.GetDifficultyTier();
        Item item;
        yield return new WaitForSeconds(0.5f);
        #region oldEnemyEquiper
        //if (GameManager.Instance.GetPlayerLevel() <= 1)
        //{
        //    item = items[1].GetComponent<EquipmentController>().item;

        //    EquipSoldier(item);

        //}
        //else
        //{
        //    if (difficultyTier == 1)
        //    {


        //        if (equippedCounter > 0)
        //        {
        //            int rand = Random.Range(1, 3);
        //            item = items[rand].GetComponent<EquipmentController>().item;
        //            EquipSoldier(item);
        //            yield return null;
        //            equippedCounter--;
        //        }
        //        else
        //        {
        //            item = items[3].GetComponent<EquipmentController>().item;
        //            EquipSoldier(item);
        //            yield return null;
        //            equippedCounter = equippedCounterMax;

        //        }
        //        yield return null;
        //    }
        //    if (difficultyTier == 2)
        //    {


        //        if (equippedCounter > 0)
        //        {
        //            int rand = Random.Range(1, 4);
        //            item = items[rand].GetComponent<EquipmentController>().item;
        //            EquipSoldier(item);
        //            yield return null;
        //            equippedCounter--;

        //        }
        //        else
        //        {
        //            item = items[3].GetComponent<EquipmentController>().item;
        //            EquipSoldier(item);
        //            yield return null;
        //            equippedCounter = equippedCounterMax;

        //        }
        //        yield return null;
        //    }
        //    if (difficultyTier == 3)
        //    {

        //        if (equippedCounter > 0)
        //        {
        //            int rand = Random.Range(2, 5);
        //            item = items[rand].GetComponent<EquipmentController>().item;
        //            EquipSoldier(item);
        //            yield return null;
        //            equippedCounter--;

        //        }
        //        else
        //        {
        //            int rand = Random.Range(5, 7);
        //            item = items[rand].GetComponent<EquipmentController>().item;
        //            EquipSoldier(item);
        //            yield return null;
        //            equippedCounter = equippedCounterMax;

        //        }
        //        yield return null;
        //    }



        //}
        #endregion
        item = GetItemForEnemy();
        EquipSoldier(item);

    }

    private Item GetItemForEnemy()
    {
        GameObject[] items = GameManager.Instance.GetAllItemTypes();
        Item item=null;
        int playerLevel = GameManager.Instance.GetPlayerLevel();
        if (playerLevel == 1 || playerLevel==2)
        {

            item= items[Random.Range(1,3)].GetComponent<EquipmentController>().item;

        }
        else if(playerLevel == 3 || playerLevel ==4)
        {
            item = items[Random.Range(1, 4)].GetComponent<EquipmentController>().item;

        }
        else if (playerLevel == 5 || playerLevel == 6)
        {
            item = items[Random.Range(1, 5)].GetComponent<EquipmentController>().item;

        }
        else if (playerLevel == 7 || playerLevel == 8)
        {
            item = items[Random.Range(1, 6)].GetComponent<EquipmentController>().item;

        }
        else if (playerLevel == 9 || playerLevel == 10)
        {
            item = items[Random.Range(1, 7)].GetComponent<EquipmentController>().item;

        }
        else if (playerLevel >10)
        {
            item = items[Random.Range(1, 7)].GetComponent<EquipmentController>().item;

        }
       
        return item;

    }

    IEnumerator EnemyFortSpawner()
    {
        while (!GameManager.Instance.IsGameOver())
        {
            if (GameManager.Instance.IsGameGoing())
            {
                float timer = coolDown;

                if (queueSize < maxQueueSize)
                {
                    while (timer > 0)
                    {

                        yield return new WaitForSeconds(0.05f);
                        timer -= 0.05f;
                    }
                    BuyVikingSoldier();
                    StartCoroutine("EnemyEquiper");
                }
            }
            yield return null;
        }
    }
    IEnumerator PlayerFortBehaviour()
    {
        while (!GameManager.Instance.IsGameOver())
        {
            if (GameManager.Instance.IsGameGoing())
            {
                float timer = coolDown;
                float maxCooldown = coolDown;
                if (queueSize < maxQueueSize)
                {
                    while (timer > 0)
                    {
                        coolDownBar.fillAmount = timer / maxCooldown;
                        yield return new WaitForSeconds(0.05f);
                        timer -= 0.05f;
                    }
                    BuyVikingSoldier();
                }
                else coolDownBar.fillAmount = timer / maxCooldown;

            }
            yield return null;
        }
    }
    public void TakeDamage(float damage)
    {
        StartCoroutine("TakeDamageAnimated", damage);
        Vibrator.Vibrate(100);
        if (transform.CompareTag("PlayerFort"))
        {
            
            GameManager.Instance.ShakeCamera();
        }

    }
    IEnumerator TakeDamageAnimated(float damage)
    {
        for (int i = 0; i < 3; i++)
        {
            health -= damage / 3;
            healthBar.fillAmount = health / fort.health;
            yield return new WaitForSeconds(0.03f);

        }
        //AudioManager.Instance.PlaySFX("HitFort");
        var particle = ObjectPooler.Instance.GetHitParticlesFromPool();
        if (particle != null)
        {
            particle.transform.position = transform.position;
            particle.SetActive(true);

        }
        for (int i = 0; i < rayFireElements.Length; i++)
        {
            rayFireElements[i].transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);

        }
        if (health <= 1)
        {
            for (int i = 0; i < rayFireElements.Length; i++)
            {
                rayFireElements[i].GetComponent<Rigidbody>().isKinematic = false;

            }


            if (transform.CompareTag("EnemyFort")) GameManager.Instance.gameWinner = GameManager.Winner.Player;
            else GameManager.Instance.gameWinner = GameManager.Winner.Enemy;
            AudioManager.Instance.PlaySFX("FortDown");
            GameManager.Instance.FinishGame();
        }
    }

    IEnumerator PlayerFortTutorial()
    {

        BuyVikingSoldier();
        while (!TutorialManager.Instance.GetFirstMergeState())
        {
            yield return null;
        }
        BuyVikingSoldier();
        StartCoroutine("PlayerFortBehaviour");
    }
    IEnumerator EnemyFortTutorial()
    {
        Item item;
        GameObject[] items = GameManager.Instance.GetAllItemTypes();
        item = items[1].GetComponent<EquipmentController>().item;
        while (!TutorialManager.Instance.GetFirstEquipState())
        {
          
            yield return null;
        }
        BuyVikingSoldier();
        yield return new WaitForSeconds(1.5f);
        EquipSoldier(item);
        while (!TutorialManager.Instance.GetSecondEquipState())
        {
            yield return null;
        }
        BuyVikingSoldier();
        EquipSoldier(item);
        StartCoroutine("EnemyFortSpawner");

    }
}
