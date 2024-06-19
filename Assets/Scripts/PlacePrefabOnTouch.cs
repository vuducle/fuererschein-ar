using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacePrefabOnTouch : MonoBehaviour
{
    /// <summary>
    /// The prefab that will be instantiated on touch.
    /// </summary>
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject placedPrefab;

    /// <summary>
    /// The instantiated object.
    /// </summary>
    List<GameObject> spawnedObject;

    /// <summary>
    /// The input touch control.
    /// </summary>
    TouchControls controls;

    ARRaycastManager aRRaycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
        spawnedObject = new List<GameObject>();
        aRRaycastManager = GetComponent<ARRaycastManager>();

        controls = new TouchControls();
        // If there is touch input being performed. call the OnPress function.
        controls.control.touch.performed += ctx =>
        {
            if (ctx.control.device is Pointer device)
            {
                OnPress(device.position.ReadValue());
            }
        };
        
        
    }

    private void OnEnable()
    {
        controls.control.Enable();
    }

    private void OnDisable()
    {
        controls.control.Disable();
    }

    void OnPress(Vector3 position)
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("is hitting UI");
            return;
        }
        
        // Check if the touch position hit a game object
        Ray ray = camera.ScreenPointToRay(position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log("Hit object: " + hitObject.name);
            // Do something with the hit object if needed
            Destroy(hitObject);
            return;
        }
        
        //Check if the raycast hit any trackables.
        if (aRRaycastManager.Raycast(position, hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first hit means the closest.
            var hitPose = hits[0].pose;
        
            // Instantiated the prefab.
            var spawned = Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
        
            // To make the spawned object always look at the camera. Delete if not needed.
            Vector3 lookPos = Camera.main.transform.position - spawned.transform.position;
            lookPos.y = 0;
            spawned.transform.rotation = Quaternion.LookRotation(lookPos);
            
            spawnedObject.Add(spawned);
        }
    }

    public void SetPlacedPrefab(GameObject prefab)
    {
        placedPrefab = prefab;
    }

    public GameObject GetPlacedPrefab()
    {
        return placedPrefab;
    }
}
