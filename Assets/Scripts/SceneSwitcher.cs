using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    private Button[] buttons;
    void Start()
    {
        buttons = GetComponentsInChildren<Button>();
        foreach (var button in buttons.Select(((button, i) =>  new { button, i})))
        {
            var i = button.i;
            var clickableButton = button.button;
            
            clickableButton.onClick.AddListener(() => ChangeScene(i));
        }
    }

    private void ChangeScene(int i)
    {
        switch (i)
        {
            case 0:
                SceneManager.LoadScene("Scenes/TappingScene");
                break;
            case 1:
                SceneManager.LoadScene("Scenes/TestScene");
                break;
        }
    }
}
