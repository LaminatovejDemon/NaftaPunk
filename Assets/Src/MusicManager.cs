using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour {

	public AudioClip _IntroMusic;
	public AudioClip _GeoMusic;
	public AudioClip _GymMusic;

	public AudioClip _GramophonePick;
	public AudioClip _Button;
	public AudioClip _Button2;
	public AudioClip _PickupXP;

	public List<AudioClip> _BerserkArt; 
	public List<AudioClip> _BerserkGym;
	public List<AudioClip> _DeathArt; 
	public List<AudioClip> _DeathGym; 

	static MusicManager _Instance = null;

	public static MusicManager GetInstance()
	{
		return _Instance;
	}

	// Use this for initialization
	void Start () 
	{
		if ( _Instance != null )
		{
			GameObject.Destroy(this);
			return;
		}
		_Instance = this;

		DontDestroyOnLoad(this);
	}

	public enum Music
	{
		Intro,
		Geo,
		Gym
	};

	public enum OneShots
	{
		GramophonePick,
		Button,
		Button2,
		PickupXP,
		Berserk_Art,
		Berserk_Gym,
		Death_Art,
		Death_Gym,
	};

	public void PlayOneShot(OneShots shot)
	{
		AudioSource source_ = GetComponent<AudioSource>();

		switch (shot)
		{
		case OneShots.GramophonePick:
			source_.PlayOneShot(_GramophonePick);
			break;
		}
	}

	public void Play(Music what, bool loop)
	{
		AudioSource source_ = GetComponent<AudioSource>();

		switch (what)
		{
		case Music.Geo:
			source_.clip = _GeoMusic;
			break;
		case Music.Gym:
			source_.clip = _GymMusic;
			break;
		default:
			source_.clip = _IntroMusic;
			break;
		}

		source_.loop = loop;
		source_.Play();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
