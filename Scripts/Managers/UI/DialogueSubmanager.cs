using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSubmanager : MonoBehaviour
{
    [SerializeField] string currentDialogue = "No Dialogue(English)";
    private string currentDialogueTyped;

    private int typeWritten = -1;
    private int dialogueLength = 0;
    private int typeSpeed = Global.Settings.dialogueTextSpeed; // higher value = slower dialogue typing speed

    public Text dialogueText; // putting the own gameobject into the unity editor is faster than Getting a component each time (GetComponent<Text>().text)


    private void Update()
    {
        if (typeWritten == -1)
        {
            dialogueText.text = currentDialogue;
        }
        else
        {
            typeWritten += 1;
            if(typeWritten / 2 >= dialogueLength)
            {
                typeWritten = -1;
            }
            else
            {
                currentDialogueTyped = currentDialogue.Substring(0, typeWritten / typeSpeed);
                dialogueText.text = currentDialogueTyped;
            }
        }
    }

    public void SetDialogue(string dialogue)
    {
        currentDialogue = dialogue;
        //dialogueText.text = currentDialogue;
        TypeWriteEffect(dialogue);
    }

    private void TypeWriteEffect(string dialogue)
    {
        dialogueLength = dialogue.Length;
        typeWritten = 0;
    }

    public void UpdateSettings()
    {
        typeSpeed = Global.Settings.dialogueTextSpeed;
    }
}

class Dialogue
{
    string name = "";
    List<string> lines = new List<string>();

    public void SetName(string name)
    {
        this.name = name;
    }

    public void AddLine(string line)
    {
        lines.Add(line);
    }
}