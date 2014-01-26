using UnityEngine;
using System.Collections;


public class MusicManager : MonoBehaviour {


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
		Menu,
		Ingame,
	};

	public void Play(Music what)
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
