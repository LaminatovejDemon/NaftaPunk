using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour 
{
	public float MOVEMENT_TOLERANCE = 0.01f;
	public float CAMERA_SPEED = 5.0f;

	public static CameraManager _Instance;

	public static CameraManager GetInstance()
	{
		return _Instance;
	}

	Vector3 _TargetPosition;

	void Start()
	{
		_Instance = this;
	}

	public void SetPosition(Vector3 position)
	{
		_TargetPosition = position;
	}

	void Update()
	{
		if ( (transform.position - _TargetPosition).magnitude < MOVEMENT_TOLERANCE )
		{
			return;
		}
		
		transform.position = Utils.Slerp(transform.position, _TargetPosition, CAMERA_SPEED);
	}

}
