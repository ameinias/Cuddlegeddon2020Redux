using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using CameraShake;
public class CodepTarget : MonoBehaviour
{


    public Transform[] nodes;
    public Animator blue;
    public Animator yellow;
    public Animator both;
    public bool P1Held;
    public bool P2Held;
    public bool canFire1;
    public bool canFire2;
    public float coolDownTimer1;
    public float coolDownTimer2;
    public float coolDown;
    public Transform startLoc;
    public bool rightPlace;
    public Vector3 endPosition;
    public float offset = 1;
    public int indexNode;
    public float movementSpeed;
    public float pressTimer = 2;
    [Space(10)]
    public int sucesses;

    bool timer;
    public bool P1Press;
    public bool P2Press;
    public float waitToFire1;
    public float waitToFire2;
    public bool CanFire1;
    public bool CanFire2;
    public float timeBetweenFires;
    [Space(10)]
    public AudioClip hitSFX;
    public AudioClip[] matchSFX;
    public AudioClip[] missSFX;
    public AudioClip[] P1HitSFX;
    public AudioClip[] P2HitSFX;
    public bool hitIt = true;

    public HeadController P1Face;
    public HeadController P2Face;


    // Start is called before the first frame update
    void Start()
    {
        endPosition.z = this.transform.position.z;
        sucesses = 0;
    }

    public void PlayRandomAudio(AudioClip[] list)
    {



        int index = Random.Range(0, list.Length);
        hitSFX = list[index];
        GetComponent<AudioSource>().PlayOneShot(hitSFX);

    }

    IEnumerator P1Holder()
    {
         P1Held = true;
        yield return new WaitForSeconds(pressTimer);
        P1Held = false;
        coolDownTimer1 = coolDown;
        yield break;

    }

    IEnumerator P2Holder()
    {
        P2Held = true;
        yield return new WaitForSeconds(pressTimer);
        P2Held = false;
        coolDownTimer2 = coolDown;
        yield break;

    }
    void CanFire()
    {

        coolDownTimer1 -= Time.deltaTime;


        if (coolDownTimer1 <= 0)
        {
            coolDownTimer1 = coolDown;
            canFire1 = true;
           // sprite.sprite = origSprite;
        }
        coolDownTimer2 -= Time.deltaTime;


        if (coolDownTimer2 <= 0)
        {
            coolDownTimer2 = coolDown;
            canFire2 = true;
            // sprite.sprite = origSprite;
        }
    }


    void OldUpdate() {

        if (!GameManager.IsPaused() && !GameManager.IsControlsDisabled())
        {

            ;

            if (Vector3.Distance(startLoc.position, transform.position) < offset)
            {

                rightPlace = true;

            }
            else { rightPlace = false; }


            //Fire
            CanFire();



            // case one - P2 is there, rightPlace is there - both 
            // right place is there, p2 is not
            // right place and p2 not there. 

            //CheckFireNew();

            if (Input.GetButtonDown("Fire1") && canFire1)
            {
                StartCoroutine(P1Holder());

            }
            if (Input.GetButtonDown("Fire2") && canFire2)
            {
                StartCoroutine(P2Holder());
            }

            if (P2Held && P1Held && rightPlace)
            {
                both.SetTrigger("Blink");
                LevelTracker.CodepScore(true);
                canFire1 = false;
                P2Held = false;
                P1Held = false;


            }
            else if (P2Held && rightPlace) { LevelTracker.CodepScore(false); }
            else if (P1Held && rightPlace) { LevelTracker.CodepScore(false); }



        }
    
    
    }




    void CheckMatchingClick() {
        //make sure they haven't fired too recently

        if (waitToFire1 > 0)
        {
            waitToFire1 -= Time.deltaTime;
        }
        else
        {
            CanFire1 = true;
        }


        if (waitToFire2 > 0)
        {
            waitToFire2 -= Time.deltaTime;
        }
        else
        {
            CanFire2 = true;
        }


        // CHeck if button is hit
        if (Input.GetButtonDown("Fire1") && coolDownTimer1 < 0 && CanFire1)
        {
            P1Press = true;
            coolDownTimer1 = coolDown;
            CanFire1 = false;
            P1Face.ChangeFace("shoot");

        }
        if (Input.GetButtonDown("Fire2") && coolDownTimer2 < 0)
        {
            P2Press = true;
            coolDownTimer2 = coolDown;
            CanFire2 = false;
            P2Face.ChangeFace("shoot");
        }








        // Check if it's within a few seconds of button being hit
        if (coolDownTimer1 > 0)
        {

            coolDownTimer1 -= Time.deltaTime;
        }
        else { P1Press = false; }

        if (coolDownTimer2 > 0)
        {

            coolDownTimer2 -= Time.deltaTime;
        }
        else { P2Press = false; }

        // Check if both are true at the same time THIS IS WHERE THE MAGIC HAPPENS


    }
    void CheckHits() {










            if (P1Press && P2Press)
        {
            
            CameraShaker.Presets.Explosion2D();
            Debug.Log("BOTHPRESS");
            P1Press = false;
            P2Press = false;
                if (rightPlace)
                {
                    both.SetTrigger("Blink");
                PlayRandomAudio(matchSFX);
                LevelTracker.CodepScore(true);
                both.GetComponent<BothTarget>().hitIt = false;
                P1Face.ChangeFace("kissy");
                P2Face.ChangeFace("kissy");



            } else { LevelTracker.CodepScore(false);
                both.SetTrigger("Miss");
                PlayRandomAudio(missSFX);
                CameraShaker.Presets.ShortShake2D();
            }
            }
        else if (P1Press && coolDownTimer2 < 0)
        {
            CameraShaker.Presets.ShortShake2D();
            Debug.Log("P1 Miss");
            P1Press = false;
            PlayRandomAudio(P1HitSFX);
            coolDownTimer1 = coolDown;

            waitToFire1 = timeBetweenFires;
            blue.SetTrigger("Blink");
            LevelTracker.CodepScore(false);
        }
        else if (P2Press && coolDownTimer1 < 0)
        {
            CameraShaker.Presets.ShortShake2D();
            Debug.Log("P2Miss");
            P2Press = false;
            coolDownTimer2 = coolDown;
            yellow.SetTrigger("Blink");
            waitToFire2 = timeBetweenFires;
            LevelTracker.CodepScore(false);
            PlayRandomAudio(P2HitSFX);
        }
    }



    private void Update()
    {

        if (!GameManager.IsPaused() && !GameManager.IsControlsDisabled())
        {

            ;

            if (Vector3.Distance(startLoc.position, transform.position) < offset)
            {

                rightPlace = true;

            }
            else { rightPlace = false; }

            if (CanFire1 && CanFire2 && both.GetComponent<BothTarget>().hitIt)
            {
                Move();
            }
            CheckMatchingClick();
            CheckHits();


        }
    }




    // Update is called once per frame
    void UpdateOLD()
    {

        var zPressed = Input.GetButton("Fire1"); // note, not keydown
        var xPressed = Input.GetButton("Fire2");
        coolDownTimer1 += Time.deltaTime;

        if (!timer && (zPressed || xPressed))
        {
            timer = true;
           // coolDown = Time.deltaTime;
            Debug.Log("reset timer");
            coolDownTimer1 = 0;
        }

        if (timer &&  coolDownTimer1 > 100) // 100 ms
        {
            timer = false;
            if (zPressed && xPressed)
                Debug.Log("Pressed Both");
            else if (zPressed)
                Debug.Log("Pressed Z");
            else if (xPressed)
                Debug.Log("Pressed X");
            else
                Debug.Log("Released keys during timer");
        }
        Debug.Log("Blob retired");


    }



    void CheckFire()
    {
        if (Input.GetButtonDown("Fire1") && canFire1)
        {

            if (rightPlace)
            {
                if (P2Held)

                {
                    both.SetTrigger("Blink");
                    LevelTracker.CodepScore(true);
                    canFire1 = false;
                    coolDownTimer1 = coolDown;
                }
                else
                {
                    StartCoroutine(P1Holder());
                    blue.SetTrigger("Blink");
                    canFire1 = false;
                    coolDownTimer1 = coolDown;
                    LevelTracker.CodepScore(false);

                }
            }
            else
            {
                StartCoroutine(P1Holder());
                blue.SetTrigger("Fail");
                canFire1 = false;
                coolDownTimer1 = coolDown;
                LevelTracker.CodepScore(false);
            }
        }

        if (Input.GetButtonDown("Fire2") && canFire2)
        {

            if (rightPlace)
            {
                if (P1Held)

                {
                    both.SetTrigger("Blink");
                    LevelTracker.CodepScore(true);
                    canFire2 = false;
                    coolDownTimer1 = coolDown;

                }
                else
                {
                    StartCoroutine(P2Holder());
                    yellow.SetTrigger("Blink");
                    canFire2 = false;
                    coolDownTimer2 = coolDown;
                    LevelTracker.CodepScore(false);
                }
            }
            else
            {
                StartCoroutine(P2Holder());
                yellow.SetTrigger("Fail");
                canFire2 = false;
                coolDownTimer2 = coolDown;
                LevelTracker.CodepScore(false);
            }






        }
    }
    public void Move()
    {

        if (transform.position != endPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, movementSpeed * Time.deltaTime);
        }
        else
        {

            indexNode = Random.Range(0, nodes.Length);
            endPosition = nodes[indexNode].position;
            movementSpeed = movementSpeed + 0.00005f;

        }


    }
}
