using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinRewardManager : UnitySingleton<WinRewardManager>
{
    [SerializeField] private GameObject[] gobletPopUps;
    private int gobletPopUpAmount;
    private float gobletPopUpDelay;
    private void Start()
    {
        gobletPopUpDelay = 0.14f;
        gobletPopUpAmount = transform.GetChild(0).childCount;

        gobletPopUps = new GameObject[gobletPopUpAmount];

        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            gobletPopUps[i] = transform.GetChild(0).GetChild(i).gameObject;
        }

    }
    public void StartRewardingGoblet(int value)
    {
        StartCoroutine("RewardGoblet", value);

    }


    IEnumerator RewardGoblet(int rewardedGobletAmount)
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < gobletPopUpAmount; i++)
        {
            gobletPopUps[i].SetActive(true);
            gobletPopUps[i].GetComponent<GobletMove>().SetGobletText(rewardedGobletAmount / gobletPopUpAmount - Random.Range(0, 5));
            yield return new WaitForSeconds(gobletPopUpDelay);
        }
        yield return null;

    }

}
