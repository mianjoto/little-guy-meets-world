using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public Scenes CurrentScene;

    void Awake()
    {
        GameObject.Find("Main Camera").GetComponent<Camera>().enabled = true;
        GameObject.Find("Main Camera").GetComponent<Camera>().tag = "MainCamera";
    }


    public void SwitchToScene(Scenes scene)
    {
        Destroy(Camera.main);
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.ToString());
        GameObject.Find("Main Camera").GetComponent<Camera>().tag = "MainCamera";
        GameObject.Find("Main Camera").GetComponent<Camera>().enabled = true;
        GameObject.Find("Main Camera").SetActive(true);
        CurrentScene = scene;
    }
}

public enum Scenes
{
    Bedroom,
    Kitchen,
    FinalRoom,
}
