using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FractionSelectScreen : MonoBehaviour 
{	
	public GameObject _TintScreen;

	public bool _Enabled = false;
	bool _EnabledLocal = false;

	void Update()
	{
		if ( _Enabled != _EnabledLocal )
		{
			_EnabledLocal = _Enabled;
		}
	}

	public void SelectGym()
	{
		gameObject.SetActive(false);
	}

	public void SelectGeo()
	{
		gameObject.SetActive(false);
	}
}
