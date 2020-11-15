using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Script assignat a els 9 ItemImage. HUD->InventoryPanel->Slot->Border->ItemImage
//La seva funció és 'Drag' arrosegar la imatge del objecte

    
//IDragHandler i IEndDragHandler son dues funcions que té Unity per fer un Drag
public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler {

    public InventoryItemBase Item { get; set; }


    //Aquesta funció es crea automàticament quan afegim IDragHandler
    public void OnDrag(PointerEventData eventData)
    {
        //Li diem que la posició de la imatge serà posició del ratolí
        transform.position = Input.mousePosition;
    }



    //Aquesta funció es crea automàticament quan afegim IEndDragHandler
    public void OnEndDrag(PointerEventData eventData)
    {
        //una vegada ha acabat ens torna a la posició zero
        transform.localPosition = Vector3.zero;
    }
}
