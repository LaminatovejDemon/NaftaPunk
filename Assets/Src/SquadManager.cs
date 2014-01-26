using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquadManager : MonoBehaviour 
{

	public GameObject EnemyTemplate;

	public Material TrooperAllyMaterial;
	public Material TrooperEnemyMaterial;
	public Material TrooperHitMaterial;

	public static float ENEMY_SPAWN_DELAY_MIN = 5.0f;
	public static float ENEMY_SPAWN_DELAY_MAX = 10.0f;
	public static float DELAY_BEFORE_ATTACK_MULTIPLIER = 1.0f;

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
	}

	public static SquadManager GetInstance()
	{
		return _Instance;
	}

	public void Awake()
	{
		_Instance = this;
	}
	
	Trooper _SelectedTrooper;

	public void OrderTrooper(Vector3 zeroPosition)
	{
		if ( _SelectedTrooper != null )
		{
			_SelectedTrooper.SetDirection(zeroPosition);
		}
	}

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
				List<GameObject> path = _Pathfinding.GetPath(_SelectedTrooper, direction, false);
				if( path.Count == 0 )
					path = _Pathfinding.GetPath(_SelectedTrooper, direction, true);
				_SelectedTrooper.Walk(path);
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
				enemy.Walk(_Pathfinding.GetPath(enemy, direction, true));
			}
		}
	}
	
	public void OnKilled(Trooper target, bool reset = false)
	{
		if( target.GetSpawner() != null )
		{
			target.GetSpawner().ResetSpawner();
		}

		_AllyList.Remove(target);
		_EnemyList.Remove(target);

		if( target._Side == Trooper.Side.F_Ally )
		{
			target.DropGrail();
			target.InvalidateSide();
			target.gameObject.SetActive(false);
			if( !reset )
				Level.GetInstance().AddKilledTrooper();
		}

		UIManager.GetInstance().RegisterTrooper(target, false);

		if( target._Side == Trooper.Side.F_Enemy )
			GameObject.Destroy(target.gameObject);
	}

	public void RegisterTrooper(Trooper target, Trooper.Side side)
	{
		List<Trooper> targetList_ = side == Trooper.Side.F_Enemy ? _EnemyList : _AllyList;
		List<Trooper> excludedList_ = side == Trooper.Side.F_Enemy ? _AllyList : _EnemyList;

		if ( excludedList_.Contains(target) )
		{
			excludedList_.Remove(target);
		}

		if ( !targetList_.Contains(target) )
		{
			targetList_.Add(target);

		}

		UIManager.GetInstance().RegisterTrooper(target, side == Trooper.Side.F_Ally);
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
			List<GameObject> pathToEnemy = _Pathfinding.GetPath(searcher, _Pathfinding.GetTileBelow(nearestTrooper.transform.position), true);
			if( pathToEnemy.Count > 1 )
				res = pathToEnemy[pathToEnemy.Count-2];
			else if( pathToEnemy.Count == 1 )
				res = pathToEnemy[0];
		}

		return res;
	}
	
	public Trooper GetClosestVisibleTrooper(Trooper watcher, Trooper.Side side)
	{
		List<Trooper> targetList_ = side == Trooper.Side.F_Ally ? _AllyList : _EnemyList;

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

		mx_ = Mx_= _AllyList[0].transform.position.x;
		mz_ = Mz_= _AllyList[0].transform.position.z;

		Vector3 tempPos_;

		for ( int i = 0; i < _AllyList.Count; ++i )
		{
			tempPos_ = _AllyList[i].transform.position;
			if ( tempPos_.x < mx_ ) mx_ = tempPos_.x;
			if ( tempPos_.x > Mx_ ) Mx_ = tempPos_.x;
			if ( tempPos_.z < mz_ ) mz_ = tempPos_.z;
			if ( tempPos_.z > Mz_ ) Mz_ = tempPos_.z;
		}

		CameraManager.GetInstance().SetPosition(new Vector3((Mx_ - mx_) * 0.5f + mx_, 0, (Mz_ - mz_) * 0.5f + mz_));
	}

	public bool IsAllyTrooperOnHex(GameObject hex)
	{
		foreach( Trooper t in _AllyList )
		{
			if( _Pathfinding.GetTileBelow(t.transform.position) == hex )
				return true;
		}

		return false;
	}

	public bool IsEnemyTrooperOnHex(GameObject hex)
	{
		foreach( Trooper t in _EnemyList )
		{
			if( _Pathfinding.GetTileBelow(t.transform.position) == hex )
				return true;
		}
		
		return false;
	}

	public bool IsAnyTrooperOnHex(GameObject hex)
	{
		return IsAllyTrooperOnHex(hex) || IsEnemyTrooperOnHex(hex);
	}

	public bool AllAlliesOnTeleport()
	{
		foreach( Trooper t in _AllyList )
		{
			GameObject tile = _Pathfinding.GetTileBelow(t.transform.position);
			if( tile )
			{
				if ( (t.transform.position - tile.transform.position).magnitude > Trooper.WALKING_TOLERANCE )
					return false;
				if( !tile.GetComponent<HexData>().m_Teleport )
					return false;
			}
		}
		
		return true;
	}

	public bool SpawnerVisibleByTroopers(GameObject spawner)
	{
		foreach( Trooper t in _AllyList )
		{
			if( _Pathfinding.GetTileBelow(t.transform.position) == spawner )
				return true;
			if( HasVisibility(t, spawner, false) )
				return true;
		}

		return false;
	}

	public Trooper GetAllyTrooperOnHex(GameObject hex)
	{
		foreach( Trooper t in _AllyList )
		{
			if( _Pathfinding.GetTileBelow(t.transform.position) == hex )
				return t;
		}
		
		return null;
	}

	public void KillAllEnemies()
	{
		for( int i = _EnemyList.Count -1; i >= 0; --i )
		{
			OnKilled(_EnemyList[i]);
		}
	}

	public void DropGrailFromAnyone()
	{
		foreach( Trooper t in _AllyList )
		{
			t.DropGrail();
		}
	}

	public void LateUpdate()
	{
		SquadManager.GetInstance().SetCenterPoint();
	}

	// pouze na test
	public void KillGrailCarrier()
	{
		foreach( Trooper t in _AllyList )
		{
			if( t.IsCarryingGrail() )
			{
				OnKilled(t);
				return;
			}
		}
	}
}
