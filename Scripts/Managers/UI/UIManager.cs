using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class LocalText
{
    public class UI
    {
        public class MainMenuUIClass
        {
            public int page = 0;
            public string menuName = "menuName";
            public string description = "description";

            public MainMenuUIClass(int page, string menuName, string description)
            {
                this.page = page;
                this.menuName = menuName;
                this.description = description;
            }
        }
    }
}

public class UIManager : MonoBehaviour
{
    //get this object and children
    public static UIManager instance; // singleton instance of script
    private GameObject DialogueUI;

    //dialogue (&loading) requirements
    char nameSeperator = '`';
    List<Dialogue> dialogueList = new List<Dialogue>();
    DialogueSubmanager DialogueScripts;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DebugStats.AddLog("More than one UI manager in scene");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        DialogueUI = transform.GetChild(0).gameObject; //set child object
        DialogueScripts = DialogueUI.GetComponent<DialogueSubmanager>(); //easy way to manage scripts
        if (DialogueUI.activeSelf) DialogueUI.SetActive(false); //Dialogue shouldn't show on the title screen
        LoadDialogue(Global.Settings.language); // build dialogue upon game startup, instead of DialogueSubmanager initilisation to save time (and avoid glitches)
    }

    public void ShowDialogue()
    {
        DialogueUI.SetActive(true);
        DialogueScripts.SetDialogue("A function to typewrite text in DialogueSubmanager.cs – in " +
            "which the script writes a character at a time of the dialogue, every frame until the whole dialogue string is complete.");
    }

    public void SetDialogue(string dialogue)
    {
        DialogueUI.SetActive(true);
        DialogueScripts.SetDialogue(dialogue);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadDialogue(string folder)
    {
        string getTextFile(string textFile) => "Assets\\Text\\" + folder + "\\" + textFile;

        //UI
        /* format
         * <--MainMenu--> | What UI this is getting
         * <-page0-> | page number of the main menu UI (different submenus)
         * New Game|Start a new game! | menu button name and it's description
         * ...............
         * 
         */
        string path = getTextFile("UI.txt");
        string[] lines = File.ReadAllLines(path);

        List<LocalText.UI.MainMenuUIClass> mainMenuUI = new();

        int page = 0;
        foreach (var i in lines)
        {
            if (i.Contains("<--"))
            {
                string title = i.Replace("<--", "").Replace("-->", ""); //.Replace is faster than a regex
            }
            else if (i.Contains("<-"))
            {
                page = int.Parse(i.Replace("<-page", "").Replace("->", ""));
            }
            else if (i != "")
            {
                mainMenuUI.Add(new LocalText.UI.MainMenuUIClass(page, i.Split('|')[0], i.Split('|')[1])); // add and set new main menu class of the button properties
            }
        }

        MainMenuManager.instance.GenerateMenuList(mainMenuUI); // send list of main menu classes to the mainmenumanager.cs file for that to construct its own list and classes
        mainMenuUI.Clear(); // save RAM



        path = getTextFile("dialogue.txt");
        lines = File.ReadAllLines(path);

        dialogueList.Clear();

        Dialogue dialogue = new Dialogue();

        foreach (var i in lines)
        {

            if (i.Contains(nameSeperator))
            {
                string[] tempSplit = i.Split(nameSeperator);
                dialogue.SetName(tempSplit[0]);
                dialogue.AddLine(tempSplit[1]);
            }
            else
            {
                dialogue.AddLine(i);
            }
            if (i == "")
            {
                dialogueList.Add(dialogue);
                dialogue = new Dialogue();
            }
        }
        DebugStats.AddLog("Done building dialogue.");
    }
}