using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Aquest script és cridat per cadascun dels scripts que creeu amb els vostres items: Axe, Gem...

//Tipus de item: Per defecte, Consumible o Arma
public enum EItemType
{
    Default,
    Consumable,
    Weapon
}

public class InteractableItemBase : MonoBehaviour
{
    //Haurem de posar un nom al objecte interactuable creat
    public string Name;

    //Haurem de associar una imatge
    public Sprite Image;

    //Farem que aparegui la frase "Pitja la tecla F per agafar el item"
    public string InteractText = "Press F to pickup the item";

    //Definirem quin tipus d'objecte és: per defecte, consumible o arma (definit al principi d'aquest script)
    public EItemType ItemType;

    public virtual void OnInteractAnimation(Animator animator)
    {
        animator.SetTrigger("tr_pickup");
    }

    public virtual void OnInteract()
    {
    }

    public virtual bool CanInteract(Collider other)
    {
        return true;   
    }
}

public class InventoryItemBase : InteractableItemBase
{
    public InventorySlot Slot
    {
        get; set;
    }

    //Quan s'utilitzi l'objecte, aquest objecte tindrà la posició i rotació indicada al script
    public virtual void OnUse()
    {
        transform.localPosition = PickPosition;
        transform.localEulerAngles = PickRotation;
    }

    //Funció per fer el Drop
    public virtual void OnDrop()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            gameObject.SetActive(true);
            gameObject.transform.position = hit.point;
            gameObject.transform.eulerAngles = DropRotation;
        }
    }

    //Funció per quan agafar l'objecte
    //El que fa és destruir el Rigidbody i el Objecte agafat
    public virtual void OnPickup()
    {
        Destroy(gameObject.GetComponent<Rigidbody>());
        gameObject.SetActive(false);
        
    }

    //Primer hem de copiar el objecte i pegar-lo com a child de hand.r
    //Resetejem els seus valors i el posicionem com vulguem
    //Utilitzarem aquests valors per posar-los en els següents dos Vectors3

    //Aqui haurem de posar els valors de posició quan l'objecte està a la mà del player
    public Vector3 PickPosition;

    //Aqui haurem de posar els valors de rotació quan l'objecte està a la mà del player
    public Vector3 PickRotation;



    //Aqui haurem de posar els valors de rotació (si volem) quan l'objecte es deixi caure a l'escena
    //Aquests valors haurien de ser els mateixos que tenen l'objecte abans d'agafar-lo, pq quedi igual quan el deixem
    public Vector3 DropRotation;

    //Aqui haurem de posar els valors de true o false si el nostre objecte es podrà utilitzar
    //En el cas de Axe serà true i en el cas de GEM serà false
    public bool UseItemAfterPickup = false;


}
