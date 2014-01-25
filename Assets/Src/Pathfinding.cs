//#define DEBUG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour 
{
	enum ENodeState
	{
		FRESH,
		OPEN,
		CLOSED
	}

	struct TNode
	{
		public GameObject PredTile;
		public ENodeState State;

		public TNode(GameObject pred)
		{
			PredTile = pred;
			State = ENodeState.FRESH;
		}
	}

	float m_DistanceOneTile;

#if DEBUG
	List<Ray> _PathfindingRayList = new List<Ray>();
#endif

	void Awake()
	{
		m_DistanceOneTile = Utils.c_HexRadius * Mathf.Sqrt (3);
	}

	public List<GameObject> GetPath(Trooper walker, GameObject target)
	{

#if DEBUG
		_PathfindingRayList.Clear();
#endif
		bool pathFound = false;

		Dictionary<GameObject, GameObject> searchSpace = new Dictionary<GameObject, GameObject> ();
		Queue<GameObject>                  searchQueue = new Queue<GameObject> ();
		List<GameObject>                   path        = new List<GameObject> ();

		GameObject startTile = GetTileBelow (walker.transform.position);
		searchSpace.Add(startTile, null);
		searchQueue.Enqueue(startTile);

		while( searchQueue.Count > 0 )
		{
			GameObject tile = searchQueue.Dequeue();

			for( int i = 0; i < 6; ++i )
			{
				float alpha = (30f + i*60f)*(Mathf.PI/180f);
				float x = m_DistanceOneTile * Mathf.Cos(alpha);
				float z = m_DistanceOneTile * Mathf.Sin(alpha);

				Vector3 checkPosition = tile.transform.position + new Vector3(x, 0, z);
				GameObject t = GetTileBelow(checkPosition);
				if( t == null )
					continue;

				if( searchSpace.ContainsKey(t) )
					continue;

				searchSpace.Add(t, tile);
				searchQueue.Enqueue(t);

				if( t == target )
				{
					pathFound = true;
					searchQueue.Clear();
					break;
				}
			}
		}

		if( pathFound )
		{
			Vector3 lastDir = Vector3.zero;
			GameObject tBack = target;

			path.Add(tBack);
			while( tBack != startTile )
			{
				GameObject t = searchSpace[tBack];
				Vector3 dir = (t.transform.position - tBack.transform.position).normalized;

				if( lastDir == Vector3.zero )
					lastDir = dir;
				if( Vector3.Dot(lastDir, dir) < 0.9962 )
				{
					path.Add(tBack);
				}
				lastDir = dir;
				tBack = t;
			}
		}
		path.Reverse ();
		return path;

/*		float distanceOneTile = Utils.c_HexRadius * Mathf.Sqrt (3);
		int distance_ = (int) Mathf.Ceil( (target.transform.position - walker.transform.position).magnitude / distanceOneTile);
		GameObject ret_ = null;
		GameObject temp_ = null;
		Vector3 checkPosition_;


		for ( int i = 0; i < (int) distance_+1; ++i )
		{
			checkPosition_ = GetPositionBetween(walker, target.transform.position, i);

			temp_ = GetTileBelow(checkPosition_);

			if ( temp_ == null )
			{
				return ret_;
			}

			ret_ = temp_;
		}

		return temp_;*/
	}

#if DEBUG
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		for ( int i = 0; i < _PathfindingRayList.Count; ++i )
		{
			Gizmos.DrawLine(_PathfindingRayList[i].origin, _PathfindingRayList[i].origin + _PathfindingRayList[i].direction * 3.0f);
		}
	}
#endif

	Vector3 GetPositionBetween(Trooper walker, Vector3 targetPosition, int index)
	{
		return walker.transform.position + walker._Body.transform.rotation * Vector3.back * index * Utils.c_HexRadius * Mathf.Sqrt (3);
	}

	public GameObject GetTileBelow(Vector3 position)
	{
		Ray ray_ = new Ray(position + Vector3.up * 2.0f, Vector3.down);
#if DEBUG
		_PathfindingRayList.Add(ray_);
#endif
		
		RaycastHit rayHit_;
		
		if ( Physics.Raycast(ray_, out rayHit_, 3, 1<<LayerMask.NameToLayer("Floor")) )
		{
			return rayHit_.collider.gameObject;
		}
		
		return null;
	}
}
