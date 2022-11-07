using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField]
    private Scenes GoToScene;
    
    void Update()
    {
        Interact();
    }

    public override void Interact()
    {
        if (Input.GetKeyDown(GameManager.InteractKey) && CanInteract)
        {
            print("Entered door");  
            GameManager.SceneManager.SwitchToScene(GoToScene);
        }
    }
}