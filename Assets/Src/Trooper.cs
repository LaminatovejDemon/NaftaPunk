using UnityEngine;
using System.Collections;

public class Trooper : MonoBehaviour {

	public GameObject _HighlightCircle;

	public bool _Selected = false;

	public void OnSelect(bool state)
	{
		_Selected = state;
		_HighlightCircle.SetActive(state);
	}

}
