using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

//wichtig damit der Inspector die klasse displayed, ohne dass sie "monobehavior" hat.
[System.Serializable]
public class Sound {

    public string name;

    public AudioClip clip;

    [Range(0f,2f)]
	public float targetVolume = 1;

    [Range(0.1f, 3f)]
    public float pitch = 1;

    [HideInInspector]
    public AudioSource source;

	public bool fadeActivated = false;

	public float fadeTime;

    [HideInInspector] public bool sustained = false;
		
}

