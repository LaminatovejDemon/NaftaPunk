using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquadManager : MonoBehaviour {

	public Material TrooperAllyMaterial;
	public Material TrooperEnemyMaterial;

	public List<Trooper> _AllyList = new List<Trooper>();
	public List<Trooper> _EnemyList = new List<Trooper>();

	private static SquadManager _Instance = null;
	
	public static SquadManager GetInstance()
	{
		if ( _Instance == null )
		{
			_Instance = new GameObject().AddComponent<SquadManager>();
			_Instance.gameObject.name = "_SquadManager";
			_Instance.transform.parent = null;
		}
		
		return _Instance;
	}

	public void Start()
	{
		_Instance = this;
	}
	
	Trooper _SelectedTrooper;

	public void OrderTrooper(GameObject direction)
	{
		if ( _SelectedTrooper != null )
		{
			if ( !_SelectedTrooper.HasDirection(direction) )
			{
				_SelectedTrooper.SetDirection(direction);
			}
			else
			{
				_SelectedTrooper.Walk(direction);
			}
		}
	}

	public void RegisterTrooper(Trooper target, Trooper.Fraction fraction)
	{
		List<Trooper> targetList_ = fraction == Trooper.Fraction.F_Enemy ? _EnemyList : _AllyList;
		List<Trooper> excludedList_ = fraction == Trooper.Fraction.F_Enemy ? _AllyList : _EnemyList;

		if ( excludedList_.Contains(target) )
		{
			excludedList_.Remove(target);
		}

		if ( !targetList_.Contains(target) )
		{
			targetList_.Add(target);
		}
	}
	
	public void SelectTrooper(Trooper trooper)
	{
		Debug.Log ("Selecting trooper" + trooper + " after " + _SelectedTrooper);
		if ( _SelectedTrooper != null )
		{
			_SelectedTrooper.OnSelect(false);
		}

		_SelectedTrooper = trooper;

		if ( _SelectedTrooper != null )
		{
			_SelectedTrooper.OnSelect(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
