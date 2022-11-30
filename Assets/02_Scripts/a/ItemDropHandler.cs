using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler //아이템 드롭, 떨구기
{
    public Inventory _Inventory;
    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;

        if(!RectTransformUtility.RectangleContainsScreenPoint(invPanel,Input.mousePosition))
        {
            InventoryItemBase item = (InventoryItemBase)eventData.pointerDrag.gameObject.GetComponent<ItemDragHandler>().Item;;
            if(item != null)
            {
                _Inventory.RemovedItem(item);
                item.Ondrop();
            }
            
        }
    }
}
