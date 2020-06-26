using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

//  WICHTIG!
//um nun Sounds abspielen zu lassen, muss an den stellen, wo der sound abgespielt werden sollte ( zum beispiel bei einer Kollision) : " FindObjectOfType<AudioManager>().Play("NameDesSounds"); " stehen.


public class SoundManager : MonoBehaviour {

	public bool initializeOnAwake = true;
	public AudioMixerGroup audioMixer;
	public static SoundManager instance;
	
	//erstellt ein Array, "sounds" der Klasse Sound( anderes Script)

	public Sound[] sounds;

	private void Awake()
	{
		if (initializeOnAwake)
		{
			Init();
		}
	}
	// initialized through gamemanager
	public void Init()
	{
		MakeUndestructibleSingleton();

		foreach (Sound s in sounds)
		{
			//sucht fuegt eine AudioSource komponente fuer jedes Element in Array "sounds" zu
			s.source = gameObject.AddComponent<AudioSource>();

			//setzt den clip, volume, pitch der Inspector eingabe in die hinzugefuegte AudioSource komponente ein
			s.source.clip = s.clip;

			if (s.fadeActivated)
			{
				s.source.volume = 0;
			}
			else
			{
				s.source.volume = s.targetVolume;
			}

			s.source.pitch = s.pitch;
			s.source.outputAudioMixerGroup = audioMixer;
			s.source.playOnAwake = false;

		}
	}

	private void Update()
    {
        // fade out all fade-out possible sounds that have not beed "sustained in this frame"
        foreach (Sound s in sounds)
        {
            if (s.fadeActivated)
            {
                if (s.sustained)
                {
                    s.sustained = false;
                }
                else
                {
                    StopWithFadeOut(s.name);
                }
            }

        }
    }

    public void Play(string name)
    {
		if (Time.frameCount > 1) {
			Sound s = Array.Find (sounds, sound => sound.name == name);

			if (s == null) { 
				return;
			}

			//Debug.Log (s.name);
			if (s.source.isPlaying == false) {
				s.source.Play ();
			}
		}

    }

	public void PlayOrRestart(string name)
	{
		if (Time.frameCount > 1)
		{
			Sound s = Array.Find(sounds, sound => sound.name == name);

			if (s == null)
			{
				return;
			}

			s.source.Play();
			
		}

	}

	public void Stop(string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);

        s.sustained = false;

		if (s == null)
		{ 
			return;
		}

		s.source.Stop();
		s.source.loop = false;

		if (s.fadeActivated) {
			s.source.volume = 0;
		}
	}


	// fades in till the max volume is reached and keeps playing
	public void PlayWithFadeIn (string name) 
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        PlayWithFadeIn(name, s.targetVolume);
		
	}


	public void PlayWithFadeIn ( string name, float volume) {

		Sound s = Array.Find (sounds, sound => sound.name == name);

        s.sustained = true; 

		if (s.fadeActivated) {
			
			if (s.source.volume == 0) {
				Play (name);
				s.source.loop = true;

			}

			if (s.fadeTime != 0) {
				if (s.source.volume < volume) {
					s.source.volume += Time.deltaTime / s.fadeTime;

				}
			} else {
				s.source.volume = volume;
			}
		} else {
			Play (s.name);
            SetVolume (s.name, volume);
		}
	}

	// fades out and stops 
	public void StopWithFadeOut (string name) {

		Sound s = Array.Find(sounds, sound => sound.name == name);	

		if (s.source.volume > 0) {
			if (s.fadeActivated) {
				if (s.fadeTime != 0) {
					if (isPlaying (name)) {
						s.source.volume -= Time.deltaTime / s.fadeTime;
					}
				} else {
					s.source.volume = 0; 
				}

				if (s.source.volume == 0) {
					Stop (name);
					s.source.loop = false;
				}

			} else {
				Stop (s.name);
			}
		}
	}

    public IEnumerator C_StopWithFadeOut (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        // 1. Fade enabled?
        if (s.fadeActivated && s.fadeTime != 0)
        {
            // 2. loop
            while (s.source.volume > 0 && isPlaying(name))
            {
                s.source.volume -= Time.deltaTime / s.fadeTime;
                yield return null;
            }
            s.source.volume = 0;
            s.source.loop = false;
        }
        Stop(name);
    }


	//fades the sound towards the given value

	public void FadeTowardsGivenVolume(string name, float volume) {


		Sound s = Array.Find(sounds, sound => sound.name == name);

        s.sustained = true; 

		if (s.source.volume == 0) {

			Play (name);
			s.source.loop = true;

		}
		if (s.fadeTime != 0) {
			if (s.source.volume < volume) {

				s.source.volume += Time.deltaTime / s.fadeTime;

			} else if (s.source.volume > volume) {

				s.source.volume -= Time.deltaTime / s.fadeTime;

			}
		}
	}  



	public bool isPlaying (string name){
		Sound s = Array.Find(sounds, sound => sound.name == name);

		if (s == null)
		{ 
			return false;
		}

		if (s.source.isPlaying) {
			return true;
		}
		else {
			return false;
		}
	
	}


	public void SetVolume (string name, float volume) {

		Sound s = Array.Find(sounds, sound => sound.name == name);

		s.source.volume = volume;
	}
		

	public void MakeUndestructibleSingleton() {

		// implementation of the singleton pattern. (accesing static elements  is easier na more efficient)

		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this.gameObject);

		} 

		else if (instance != null && instance != this) 
		{
			Destroy(this);
			Destroy (this.gameObject);
		}
	}

	public void StopAllSounds() {
		foreach (Sound s in sounds) {
			Stop (s.name);
			} 
	}

	public void StopAllSoundsExcept( List<string> soundNames) {
		foreach (Sound s in sounds) {
			if (!soundNames.Contains(s.name)) { 
				Stop (s.name);
			}
		} 
	}

	public AudioSource GetSource(string name) {

		Sound s = Array.Find(sounds, sound => sound.name == name);

		 return s.source;
	}



}
