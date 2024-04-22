using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;
    [SerializeField] Object MainScene;
    public void startgame()
    {
        SceneManager.LoadScene(MainScene.name);
    }

    public void showsettings()
    {
        settings.SetActive(true);
        menu.SetActive(false);

    }

    public void Back()
    {
        settings.SetActive(false);
        menu.SetActive(true);
    }
    public void quitgame()
    {
        Application.Quit();
    }
}
