using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Toggle invertToggle;


    private static UIManager _instance;
    public static UIManager Instance => _instance;
    public bool isInvert;

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            _instance = this;

        }
        DontDestroyOnLoad(this.gameObject);

        isInvert = true;
    }

    public void InvertToggle()
    {
        if (invertToggle.isOn)
        {
            isInvert = true;

        }
        else
        {
            isInvert = false;

        }

    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  
    }

    public void ExitButton()
    {
        Debug.Log("Bye");
        Application.Quit();
    }

 
}
