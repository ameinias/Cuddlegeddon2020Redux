using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraShake;
public class Projectile : MonoBehaviour
{
    public int damage;
    public float speed;
    Rigidbody rb;
    public float bounce = 100;
    public int count;
    public string[] reaction;
    AudioSource audioS;
    public AudioClip hitSFX;
    public AudioClip bounceSFX;
    Animator anim;
    public AudioClip[] P1hitClips;
    public AudioClip[] P2hitClips;
    public AudioClip[] monsterhitClips;
    public bool isQ;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {

    }

    public void ChooseClip(string player)
    {
        AudioClip[] list;

        if (player == "P1") { list = P1hitClips; } else if (player == "P2") { list = P2hitClips; } else { list = monsterhitClips; }


        int index = Random.Range(0, list.Length);
        hitSFX = list[index];
        GetComponent<AudioSource>().PlayOneShot(hitSFX);

    }

    private void OnCollisionEnter(Collision collision)  // You hit a hand
    {
    //    Debug.Log("HIT SOMETHING");
        count++;
        if (count > 3)
        {
            KillProj();
        }
        else if (collision.gameObject.tag == "Projectile")
        {
            // Destroy(collision.gameObject);
            collision.gameObject.GetComponent<Projectile>().KillProj();
            KillProj();
        }


        else if (collision.gameObject.name == "hand_1")
        {
            rb.velocity = Vector3.zero;
            GetComponent<AudioSource>().PlayOneShot(bounceSFX);
            Vector3 v = Vector3.Reflect(transform.forward, collision.GetContact(0).normal);
            transform.rotation = Quaternion.FromToRotation(Vector3.forward, v);
            GetComponent<SpriteRenderer>().flipX = false;
            rb.AddForce(collision.contacts[0].normal * bounce, ForceMode.Impulse);



        }
        else if (collision.gameObject.name == "hand_2")
        {
            Vector3 v = Vector3.Reflect(transform.forward, collision.GetContact(0).normal);
            transform.rotation = Quaternion.FromToRotation(Vector3.forward, v);
            rb.velocity = Vector3.zero;
            GetComponent<AudioSource>().PlayOneShot(bounceSFX);
            GetComponent<SpriteRenderer>().flipX = true;
            rb.AddForce(collision.contacts[0].normal * bounce, ForceMode.Impulse);
        }
        else
        {

            Vector3 v = Vector3.Reflect(transform.forward, collision.GetContact(0).normal);
            transform.rotation = Quaternion.FromToRotation(Vector3.forward, v);
            rb.velocity = Vector3.zero;
            GetComponent<AudioSource>().PlayOneShot(bounceSFX);
            GetComponent<SpriteRenderer>().flipX = true;
            rb.AddForce(collision.contacts[0].normal * bounce, ForceMode.Impulse);
        }
    }


    public bool CoinFlip()
    {
        int num = Random.Range(0, 1);
        if (num == 0)
        { return false; }
        else { return true; }
    }

    public bool OneInAMillion()
    {
        int num = Random.Range(0, 10);
        if (num == 0)
        { return true; }
        else { return false; }
    }



    public int ChanceNegative(int number)
    {
        int num = Random.Range(0, 2);
        if (num == 0)
        { return number; }
        else { return number * 1; }
    }

    public void Boundries(string player)
    {




        if (LevelTracker.RelationshipStatus() == "fine")
        {
            if (OneInAMillion())
            {
                LevelTracker.UpdateScore(player, -1);
                LevelTracker.Express(reaction[0], player);
                Debug.Log("Didnt take it well");
            }
            else
            {
                LevelTracker.UpdateScore(player, +2);
                LevelTracker.Express(reaction[1], player);
            }


        }
        else if (LevelTracker.RelationshipStatus() == "obsessed")
        {
            LevelTracker.UpdateScore(player, -3);
            LevelTracker.Express(reaction[2], player);
        }
        else if (LevelTracker.RelationshipStatus() == "miserable")
        {
            if (CoinFlip())
            {
                LevelTracker.UpdateScore(player, 1);
                LevelTracker.Express(reaction[0], player);
            } else
            {
                LevelTracker.UpdateScore(player, -1);
                LevelTracker.Express(reaction[1], player);
            }
        }

    }

    private void OnTriggerEnter(Collider other) // You hit a body
    {

        // Shaking the camera.
        CameraShaker.Presets.ShortShake2D();

        int index;
        index = Random.Range(0, reaction.Length);


        if (other.gameObject.name == "Body2")
        {
            ChooseClip("P2");
            if (isQ) { Boundries("P2"); }
            else
            {

                LevelTracker.UpdateScore("P2", damage);
                LevelTracker.Express(reaction[index], "P2");
            }
            KillProj();

        }
        else if (other.gameObject.name == "Body1")
        {
            ChooseClip("P1");

            if (isQ) { Boundries("P1"); }
            else
            {
                LevelTracker.Express(reaction[index], "P1");
                LevelTracker.UpdateScore("P1", damage);
            }
            KillProj();


        }
        else if (other.gameObject.name == "Monster")
        {
            ChooseClip("monster");

            Debug.Log("hit monster");
            LevelTracker.UpdateScore("monster", damage);
            KillProj();
            LevelTracker.Express(reaction[index], "monster");

        }
        else
        {

            Debug.Log("HIT SOMETHING MYSTERIOUS");


        }
    }

    void KillProj()
    {
        rb.velocity = Vector3.zero;



        anim.SetTrigger("Die");

        // explode animation
    }

    public void Explode()
    {

        Destroy(this.gameObject);
    }
}
