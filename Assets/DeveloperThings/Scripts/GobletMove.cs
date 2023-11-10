using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class GobletMove : MonoBehaviour
{
    private Transform moneyIconTransform;
    private TMP_Text moneyText;
    private float moneyValue;
    private Vector3 startedScale;


    private void OnEnable()
    {
        startedScale = Vector3.zero;
        moneyText = transform.GetChild(1).GetComponent<TMP_Text>();
        moneyIconTransform = GameManager.Instance.GetMoneyIconTransform();
        StartCoroutine("StartMove");



    }

    IEnumerator StartMove()
    {
        transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 1.5f);
        yield return new WaitForSeconds(0.5f);
        transform.DOMove(moneyIconTransform.position, 1.2f).OnComplete(() =>
        {
            transform.DOScale(startedScale, 0.5f);
            gameObject.SetActive(false);
            GameManager.Instance.EarnMoneyAnim(moneyValue);
        });


    }


    public void SetGobletText(int value)
    {
        moneyValue=value;
        moneyText.text = value.ToString();
    } 


}
