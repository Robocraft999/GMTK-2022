using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void OnButtonPressed()
    {
        GameManager.Instance.SwitchScene(GameState.CLASH);
    }
}
