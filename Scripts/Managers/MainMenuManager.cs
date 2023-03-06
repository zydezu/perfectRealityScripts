using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tree;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static LocalText;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [SerializeField] List<MenuOptions> menuOptions;
    public static int currentPage, selection = 0;
    public static bool optionSelected = false;
    private Tuple<int, int> selectionLimit = Tuple.Create(0, 5); //limits for menu selection (inclusive), wrap to other value if passed
    private List<SetUpMenuOption> currentMenuButtons = new List<SetUpMenuOption>();

    public static bool inputAllowed;
    private int waitToProcessStick = -1;
    private int stickTimeout;

    float horizontalInput, verticalInput;

    //main menu objects
    GameObject panel;
    [SerializeField] SetUpMenuOption menuObject; //prefab - "SetUpMenuOption" instead of "GameObject" so script from the instantiation can be directly accessed!!

    private void Awake()
    {
        instance = this;
        panel = transform.GetChild(0).gameObject; // get UI panel, to place more elements dynamically
    }

    void Start() // attached to CANVAS
    {
        DebugStats.AddLog("Main Menu Manager Started");
        inputAllowed = true;
        GenerateUpMenuOptions(0);
        AudioManager.instance.PlaySound("LcdDem OST - End", true);
    }

    public void GenerateMenuList(List<LocalText.UI.MainMenuUIClass> mainMenuUI) // set the menu names based on the selected language, done by reading save files in UIManager.cs
    {
        menuOptions.Clear();
        int currentPage = 0;
        int index = 0;
        foreach (LocalText.UI.MainMenuUIClass option in mainMenuUI)
        {
            if (option.page != currentPage)
            {
                currentPage = option.page;
                index = 0;
            }
            menuOptions.Add(new MenuOptions(option.page, index, option.menuName, option.description));
            index++;
        }
    }

    void GenerateUpMenuOptions(int page)
    {
        foreach (MenuOptions option in menuOptions)
        {
            // here we would get the appropiate names
            if (option.menuPage == page)
            {
                Tuple<int, int, string, string> data = option.GrabData();
                SetUpMenuOption menuOption = Instantiate(menuObject, new Vector3(70, 250 - data.Item2 * 80, 0), transform.rotation, panel.transform); // instantiate as CHILD of the panel object (a helper script/class could simply this)
                menuOption.name = "MenuOption" + data.Item2; // unity internal name
                menuOption.page = data.Item1;
                menuOption.index = data.Item2;
                menuOption.menuName = data.Item3;
                menuOption.description = data.Item4;

                //DebugStats.AddLog(LocalText.UI.mainMenuUI[0].ReturnData());

                currentMenuButtons.Add(menuOption); // keep track of these options 
            }
        }
        selectionLimit = Tuple.Create(0, currentMenuButtons.Count - 1);
    }

    public void ChangePage(int page)
    {
        foreach (SetUpMenuOption option in currentMenuButtons)
        {
            option.DestroySelf();
        }
        currentMenuButtons.Clear();
        selection = 0;
        GenerateUpMenuOptions(page);
    }

    // check all keys
    void Update()
    {
        if (stickTimeout > 0) stickTimeout--;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        if (inputAllowed)
        {

            //keyboard
            //if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)) processKey(0);      // CheckKeyboardStickRelease takes care of this
            // if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A)) processKey(1);
            // if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D)) processKey(2);
            //if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S)) processKey(3);

            if (Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("Cancel")) processKey(8);
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return) || Input.GetButtonUp("Submit")) processKey(9);

            //controller
            CheckKeyboardStickRelease(verticalInput);
        }
    }

    void CheckKeyboardStickRelease(float verticalInput)
    {
        if (Mathf.Abs(verticalInput) > 0.5f) //if vertical stick is being moved at all
        {
            if (verticalInput > 0) waitToProcessStick = 0; // stick moved UP
            else if (verticalInput < 0) waitToProcessStick = 3; // stick moved DOWN
        }
        else
        {
            if (waitToProcessStick > -1)
            {
                processKey(waitToProcessStick);
                waitToProcessStick = -1;
            }
        }
    }

    void processKey(int index)
    {
        switch (index)
        {
            case 0:
                ChangeSelection(-1);
                break;
            case 1:
                ChangeSelection(-1);
                break;
            case 2:
                ChangeSelection(1);
                break;
            case 3:
                ChangeSelection(1);
                break;

            /*gap*/

            case 8:
                print("BACK");
                break;
            case 9:
                print("ENTER");
                currentMenuButtons[selection].MenuSelected(); // process ENTER on the SetUpMenuOption.cs file
                break;
            default:
                break;
        }
    }

    void ChangeSelection(int change)
    {
        selection = selection + change;
        if (selection < selectionLimit.Item1)
        {
            selection = selectionLimit.Item2;
        }
        if (selection > selectionLimit.Item2)
        {
            selection = selectionLimit.Item1;
        }
    }

}

// this class is an easy way to store data of menu options in a list, then generate that "page" of the menu options using the classes data
[Serializable] public class MenuOptions
{
    [SerializeField] public int menuPage = 0; // serialise field is just for DEBUG viewing, these menus are set via UIManager.cs and the ...-UI.txt files
    [SerializeField] public int index = 0;
    [SerializeField] string menuName = "Test";
    [SerializeField] string description = "Test";

    public MenuOptions(int menuPage, int index, string menuName, string description) {
        this.menuPage = menuPage;
        this.index = index;
        this.menuName = menuName;
        this.description = description;
    }

    public Tuple<int,int,string,string> GrabData() => Tuple.Create(menuPage, index, menuName, description); // return all class data needed to instantiate a new object
}