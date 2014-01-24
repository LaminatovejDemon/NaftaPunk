//#define DEBUG

using UnityEngine;
using System.Collections;
#if DEBUG
using System.Collections.Generic;
#endif

public class Pathfinding : MonoBehaviour 
{

#if DEBUG
	List<Ray> _PathfindingRayList = new List<Ray>();
#endif

	public GameObject GetPath(Trooper walker, GameObject target)
	{

#if DEBUG
		_PathfindingRayList.Clear();
#endif

		float distance_ = (target.transform.position - walker.transform.position).magnitude;
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

		return temp_;
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
		return walker.transform.position + walker._Body.transform.rotation * Vector3.back * index;
	}

	GameObject GetTileBelow(Vector3 position)
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
