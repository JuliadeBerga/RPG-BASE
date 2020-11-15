using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeY : MonoBehaviour, IInventoryItemY
{
  public string Name 
    {
        get 
        {
            return "Axe";
        }
    }

    public Sprite _Image = null;

    public Sprite Image 
    {
        get
        {
            return _Image;
        }
    }

    public void OnPickup() 
    {
        gameObject.SetActive(false);
    }

}
