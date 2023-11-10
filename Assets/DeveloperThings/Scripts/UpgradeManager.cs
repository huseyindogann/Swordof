using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public void UpgradePlayerFort()
    {
        FortController fort=transform.parent.GetComponent<FortController>();
        fort.UpgradeFort();
    }
}
