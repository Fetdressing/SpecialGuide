using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public void LocalPlay()
    {
        SceneManager.LoadScene("MainMenuLocal");
    }

    public void NetworkPlay()
    {
        SceneManager.LoadScene("MainMenuNetwork");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
