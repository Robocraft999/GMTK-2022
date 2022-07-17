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
    public float speed;
    public float interval;

    public GameObject set_main;
    public GameObject set_options;
    public GameObject HTP;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        rolls = 2;
        rounds = 5;
        speed = 2;
        interval = 0.8f;
        set_main.SetActive(true);
        set_options.SetActive(false);
        HTP.SetActive(false);
    }

    public void ButtonPressedStart()
    {
        GameManager.Instance.SwitchScene(GameState.BUILDING);
    }

    public void ButtonPressedExit()
    {
        Debug.Log("bye");
        Application.Quit();
    }

    public void ButtonPressedDefault()
    {
        rolls = GameObject.Find("sliderRolls").GetComponent<Slider>().value = 10;
        rounds = GameObject.Find("sliderRounds").GetComponent<Slider>().value = 5;
        speed = GameObject.Find("sliderSpeed").GetComponent<Slider>().value = 2;
        interval = GameObject.Find("sliderInterval").GetComponent<Slider>().value = 0.8f;
    }

    public void ButtonPressedHTP()
    {
        if (HTP.active == true)
        {
            HTP.SetActive(false);
        } else
        {
            HTP.SetActive(true);
        }
        
    }

    public void Update()
    {
        if (set_options.active == true)
        {
            UpdateSliderText();
        }

    }

    public void UpdateSliderText()
    {
        rolls = GameObject.Find("sliderRolls").GetComponent<Slider>().value;
        GameObject.Find("rolls").GetComponent<TextMeshProUGUI>().text = rolls.ToString();
        rounds = GameObject.Find("sliderRounds").GetComponent<Slider>().value;
        GameObject.Find("rounds").GetComponent<TextMeshProUGUI>().text = rounds.ToString();
        speed = GameObject.Find("sliderSpeed").GetComponent<Slider>().value;
        GameObject.Find("speed").GetComponent<TextMeshProUGUI>().text = speed.ToString();
        interval = GameObject.Find("sliderInterval").GetComponent<Slider>().value;
        GameObject.Find("interval").GetComponent<TextMeshProUGUI>().text = Math.Round(interval, 1).ToString();
    }

}


