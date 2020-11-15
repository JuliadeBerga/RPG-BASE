using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Aquest script l'assignarem als 9 Border. HUD->InventoryPanel->Slot->Border

public class ItemClickHandler : MonoBehaviour
{

    //Es crea aquesta variable tipus Inventory per cridar a la funció UseItem
    public Inventory _Inventory;

    public KeyCode _Key;

    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
    }

    //Funció per fer cannviar el color del Borde quan cliqui
    void Update()
    {
        if(Input.GetKeyDown(_Key))
        {
            FadeToColor(_button.colors.pressedColor);

            // Click the button
            _button.onClick.Invoke();
        }
        else if(Input.GetKeyUp(_Key))
        {
            FadeToColor(_button.colors.normalColor);
        }
    }

    void FadeToColor(Color color)
    {
        Graphic graphic = GetComponent<Graphic>();
        graphic.CrossFadeColor(color, _button.colors.fadeDuration, true, true);
    }

    private InventoryItemBase AttachedItem
    {
        get
        {
            ItemDragHandler dragHandler =
            gameObject.transform.Find("ItemImage").GetComponent<ItemDragHandler>();

            return dragHandler.Item;
        }
    }

    //Funció que s'executa quan es clica l'item del inventari
    //A Border haurem d'afegir un Add Component On Click() i quan es cliqui Border entrarà aquesta funció
    public void OnItemClicked()
    {
        InventoryItemBase item = AttachedItem;

        if (item != null)
        {
            _Inventory.UseItem(item);
        }
    }

}
