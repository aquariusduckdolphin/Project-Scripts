using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    //An array to store the audio clips
    public AudioClip[] clip;

    //Stores the audio source from the game object
    public AudioSource source;

    //A variable to change the clips sound (range is zero or one)
    public float volume = 1f;

    //When the game begins
    void Start()
    {

        //Get the audio sorce and store it
        source = GetComponent<AudioSource>();
        
    }

    //Custom function that will take in the clip and volume to play the audio
    public void PlayOnce( AudioClip effect, float loudness)
    {

        //On the audio source play a clip at a certain volume. Cannot be looped
        source.PlayOneShot(effect, loudness);

    }

    //Custom function that will or will not loop 
    public void LoopableSound(AudioClip effect, bool canLoop)
    {

        GetComponent<AudioSource>().clip = effect;

        GetComponent<AudioSource>().loop = canLoop;

        GetComponent<AudioSource>().Play();

    }

    //Custom function that will play the clip at a cetain location
    public void AreaOfSound(AudioClip effect, int Xpos, int Ypos, int Zpos)
    {

        AudioSource.PlayClipAtPoint(effect, new Vector3(Xpos, Ypos, Zpos));

    }

    //Custom function thta will delay the audio
    public IEnumerator DelaySound(AudioClip effect, float loudness, float timeDelay)
    {

        source.PlayOneShot(effect, loudness);

        yield return new WaitForSeconds(timeDelay);

    }

}
