using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject savescreen;
    private GameObject MenuObject; 
    // Start is called before the first frame update
    void Start()
    {
        MenuObject = gameObject.transform.GetChild(1).gameObject;
        MenuObject.SetActive(false);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        savescreen.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuObject.SetActive(!MenuObject.activeSelf);
            gameObject.transform.GetChild(0).gameObject.SetActive(!gameObject.transform.GetChild(0).gameObject.activeSelf);


        }
    }

    public void Save()
    {
        savescreen.gameObject.SetActive(true);
        SaveSystem.SavePlayer(GameObject.Find("Player").GetComponent<Player>());
        savescreen.gameObject.SetActive(false);
    }
    public void Continue()
    {
        MenuObject.SetActive(!MenuObject.activeSelf);
        gameObject.transform.GetChild(0).gameObject.SetActive(!gameObject.transform.GetChild(0).gameObject.activeSelf);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
