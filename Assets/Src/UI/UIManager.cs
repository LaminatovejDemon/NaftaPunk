using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour 
{
	private static UIManager _Instance = null;
	public UIPortrait PortraitContainerTemplate;
	List<Trooper> _RegisteredTrooperList = new List<Trooper>();
	List<UIPortrait> _RegisteredTrooperPortraits = new List<UIPortrait>();

	public Material SelectedPortrait;
	public Material DefaultPortrait;

	public static UIManager GetInstance()
	{
		return _Instance;
	}

	void Start()
	{
		_Instance = this;
	}

	public void OnSelect(Trooper target, bool state)
	{
		UIPortrait targetPortrait_ = GetPortrait(target);
		if ( targetPortrait_ != null )
		{
			targetPortrait_.OnSelect(state);
		}
	}

	public List<UIPortrait> GetPortraits()
	{
		return _RegisteredTrooperPortraits;
	}

	void RemovePortrait(int index)
	{
		_RegisteredTrooperPortraits[index].gameObject.SetActive(false);

		//_RegisteredTrooperList.RemoveAt(index);
		//_RegisteredTrooperPortraits.RemoveAt(index);
	}

	public void SortPortraits()
	{
		for ( int i = 0; i < _RegisteredTrooperPortraits.Count; ++i )
		{
			_RegisteredTrooperPortraits[i].transform.position = camera.ViewportToWorldPoint(new Vector3(0,1,0)) 
				+ Vector3.right * camera.orthographicSize * 0.02f
					+ Vector3.down * (camera.orthographicSize * 0.02f * (i+1)
					                  + (i) * _RegisteredTrooperPortraits[i].collider.bounds.extents.y * 2.0f);

		}
	}

	void AddPortrait(Trooper target)
	{
		if ( _RegisteredTrooperList.Contains(target) )
		{
			_RegisteredTrooperPortraits[_RegisteredTrooperList.IndexOf(target)].gameObject.SetActive(true);
			return;
		}

		GameObject portrait_ = (GameObject)GameObject.Instantiate(PortraitContainerTemplate.gameObject);
		portrait_.name = "Portrait_" + _RegisteredTrooperList.Count;
		portrait_.transform.parent = transform;

		UIPortrait newPortrait_ = portrait_.GetComponent<UIPortrait>();

		newPortrait_.SetStats(target);

		_RegisteredTrooperPortraits.Add(newPortrait_);
		_RegisteredTrooperList.Add(target);
	}

	UIPortrait GetPortrait(Trooper target)
	{
		if ( _RegisteredTrooperList.Contains(target) )
		{
			return _RegisteredTrooperPortraits[_RegisteredTrooperList.IndexOf(target)].GetComponent<UIPortrait>();
		}

		return null;
	}

	public void SetStats(Trooper target)
	{
		UIPortrait targetPortrait_ = GetPortrait(target);
		if ( targetPortrait_ != null )
		{
			targetPortrait_.SetStats(target);
		}
	}

	public void RegisterTrooper(Trooper target, bool state)
	{
		if ( _RegisteredTrooperList.Contains(target) )
		{
			if ( !state )
			{
				RemovePortrait(_RegisteredTrooperList.IndexOf(target));
				SortPortraits();
			}
			return;
		}

		if ( !state )
		{
			return;
		}

		AddPortrait(target);

		SortPortraits();
	}
}
