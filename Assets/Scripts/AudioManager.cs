using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //all audio sound fx
    public  AudioClip clickSound, gameOverSound, correctSound, incorrectSound, matchFlipSound;
    AudioSource audioSource;

    //Load all sounds from Resource folder and assign them
    private void Start()
    {
        clickSound = Resources.Load<AudioClip>("ClickSound");
        gameOverSound = Resources.Load<AudioClip>("GameOverSound");
        correctSound = Resources.Load<AudioClip>("ImageCorrect");
        incorrectSound = Resources.Load<AudioClip>("Incorrect");
        matchFlipSound = Resources.Load<AudioClip>("MatchFlip");

        audioSource = GetComponent<AudioSource>();
    }


    //Fire off sound depending on the switch statement
    public  void PlaySound(string clip)
    {
        switch(clip)
        {
            case "click":
                audioSource.PlayOneShot(clickSound);
                break;
            case "gameOver":
                audioSource.PlayOneShot(gameOverSound);
                break;
            case "correct":
                audioSource.PlayOneShot(correctSound);
                break;
            case "incorrect":
                audioSource.PlayOneShot(incorrectSound);
                break;
            case "matchFlip":
                audioSource.PlayOneShot(matchFlipSound);
                break;
        }
    }

}
