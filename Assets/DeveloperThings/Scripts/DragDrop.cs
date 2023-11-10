using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragDrop : MonoBehaviour
{
    private Camera cam;
    private GameObject toDrag;
    private Collider col;
    [SerializeField] private GameObject toMerge;
    private Transform toDragStartPos;
    private float toDragY;
    private bool isDrag;
    private Vector3 offset;
    void Start()
    {
        cam = Camera.main;

    }
    void Update()
    {
        if (!GameManager.Instance.IsGameOver() && GameManager.Instance.IsGameGoing())
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = cam.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
                    {
                        if (hitInfo.transform.gameObject.CompareTag("Dragable"))
                        {
                            toDrag = hitInfo.transform.gameObject;
                            toDragStartPos = toDrag.transform;
                            toDragY = toDragStartPos.position.y;
                            col = toDrag.GetComponent<Collider>();
                            col.enabled = false;
                            offset = new Vector3(touch.position.x, touch.position.y, 0) - cam.WorldToScreenPoint(toDrag.transform.position);
                            isDrag = true;
                            StartCoroutine("SpinObject", toDrag);
                        }
                        if (hitInfo.transform.gameObject.CompareTag("EquipmentSlot"))
                        {
                            EquipmentSlot slot = hitInfo.transform.GetComponent<EquipmentSlot>();
                            MergeArea slotMergeArea = slot.GetMergeAreaInfo();
                            if (slotMergeArea.isSolded)
                            {
                                AudioManager.Instance.PlaySFX("BuyEquipment");
                                slot.BuyEquipment();
                            }
                            else slot.BuySlot();
                        }
                    }
                }
                if (isDrag && touch.phase == TouchPhase.Moved)
                {

                    toDrag.transform.position = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0) - offset);
                    toDrag.transform.position = new Vector3(toDrag.transform.position.x, toDragY, toDrag.transform.position.z);

                }
                if (isDrag && touch.phase == TouchPhase.Ended)
                {
                    Ray ray2 = cam.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray2, out RaycastHit hitInfo, Mathf.Infinity) && hitInfo.transform.gameObject.CompareTag("Dragable"))
                    {
                        toMerge = hitInfo.transform.gameObject;
                        EquipmentController toDragEquipment = toDrag.GetComponent<EquipmentController>();
                        EquipmentController toMergeEquipment = toMerge.GetComponent<EquipmentController>();
                        if (toDragEquipment.item.itemLevel == toMergeEquipment.item.itemLevel && toMergeEquipment.item.itemLevel != 6)
                            Merge(toMerge, toDrag, toDragEquipment.item.itemLevel + 1);
                        else if (toDragEquipment.item.itemLevel != toMergeEquipment.item.itemLevel || toMergeEquipment.item.itemLevel == 6)
                            Swap(toMerge, toDrag);

                    }
                    else if (Physics.Raycast(ray2, out RaycastHit hitInfo2, Mathf.Infinity) && hitInfo2.transform.gameObject.CompareTag("EquipmentSlot"))
                    {
                        EquipmentSlot slot = hitInfo2.transform.GetComponent<EquipmentSlot>();
                        if (slot.GetMergeAreaInfo().isSolded)
                        {
                            slot = toDrag.transform.parent.GetComponent<EquipmentSlot>();
                            slot.EmpySlot();
                            slot = hitInfo2.transform.GetComponent<EquipmentSlot>();
                            slot.FillSlot(toDrag);
                            slot.SetNewEquipmentTransform(toDrag, true);
                            toDrag.transform.parent = hitInfo2.transform;
                        }
                        else RelocateStartPos(toDrag);

                    }
                    else if (Physics.Raycast(ray2, out RaycastHit hitInfo3, Mathf.Infinity) && hitInfo2.transform.gameObject.CompareTag("PlayerFort") && hitInfo.transform.GetComponent<FortController>().CheckQueue())
                    {
                        AudioManager.Instance.PlaySFX("Equip");
                        EquipmentController draggedEquipment = toDrag.GetComponent<EquipmentController>();
                        EquipmentSlot slot = toDrag.transform.parent.GetComponent<EquipmentSlot>();
                        slot.EmpySlot();
                        hitInfo.transform.GetComponent<FortController>().EquipSoldier(draggedEquipment.item);
                        toDrag.transform.parent = FindObjectOfType<ObjectPooler>().transform;
                        toDrag.SetActive(false);

                    }
                    else RelocateStartPos(toDrag);
             
                    col.enabled = true;
                    isDrag = false;
                    toDrag = null;
                    toMerge = null;
                    StopAllCoroutines();
                }
            }
        }
    }
    private void Merge(GameObject toMerge, GameObject toDrag, int newLevel)
    {

        var newEquipment = ObjectPooler.Instance.GetEquipmentFromPool(newLevel);
        if (newEquipment != null)
        {
            newEquipment.transform.position = toMerge.transform.position;
            newEquipment.transform.rotation = toMerge.transform.rotation;
            newEquipment.transform.parent = toMerge.transform.parent;
            newEquipment.transform.localScale = Vector3.zero;
            newEquipment.SetActive(true);
            newEquipment.transform.DOScale(new Vector3(0.014f, 0.014f, 0.014f), 0.4f);
        }
        var particle = ObjectPooler.Instance.GetMergeEffects(newLevel);
        if (particle != null)
        {
            particle.transform.position = toMerge.transform.position;
            particle.SetActive(true);

        }
        EquipmentSlot slot = toDrag.transform.parent.GetComponent<EquipmentSlot>();
        slot.EmpySlot();
        slot = toMerge.transform.parent.GetComponent<EquipmentSlot>();
        slot.FillSlot(newEquipment);
        toDrag.transform.parent = FindObjectOfType<ObjectPooler>().transform;
        toMerge.transform.parent = FindObjectOfType<ObjectPooler>().transform;
        toDrag.SetActive(false);
        toMerge.SetActive(false);
        GameManager.Instance.IncreaseMergedEquipment();
        AudioManager.Instance.PlaySFX("Merge");
        Vibrator.Vibrate(50);


    }
    private void Swap(GameObject toMerge, GameObject toDrag)
    {
        Transform toMergeParent = toMerge.transform.parent;
        EquipmentSlot slot = toDrag.transform.parent.GetComponent<EquipmentSlot>();
        slot.SetNewEquipmentTransform(toMerge, true);
        slot.FillSlot(toMerge);
        toMerge.transform.parent = toDrag.transform.parent;
        ///////////////////////////////
        slot = toMergeParent.GetComponent<EquipmentSlot>();
        slot.SetNewEquipmentTransform(toDrag, true);
        slot.FillSlot(toDrag);
        toDrag.transform.parent = toMergeParent;
        AudioManager.Instance.PlaySFX("FailDrop");
    }
    private void RelocateStartPos(GameObject toDrag)
    {
        EquipmentSlot slot = toDrag.transform.parent.GetComponent<EquipmentSlot>();
        slot.SetNewEquipmentTransform(toDrag, true);
        AudioManager.Instance.PlaySFX("FailDrop");

    }

    IEnumerator SpinObject(GameObject x)
    {
        while (true)
        {
            x.transform.Rotate(0f, 90 * Time.deltaTime, 0f, Space.Self);
            yield return null;
        }
    }
}
