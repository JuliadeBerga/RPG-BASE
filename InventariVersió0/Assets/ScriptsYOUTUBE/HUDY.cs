using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Carregar la llibreria UI
using UnityEngine.UI;

public class HUDY : MonoBehaviour
{
    //Crea una variable per poder connectar amb l'script de Inventory
    public InventoryY Inventory;

    //Només arrencar afegeix
    void Start()
    {
        Inventory.ItemAdded += InventoryScript_ItemAdded;
        
    }

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgsY e)
    {
        //Busca el primer slot lliure
        Transform inventoryPanel = transform.Find("InventoryPanel");
        
        foreach (Transform slot in inventoryPanel)
        {
            //Actualitza Borde... Imatge..
            Image image = slot.GetChild(0).GetComponent<Image>();
            
            //Una vegada troba el primer slot buit
            if (!image.enabled)
            {
                //Activa la imatge
                image.enabled = true;
                image.sprite = e.Item.Image;


                // Emmagatzemar la referència del item
                

                break;
            }
        }
    }

}
