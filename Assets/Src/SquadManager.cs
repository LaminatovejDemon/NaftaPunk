using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquadManager : MonoBehaviour {

	public Material TrooperAllyMaterial;
	public Material TrooperEnemyMaterial;

	public Pathfinding _Pathfinding;

	public List<Trooper> _AllyList = new List<Trooper>();
	public List<Trooper> _EnemyList = new List<Trooper>();

	private static SquadManager _Instance = null;
	
	public static SquadManager GetInstance()
	{
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
				_SelectedTrooper.Walk(_Pathfinding.GetPath(_SelectedTrooper, direction));
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
			SetCenterPoint();
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
	
	public Trooper GetClosestVisibleTrooper(Trooper watcher, Trooper.Fraction fraction)
	{
		List<Trooper> targetList_ = fraction == Trooper.Fraction.F_Ally ? _AllyList : _EnemyList;

		if ( targetList_.Count == 0 )
		{
			return null;
		}

		Trooper closestTrooper_ = null;
		float closestDelta_ = 0;

		for ( int i = 0; i < targetList_.Count; ++i )
		{
			float delta_ = (targetList_[i].transform.position - watcher.transform.position).magnitude;

			if ( closestTrooper_ != null && closestDelta_ <= delta_ )
			{
				continue;
			}

			GameObject targetBelow_ = _Pathfinding.GetTileBelow(targetList_[i].transform.position);

			// FIXME
/*			if ( _Pathfinding.GetPath(watcher, targetBelow_) == targetBelow_ )
			{
				closestTrooper_ = targetList_[i];
				closestDelta_ = delta_;
			}*/
		}

		return closestTrooper_;
	}

	public void SetCenterPoint()
	{
		int allyCount_ = _AllyList.Count;
		if (allyCount_ == 0 )
		{
			return;
		}
		float mx_, mz_, Mx_, Mz_;

		mx_ = Mx_= _AllyList[0]._TargetPosition.x;
		mz_ = Mz_= _AllyList[0]._TargetPosition.z;

		Vector3 tempPos_;

		for ( int i = 0; i < _AllyList.Count; ++i )
		{
			tempPos_ = _AllyList[i]._TargetPosition;
			if ( tempPos_.x < mx_ ) mx_ = tempPos_.x;
			if ( tempPos_.x > Mx_ ) Mx_ = tempPos_.x;
			if ( tempPos_.z < mz_ ) mz_ = tempPos_.z;
			if ( tempPos_.z > Mz_ ) Mz_ = tempPos_.z;
		}

		CameraManager.GetInstance().SetPosition(new Vector3((Mx_ - mx_) * 0.5f + mx_, 0, (Mz_ - mz_) * 0.5f + mz_));
	}
}
