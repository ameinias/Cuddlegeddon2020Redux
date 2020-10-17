using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
     
{

    public string sceneANumber;
    public void LoadB()
    {
        Debug.Log("sceneBuildIndex to load: " + sceneANumber);
        SceneManager.LoadScene(sceneANumber);
    }
}
