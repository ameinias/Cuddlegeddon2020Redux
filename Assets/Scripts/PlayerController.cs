using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject face;
    public ChooseProjectile chamber;
    [Space(10)] // 10 pixels of spacing here.
    public float handSpeed = 10;
    public float bulletSpeed = 40f;
    public float offset = 0.9f;
    [Space(10)] // 10 pixels of spacing here.
    public string upKey;
    public string downKey;
    public string fireKey;
    public string projUpKey;
    public string projDownKey;
    Rigidbody rb;
    public GameObject projectile;
    [Space(10)] // 10 pixels of spacing here.
    public float coolDown;
    public bool canFire;
    public float coolDownTimer = 10f;
    [Space(10)] // 10 pixels of spacing here.
    public bool isP2;
    float projDir = -1;
    [Space(10)] // 10 pixels of spacing here.
    AudioSource audioS;
    public AudioClip hitHugSFX;
    public AudioClip hitAskSFX;
    public AudioClip hitColdSFX;
    public AudioClip shootSFX;
    public Sprite handBlast;
    Sprite origSprite;
    SpriteRenderer sprite;
    public ChooseProjectile1Bliss blissChamber;
    public float progAngle;
    [Header("SFX")]
    public AudioClip beep1SFX;
    public AudioClip beep2SFX;


    public GameObject blink;
    public GameObject blink2;


    // Start is called before the first frame update
    void Start()
    {
        audioS = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();
        coolDownTimer = coolDown;
        origSprite = sprite.sprite;
        blink.SetActive(false);
        blink2.SetActive(false);
        if (isP2)
        {

            projDir = -1;
        }
        else { projDir = 1; }
    }


    void CanFire()
    {

        coolDownTimer -= Time.deltaTime;


        if (coolDownTimer <= 0)
        {
            coolDownTimer = coolDown;
            canFire = true;
            sprite.sprite = origSprite;
        }
    }
    // Update is called once per frame
    void Update()
    {


        if (!GameManager.IsPaused() && !GameManager.IsControlsDisabled() && LevelTracker.GetLevel() != "coDep")
        {
            //Move Hand
            if (Input.GetButton(upKey)) { rb.velocity = new Vector3(0, handSpeed, 0); }
            else if (Input.GetButton(downKey)) { rb.velocity = new Vector3(0, -handSpeed, 0); }
            else { rb.velocity = new Vector3(0, 0, 0); }

            //Fire
            CanFire();

     


            if (Input.GetButtonDown(fireKey))
            {
                
              //  Debug.Log("hit the fire button");
                if (canFire)
                {
                    LevelTracker.TextJump("elBlinko, P2: " + isP2, "big");
                    FireProjectile();
                    audioS.PlayOneShot(hitColdSFX);
                }

            } else {
                //blink.SetActive(false);
            }
            


            if (LevelTracker.GetLevel() != "hell")
            {
                // Switch Projectile
                if (Input.GetButtonDown(projUpKey))
                {
                    LastProjectile();
                    audioS.PlayOneShot(beep1SFX);
                    LevelTracker.TextJump("change1, P2: " + isP2, "small");
                }
                else if (Input.GetButtonDown(projDownKey))
                {
                    NextProjectile();
                    LevelTracker.TextJump("change2, P2: " + isP2, "small");
                    audioS.PlayOneShot(beep2SFX);
                }
            }
        }
    }



    void LastProjectile()
    {

        if (LevelTracker.GetLevel() == "bliss")
        { blissChamber.NextProjectile(); }
        else if (LevelTracker.levelGlobal != "hell")
        { chamber.LastProjectile();
   
        }
    }

    void NextProjectile()
    {
        if (LevelTracker.GetLevel() == "bliss")
        { blissChamber.NextProjectile(); }
        else if (LevelTracker.levelGlobal != "hell")
        { chamber.NextProjectile();

        }

    }

    GameObject Bullet()
    {

        GameObject projectileToFire;


        if (LevelTracker.GetLevel() == "bliss")
        {
            projectileToFire = blissChamber.preCurrent;
            Debug.Log(blissChamber.preCurrent.name + " bliss");
        }

        else if (LevelTracker.levelGlobal == "hell")
        {
            projectileToFire = projectile;
            Debug.Log(projectile.name + " hell");

        }
        else if (LevelTracker.levelGlobal == "vanilla")
        {

            projectileToFire = chamber.preCurrent;

        }
        else
        {

            projectileToFire = chamber.preCurrent;
            Debug.Log(chamber.preCurrent.name + " else");
        }



        return projectileToFire;
    }

    public void FireProjectile()
    {
        sprite.sprite = handBlast;

        canFire = false;


        GameObject bullet = Instantiate(Bullet(), new Vector3(this.transform.position.x + offset * projDir
            , this.transform.position.y + 0.5f, this.transform.position.z), Quaternion.identity);

        float speed = bullet.GetComponent<ProjIce>().speed;
        if (isP2)
        {
            bullet.GetComponent<SpriteRenderer>().flipX = true;
        }

        bullet.GetComponent<Rigidbody>().AddForce(new Vector3(speed * projDir, 0, 0)); // 

        audioS.PlayOneShot(shootSFX);


    }






}
