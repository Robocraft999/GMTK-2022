using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ClashSceneUIManager : MonoBehaviour
{
    public static ClashSceneUIManager Instance { get; private set; }

    public List<PlayerController> Players { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        InitClashScene();
    }

    private void InitClashScene()
    {

    }
}
