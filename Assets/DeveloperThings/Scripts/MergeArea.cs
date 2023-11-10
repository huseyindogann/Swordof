using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class MergeArea
{
    public int areaLevel;
    public bool isSolded;
    public int id;
    public float purchaseCost;

    public MergeArea(int id)
    {
        areaLevel=1;
        isSolded = false;
        this.id=id;
        switch(id)
        {
            case 3: purchaseCost = 200; break;
            case 4: purchaseCost = 500; break;
            case 5: purchaseCost = 750; break;
            case 6: purchaseCost = 1000; break;

        }
    }

}
