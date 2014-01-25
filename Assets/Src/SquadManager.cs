using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquadManager : MonoBehaviour 
{

	public GameObject EnemyTemplate;

	public Material TrooperAllyMaterial;
	public Material TrooperEnemyMaterial;
	public Material TrooperHitMaterial;

	public float ENEMY_SPAWN_DELAY_MIN = 5.0f;
	public float ENEMY_SPAWN_DELAY_MAX = 10.0f;
	public float DELAY_BEFORE_ATTACK_MULTIPLIER = 1.0f;

	public GameObject TargetHighlightTemplate;
	GameObject _TargetHighlightInstance;

	public Pathfinding _Pathfinding;

	public List<Trooper> _AllyList = new List<Trooper>();
	public List<Trooper> _EnemyList = new List<Trooper>();

	private static SquadManager _Instance = null;

	public void TargetHighlight(Vector3 position)
	{
		if ( _TargetHighlightInstance == null )
		{
			_TargetHighlightInstance = (GameObject)GameObject.Instantiate(TargetHighlightTemplate);
			_TargetHighlightInstance.name = "#TargetHighligh";
		}

		_TargetHighlightInstance.transform.position = position;
		_TargetHighlightInstance.animation.Stop();
		_TargetHighlightInstance.animation.Play();
		Debug.Log ("Highlight");
	}

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
				_SelectedTrooper.Walk(_Pathfinding.GetPath(_SelectedTrooper, direction, true));
			}
		}
	}

	public void OrderEnemyTrooper(Trooper enemy, GameObject direction)
	{
		if ( enemy != null )
		{
			if ( !enemy.HasDirection(direction) )
			{
				enemy.SetDirection(direction);
			}
			else
			{
				enemy.Walk(_Pathfinding.GetPath(enemy, direction));
			}
		}
	}
	
	public void OnKilled(Trooper target)
	{
		if( target.GetSpawner() != null )
		{
			target.GetSpawner().ResetSpawner();
		}
		_AllyList.Remove(target);
		_EnemyList.Remove(target);
		UIManager.GetInstance().RegisterTrooper(target, false);
		GameObject.Destroy(target.gameObject);
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

		UIManager.GetInstance().RegisterTrooper(target, fraction == Trooper.Fraction.F_Ally);
	}
	
	public void SelectTrooper(Trooper trooper)
	{
		if ( _SelectedTrooper != null )
		{
			_SelectedTrooper.OnSelect(false);
			UIManager.GetInstance().OnSelect(_SelectedTrooper, false);
		}

		_SelectedTrooper = trooper;

		if ( _SelectedTrooper != null )
		{
			_SelectedTrooper.OnSelect(true);
			UIManager.GetInstance().OnSelect(_SelectedTrooper, true);
		}
	}

	public bool HasVisibility(Trooper watcher, GameObject target, bool mentionAngle = true)
	{
		GameObject targetBelow_ = _Pathfinding.GetTileBelow(target.transform.position);

		List<GameObject> Path_ = _Pathfinding.GetPath(watcher, targetBelow_);

		if ( Path_ == null || Path_.Count == 0 || (mentionAngle && watcher.GetDot(target) < watcher.SHOOT_ANGLE_DOT) )
		{
			return false;
		}

		return Path_[0] == targetBelow_;
	}

	private Trooper GetNearestEnemyTrooper(Trooper watcher)
	{
		List<Trooper> targetList_ = _AllyList;

		Trooper closestTrooper_ = null;
		float closestDelta_ = float.MaxValue;

		for ( int i = 0; i < targetList_.Count; ++i )
		{
			float delta_ = (targetList_[i].transform.position - watcher.transform.position).magnitude;
			if( delta_ < closestDelta_ )
			{
				closestTrooper_ = targetList_[i];
				closestDelta_ = delta_;
			}
		}

		return closestTrooper_;
	}

	public GameObject GetMoveTargetForEnemy(Trooper searcher)
	{
		GameObject res = null;

		Trooper nearestTrooper = GetNearestEnemyTrooper (searcher);
		if( nearestTrooper != null )
		{
			List<GameObject> pathToEnemy = _Pathfinding.GetPath(searcher, _Pathfinding.GetTileBelow(nearestTrooper.transform.position));
			if( pathToEnemy.Count > 1 )
				res = pathToEnemy[pathToEnemy.Count-2];
			else if( pathToEnemy.Count == 1 )
				res = pathToEnemy[0];
		}

		return res;
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

			if ( HasVisibility(watcher, targetList_[i].gameObject) )
			{
				closestTrooper_ = targetList_[i];
				closestDelta_ = delta_;
			}
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

	public bool IsAnyTrooperOnHex(GameObject hex)
	{
		foreach( Trooper t in _AllyList )
		{
			if( _Pathfinding.GetTileBelow(t.transform.position) == hex )
				return true;
		}
		foreach( Trooper t in _EnemyList )
		{
			if( _Pathfinding.GetTileBelow(t.transform.position) == hex )
				return true;
		}

		return false;
	}

	public bool SpawnerVisibleByTroopers(GameObject spawner)
	{
		foreach( Trooper t in _AllyList )
		{
			if( HasVisibility(t, spawner, false) )
				return true;
		}

		return false;
	}
}
