using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using CameraShake;
using UnityEngine.UI;

public class CodepTarget : MonoBehaviour
{


    public Transform[] nodes;
    public Animator blue;
    public Animator yellow;
    public Animator both;
/*    public bool P1Held;
    public bool P2Held;*/
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
    bool timer;
    public bool P1Press;
    public bool P2Press;
    public float waitToFire1;
    public float waitToFire2;

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

}

    public void PlayRandomAudio(AudioClip[] list)
    {



        int index = Random.Range(0, list.Length);
        hitSFX = list[index];
        GetComponent<AudioSource>().PlayOneShot(hitSFX);

    }

    IEnumerator P1Holder()
    {
      //   P1Held = true;
        yield return new WaitForSeconds(pressTimer);
     //   P1Held = false;
        coolDownTimer1 = coolDown;
        yield break;

    }

    IEnumerator P2Holder()
    {
       // P2Held = true;
        yield return new WaitForSeconds(pressTimer);
     ///   P2Held = false;
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

/*
    void OldUpdate() {

        Debug.Log("Here");
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
                canFire2 = false;
                P2Held = false;
                P1Held = false;


            }
            else if (P2Held && rightPlace) { LevelTracker.CodepScore(false); }
            else if (P1Held && rightPlace) { LevelTracker.CodepScore(false); }


        }


    }

*/


    void CheckMatchingClick() {
        //make sure they haven't fired too recently

        if (waitToFire1 > 0)
        {
            waitToFire1 -= Time.deltaTime;
            canFire1 = false;
        }
        else
        {
            canFire1 = true;
        }


        if (waitToFire2 > 0)
        {
            waitToFire2 -= Time.deltaTime;
            canFire2 = false;
        }
        else
        {
           canFire2 = true;
        }
 /*       if (Input.GetButtonDown("Fire1") && CanFire1)
        {
            P1Press = true;
            Debug.Log("save the beanddddds");
            coolDownTimer1 = coolDown;
        }*/

            // CHeck if button is hit
            if (Input.GetButtonDown("Fire1") && canFire1)
        {
            P1Press = true;
            coolDownTimer1 = coolDown;
           canFire1 = false;
            P1Face.ChangeFace("shoot");
 
        }




        if (Input.GetButtonDown("Fire2") && canFire2)
        {
            P2Press = true;
            coolDownTimer2 = coolDown;
            canFire2 = false;
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
                Debug.Log("BOTH");


            }
            else
            {
                LevelTracker.CodepScore(false);
                both.SetTrigger("Miss");
                PlayRandomAudio(missSFX);
                CameraShaker.Presets.ShortShake2D();

            }


        }
        else
        {

            if (P1Press && coolDownTimer2 < 0)
            {
                CameraShaker.Presets.ShortShake2D();

                P1Press = false;
                PlayRandomAudio(P1HitSFX);
                coolDownTimer1 = coolDown;

                waitToFire1 = timeBetweenFires;
                blue.SetTrigger("Blink");

                LevelTracker.CodepScore(false);
            }
            if (P2Press && coolDownTimer1 < 0)
            {
                CameraShaker.Presets.ShortShake2D();

                P2Press = false;
                PlayRandomAudio(P2HitSFX);
                coolDownTimer2 = coolDown;

                waitToFire2 = timeBetweenFires;
                yellow.SetTrigger("Blink");

                LevelTracker.CodepScore(false);


            }
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

            if (canFire1 && canFire2 && both.GetComponent<BothTarget>().hitIt)
            {
                Move();
            }
     
            CheckMatchingClick();
            CheckHits();
    

        }
    }





    void CheckFire()
    {
/*        if (Input.GetButtonDown("Fire1") && canFire1)
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






        }*/
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
