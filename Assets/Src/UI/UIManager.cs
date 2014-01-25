using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour 
{
	private static UIManager _Instance = null;
	public UIPortrait PortraitContainerTemplate;
	List<Trooper> _RegisteredTrooperList = new List<Trooper>();
	List<UIPortrait> _RegisteredTrooperPortraits = new List<UIPortrait>();
	
	public static UIManager GetInstance()
	{
		return _Instance;
	}

	void Start()
	{
		_Instance = this;
	}

	void RemovePortrait(int index)
	{
		GameObject.Destroy(_RegisteredTrooperPortraits[index].gameObject);

		_RegisteredTrooperList.RemoveAt(index);
		_RegisteredTrooperPortraits.RemoveAt(index);
	}

	void AddPortrait(Trooper target)
	{
		GameObject portrait_ = (GameObject)GameObject.Instantiate(PortraitContainerTemplate.gameObject);
		portrait_.name = "Portrait_" + _RegisteredTrooperList.Count;
		portrait_.transform.parent = transform;
		portrait_.transform.position = camera.ViewportToWorldPoint(new Vector3(0,1,0)) 
			+ Vector3.right * camera.orthographicSize * 0.02f
			+ Vector3.down * (camera.orthographicSize * 0.02f * (_RegisteredTrooperPortraits.Count+1)
				                  + (_RegisteredTrooperPortraits.Count) * portrait_.collider.bounds.extents.y * 2.0f);

		UIPortrait newPortrait_ = portrait_.GetComponent<UIPortrait>();

		newPortrait_.SetStats(target);

		_RegisteredTrooperPortraits.Add(newPortrait_);
		_RegisteredTrooperList.Add(target);
	}

	public void SetStats(Trooper target)
	{
		if ( _RegisteredTrooperList.Contains(target) )
		{
			_RegisteredTrooperPortraits[_RegisteredTrooperList.IndexOf(target)].GetComponent<UIPortrait>().SetStats(target);
		}
	}

	public void RegisterTrooper(Trooper target, bool state)
	{
		if ( _RegisteredTrooperList.Contains(target) )
		{
			if ( !state )
			{
				RemovePortrait(_RegisteredTrooperList.IndexOf(target));
			}
			return;
		}

		if ( !state )
		{
			return;
		}

		AddPortrait(target);
	}
}
