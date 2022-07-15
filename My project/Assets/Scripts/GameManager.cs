using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState State {get; private set;}

    private void Awake()
    {
        if (GameObject.FindObjectsOfType<GameManager>().Length > 1) Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        instance = this;
    }

    public void SwitchScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    
    void Update()
    {
        
    }

    public enum GameState
    {
        MENU, CLASH, SHOP, BUILDING
    }
}
