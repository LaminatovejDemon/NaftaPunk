using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour{

	public const float c_HexRadius = 0.5f;

	public static Vector3 Slerp(Vector3 actualPosition, Vector3 targetPosition, float speed)
	{
		Vector3 delta_ = (targetPosition - actualPosition).normalized * speed * Time.deltaTime;
		
		float dot_ = Vector3.Dot(actualPosition - targetPosition, actualPosition - targetPosition + delta_);

		if ( dot_ < 0 || actualPosition == targetPosition )
		{
			actualPosition = targetPosition;
		}
		else
		{
			actualPosition += delta_;
		}

		return actualPosition;
	}
}
