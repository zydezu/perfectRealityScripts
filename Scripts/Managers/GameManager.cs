using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using saveManagerScript;

public enum GameState
{
    BootToTitle,
    QuitToTitle,
    NewGame,
    LoadGame,
    Overworld,
    Battle
}

public enum scenesCorresponding
{
    Title,
    Overworld
}

public static class Global // global variables all scripts/classes can access - used with "Global.variable"
{
    public class Settings
    {
        public static string language = "enGB";

        public static int FPSLimit = 60;

        public static int forceControllerButtons = 0; // 1 - force playstation buttons, 2 - force XBOX buttons

        public static int dialogueTextSpeed = 2;

        //advanced/debug
        public static bool errorGUI = true; // enables a GUI that shows all errors that would normally be logged through unity (Debug.Log)
        public static bool quickBattleSummary = false; // speeds up the battle completion process
        public static bool noShaderEffects = false; // stop blur effects
        public static bool showParticles = true; // if particles (specs of dust and sparks, explosions) show
    }

    public static bool playerMovementActive = false;
    public static bool cameraLocked = false;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // singleton instance of script

    [SerializeField] GameObject player; //get prefab

    public GameState gameState;
    public static event Action<GameState> GameStateChanged;

    loadingScript loaderScript;
    SaveManager savemanager;

    // CREATE A SINGLETON CLASS THAT MANAGES/STORES VARIABLES , THIS WILL GET ENTANGLED SOON

    private void Awake() //first thing that runs
    {
        QualitySettings.vSyncCount = 0; // 0 off, 1 on
        Application.targetFrameRate = Global.Settings.FPSLimit;
        Screen.SetResolution(640, 480, true, 60); //resolution and hertz (change the monitor resolution)

        SceneManager.sceneLoaded += onSceneLoaded;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DebugStats.AddLog("More than one Game Manager in scene");
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        SetupGame();

    }

    void SetupGame()
    {
        savemanager = new SaveManager();

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            UpdateGameState(GameState.BootToTitle);
        }
    }

    void SpawnPlayer()
    {
        var playerSpawn = Instantiate(player, new Vector3(0,0,0), transform.rotation); // instantiate and set attributes
        playerSpawn.name = "Player";
        //playerSpawn.transform.position.Set(0,0,0); // set position
        Global.playerMovementActive = true;
        DebugStats.AddLog("Spawned player");
    }

    void ShowDialogue() => UIManager.instance.ShowDialogue(); // essentially sending a "message" to another singleton class (without the use of EVENTS)

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateGameState(GameState setToState) {
        gameState = setToState;

        switch (setToState) //set new state
        {
            case GameState.BootToTitle:
                SceneManager.LoadScene((int)scenesCorresponding.Title);
                break;
            case GameState.QuitToTitle:
                //saveGame();
                //mainMenu();
                //DELETE SINGLETONS and player and stuff
                DebugStats.AddLog("event called");
                break;
            case GameState.NewGame:
                //newGame();
                loaderScript.LoadScene((int)scenesCorresponding.Overworld);
                ShowDialogue();
                break;
            case GameState.LoadGame:
                //loadGame();
                SpawnPlayer();
                ShowDialogue();
                break;
            case GameState.Overworld:
                //activatePlayerMovement() ect...
                break;
            case GameState.Battle:
                //startbattle ect...
                break;
            default: //error
                throw new ArgumentOutOfRangeException(nameof(setToState));
        }

        GameStateChanged?.Invoke(setToState);
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode) //an event that is called everytime a new scene is loaded
    {
        DebugStats.AddLog("Loaded scene: " + scene.name);
        switch(scene.buildIndex)
        {
            case 0:
                break;
            case 1:
                SpawnPlayer();
                ShowDialogue();
                AudioManager.instance.PlaySound("Cat Soup OST - ブリキの花園~Metallic World~ TRACK 13", true); // need to move
                break;
            default:
                DebugStats.AddLog("Unknown scene loaded!");
                break;
        }
    }
}