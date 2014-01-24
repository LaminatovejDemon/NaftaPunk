using UnityEngine;
using System.Collections;

public class Pathfinding : MonoBehaviour 
{
	public GameObject GetPath(Trooper walker, GameObject target)
	{
		float distance_ = (target.transform.position - walker.transform.position).magnitude;
		GameObject ret_ = null;
		GameObject temp_ = null;
		Vector3 checkPosition_;

		for ( int i = 0; i < (int) distance_; ++i )
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

	Vector3 GetPositionBetween(Trooper walker, Vector3 targetPosition, int index)
	{
		return Vector3.zero;
	}

	GameObject GetTileBelow(Vector3 position)
	{
		return null;
	}
}
