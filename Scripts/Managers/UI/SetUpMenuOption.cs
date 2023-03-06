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

    private int slidingIn = 0;
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
        transform.position = new Vector3(initialXPos - 400 - (index * index * 1500), 250 - index * 80, transform.position.z); // this is dumb but it makes each element slide in after each other
        slidingIn = 1;
    }

    public void MenuSelected()
    {
        if (page == 0)
        {
            switch (index)
            {
                case 0:
                    loadingScript.instance.LoadScene(1); // use .instance to not have to set everything static
                    break;
                case 1:
                    MainMenuManager.instance.ChangePage(1); // go to settings menu
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }
        if (page == 1)
        {
            switch (index)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    MainMenuManager.instance.ChangePage(0); // go to main menu
                    break;
                default:
                    break;
            }
        }
    }

    public void DestroySelf()
    {
        slidingIn = -1;
    }

    void Update()
    {
        if (MainMenuManager.selection == index)
        {
            textbox.GetComponent<Text>().text = ">" + menuName;
        }
        else
        {
            textbox.GetComponent<Text>().text = menuName;
        }

        // sliding in animation
        if (slidingIn == 1)
        {
            transform.position = new Vector3(transform.position.x + (initialXPos - transform.position.x) / 10, 250 - index * 80, transform.position.z);
            if (Mathf.Round(transform.position.x) >= initialXPos)
            {
                slidingIn = 0;
                transform.position = new Vector3(70, 250 - index * 80, transform.position.z);
            }
        }
        if (slidingIn == -1)
        {
            transform.position = new Vector3(transform.position.x + (initialXPos - 500 - transform.position.x) / 10, transform.position.y, transform.position.z);
            if (Mathf.Round(transform.position.x) <= initialXPos - 400)
            {
                Destroy(gameObject);
            }
        }
    }
}
