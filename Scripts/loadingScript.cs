using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadingScript : MonoBehaviour
{
    public static loadingScript instance { get; private set; } // singleton instance of script

    AudioManager audioManager;

    //loading screen
    public GameObject LoadingCanvas;
    public GameObject LoadingScreen;
    public Slider loadingSlider;

    //fading effect
    public GameObject BlackFade;
    private float alpha = 0f;
    private int fading = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DebugStats.AddLog("More than one loader in scene");
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(LoadingCanvas);
    }

    //loading event
    public void LoadScene(int sceneIndex) // could use integers instead ?? an enum format  ? ( use .....GetActiveScene().buildIndex )
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.FadeOutBGM(); // fade out sound smoothly, instead of cutting it off

        if (SceneManager.GetActiveScene().buildIndex != sceneIndex)
        {
            StartCoroutine(WaitForSceneLoad(sceneIndex)); // dont stop processing threads
        }
    }

    private IEnumerator WaitForSceneLoad(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex); // async to not block the thread
        operation.allowSceneActivation = false; // dont show the scene loading - show the black fade instead
        LoadingCanvas.SetActive(true); // activate the components of the loading screen
        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            yield return null; //repeat the loop

            float loadingProgress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = loadingProgress;

            if (loadingProgress >= 0.9f) // 0.9f due to what unity counts as a "loading complete" value
            {
                BlackFade.SetActive(true); // set fadeblack game object
                alpha = 0; // and immidiately make it transparent
                BlackFade.GetComponent<Image>().color = new UnityEngine.Color(0f, 0f, 0f, alpha); //transparent

                fading = 1; //fade in
                yield return new WaitForSeconds(0.5f); // gap to stop the fade to black not completing (originally 1f)

                operation.allowSceneActivation = true; //finish scene loading

                yield return new WaitForSeconds(0.5f); // (originally 1f)
                fading = 2; //fade out

                LoadingScreen.SetActive(false);
                yield return new WaitForSeconds(0.2f); // wait for fade out to complete (originally 0.5f)

                LoadingCanvas.SetActive(false);
                BlackFade.SetActive(false);
                fading = 0;
            }
        }
    }

    float Fade(float alpha, int type, ref int fading)
    {
        if (alpha > 1f && type == 1)
        {
            fading = 0;
            return 1f;
        }
        if (alpha < 0f && type == -1)
        {
            fading = 0;
            return 0f;
        }
        alpha += 0.1f * type;
        BlackFade.GetComponent<Image>().color = new UnityEngine.Color(0f, 0f, 0f, alpha);
        return alpha;
    }

    private void Update()
    {
        if (fading > 0)
        {
            if (fading == 1) alpha = Fade(alpha, 1, ref fading); //fade to black
            if (fading == 2) alpha = Fade(alpha, -1, ref fading); //fade from black
        }
    }
}