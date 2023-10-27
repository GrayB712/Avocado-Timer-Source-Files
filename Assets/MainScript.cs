using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour
{
int pace = 0;
    float lastTimeChanged = 0f;
    public Image timerCircle;
    public TextMeshProUGUI outputText;
    public TextMeshProUGUI currentTimeText;
    public Image backgroundImage;
    public bool isPaused = true;
    public AudioClip[] pacesAudio = new AudioClip[3];
    public AudioSource audioPlayer;
    float currentTime;

    paceVariable[] paces;
    // Start is called before the first frame update
    void Start()
    {
        paces = new paceVariable[]{
        new paceVariable("Jog", new Color32(139,192,139,255), 30, pacesAudio[0]),
        new paceVariable("Race Pace", new Color32(73,145,73, 255), 30, pacesAudio[1]),
        new paceVariable("Sprint", new Color32(243, 33, 27, 255), 10, pacesAudio[2])};
        lastTimeChanged = Time.time;
        outputText.text = paces[pace].name;
        currentTime = 0f;
    }
    void PlayAndPause()
    {
        if(isPaused)
        {
            audioPlayer.clip = paces[pace].audio;
		    audioPlayer.Play ();
            //lastTimeChanged = (Time.time-currentTime);
        }
        isPaused = !isPaused;
    }
    void Restart()
    {
        // Scene scene = SceneManager.GetActiveScene();
        // SceneManager.LoadScene(scene.name);
    }
    // Update is called once per frame
    void Update()
    {
        if(!isPaused)
        {
        float totalTime = paces[pace].time+1;
        currentTime += Time.deltaTime;//(Time.time-lastTimeChanged);
        currentTimeText.text = (Math.Truncate(currentTime) + "s");
        if(currentTime>=totalTime){
            changePace();
            
        }else{
            timerCircle.fillAmount = (currentTime/(totalTime));
        }
        }

    }
    void changePace()
    {
        lastTimeChanged = Time.time;
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
		audioPlayer.Play ();
    }
}


public class paceVariable
{
    public string name;
    public float time;
    public Color color;
    public AudioClip audio;
    public paceVariable(string _name, Color _color, int _time, AudioClip _audio)
    {
        name=_name;
        time=_time;
        color=_color;
        audio=_audio;
    }
}
