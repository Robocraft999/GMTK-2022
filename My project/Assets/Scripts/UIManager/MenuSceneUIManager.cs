using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;


public class MenuSceneUIManager : MonoBehaviour
{

    public static MenuSceneUIManager Instance { get; private set; }

    public float rolls;
    public float rounds;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        InitMenuScene();
        GameObject.Find("set_main").SetActive(true);
        GameObject.Find("set_options").SetActive(false);

        rolls = GameObject.Find("sliderRolls").GetComponent<Slider>().value;
        rounds = GameObject.Find("sliderRounds").GetComponent<Slider>().value;
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

    public void Update()
    {
        UpdateSliderText();
    }

    public void UpdateSliderText()
    {
        rolls = GameObject.Find("sliderRolls").GetComponent<Slider>().value;
        GameObject.Find("rolls").GetComponent<TextMeshProUGUI>().text = rolls.ToString();
        rounds = GameObject.Find("sliderRounds").GetComponent<Slider>().value;
        GameObject.Find("rounds").GetComponent<TextMeshProUGUI>().text = rounds.ToString();
    }

}
