using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Script assignat a InventoryPanel, HUD->InventoryPanel
//La seva funció és 'Drop' deixar caure l'objecte després del Drag 'arrossegar'


//DropHandler és una funció que té Unity per fer un Drop
public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public Inventory _Inventory;

    //Aquesta funció es crea automàticament quan afegim IDropHandler
    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;

        //Mira si el rectangle conté una imatge
        if(!RectTransformUtility.RectangleContainsScreenPoint(invPanel,
            Input.mousePosition))
        {

            InventoryItemBase item = eventData.pointerDrag.gameObject.GetComponent<ItemDragHandler>().Item;
            if(item != null)
            {
                //Si conté una imatge l'esborra
                _Inventory.RemoveItem(item);
                item.OnDrop();
            }

        }
    }
}
