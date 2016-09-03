using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebugMenuScript : MonoBehaviour {

    public GameObject buttonPrefab;

    private SceneManagerHelper sceneManagerHelper;
    private List<ButtonWrapper> buttons = new List<ButtonWrapper>();

    private class ButtonWrapper
    {
        public ButtonWrapper(GameObject gameobject, Transform parent, string name)
        {
            button = gameobject.GetComponent<Button>();
            rectTransform = gameobject.GetComponent<RectTransform>();

            gameobject.transform.SetParent(parent);
            rectTransform.GetComponentsInChildren<Text>()[0].text = name;
        }

        public Button button { get; private set; }
        public RectTransform rectTransform { get; private set; }
    }

    // Use this for initialization
    void Start () {
        sceneManagerHelper = new SceneManagerHelper();
        buildButtons();
    }

    private void buildButtons()
    {
        int i = 0;
        foreach(string name in sceneManagerHelper.scenes)
        {
            ButtonWrapper buttonWrapper = new ButtonWrapper(Object.Instantiate<GameObject>(buttonPrefab), transform, name);
            buttons.Add(buttonWrapper);

            string nameAsANonSharedString = name; // this must be done, otherwise the cose will use the last value of name for all callbacks
            buttonWrapper.button.onClick.AddListener(() => LoadScene(nameAsANonSharedString));
            buttonWrapper.rectTransform.anchoredPosition = new Vector2(-220, 200 - i * 70 );
            i++;
        }

        /*
        var a = button.GetComponent<RectTransform>();
        a.GetComponent<RectTransform>().GetComponent<Text>().text = "HEJ";
        a.anchoredPosition = new Vector2(50, 50);
        */
    }

    private void LoadScene(string sceneName)
    {
        Debug.Log(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
