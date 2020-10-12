using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;






    public string level;

    public bool paused;


    public bool muted;
    public bool muteMusic;



    [Header("Menus")]
    public GameObject pausedBG;
    public AudioSource player;
    public string nextLevel;
    public AudioClip levelIntroMusic;
    public AudioClip levelBGMusic;

    [Header("Musics")]
    public AudioClip vanillaBGClip;
    public AudioClip hellBGClip;
    public AudioClip hellBGClipIntro;

    public AudioClip blissBGClipIntro;
    public AudioClip blissBGClip;

      
    public AudioClip codepBGClipIntro;
    public AudioClip codepBGClip;






    public bool created = false;
    public bool controlDisabled = false;

    public static void DisableControls(bool value)
    {
        instance.controlDisabled = value;


    }





    void Awake()
    {
        /*    if (instance != null)
            {
                GameObject.Destroy(instance);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this);
            }*/

        /*       if (!created)
               {
                   DontDestroyOnLoad(this.gameObject);
                   created = true;
               }

               else
               {
                   Destroy(this.gameObject);
               }*/

        controlDisabled = true;
    }


    void Start()
    {
        instance = this;
        Pause(false);




            

  

        player = instance.GetComponent<AudioSource>();
        StartMusic();

    }

    public void StartMusic()
    {
        player = instance.GetComponent<AudioSource>();
        level = LevelTracker.levelGlobal;
      //  Debug.Log(level + " " + LevelTracker.levelGlobal + " manager");

        if (level == "vanilla")
        {
            levelIntroMusic = vanillaBGClip;
            levelBGMusic = vanillaBGClip;
            player.clip = levelBGMusic;
        }
        else if (level == "hell")
        {
            levelIntroMusic = hellBGClipIntro;
            levelBGMusic = hellBGClip;
            player.clip = levelIntroMusic;
        }
        else if (level == "bliss")
        {
            levelIntroMusic = blissBGClipIntro;
            levelBGMusic = blissBGClip;
            player.clip = levelIntroMusic;
        }

        else if (level == "coDep")
        {
            levelIntroMusic = codepBGClipIntro;
            levelBGMusic = codepBGClip;
            player.clip = levelIntroMusic;
        }
        else
        {
            levelIntroMusic = vanillaBGClip;
            levelBGMusic = vanillaBGClip;
            player.clip = levelBGMusic;
        }



         

        if (!muteMusic) {


            StartCoroutine(playEngineSound()); 
            


        }

    }


    IEnumerator playEngineSound()
    {
        player.clip = levelIntroMusic;
        player.Play();
        yield return new WaitForSeconds(levelIntroMusic.length);
        player.clip = levelBGMusic;
        player.Play();

        player.loop = true;

      


    }











    void Update()
    {



        //if (muteMusic) { player.Play(); } else { player.Pause(); }




        if (Input.GetButtonDown("Cancel"))
        {
            if (!paused)
            {

                Pause(true);
            }
            else
            {
                Pause(false);

            }

        }



    }


    public static void Mute()
    {
        instance.muted = !instance.muted;

        if (instance.muted)
            AudioListener.volume = 1f;

        else
            AudioListener.volume = 0f;
    }

    public  void MuteMusic()
    {
        instance.muteMusic = !instance.muteMusic;

    }
    public static void HardMute()
    {

        instance.muteMusic = true;
        instance.player.Pause();
        

    }


    public static bool IsControlsDisabled()
    {  bool value =  instance.controlDisabled;

        return value;
    }

    public static bool IsPaused()
    {
        bool value = instance.paused;
        return value;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
    }


    public void Paused2(bool value)
    {
        Pause(value);

    }


    public static void Pause(bool value)
    {
        instance.player = instance.GetComponent<AudioSource>();
        instance.paused = value;
        instance.pausedBG.SetActive(value);

        if (value)
        {
            Time.timeScale = 0;
            instance.player.Pause();


        }
        else
        {
            Time.timeScale = 1;
            if (instance.muteMusic == false)
            {
            //    Debug.Log(instance.player);
                instance.player.Play();
            }
            else
            {
             //   Debug.Log(instance.player);
                instance.player.Pause();
            }
        }
    }









    public void LoadB(string sceneANumber)
    {
        Debug.Log("sceneBuildIndex to load: " + sceneANumber);
        SceneManager.LoadScene(sceneANumber);
    }

}