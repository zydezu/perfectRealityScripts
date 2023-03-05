using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugStats : MonoBehaviour // this will display all errors in the game aswell as FPS
{
    public string version = "v0.05 - IN DEVELOPMENT";
    public Text textBox;
    private float deltaTime;
    private string FPSText;

    private static int dequeueFrameTimer = 0;

    static Queue<string> logs = new Queue<string>();

    // Update is called once per frame
    void Update()
    {
        if (Global.Settings.errorGUI)
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            FPSText = "FPS: " + Mathf.Ceil(fps).ToString();

            textBox.text = FPSText + Environment.NewLine +
                version + Environment.NewLine + Environment.NewLine
                + String.Join(Environment.NewLine, logs.ToArray());

            dequeueFrameTimer--;
            if (dequeueFrameTimer == 0)
            {
                logs.Dequeue();
                if (logs.Count > 0) dequeueFrameTimer = Math.Max(120 - (logs.Count * 20), 20); // set another timer if the queue isn't empty
            }
        }
    }

    static public void AddLog(string message)
    {
        if (Global.Settings.errorGUI)
        {
            logs.Enqueue(message);
            dequeueFrameTimer = Math.Max(120 - (logs.Count * 20), 20); //
        }
        Debug.Log(message);
    }
}
