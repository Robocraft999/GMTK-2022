using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuSceneUIManager : MonoBehaviour
{
    public static MenuSceneUIManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        InitMenuScene();
    }

    private void InitMenuScene()
    {

    }

    //TODO rename
    public void buttonPressed_start()
    {
        GameManager.Instance.SwitchScene(GameState.BUILDING);
    }

    public void buttonPressed_exit()
    {
        Debug.Log("bye");
        Application.Quit();
    }
}
