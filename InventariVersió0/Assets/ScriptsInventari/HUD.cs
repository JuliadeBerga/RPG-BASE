using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script assignat al objecte HUD (qui conté tot el inventari, slots, bordes, panells...)

public class HUD : MonoBehaviour {

    //Crea una variable per poder connectar amb l'script de Inventory
    public Inventory Inventory;

    //Crea una variable anomenada MessagePanel perquè aparegui el missatge de Press F to Pick the item
    public GameObject MessagePanel;

    // Use this for initialization

    //Només arrencar afegeix
    void Start () {
        Inventory.ItemAdded += InventoryScript_ItemAdded;
        Inventory.ItemRemoved += Inventory_ItemRemoved;
	}

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        //Busca el primer slot lliure
        Transform inventoryPanel = transform.Find("InventoryPanel");
        int index = -1;
        foreach (Transform slot in inventoryPanel)
        {
            index++;

            // Actualitza Borders... Images...
            Transform imageTransform = slot.GetChild(0).GetChild(0);
            Transform textTransform = slot.GetChild(0).GetChild(1);
            Image image = imageTransform.GetComponent<Image>();
            Text txtCount = textTransform.GetComponent<Text>();
            ItemDragHandler itemDragHandler = imageTransform.GetComponent<ItemDragHandler>();

            //Una vegada troba el primer slot buit
            if(index == e.Item.Slot.Id)
            {
                //Activa la imatge
                image.enabled = true;
                image.sprite = e.Item.Image;

                int itemCount = e.Item.Slot.Count;
                if (itemCount > 1)
                    txtCount.text = itemCount.ToString();
                else
                    txtCount.text = "";
                         

                // Emmagatzemar la referència del item
                itemDragHandler.Item = e.Item;

                break;
            }
        }
    }

    private void Inventory_ItemRemoved(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("InventoryPanel");

        int index = -1;
        foreach (Transform slot in inventoryPanel)
        {
            index++;

            Transform imageTransform = slot.GetChild(0).GetChild(0);
            Transform textTransform = slot.GetChild(0).GetChild(1);

            Image image = imageTransform.GetComponent<Image>();
            Text txtCount = textTransform.GetComponent<Text>();

            ItemDragHandler itemDragHandler = imageTransform.GetComponent<ItemDragHandler>();

            // We found the item in the UI
            if (itemDragHandler.Item == null)
                continue;

            // Found the slot to remove from
            if(e.Item.Slot.Id == index)
            {
                int itemCount = e.Item.Slot.Count;
                itemDragHandler.Item = e.Item.Slot.FirstItem;

                if(itemCount < 2)
                {
                    txtCount.text = "";
                }
                else
                {
                    txtCount.text = itemCount.ToString();
                }

                if(itemCount == 0)
                {
                    image.enabled = false;
                    image.sprite = null;
                }
                break;
            }
           
        }
    }

    private bool mIsMessagePanelOpened = false;

    public bool IsMessagePanelOpened
    {
        get { return mIsMessagePanelOpened; }
    }

    //Funció per fer apareixer el MessagePanel, i agafar el Missatge: Press key F to Pickup de cadascun dels items
    public void OpenMessagePanel(InteractableItemBase item)
    {
        //Activa el panell MessagePanel que és un child de HUD(on està aquest script assignat)
        MessagePanel.SetActive(true);

        //Inicialitza la variable Text del Message Panel (és un child de Message Panel)
        Text mpText = MessagePanel.transform.Find("Text").GetComponent<Text>();

        //Posa el text que hi hagi a cadascun dels items
        mpText.text = item.InteractText;
        
        //Posem la variable del MessagePanel a activat
        mIsMessagePanelOpened = true;


    }

    public void OpenMessagePanel(string text)
    {
        MessagePanel.SetActive(true);

        Text mpText = MessagePanel.transform.Find("Text").GetComponent<Text>();
        mpText.text = text;


        mIsMessagePanelOpened = true;
    }

    public void CloseMessagePanel()
    {
        MessagePanel.SetActive(false);

        mIsMessagePanelOpened = false;
    }
}
