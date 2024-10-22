using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class AddressableAssetsManager : MonoBehaviour
{
    [SerializeField] AssetReferenceGameObject assetReferenceGameObject;
    // Start is called before the first frame update
    public void Start()
    {       
        AddressablePrefab();
    }
    public void AddressablePrefab()
    {
        Addressables.InstantiateAsync(assetReferenceGameObject);
    }
}
