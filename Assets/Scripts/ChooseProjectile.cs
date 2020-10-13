
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChooseProjectile : MonoBehaviour
{
    public List<ScriptableObject> projectiles;
    public List<GameObject> projectilesFab;
    public int currentInt;
    int nextInt;
    int lastInt;
    public GameObject current;
    public GameObject next;
    public GameObject last;
    
    public RawImage currentImage;
    public RawImage lastImage;
    public RawImage nextImage;

    public Texture2D Ice;
    public Texture2D Heart;
    public Texture2D Ask;

    public Text tempDisplayProj;
    [Space(10)] // 10 pixels of spacing here.
    public GameObject preIce;
    public GameObject preHug;
    public GameObject preAsk;
    public GameObject preCurrent;


    // Start is called before the first frame update
    void Start()
    {
        currentInt = 1;
      //  current = projectiles[currentInt];
        preCurrent = preIce;
       // UpdateProjectiles();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int CycleScriptableList(int locInt, List<GameObject> list)
    {
        if (locInt > list.Count - 1)
        { locInt = 0; }
        else if (locInt < 0)
        { locInt = list.Count - 1; }

 

        return locInt;
    }
    




    public void LastProjectile()
    {
        currentInt = CycleScriptableList(currentInt += 1, projectilesFab);
        UpdateProjectiles();
  
    }

    public void NextProjectile()
    {
        currentInt =  CycleScriptableList(currentInt -= 1, projectilesFab);
        UpdateProjectiles();


    } 


    void UpdateProjectiles()
    {

        // Objects
        current = projectilesFab[currentInt];
        next = projectilesFab[CycleScriptableList(currentInt + 1, projectilesFab)];
        last = projectilesFab[CycleScriptableList(currentInt - 1, projectilesFab)];



        if (current != null)
            Debug.Log("projectile:" + current.name);
        else
            Debug.Log("projectile is null");



        if (current.name == "Ask")
        {


        preCurrent = preAsk;
            nextImage.GetComponent<RawImage>().texture = Heart;
            currentImage.GetComponent<RawImage>().texture = Ask;
            lastImage.GetComponent<RawImage>().texture = Ice;
        }
        else if (current.name == "iceCube")
        {  preCurrent = preIce;
            nextImage.GetComponent<RawImage>().texture = Ask;
            currentImage.GetComponent<RawImage>().texture = Ice;
            lastImage.GetComponent<RawImage>().texture = Heart;


        }
        else if (current.name == "Hug")
        {  preCurrent = preHug;
            nextImage.GetComponent<RawImage>().texture = Ice;
            currentImage.GetComponent<RawImage>().texture = Heart;
            lastImage.GetComponent<RawImage>().texture = Ask;




        }
        else { Debug.Log("Brokem"); }

    }
}
