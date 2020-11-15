//Carregar aquesta llibreria perquè funcionin els events
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryY : MonoBehaviour
{
    // Es fixa que hi haurà 9 espais (slots) en la variable SLOTS
    private const int SLOTS = 9;

    // mItems és una llista referenciada al script InventoryItemY
    private List<IInventoryItemY> mItems = new List<IInventoryItemY>();

    // Aquí definim 1 event: Afegir un Item
    public event EventHandler<InventoryEventArgsY> ItemAdded;


    public void AddItem(IInventoryItemY item)
    {
        //Si tinc espai al inventari, o sigui, si hi ha slots lliures
        if (mItems.Count < SLOTS)
        {
            //Creem variable tipus collider i la inicialitzem
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();

            //Si està habilitada
            if (collider.enabled)
            {
                //la deshabilitem
                collider.enabled = false;

                //Cridem a la funció item
                mItems.Add(item);

                //Cridem a la funció pickup
                item.OnPickup();

                //si hem afegit un item
                if (ItemAdded != null)
                {
                    //item afegit
                    ItemAdded(this, new InventoryEventArgsY(item));
                }
            }
        }



    }


}
