using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData != null)
        {
            ItemImg child = GetComponentInChildren<ItemImg>();
            ItemImg ev = eventData.pointerDrag.transform.GetComponent<ItemImg>();
            Item i = ev.ItemInfo;

            if (child.ItemInfo != null)
            {
                ev.InsertItem(child.ItemInfo);
                child.InsertItem(i);
            }
            else
            {
                ev.ClearImg();
                child.InsertItem(i);
            }

            eventData.pointerDrag.transform.SetParent(this.transform);
            eventData.pointerDrag.transform.localPosition = Vector3.zero;

            child.gameObject.transform.SetParent(ev.PreParent);
            child.gameObject.transform.localPosition = Vector3.zero;
        }
    }
}
