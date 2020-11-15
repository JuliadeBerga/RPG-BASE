//Carregar aquesta llibreria perquè funcionin els events
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInventoryItemY 
{
    //Haurem de posar un nom al objecte interactuable creat
    string Name { get; }

    //Haurem de associar una imatge
    Sprite Image { get; }

    void OnPickup();
}

public class InventoryEventArgsY : EventArgs
{
    public InventoryEventArgsY(IInventoryItemY item)
    {
        Item = item;
    }

    public IInventoryItemY Item;
}


