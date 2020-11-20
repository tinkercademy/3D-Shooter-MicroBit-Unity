using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance;

	public Sound[] BgmSounds;
    public Sound[] SFXSounds;

    public float baseBGMSoundVol = 0.5f;
	public float baseSFXSoundVol = 0.5f;

	void Awake()
	{
		//if there is additional soundmanager created,destroy it
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			//set it to not being destroyed when going to different scene
			DontDestroyOnLoad(gameObject);
		}

		//add component into the SoundManager Object
		foreach (Sound s in BgmSounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
		}

		foreach (Sound s in SFXSounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
		}
	}
	private void Start()
	{

		//play background music when start
		PlayBGM(BgmSounds[0].soundName);
	}
	//songs for background music
    public void PlayBGM(string sound)
	{
		//search for the music thru the name of the music,if is not found,it will not play the music
		Sound s = Array.Find(BgmSounds, item => item.soundName == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}
		foreach (Sound songs in BgmSounds)
		{
			songs.source.Stop();
		}
		s.source.Play();
	}
	public void StopBGM(string sound)
	{
		//search for the music thru the name of the music,if is not found,it will not stop the music
		Sound s = Array.Find(BgmSounds, item => item.soundName == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}
		s.source.Stop();
	}
	//songs for game
	public void PlaySFX(string sound)
	{
		//search for the music thru the name of the music,if is not found,it will not play the music
		Sound s = Array.Find(SFXSounds, item => item.soundName == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}
		if (!s.source.isPlaying)
			s.source.Play();
	}
	public void StopSFX(string sound)
	{
		//search for the music thru the name of the music,if is not found,it will not play the music
		Sound s = Array.Find(SFXSounds, item => item.soundName == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}
		if (s.source.isPlaying)
			s.source.Stop();
	}
	//update the volume of all the songs that in the bgm list
	public void UpdateBGMVolume(float volumer)
	{
		foreach (Sound s in BgmSounds)
		{
			s.source.volume = volumer;
		}
		baseBGMSoundVol = volumer;
	}

	//update the volume of all the songs that in the song list
	public void UpdateSFXVolume(float volumer)
	{
		foreach (Sound s in SFXSounds)
		{
			s.source.volume = volumer;
		}
		baseSFXSoundVol = volumer;
	}

	//get the current playing music song clip length(for in game music when playing)
	public float CurrentMusicLength()
	{
		float length = 0f;
			foreach (Sound s in SFXSounds)
				if (s.source.isPlaying)
					length = s.source.clip.length;
		return length;
	}
}
