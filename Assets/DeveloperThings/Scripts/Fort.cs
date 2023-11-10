using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Fort", menuName = "Fort/Create New Fort")]
public class Fort : ScriptableObject
{
    public float health;
    public float coolDown;
    public int id;

}
