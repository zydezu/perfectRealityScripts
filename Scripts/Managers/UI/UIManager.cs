using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    void LoadDialogue(string file)
    {
        string path = "Assets\\Text\\" + file + "-dialogue.txt";
        string[] lines = File.ReadAllLines(path);

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