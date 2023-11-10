using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Soldier", menuName = "Soldier/Create New Soldier")]
public class Soldier : ScriptableObject
{

    public int id;
    public float health;
    public float damage;
    public int moveSpeed;
    public SoldierType soldierType;
    public enum SoldierType
    {
        viking,
    }

}
