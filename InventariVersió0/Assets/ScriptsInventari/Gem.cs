using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Aquest script està assignat a l'objecte Gem (Gemma o Diamant)
//Cada objecte que es vulgui crear com a 'Interactable' s'ha de crear un script com aquest
//Si us fixeu en la següent línia de codi no és Monobehaviour sino: InventoryItemBase
//Això vol dir que heretarà tot el que hi hagi al script InventoryItemBase
//Els Objectes que vulgueu crear com a Interactables hauran de tenir un collider
//També hauran de tenir una imatge petita que es colocarà al inventari

public class Gem : InventoryItemBase {


    public override void OnUse()
    {
        //Utilitzar la funció que està a InventoryItemBase per posar posició i rotació quan està en ús

        base.OnUse();
    }
}
