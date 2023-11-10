using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class EquipmentSlot : MonoBehaviour
{
    enum SlotState { filled, empty }
    private MergeArea mergeArea;
    private Collider col;
    private Transform slotPointTransform;
    private Transform tabletPointTransform;
    [SerializeField] private int id;
    [SerializeField] private bool isSolded;
    private int slotLevel;
    private GameObject equipmentOnSLot;
    private GameObject tabletOnSlot;
    private TMP_Text costText;
    private float slotCost;
    private TMP_Text purchaseCostText;
    private float purchaseCost = 5000;
    private GameObject slotCanvas;
    private GameObject slotLockCanvas;

    private float changeColorDuration = 0.5f;


    

    void Start()
    {
        mergeArea = new MergeArea(id);
        mergeArea = GameManager.Instance.GetMergeAreas(id);
        slotLevel = mergeArea.areaLevel;
        purchaseCost = mergeArea.purchaseCost;
        costText = transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        slotCanvas = transform.GetChild(1).gameObject;
        purchaseCostText = transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        slotLockCanvas = transform.GetChild(2).gameObject;
        tabletPointTransform = transform.GetChild(4);
        col = transform.GetComponent<Collider>();
        slotPointTransform = transform.GetChild(0).transform;
        slotCost = 10;
        costText.text = slotCost.ToString();
        purchaseCostText.text = purchaseCost.ToString();
        if (mergeArea.isSolded)
        {
            slotLockCanvas.SetActive(false);
            slotCanvas.SetActive(true);
        }
        else
        {
            slotLockCanvas.SetActive(true);
            slotCanvas.SetActive(false);

        }
        CheckSlotState();

    }

    public void BuyEquipment()
    {
        if (GameManager.Instance.GetMoneyValue() >= slotCost)
        {
            GameManager.Instance.SpendMoney(slotCost);
            var equipment = ObjectPooler.Instance.GetEquipmentFromPool(slotLevel);
            if (equipment != null)
            {
                equipment.transform.parent = transform;
                equipment.transform.localScale = Vector3.zero;
                FillSlot(equipment);
                equipment.SetActive(true);
                SetNewEquipmentTransform(equipment, false);
            }
            var particle = ObjectPooler.Instance.GetBuyEquipmentParticlesFromPool();
            if (particle != null)
            {
                particle.transform.position = slotPointTransform.position;
                particle.SetActive(true);
            }
            Vibrator.Vibrate(50);


        }
        else
        {
            for (int i = 0; i < transform.GetChild(1).childCount; i++)
            {
                if (transform.GetChild(1).GetChild(i).GetComponent<Image>() != null)
                {
                    Image image;
                    image = transform.GetChild(1).GetChild(i).GetComponent<Image>();
                    image.DOColor(Color.red, changeColorDuration).OnComplete(() => { image.DOColor(Color.white, changeColorDuration); });


                }
                if (transform.GetChild(1).GetChild(i).GetComponent<TMP_Text>() != null)
                {
                    TMP_Text text;
                    text = transform.GetChild(1).GetChild(i).GetComponent<TMP_Text>();
                    text.DOColor(Color.red, changeColorDuration).OnComplete(() => { text.DOColor(Color.white, changeColorDuration); });

                }
            }

        }
        CheckSlotState();
    }
    public void BuySlot()
    {
        if (GameManager.Instance.GetMoneyValue() >= purchaseCost)
        {
            GameManager.Instance.SpendMoney(purchaseCost);
            mergeArea.isSolded = true;
            GameManager.Instance.SetIntoMergeAreas(mergeArea);
            CheckSlotState();
        }
        else
        {
            for (int i = 0; i < transform.GetChild(2).childCount; i++)
            {
                if (transform.GetChild(2).GetChild(i).GetComponent<Image>() != null)
                {
                    Image image;
                    image = transform.GetChild(2).GetChild(i).GetComponent<Image>();
                    image.DOColor(Color.red, changeColorDuration).OnComplete(() => { image.DOColor(Color.white, changeColorDuration); });


                }
                if (transform.GetChild(2).GetChild(i).GetComponent<TMP_Text>() != null)
                {
                    TMP_Text text;
                    text = transform.GetChild(2).GetChild(i).GetComponent<TMP_Text>();
                    text.DOColor(Color.red, changeColorDuration).OnComplete(() => { text.DOColor(Color.white, changeColorDuration); });
                }
            }

        }
        
    }

    public bool CheckSlotState()
    {
        if (equipmentOnSLot != null)
        {
            if (tabletOnSlot != null) tabletOnSlot.SetActive(false);
            tabletOnSlot = null;
            var tablet = ObjectPooler.Instance.GetTabletFromPool(equipmentOnSLot.GetComponent<EquipmentController>().GetItemLevel());
            if (tablet != null)
            {
                tablet.transform.position = tabletPointTransform.position;
                tablet.transform.rotation = tabletPointTransform.rotation;
                tabletOnSlot = tablet;
                tablet.SetActive(true);
            }
        }
        else
        {
            if (tabletOnSlot != null) tabletOnSlot.SetActive(false);
            tabletOnSlot = null;

        }
        if (mergeArea.isSolded && equipmentOnSLot != null)
        {
            slotCanvas.gameObject.SetActive(false);
            slotLockCanvas.gameObject.SetActive(false);
            col.enabled = false;
            return true;
        }
        else if (mergeArea.isSolded && equipmentOnSLot == null)
        {
            slotCanvas.gameObject.SetActive(true);
            slotLockCanvas.gameObject.SetActive(false);
            col.enabled = true;
            return false;
        }
        else if (!mergeArea.isSolded)
        {
            slotCanvas.gameObject.SetActive(false);
            slotLockCanvas.gameObject.SetActive(true);
            col.enabled = true;
            return false;

        }
        else return false;
    }
    public void EmpySlot()
    {
        equipmentOnSLot = null;
        CheckSlotState();
    }
    public void FillSlot(GameObject equipment)
    {
        // if (tabletOnSlot != null)
        // {
        //     tabletOnSlot.transform.parent = FindObjectOfType<ObjectPooler>().transform;
        //     tabletOnSlot.SetActive(false);
        //     tabletOnSlot = null;
        // }

        equipmentOnSLot = equipment;
        CheckSlotState();
    }
    public void SetNewEquipmentTransform(GameObject newEquipment, bool isSwap)
    {
        if (isSwap)
        {
            newEquipment.transform.rotation = slotPointTransform.rotation;
            newEquipment.transform.DOMove(slotPointTransform.position, 0.3f).SetEase(Ease.Linear);

        }
        else
        {
            newEquipment.transform.rotation = slotPointTransform.rotation;
            newEquipment.transform.position = slotPointTransform.position;
            newEquipment.transform.DOScale(new Vector3(0.014f, 0.014f, 0.014f), 0.4f);

        }

    }
    public bool GetAreaLockState()
    {
        if (mergeArea.isSolded) return true;
        else return false;

    }
    public MergeArea GetMergeAreaInfo() => mergeArea;
    public float GetSlotCost() => slotCost;
    public float GetPurchaseCost() => purchaseCost;






}
