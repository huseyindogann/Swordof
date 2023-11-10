using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    public Item item;
    public int GetItemLevel() => item.itemLevel;

    [SerializeField] private Transform equipmentBase;


    private void OnEnable()
    {
        equipmentBase = GameObject.FindWithTag("NonUseEquipmentPoint").transform;
    }
    private void OnDisable()
    {
        transform.position = equipmentBase.position;
    }


}
