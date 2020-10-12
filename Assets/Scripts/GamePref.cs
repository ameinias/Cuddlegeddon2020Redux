using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePref : MonoBehaviour

     

{

    private static GamePref singleton;
    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
