using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    void Update()
    {
        Interact();
    }

    public override void Interact()
    {
        if (Input.GetKeyDown(GameManager.InteractKey) && CanInteract)
        {
            print("Entered scene in " + this.gameObject.name);
        }
    }
}