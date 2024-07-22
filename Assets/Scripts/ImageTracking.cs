using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsToSpawn;

    [SerializeField] private bool ShouldMatchImageRotation = false;

    private Dictionary<string, GameObject> arObjects;
    
    private ARTrackedImageManager arTrackedImageManager;

    // Start is called before the first frame update
    private void Awake()
    {
        arObjects = new Dictionary<string, GameObject>();
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void Start()
    {
        foreach (var prefab in prefabsToSpawn)
        {
            var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obj.name = prefab.name;
            obj.gameObject.SetActive(false);
            arObjects.Add(obj.name, obj);
        }
    }

    private void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnTrackedImageChanged;
    }

    private void OnDestroy()
    {
        arTrackedImageManager.trackedImagesChanged += OnTrackedImageChanged;
    }

    private void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var gameObject in eventArgs.added)
        {
            UpdateImageTracked(gameObject);
        }
        foreach (var gameObject in eventArgs.updated)
        {
            UpdateImageTracked(gameObject);
        }
        foreach (var gameObject in eventArgs.removed)
        {
            arObjects[gameObject.referenceImage.name].gameObject.SetActive(false);
        }
        
    }

    private void UpdateImageTracked(ARTrackedImage image)
    {
        if (image.trackingState is TrackingState.Limited or TrackingState.None)
        {
            arObjects[image.referenceImage.name].gameObject.SetActive(false);
            return;
        }

        if (prefabsToSpawn != null)
        {
            arObjects[image.referenceImage.name].gameObject.SetActive(true);
            arObjects[image.referenceImage.name].gameObject.transform.position = image.transform.position;
            if (ShouldMatchImageRotation)
            {
                arObjects[image.referenceImage.name].gameObject.transform.rotation = image.transform.rotation;
                if (image.referenceImage.name == "Porsche")
                {
                    arObjects[image.referenceImage.name].gameObject.transform.Rotate(-180f, 0f, 0f, 
                        Space.Self);
                }
            }
        }
    }
}
