using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialManager : UnitySingleton<TutorialManager>
{
    public GameObject tutorialCanvas;
    public GameObject firstBuyPanel;
    public GameObject firstEquipPanel;
    public GameObject secondBuyFirstPanel;
    public GameObject secondBuySecondPanel;
    public GameObject showMergePanel;
    public GameObject secondEquipPanel1;
    public GameObject secondEquipPanel2;
    public GameObject levelCanvas;
    private GameObject toClosePanel;
    public EquipmentSlot slotForFirstBuy;
    public EquipmentSlot slotForSecondBuy;
    private bool is0LevelTutorialPlayed;
    private bool isFirstEquipComplete = false;
    private bool isFirstMergeComplete = false;
    private bool isSecondEquipComplete = false;
    private bool isFirstEnemySoldierDied = false;
    [SerializeField] private GameObject[] equipmentSlots;


    private void Start()
    {
        


        if (GameManager.Instance.GetPlayerLevel() == 1)
        {
            GameManager.Instance.EarnMoney(slotForFirstBuy.GetPurchaseCost());
            slotForFirstBuy.BuySlot();
            GameManager.Instance.EarnMoney(slotForSecondBuy.GetPurchaseCost());
            slotForSecondBuy.BuySlot();
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                equipmentSlots[i].GetComponent<Collider>().enabled = false;
            }
            GameManager.Instance.SetGameState(true);
            tutorialCanvas.SetActive(true);
            Start0LevelTutorial();

        }
    }

    #region 0 level tutorial

    private void Start0LevelTutorial()
    {
        

        firstBuyPanel.SetActive(true);
    }
    public void BuyFirstEquipment()
    {
        GameManager.Instance.EarnMoney(slotForFirstBuy.GetSlotCost());
        slotForFirstBuy.BuyEquipment();
        AudioManager.Instance.PlaySFX("BuyEquipment");
        firstBuyPanel.SetActive(false);
        firstEquipPanel.SetActive(true);
        StartCoroutine("WaitForFirstMove");
    }
    IEnumerator WaitForFirstMove()
    {
        while (!isFirstEquipComplete)
        {
            if (GameManager.Instance.GetEquippedSoldier() > 0)
            {
                isFirstEquipComplete = true;
                firstEquipPanel.SetActive(false);
                slotForFirstBuy.GetComponent<Collider>().enabled = false;
            }

            yield return null;
        }

        while (!isFirstEnemySoldierDied)
        {
            yield return null;
        }
        GameManager.Instance.SetGameState(false);
        secondBuyFirstPanel.SetActive(true);
        GameManager.Instance.EarnMoney(slotForFirstBuy.GetSlotCost());
        GameManager.Instance.EarnMoney(slotForSecondBuy.GetSlotCost());
    }
    public void SecondBuyFirstEquipment()
    {

        slotForFirstBuy.BuyEquipment();
        AudioManager.Instance.PlaySFX("BuyEquipment");
        secondBuyFirstPanel.SetActive(false);
        secondBuySecondPanel.SetActive(true);
    }
    public void SecondBuySecondEquipment()
    {

        slotForSecondBuy.BuyEquipment();
        AudioManager.Instance.PlaySFX("BuyEquipment");
        secondBuySecondPanel.SetActive(false);
        showMergePanel.SetActive(true);
        StartCoroutine("CheckMergeState");

    }
    IEnumerator CheckMergeState()
    {


        GameManager.Instance.SetGameState(true);
        while (!isFirstMergeComplete)
        {
            if (GameManager.Instance.GetMergedEquipment() > 0)
            {
                isFirstMergeComplete = true;
                showMergePanel.SetActive(false);

            }
            yield return null;
        }
        slotForFirstBuy.GetComponent<Collider>().enabled = false;
        slotForSecondBuy.GetComponent<Collider>().enabled = false;

        if (slotForFirstBuy.CheckSlotState())
        {
            secondEquipPanel1.SetActive(true);
            toClosePanel = secondEquipPanel1;

        }
        if (slotForSecondBuy.CheckSlotState())
        {
            secondEquipPanel2.SetActive(true);
            toClosePanel = secondEquipPanel2;

        }

        while (!isSecondEquipComplete)
        {
            if (GameManager.Instance.GetEquippedSoldier() > 1)
            {
                isSecondEquipComplete = true;
                toClosePanel.SetActive(false);
            }
            yield return null;
        }
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].GetComponent<Collider>().enabled = true;
        }
        levelCanvas.transform.DOScale(new Vector3(3f, 3f, 3f), 0.3f);
        is0LevelTutorialPlayed=true;
        yield return null;


    }





    public void SetFirstMergeState(bool value) => isFirstMergeComplete = value;
    public void SetFirstEquipState(bool value) => isFirstEquipComplete = value;
    public void SetSecondEquipState(bool value) => isSecondEquipComplete = value;
    public void SetFirstEnemySoldierState(bool value) => isFirstEnemySoldierDied = value;
    public bool GetFirstMergeState() => isFirstMergeComplete;
    public bool GetIs0LevelTutorialPlayed() => is0LevelTutorialPlayed;
    public bool GetFirstEquipState() => isFirstEquipComplete;
    public bool GetSecondEquipState() => isSecondEquipComplete;
    public bool GetFirstEnemySoldierState() => isFirstEnemySoldierDied;


    #endregion


}
