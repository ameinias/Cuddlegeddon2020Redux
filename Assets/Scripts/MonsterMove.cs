using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{

    public Transform[] nodes;
    public float maxFire;
    public float minFire;
    public float timer;
    float shootTimer;
    public float offset;
    public float movementSpeed = 5;
    public Vector3 endPosition;
    public int indexNode;
    public GameObject projPrefab;
    public AudioClip monsterShootSFX;
    AudioSource player;
    public GameObject healthBar;
    public GameObject followBar;
    int projDir;
    public float bulletspeed = 0.5f;

    int chance;
    // Start is called before the first frame update
    void Start()
    {
        timer = maxFire;
        endPosition.z = this.transform.position.z;
        player = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(followBar.transform.position);
        healthBar.transform.position = pos;

        if (GameManager.IsPaused() == false && GameManager.IsControlsDisabled() == false)
        {
            Move();

            if (timer > 0)
            {

                timer = timer - Time.deltaTime;
            }
            else { Shoot(); }

        }
    }


    public void Shoot() {
  //      Debug.Log("Shoot!");
        shootTimer = Random.Range(minFire, maxFire);
        timer = shootTimer;
        player.PlayOneShot(monsterShootSFX);
        FireProjectile();


    }

    public void FireProjectile()
    {

        chance = Random.Range(0, 2);
        if (chance == 0) { projDir = 1; //Debug.Log("right" + chance); 
        } else { projDir = -1;
            //Debug.Log("left" + chance); 
        }

        GameObject bullet = Instantiate(projPrefab, new Vector3(this.transform.position.x + offset * projDir
            , this.transform.position.y + 0.5f, this.transform.position.z), Quaternion.identity);

        float speed = bulletspeed ;
        if (projDir == -1)
        {
            bullet.GetComponent<SpriteRenderer>().flipX = true;
        }

        bullet.GetComponent<Rigidbody>().AddForce(new Vector3(speed * projDir, 0, 0)); // 




    }





    public void Move()
    {

        if (transform.position != endPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, movementSpeed * Time.deltaTime);
        }
        else
        {

          indexNode =  Random.Range(0, nodes.Length);
            endPosition = nodes[indexNode].position;

        }


    }
}
