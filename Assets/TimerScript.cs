using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    // These are all of my declared variables and references.
    // They usually define the starting value of all of my variables,
    // so, for example, I start my pace variable on the first pace, "0" (jog)
    //
    // These also contain references to Unity GameObjects (On screen objects
    // like the avocado or the pause button), so that I can change their properties
    // in this script. You can then go into Unity, and when viewing this script, it will
    // show you slots for each reference. You then can drag gameobjects into each reference.  
    int pace = 0;
    float lastTimeChanged = 0f;
    public Animator animatorOne;
    public Animator animatorTwo; //arms animator
    public Animator avocadoOutlineAnimator;
    public Animator lockAnimator;
    public Image timerCircle;
    public Image playButtonImage;
    // For example, this next line is a reference to the image on the restart button.
    // I first that I what kind of object it is, in this case an "Image", and then I
    // give it a name that I will refer to it by in the rest of this script.
    public Image restartButtonImage;
    public TextMeshProUGUI outputText;
    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI totalTimeText;
    public Image backgroundImage;
    public bool isPaused = true;
    public AudioClip[] pacesAudio = new AudioClip[3];
    public AudioSource audioPlayer;
    float currentTime;
    public SwitchButtonImage playButton;
    public float timeSinceStart = 1f;
    public static bool isLocked = false;
    float lastFrameTime;
    public Animator lockAlertAnimator;


    paceProperties[] paces;
    // Start is called once when the timer is first opened
    void Start()
    {
        //I set the initial properties of several things

        //I record what time we started at
        lastFrameTime = Time.time;

        //I create a new array called paces, that is just a list of each of the different
        //paces and their properties. In this paces array, jog is number 0 -- that's why I
        // set the current "pace" variable to 0 in the section above the "Start() function.
        paces = new paceProperties[]{
        new paceProperties("Jog", new Color32(99,179,99,255), 30, pacesAudio[0], 1f),
        new paceProperties("Race Pace", new Color32(60,156,60, 255), 30, pacesAudio[1], 1.5f),
        new paceProperties("Sprint", new Color32(243, 33, 27, 255), 10, pacesAudio[2], 2f)};

        //I record what time we started at... again (look, I'm not the best coder)
        lastTimeChanged = Time.time;

        //I look at what pace we are currently at and change the "Output Text"'s text property 
        // to the name of that pace. So, it will output the word "Jog."
        outputText.text = paces[pace].name;

        //I set the starting time to 0
        currentTime = 0f;

        // I change the color halo around the avocado, which is a Game Object that I called
        // "background image" for some reason, to the color of the pace we are currently at.
        // (that's how I change the color to red when sprinting)
        backgroundImage.color = paces[pace].color;
    }

    // This is a little function that I have told Unity to run every time the Play/pause 
    // button is pressed. -- You can create a button in the Unity editor, drag it where
    // you want, and tell it what function in what script you want it to tun when pressed. 
    public void PlayAndPause()
    {
        // So this if statement runs when the isLocked variable is false. I only make the
        // Islocked variable true when the user pressed the lock button at the top of the
        // screen.
        if(!isLocked)
        {
            //This next if statement runs if the timer is currently paused, and so needs to be 
            //restarted.
            if(isPaused)
            {
                //This gets the "audioplayer" game object and tells it to play the audio recording
                // for the pace that we are currently on.
                audioPlayer.clip = paces[pace].audio;
                audioPlayer.Play ();

            }else{

            }
            // This changes the paused variable to its opposite, since the Play/Pause button got pressed
            isPaused = !isPaused;

            //This unpauses/pauses the animators that I have on the arms (which I creatively named
            // animatorOne and animatorTwo), and the animator on the colored halo around the avocado
            //(avocado outline timer).  
            animatorOne.SetBool("isPaused", isPaused);
            animatorTwo.SetBool("isPaused", isPaused);
            avocadoOutlineAnimator.SetBool("IsPaused", isPaused); 

            //This changes the speed of the aforementioned animators to the speed of the current pace
            animatorOne.speed = paces[pace].animSpeed;
            animatorTwo.speed = paces[pace].animSpeed;
            avocadoOutlineAnimator.speed = paces[pace].animSpeed;

            
            lastFrameTime = Time.time;
        } else{
            // This part runs when the user has pressed the PlayAndPause button, but also has the 
            // timer lock turned on. So, I have a little error message animation run that tells the
            // user that they have the timer locked, and so can't play/pause it.
            lockAlertAnimator.SetBool("hasClicked", true);

            // This stops the error message animation a second later. You can look at the 
            // stopLockAlert() function later in the code to see what exactly it does.
            StartCoroutine(stopLockAlert());
        }
    }

    //I have this run every time the player pressed the restart button
    public void Restart()
    {

        if(!isLocked)
        {
            //I described what most of these variables do in the PlayAndPause() function, so
            //I won't do it again here. The main difference is that I change them to what
            // they should be if we are restarting, instead of pausing -- for example, resetting
            // the total time to 0
        pace = 0;
        outputText.text = paces[pace].name;
        backgroundImage.color = paces[pace].color;
        isPaused = true;
        animatorOne.SetBool("isPaused", isPaused);
        animatorTwo.SetBool("isPaused", isPaused);
        avocadoOutlineAnimator.SetBool("IsPaused", isPaused);
        currentTime = 0f;
        currentTimeText.text = Math.Truncate(currentTime) + "s";
        timerCircle.fillAmount = 0;
        playButton.setImage(false);
        timeSinceStart = 1f;
        totalTimeText.text = "00:00";
        lastFrameTime = Time.time;
        }else{
            // This part runs when the user has pressed the PlayAndPause button, but also has the 
            // timer lock turned on. So, I have a little error message animation run that tells the
            // user that they have the timer locked, and so can't play/pause it.
            lockAlertAnimator.SetBool("hasClicked", true);

            // This stops the error message animation a second later. You can look at the 
            // stopLockAlert() function later in the code to see what exactly it does.
            StartCoroutine(stopLockAlert());
        }

    }

    //This next funtion stops a little error message animation .1 seconds after it starts
    IEnumerator stopLockAlert()
    {
        yield return new WaitForSeconds(.1f);
        lockAlertAnimator.SetBool("hasClicked", false);
    }

    // Update() function is called once per frame
    void Update()
    {
        //This is supposed to make the screen not fall alseep.. But it doesn't work too well
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // This runs only if the timer is unpaused
        if(!isPaused)
        {
            //These update all of my convoluted variables that keep track of time by adding
            //the amount of time since the last frame to them.
        float changeInTime = Time.time-lastFrameTime;
        timeSinceStart += changeInTime;
        currentTime += changeInTime;

        //This strange if/else statement is just a convoluted way of changing the current time to
        // the format that I want (00:00 or 00:00:00), and then updating the total time
        if(timeSinceStart<3600f)
        {
            float minutesSinceStart = Convert.ToSingle(Math.Truncate(timeSinceStart/60));
            totalTimeText.text = String.Format("{0:00}",minutesSinceStart)+":"+String.Format("{0:00}",Math.Truncate(timeSinceStart-(minutesSinceStart*60)));
        }else{
            float hoursSinceStart = Convert.ToSingle(Math.Truncate(timeSinceStart/3600));
            float minutesSinceStart = Convert.ToSingle(Math.Truncate(timeSinceStart/60-(hoursSinceStart*60)));
            totalTimeText.text = String.Format("{0:00}",hoursSinceStart)+":"+String.Format("{0:00}",minutesSinceStart)+":"+String.Format("{0:00}",Math.Truncate(timeSinceStart-(Math.Truncate(timeSinceStart/60)*60)));

        }

        //This stores the amount of time that the current pace should take
        float totalTime = paces[pace].time;
        
        // This takes the timer object that tells you how much time is left in the current
        // pace (1s, 2s, 3s, etc.) and updates it.
        // Math.Truncate just cuts off the long decimal part (For example it changes 4.243 seconds
        // to 4 seconds)
        currentTimeText.text = Math.Truncate(currentTime)+1 + "s";

        //This checks if the current time left in the pace is greater than or equal how long the pace should
        // last -- if it is longer, it changes the pace with the changePace() function, which you can see later
        // in this script
        if(currentTime>=totalTime){
            changePace();
            
        }else{
            //This takes the little circle timer around the Avocado pit  (which I have named
            // "timerCircle") and changes what percent it is filled to what percent of the way
            // you are through a pace.
            timerCircle.fillAmount = currentTime/totalTime;
        }
        }
        lastFrameTime = Time.time;
    }


    //This next function changes the UI (User Interface) when the lock button is pressed
    //for example, dimming the color of the restart and play/pause buttons
    public void lockInput()
    {
        isLocked = !isLocked;
        if(isLocked)
        {
            playButtonImage.color = new Color32(255, 255, 255, 80);
            restartButtonImage.color = new Color32(255, 255, 255, 80);
            lockAnimator.SetBool("IsLocked", isLocked);
        } else
        {
            playButtonImage.color = new Color32(255, 255, 255, 255);
            restartButtonImage.color = new Color32(255, 255, 255, 255);
            lockAnimator.SetBool("IsLocked", isLocked);
        }
    }

    //This function is called whenever it is time to change the pace.
    void changePace()
    {
        //CHanges the time left in the pace to 0
        currentTime = 0f;

        //This increases which pace we are on by one, unless we are already on
        // the last pace (sprint), in which case, it resets it back to the 0th pace
        // (jog)
        if((pace+1)>=paces.Length)
        {
            pace = 0;
        }else
        {
            pace++;

        }

        //This updates all of the UI (User Interface) when you change pace, for example, changing
        // the text, like "jog" to the text of the next pace, like "race pace"
        outputText.text = paces[pace].name;
        backgroundImage.color = paces[pace].color;
        //This next line plays the audio for the current pace
        audioPlayer.clip = paces[pace].audio;
		audioPlayer.Play();
        //This next line changes the speed of the arms animations
        animatorOne.speed = paces[pace].animSpeed;
        animatorTwo.speed = paces[pace].animSpeed;
        avocadoOutlineAnimator.speed = paces[pace].animSpeed;
        
    }
}

//using "class"es in C#, you can essentially create your own types
// of variable with all of the properties you want. In this case, I am
// creating a variable that stores all of the properties for any given pace.
// These include its name, how much time it takes, and what color the
// halo around the avocado should be.
public class paceProperties
{
    public string name;
    public float time;
    public Color color;
    public AudioClip audio;
    public float animSpeed;

    // I believe this next part is called a constructor. Basically, it updates all of
    // the properties in a pace when you change the properties of the pace.
    public paceProperties(string _name, Color _color, int _time, AudioClip _audio, float _animSpeed)
    {
        name=_name;
        time=_time;
        color=_color;
        audio=_audio;
        animSpeed = _animSpeed;
    }
}

//There are other scripts that interact with this script, but this script does most of
// the work.