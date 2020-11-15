using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script assignat a un 'Empty Object' Objecte Buit anomenat Inventory

public class Inventory : MonoBehaviour
{

    // ES fixa que hi haurà 9 espais (slots) en la variable SLOTS
    private const int SLOTS = 9;

    // mSlots és una llista referenciada al script InventorySlot
    private IList<InventorySlot> mSlots = new List<InventorySlot>();

    // Aquí definim 3 events: Afegir un Item a l'inventari, Eliminar-lo i Utilitzar-lo
    public event EventHandler<InventoryEventArgs> ItemAdded;
    public event EventHandler<InventoryEventArgs> ItemRemoved;
    public event EventHandler<InventoryEventArgs> ItemUsed;

    public Inventory()
    {
        for (int i = 0; i < SLOTS; i++)
        {
            mSlots.Add(new InventorySlot(i));
        }
    }

    private InventorySlot FindStackableSlot(InventoryItemBase item)
    {
        foreach (InventorySlot slot in mSlots)
        {
            if (slot.IsStackable(item))
                return slot;
        }
        return null;
    }

    private InventorySlot FindNextEmptySlot()
    {
        foreach (InventorySlot slot in mSlots)
        {
            if (slot.IsEmpty)
                return slot;
        }
        return null;
    }

    public void AddItem(InventoryItemBase item)
    {
        InventorySlot freeSlot = FindStackableSlot(item);
        if (freeSlot == null)
        {
            freeSlot = FindNextEmptySlot();
        }
        if (freeSlot != null)
        {
            freeSlot.AddItem(item);

            //Aquest troç de script es repeteix en els tres events: Ara és Afegir Item
            if (ItemAdded != null)
            {
                ItemAdded(this, new InventoryEventArgs(item));
            }

        }
    }

    //Funció creada directament desde ItemClickHandler
    internal void UseItem(InventoryItemBase item)
    {

        //Aquest troç de script es repeteix en els tres events: Ara és Utilitzar Item
        if (ItemUsed != null)
        {
            ItemUsed(this, new InventoryEventArgs(item));
        }

        item.OnUse();
    }

    //Aquesta funció elimina el item del inventari
    public void RemoveItem(InventoryItemBase item)
    {
        foreach (InventorySlot slot in mSlots)
        {
            if (slot.Remove(item))
            {

                //Aquest troç de script es repeteix en els tres events: Ara és Eliminar Item
                if (ItemRemoved != null)
                {
                    ItemRemoved(this, new InventoryEventArgs(item));
                }
                break;
            }

        }
    }
}
