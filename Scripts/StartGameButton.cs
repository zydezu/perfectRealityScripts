using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    // this is a temporary script and a very rudimentary main menu
    loadingScript loaderScript;

    private void Start()
    {
        loaderScript = GameObject.Find("loader").GetComponent<loadingScript>();
    }

    public void StartLoading()
    {
        loaderScript.LoadScene(1);
    }
}