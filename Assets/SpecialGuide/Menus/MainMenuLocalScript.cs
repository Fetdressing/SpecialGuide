using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLocalScript : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
