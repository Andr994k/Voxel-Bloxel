using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;
    [SerializeField] private Object MainScene;
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
