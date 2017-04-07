using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManagement : singleton<MenuManagement>
{
    [Header("Available Menus")]
    public GameObject Menu_GameOver;
    public GameObject Menu_Main;

    [Header("Available Texts")]
    public Text Text_GameOver;

    [Header("Current Menu Reference")]
    public GameObject currentMenu;

    //Monobehavior functions
    private void Start()
    {
        Close(Menu_GameOver);
        OpenMenu(Menu_Main);
    }

    //Button functions
    public void Close(GameObject menu)
    {
        menu.SetActive(false);
    }

    public void CloseMenu()
    {
        currentMenu.SetActive(false);
        currentMenu = null;
    }

    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
        currentMenu = menu;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        CloseMenu();

        GameManager.Instance.InitializeNewGame();
    }


    //Scriptable functions
    public void DisplayGameOver(string t)
    {
        Text_GameOver.text = t;

        OpenMenu(Menu_GameOver);
    }

}
