using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    int pace = 0;
    float lastTimeChanged = 0f;
    public Animator animatorOne;
    public Animator animatorTwo; //arms animator
    public Animator avocadoOutlineAnimator;
    public Animator lockAnimator;
    public Image timerCircle;
    public Image playButtonImage;
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
    // Start is called before the first frame update
    void Start()
    {
        lastFrameTime = Time.time;
        //avocadoOutlineAnimator = backgroundImage.GetComponent<Animator>();
        //Screen.sleepTimeout = SleepTimeout.NeverSleep;
        paces = new paceProperties[]{
        new paceProperties("Jog", new Color32(99,179,99,255), 30, pacesAudio[0], 1f),
        new paceProperties("Race Pace", new Color32(60,156,60, 255), 30, pacesAudio[1], 1.5f),
        new paceProperties("Sprint", new Color32(243, 33, 27, 255), 10, pacesAudio[2], 2f)};
        lastTimeChanged = Time.time;
        outputText.text = paces[pace].name;
        currentTime = 0f;
        backgroundImage.color = paces[pace].color;
    }
    public void PlayAndPause()
    {
        if(!isLocked)
        {
        if(isPaused)
        {
            audioPlayer.clip = paces[pace].audio;
		    audioPlayer.Play ();
            //lastTimeChanged = (Time.time-currentTime);

        }else{

        }
        isPaused = !isPaused;
        animatorOne.SetBool("isPaused", isPaused);
        animatorTwo.SetBool("isPaused", isPaused);
        avocadoOutlineAnimator.SetBool("IsPaused", isPaused); 
        animatorOne.speed = paces[pace].animSpeed;
        animatorTwo.speed = paces[pace].animSpeed;
        avocadoOutlineAnimator.speed = paces[pace].animSpeed;
        lastFrameTime = Time.time;
        } else{
            lockAlertAnimator.SetBool("hasClicked", true);
            StartCoroutine(stopLockAlert());
        }
    }
    public void Restart()
    {
        if(!isLocked)
        {
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
            lockAlertAnimator.SetBool("hasClicked", true);
            StartCoroutine(stopLockAlert());
        }

    }
    IEnumerator stopLockAlert()
    {
        yield return new WaitForSeconds(.1f);
        lockAlertAnimator.SetBool("hasClicked", false);
    }
    // Update is called once per frame
    void Update()
    {

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        if(!isPaused)
        {
        float changeInTime = Time.time-lastFrameTime;
        timeSinceStart += changeInTime;
        currentTime += changeInTime;
        if(timeSinceStart<3600f)
        {
            float minutesSinceStart = Convert.ToSingle(Math.Truncate(timeSinceStart/60));
            totalTimeText.text = String.Format("{0:00}",minutesSinceStart)+":"+String.Format("{0:00}",Math.Truncate(timeSinceStart-(minutesSinceStart*60)));
        }else{
            float hoursSinceStart = Convert.ToSingle(Math.Truncate(timeSinceStart/3600));
            float minutesSinceStart = Convert.ToSingle(Math.Truncate(timeSinceStart/60-(hoursSinceStart*60)));
            totalTimeText.text = String.Format("{0:00}",hoursSinceStart)+":"+String.Format("{0:00}",minutesSinceStart)+":"+String.Format("{0:00}",Math.Truncate(timeSinceStart-(Math.Truncate(timeSinceStart/60)*60)));

        }
        float totalTime = paces[pace].time;
        
        currentTimeText.text = Math.Truncate(currentTime)+1 + "s";
        if(currentTime>=totalTime){
            changePace();
            
        }else{
            timerCircle.fillAmount = currentTime/totalTime;
        }
        }
        lastFrameTime = Time.time;
    }
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
    void changePace()
    {
        //lastTimeChanged = Time.time;
        currentTime = 0f;
        if((pace+1)>=paces.Length)
        {
            pace = 0;
        }else
        {
            pace++;

        }
        outputText.text = paces[pace].name;
        backgroundImage.color = paces[pace].color;
        
        audioPlayer.clip = paces[pace].audio;
		audioPlayer.Play();
        animatorOne.speed = paces[pace].animSpeed;
        animatorTwo.speed = paces[pace].animSpeed;
        avocadoOutlineAnimator.speed = paces[pace].animSpeed;
        
    }
}


public class paceProperties
{
    public string name;
    public float time;
    public Color color;
    public AudioClip audio;
    public float animSpeed;
    public paceProperties(string _name, Color _color, int _time, AudioClip _audio, float _animSpeed)
    {
        name=_name;
        time=_time;
        color=_color;
        audio=_audio;
        animSpeed = _animSpeed;
    }
}