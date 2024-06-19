using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class CanvasClickEvent : MonoBehaviour
{
    private Button[] buttons;
    [SerializeField]
    private GameObject[] prefabsToSpawn;
    [SerializeField] private TextMeshProUGUI text;
    private PlacePrefabOnTouch placeScript;

    [SerializeField] private GameObject prefabSpawnerGameObject;
    
    // Start is called before the first frame update
    void Start()
    {
        placeScript = prefabSpawnerGameObject.GetComponent<PlacePrefabOnTouch>();
        buttons = GetComponentsInChildren<Button>();

        placeScript.SetPlacedPrefab(prefabsToSpawn[0]);
        text.text = placeScript.GetPlacedPrefab().name;
        foreach (var element in buttons.Select(((button, i) => new { i, button })))
        {
            var i = element.i;
            var button = element.button;
            button.onClick.AddListener(() => SelectPrefabToSpawn(i));
        }
    }

    private void OnClick()
    {
        Debug.Log("clicked");
    }

    private void SelectPrefabToSpawn(int i)
    {
        //selectedPrefabToSpawn = prefabsToSpawn[i];
        placeScript.SetPlacedPrefab(prefabsToSpawn[i]);
        text.text = placeScript.GetPlacedPrefab().name;
    }
}
