using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUpMenuOption : MonoBehaviour
{
    public int page;
    public int index;
    public string menuName;
    public string description;

    private GameObject textbox; // the text child GameObject

    private bool slidingIn = false;
    private float initialXPos;

    private void Awake()
    {
        textbox = transform.GetChild(0).gameObject;
        initialXPos = transform.position.x;
    }

    void Start()
    {
        textbox.GetComponent<Text>().text = menuName;
        SlideIn();
    }

    void SlideIn() // sliding in animation
    {
        transform.position = new Vector3(initialXPos - 400 - (index * index * 1500), transform.position.y, transform.position.z); // this is dumb but it makes each element slide in after each other
        slidingIn = true;
    }

    void Update()
    {
        // sliding in animation
        transform.position = new Vector3(transform.position.x + (initialXPos - transform.position.x) / 10, transform.position.y, transform.position.z);
        if (Mathf.Round(transform.position.x) >= initialXPos)
        {
            slidingIn = false;
            transform.position = new Vector3(70, transform.position.y, transform.position.z);
        }

        if (MainMenuManager.selection == index)
        {
            textbox.GetComponent<Text>().text = ">" + menuName;
        }
        else
        {
            textbox.GetComponent<Text>().text = menuName;
        }
        if (MainMenuManager.optionSelected && MainMenuManager.selection == index && MainMenuManager.currentPage == page)
        {
            if (page == 0)
            {
                switch (index)
                {
                    case 0:
                        GameObject.Find("loader").GetComponent<loadingScript>().LoadScene(1);
                        break;
                    default:
                        break;
                }
            }
            MainMenuManager.optionSelected = false;
        }
    }
}
