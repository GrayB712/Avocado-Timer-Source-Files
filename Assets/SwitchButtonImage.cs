using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButtonImage : MonoBehaviour
{
    public Image buttonImage;
    public Sprite playButton;
    public Sprite pauseButton;
    public bool isPlay = false;
    public void switchImage()
    {
        if (!TimerScript.isLocked)
        {
        if(isPlay)
        {
            buttonImage.sprite = playButton;
        } else{
            buttonImage.sprite = pauseButton;
        }
        isPlay = !isPlay;
        }
        
    }
    public void setImage(bool isPlaying)
    {
        if (!TimerScript.isLocked)
        {
        if(isPlaying)
        {
            buttonImage.sprite = pauseButton;
        } else{
            buttonImage.sprite = playButton;
        }
        isPlay = false;
        }
        
    }
}
