using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DestroyGO : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "SampleScene")
        {
            DestroyImmediate(this.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
