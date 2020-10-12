using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonTest : MonoBehaviour
{

    public string levelName;

    private static SingletonTest singleton;

    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void LoadB(string sceneANumber)
    {
        Debug.Log("sceneBuildIndex to load: " + sceneANumber);
        SceneManager.LoadScene(sceneANumber);
    }

}
