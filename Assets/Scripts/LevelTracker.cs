
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

public class LevelTracker : MonoBehaviour
{


    private static LevelTracker singleton;



    public string level;
    public static string levelGlobal;


    public bool noTimer;

    bool heckOver = false;



    [Header("HealthBars")]
    public Healthbar healthBar;
    public Healthbar P1HealthBar;
    public Healthbar P2HealthBar;
    public Healthbar baddieHealthBar;
    public Healthbar coDephealthBar;

    [Header("ScoreNumbers")]
    public Text scoreP1;
    public Text scoreP2;
    public int relNum;
    public string relationshipResult;
    public string relationshipResultTxt;


    [Header("Score Descriptions")]
    public Text relDescText;
    public Text relNumText;
    public SpriteRenderer background;

    [Space(10)] // 10 pixels of spacing here.
    /*    public int player1Score = 0;
        public int player2Score = 0;*/



    [Space(10)] // 10 pixels of spacing here.
    public HeadController P1Head;
    public HeadController P2Head;

    [Header("CountdownTimer")]
    public float timer;
    public int toxicTimeWarn = 60;
    public int toxicTime = 50;
    public int levelLength = 100;


    [Header("CountUpTimer")]
    public float timerStart;
    public float timerStartLength = 5;





    [Header("Audio")]
    AudioSource audioS;
    public AudioClip bgMusic;
    public AudioClip beepSFX;

    public AudioClip heckWinSFX;
    public AudioClip blissWinSFX;
    public AudioClip vanillaWinSFX;

    public AudioClip goSFX;

    public AudioClip coDepWinSFX;
    public AudioClip coDepLooseSFX;
    public AudioClip P1WinSFX;
    public AudioClip P2WinSFX;

    [Space(10)] // 10 pixels of spacing here.

    public Text countDownTimer;

    public bool finishedEverything = false;
    [Header("Menus")]

    public string nextLevel;
    public GameObject TooToxicMenu;

    [Header("JustFerHiding")]
    public GameObject projectileSwitch;
    public GameObject heckSpek;

    public string timeString1;
    public string timeString2;
    public string timeString3;


    [Header("Vanilla Mode")]
    public bool finishedVanilla = false;
    public GameObject coDepScreen;
    public GameObject heckScreen;
    public GameObject blissScreen;


    [Header("Domestic Heck Mode")]
    public GameObject P1WinScreen;
    public GameObject P2WinScreen;

    [Header("BlissMode")]
    public GameObject BlissWinScreen;
    public GameObject P1DieScreen;
    public GameObject P2DieScreen;
    public GameObject BlissProjectiles;
    public HeadController Baddie;

    [Header("Codependancy Mode")]
    public GameObject CodepWinScreen;
    public GameObject CoDepLoseScreen;
    public Healthbar coDepMatch;

    public int coDepFail;
    public int coDepWins;
    public Text agreementText;
    public Text disagreementText;


    [Space(10)]
    public Text debugText;
    [Space(10)]
    public GameObject badHeartFab;
    public GameObject goodHeartFab;
    public GameObject badHeartparent;
    public GameObject goodHeartparent;
    [Header("TextLists Mode")]
    public List<string> goodText;
    public List<string> badText;

    public List<string> monsterHealText;
    public List<string> monsterHitText;


    public static void TextJump(string text, string size)
    {
        Text textBox = singleton.debugText;
        textBox.text = text;
        textBox.gameObject.SetActive(true);
        if (size == "big")
        { textBox.GetComponent<Animator>().SetTrigger("big"); }
        else { textBox.GetComponent<Animator>().SetTrigger("small"); }


    }

    public static AudioClip LevelBGMusic()
    {
        AudioClip localtunes;
        localtunes = singleton.bgMusic;

        return localtunes;
    }


    void Awake()
    {
        /*        if (singleton != null)
                {
                    gameobject.destroy(singleton);
                }
                else
                {
                    singleton = this;
                    dontdestroyonload(this);
                }*/

        AllStart();
        levelGlobal = level;
        heckOver = false;
        finishedEverything = false;
        if (level == "vanilla")
        {
            VanillaStart();


        }
        else if (level == "hell")
        {
            HellStart();
        }
        else if (level == "bliss")
        {
            BlissStart();
        }
        else if (level == "coDep")
        {
            CodepStart();
        }

        else
        {
            VanillaStart();
        }



    }




    public static string GetLevel()
    {

        return singleton.level;

    }

    IEnumerator PlaySoundEvery(float t, int times)
    {
        countDownTimer.gameObject.SetActive(true);
        //  
        for (; ; )
        {

            // yield return new WaitForSeconds(2);
            for (int i = 0; i < times; i++)
            {
                bool check = false;
                if (check == false)
                {
                    GetComponent<AudioSource>().PlayOneShot(beepSFX);
                    check = true;
                }
                //  countDownTimer.text = " h " + i.ToString();

                if (i == 0)
                { countDownTimer.text = timeString1; }
                else if (i == 1)
                { countDownTimer.text = timeString2; }
                else if (i == 2)
                { countDownTimer.text = timeString3; }
                //  }




                yield return new WaitForSeconds(t);


                //  
            }
            GetComponent<AudioSource>().PlayOneShot(goSFX);

            //   GameManager.DisableControls(false);


            yield return new WaitForSeconds(goSFX.length);
            GameManager.DisableControls(false);
            countDownTimer.gameObject.SetActive(false);








            if (!noTimer)
            {
                if (level == "vanilla" || level == "coDep")
                {

                    yield return new WaitForSeconds(levelLength);
                    countDownTimer.gameObject.SetActive(true);

                    for (int i = 0; i < toxicTimeWarn; i++)
                    {
                        bool check = false;
                        if (check == false)
                        {
                            GetComponent<AudioSource>().PlayOneShot(beepSFX);
                            check = true;
                        }

    ;
                        countDownTimer.text = "00:0" + i.ToString(); //





                        yield return new WaitForSeconds(t);



                    }
                    //  GetComponent<AudioSource>().PlayOneShot(goSFX);
                    countDownTimer.gameObject.SetActive(false);
                    if (level == "vanilla")
                    {
                        EndLevelVanilla();
                    }
                    else if (level == "coDep") { EndLevelCoDep(); }


                }
            }






            yield break;

        }
    }


    /// <summary>
    /// //////////////////////////////////////////////STATRTS
    /// </summary>
    /// 

    public List<string> Agree;
    public List<string> Disagree;


    void AnnounceGood()
    {

        int randomshout = Random.Range(0, Agree.Count);


        TextJump(Agree[randomshout], "big");

    }


    void AnnounceBad()
    {

        int randomshout = Random.Range(0, Disagree.Count);


        TextJump(Disagree[randomshout], "big");

    }



  


    void Start()
    {
        StartCoroutine(PlaySoundEvery(1.0f, 3));
        singleton = this;
        //RelationShipScore();

        levelGlobal = level;

        audioS = GetComponent<AudioSource>();


        timerStart = timerStartLength;
        timer = levelLength;

    }

    void AllStart()
    {
        countDownTimer.gameObject.SetActive(false);
        P1WinScreen.SetActive(false);
        P1WinScreen.SetActive(false);
        coDepScreen.SetActive(false);
        heckScreen.SetActive(false);
        blissScreen.SetActive(false);
        TooToxicMenu.SetActive(false);


        BlissWinScreen.SetActive(false);
        P1DieScreen.SetActive(false);
        P2DieScreen.SetActive(false);


        CodepWinScreen.SetActive(false);
        CoDepLoseScreen.SetActive(false);




    }

    void VanillaStart()
    {

        projectileSwitch.SetActive(true);
        healthBar.gameObject.SetActive(true);

        //heck
        P1HealthBar.gameObject.SetActive(false);
        P2HealthBar.gameObject.SetActive(false);
        heckSpek.SetActive(false);

        //codep
        coDephealthBar.gameObject.SetActive(false);
        //   coDepMatch.gameObject.SetActive(false);
        //bliss
        BlissProjectiles.gameObject.SetActive(false);
        // Baddie.gameObject.SetActive(false);
        baddieHealthBar.gameObject.SetActive(false);
    }

    void HellStart()
    {

        //heck
        projectileSwitch.SetActive(false);
        heckSpek.SetActive(true);
        P1HealthBar.gameObject.SetActive(true);
        P2HealthBar.gameObject.SetActive(true);

        //vanilla
        healthBar.gameObject.SetActive(false);
        projectileSwitch.SetActive(false);

        //bless
        baddieHealthBar.gameObject.SetActive(false);
        // Baddie.gameObject.SetActive(false);
        BlissProjectiles.gameObject.SetActive(false);

        //codep
        coDephealthBar.gameObject.SetActive(false);
        //  coDepMatch.gameObject.SetActive(false);


    }



    void BlissStart()
    {
        //bliss
        BlissProjectiles.gameObject.SetActive(true);
        Baddie.gameObject.SetActive(true);
        baddieHealthBar.gameObject.SetActive(true);
        P1HealthBar.gameObject.SetActive(true);
        P2HealthBar.gameObject.SetActive(true);

        // Codep
        coDephealthBar.gameObject.SetActive(false);
        //        coDepMatch.gameObject.SetActive(false);

        //vanilla
        projectileSwitch.SetActive(false);
        healthBar.gameObject.SetActive(false);
        //hell

        heckSpek.SetActive(false);
    }


    void CodepStart()
    {

        // Codep
        //  coDephealthBar.gameObject.SetActive(true);
        //  coDepMatch.gameObject.SetActive(true);
        //bliss
        BlissProjectiles.gameObject.SetActive(false);
        //  Baddie.gameObject.SetActive(false);
        baddieHealthBar.gameObject.SetActive(false);
        //vanilla
        projectileSwitch.SetActive(false);
        healthBar.gameObject.SetActive(false);
        //hell
        P1HealthBar.gameObject.SetActive(false);
        P2HealthBar.gameObject.SetActive(false);
        heckSpek.SetActive(false);

    }


    void Update()
    {




        if (!GameManager.IsPaused())
        {
            ;

 /*           if (finishedVanilla == true)
            {
                if (Input.anyKeyDown)
                { LoadB(nextLevel); }
            }*/

     /*       if (finishedEverything == true)
            {
                if (Input.anyKeyDown)
                { LoadB("MainMenu"); }
            }*/


        }



    }


    public void LoadB(string sceneANumber)
    {
        Debug.Log("sceneBuildIndex to load: " + sceneANumber);
        SceneManager.LoadScene(sceneANumber);
    }


    // Happiness Calculator

    public static void Express(string face, string player)
    {




        if (player == "P1")
        {
            singleton.P1Head.ChangeFace(face);

        }
        else if (player == "P2")
        { singleton.P2Head.ChangeFace(face); }
        else if (player == "monster")
        {
            singleton.Baddie.ChangeFace(face);

        }
        else { Debug.Log("You are trying to elicite a reaction from a non-existant person."); }

    }




   

    public static void UpdateScore(string player, int damageValue)
    {
        if (singleton.level != "coDep")
        {


            singleton.CodeptText(damageValue, player);


        }

        //     Debug.Log("Hit, trying to update score");
        if (singleton.level == "vanilla")
        {
            //   Debug.Log("is vanilla");

            singleton.VanillaScore(damageValue);
        }

        else if (singleton.level == "hell")
        {
            singleton.DomesticHeckScore(player, damageValue);


        }

        else if (singleton.level == "bliss")
        {
            singleton.BlissScore(player, damageValue);


        }
        else if (singleton.level == "coDep")
        {
            singleton.CodepScore(damageValue);


        }

    }


    /// <summary>
    /// // VANILLA FUNCTIONS
    /// </summary>
    //




    public void VanillaScore(int damage)
    {

        healthBar.GainHealth(damage);

      //  relDescText.text = PlayerSatisfaction(healthBar.health);

        if (healthBar.health <= -99)
            if (healthBar.health <= -99)
            {
                Time.timeScale = 0;
                GameManager.HardMute();
                GetComponent<AudioSource>().PlayOneShot(vanillaWinSFX);
                TooToxicMenu.SetActive(true);
                nextLevel = "MainMenu";
            }

    }

    public static string RelationshipStatus()
    {
        string joy = "unassigned";

        singleton.PlayerSatisfaction(singleton.healthBar.health);

        return joy;
    }


    public string PlayerSatisfaction(float score)


    {

        if (score > healthBar.lowHealth && score < healthBar.highHealth)
        { return "fine"; }

        else if (score > healthBar.highHealth)
        { return "obsessed"; }

        else if (score < healthBar.lowHealth)
        {


            return "miserable";
        }
        else { return "broken"; }





    }




    public void LevelTimerEnd()
    {
        if (timer > 1)
        {
            timer = timer - Time.deltaTime;
            if (timer < toxicTimeWarn)
            {
                background.color = Color.yellow;
                countDownTimer.gameObject.SetActive(true);

                float minutes = Mathf.FloorToInt(timerStart / 60);
                float seconds = Mathf.FloorToInt(timerStart % 60);
                countDownTimer.text = "00:0" + seconds.ToString(); //string.Format("{0:00}:{1:00}", minutes, seconds);


                if (Mathf.Approximately(timer / 60, Mathf.RoundToInt(timer / 60)))
                {
                    GetComponent<AudioSource>().PlayOneShot(beepSFX);
                    // Debug.Log(timer);
                }

            }



            if (timer < toxicTime * 60)
            {



                if (relationshipResult == "hell") { EndLevelVanilla(); } else if (relationshipResult == "fine") { background.color = Color.blue; }
            }
        }
        else
        {
            EndLevelVanilla();



        }


    }

    void LevelTimerStart()
    {



        if (timerStart > 1)
        {
            GameManager.DisableControls(true);
            timerStart = timerStart - Time.deltaTime;
            countDownTimer.gameObject.SetActive(true);




        }
        else
        {
            countDownTimer.gameObject.SetActive(false);
            //Debug.Log("StartTimerDown");
            GameManager.DisableControls(false);

        }

    }




    public void EndLevelVanilla()
    {


        GameManager.HardMute();

        if (!finishedVanilla)
        {
            GetComponent<AudioSource>().PlayOneShot(vanillaWinSFX);

        }

        //Pause(true);
        //relDescText.text = relationshipResult;
        Time.timeScale = 0;
        if (PlayerSatisfaction(healthBar.health) == "fine")
        {
            background.color = Color.green;
            blissScreen.SetActive(true);
            nextLevel = "Bliss";


        }
        else if (PlayerSatisfaction(healthBar.health) == "obsessed")
        {
            background.color = Color.yellow;
            coDepScreen.SetActive(true);

            nextLevel = "Codep";

        }

        else
        {
            background.color = Color.red;
            heckScreen.SetActive(true);
            nextLevel = "DomesticHeck";

        }

        finishedVanilla = true;




    }


    // BLISS FUNCTIONS




    public void BlissScore(string player, int damageValue)
    {

        if (player == "P1")
        {
            if (singleton.P1HealthBar.health > 0)
            {

                singleton.P1HealthBar.GainHealth(damageValue);

            }
            else
            {

                singleton.BlissDefeated("P1");

            }

        }
        else if (player == "P2")
        {
            if (singleton.P2HealthBar.health > 0)

            {

                singleton.P2HealthBar.GainHealth(damageValue);


            }
            else
            {
                singleton.BlissDefeated("P2");


            }
        }

        else if (player == "monster")
        {
            if (singleton.baddieHealthBar.health > 0)

            {
                singleton.baddieHealthBar.GainHealth(damageValue);
            }
            else
            {
                singleton.BlissDefeated("monster");


            }


        }
        else { Debug.Log(player + "does not exist, cannot damage"); }









    }



    public void BlissDefeated(string player)
    {
        //Time.timeScale = 0;
        GameManager.DisableControls(true);
        GameManager.HardMute();
        GetComponent<AudioSource>().PlayOneShot(blissWinSFX);
        nextLevel = "MainMenu";
        finishedEverything = true;
        if (player == "monster")

        { singleton.BlissWinScreen.SetActive(true); }
        else if (player == "P1")
        { singleton.P1DieScreen.SetActive(true); }
        else
        { singleton.P2DieScreen.SetActive(true); }







    }




    /*HECK FUNCTIONS*/

    public void DomesticHeckScore(string player, int damageValue)
    {

        if (player == "P1")
        {
            if (singleton.P1HealthBar.health > 0)
            {

                singleton.P1HealthBar.GainHealth(damageValue);
                Debug.Log("Hit P1");
            }
            else
            {
                if (!heckOver)
                {
                    singleton.HeckDefeated("P1");
                    heckOver = true;
                }
            }

        }
        else if (player == "P2")
        {
            if (singleton.P2HealthBar.health > 0)

            {

                singleton.P2HealthBar.GainHealth(damageValue);

                Debug.Log("hitP2  THIS ONE is over updating");
            }
            else
            {
                if (!heckOver)
                {
                    singleton.HeckDefeated("P2");
                    heckOver = true;
                }

            }
        }
        else { Debug.Log(player + "does not exist, cannot damage"); }

    }

    public void HeckDefeated(string player)
    {
        Time.timeScale = 0;
        GameManager.HardMute();
        nextLevel = "MainMenu";
        finishedEverything = true;

        if (player == "P1")

        {
            singleton.P2WinScreen.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(P2WinSFX);
        }
        else
        {
            singleton.P1WinScreen.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(P1WinSFX);
        }
    }





  








    /*CODEPENDANT FUCNTONS*/

    public static void CodepScore(bool win)
    {





        if (win)
        {
            singleton.coDepWins++;


            GameObject newHeart = Instantiate(singleton.goodHeartFab);
            newHeart.transform.SetParent(singleton.goodHeartparent.transform);
         //  singleton.scaleChange = new Vector3(0.82375f, 0.82375f, 0.82375f);
            newHeart.transform.localScale = new UnityEngine.Vector3(0.82375f, 0.82375f, 0.82375f);



            if (singleton.coDepWins > 9)
            {
                LevelTracker.TextJump("One more!", "big");
            }
            else if (singleton.coDepWins > 8)
            {
                LevelTracker.TextJump("just a few more", "big");
            }
            else
            {
                singleton.AnnounceGood();
            }
            //   singleton.agreementText.text = "Agreements: " + singleton.coDepWins; 



        }
        else
        {
            singleton.coDepFail++;

            GameObject newHeart = Instantiate(singleton.badHeartFab);

            newHeart.transform.SetParent(singleton.badHeartparent.transform);
            newHeart.transform.localScale = new UnityEngine.Vector3(0.82375f, 0.82375f, 0.82375f);


            // singleton.disagreementText.text = "Disagreements: " + singleton.coDepFail;

            if (singleton.coDepFail > 12)
            {
                LevelTracker.TextJump("they're losing patience", "big");
            }
            else if (singleton.coDepFail > 14)
            {
                LevelTracker.TextJump("NO MORE CHANCES", "big");
            }
            else
            {
                singleton.AnnounceBad();
            }

        }

        if (singleton.coDepWins > 10) { singleton.CodepWinScreen.SetActive(true); GameManager.DisableControls(true); }
        else if (singleton.coDepFail > 15)
        {

            GameManager.DisableControls(true);
            singleton.CoDepLoseScreen.SetActive(true);
        }
    }




    void CodeptText(int damageValue, string player)
    {
        int choose = Random.Range(0, 3);
        string textSize;

        if (choose > 1)
        { textSize = "small"; }
        else { textSize = "big"; }

        int rando = Random.Range(0, 5);
        int textRando;

        //   Debug.Log(player);
        if (rando < 2)
        {
            if (damageValue > 0)
            {


                if (player == "monster")
                {
                    textRando = Random.Range(0, singleton.monsterHealText.Count);
                    LevelTracker.TextJump(singleton.monsterHealText[textRando], textSize);
                }
                else
                {
                    textRando = Random.Range(0, singleton.goodText.Count);
                    LevelTracker.TextJump(singleton.goodText[textRando], textSize);
                }

            }
            else
            {

                if (player == "monster")
                {
                    textRando = Random.Range(0, singleton.monsterHitText.Count);
                    LevelTracker.TextJump(singleton.monsterHitText[textRando], textSize);


                }
                else
                {
                    textRando = Random.Range(0, singleton.badText.Count);
                    LevelTracker.TextJump(singleton.badText[textRando], textSize);
                }
            }







        }
    }


    public void CodepScore(int damage)
    {

        coDephealthBar.GainHealth(damage);

        //   relDescText.text = PlayerSatisfaction(healthBar.health);

        if (coDephealthBar.health <= -99)
        {
            Time.timeScale = 0;
            GameManager.HardMute();
            GetComponent<AudioSource>().PlayOneShot(coDepLooseSFX);
            CoDepLoseScreen.SetActive(true);
            nextLevel = "MainMenu";
        }

    }

    public void CoDepWin()
    {
        Time.timeScale = 0;
        GameManager.HardMute();
        GetComponent<AudioSource>().PlayOneShot(coDepWinSFX);
        CodepWinScreen.SetActive(true);
        nextLevel = "MainMenu";
        finishedEverything = true;
    }

    void EndLevelCoDep()
    {
        Time.timeScale = 0;
        GameManager.HardMute();
        GetComponent<AudioSource>().PlayOneShot(coDepWinSFX);
        GameManager.DisableControls(true);
        singleton.CoDepLoseScreen.SetActive(true);
        finishedEverything = true;
        nextLevel = "MainMenu";
    }


}