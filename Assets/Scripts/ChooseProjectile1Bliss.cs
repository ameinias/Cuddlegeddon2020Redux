
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChooseProjectile1Bliss : MonoBehaviour
{
    public List<GameObject> projectiles;
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


    public Text tempDisplayProj;
    [Space(10)] // 10 pixels of spacing here.
    public GameObject preIce;
    public GameObject preHug;

    public GameObject preCurrent;


    // Start is called before the first frame update
    void Start()
    {
        currentInt = 1;
        current = projectiles[currentInt];
    //    UpdateProjectiles();
        
    }


    public  void LastProjectile()
    {
        currentInt = CycleScriptableList(currentInt += 1, projectiles);
        UpdateProjectiles();
    }

    public  void NextProjectile()
    {
        currentInt = CycleScriptableList(currentInt -= 1, projectiles);
        UpdateProjectiles();
    } 

int CycleScriptableList(int locInt, List<GameObject> list)
    {
        if (locInt > list.Count-1)
        { locInt = 0; }
        else if (locInt < 0)
        { locInt = list.Count-1; }
      

        return locInt;
    }

    void UpdateProjectiles()
    {

        // Objects
       current = projectiles[currentInt];
        next = projectiles[CycleScriptableList(currentInt + 1, projectiles)];
        last = projectiles[CycleScriptableList(currentInt - 1, projectiles)];


        if (current != null)
            Debug.Log("projectile:" + current.name);
        else
            Debug.Log("projectile is null");

        if (current.name == "iceCube")
        {
            preCurrent = preIce;
            nextImage.GetComponent<RawImage>().texture = Heart;
            currentImage.GetComponent<RawImage>().texture = Ice;
            lastImage.GetComponent<RawImage>().texture = Heart;


        }
        else if (current.name == "Hug")
        {
            preCurrent = preHug;
            nextImage.GetComponent<RawImage>().texture = Ice;
            currentImage.GetComponent<RawImage>().texture = Heart;
            lastImage.GetComponent<RawImage>().texture = Ice;




        }
        else
        {
            preCurrent = preIce;

        }


    }
}
