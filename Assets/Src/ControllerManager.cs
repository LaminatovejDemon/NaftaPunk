using UnityEngine;
using System.Collections;

public class ControllerManager : MonoBehaviour 
{
	private static ControllerManager _Instance = null;

	public static ControllerManager GetInstance()
	{
		return _Instance;
	}

	public void Start()
	{
		_Instance = this;
	}

	public void OnTouchDown(GameObject source)
	{
	}

	public void OnTouchUp(GameObject source)
	{
		Trooper selected_ = source.GetComponent<Trooper>();
		
		if ( selected_ != null )
		{
			SquadManager.GetInstance().SelectTrooper(selected_);
		}
		else
		{
			SquadManager.GetInstance().OrderTrooper(source);
		}
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(_TouchRay.origin, _TouchRay.origin + _TouchRay.direction * 20.0f);
	}

	Ray _TouchRay;

	GameObject GetCollider(Vector2 position)
	{
		Vector3 pos1_ = new Vector3(position.x, position.y, 1);
		Vector3 pos2_ = new Vector3(position.x, position.y, 2);
		Vector3 origin_ = Camera.main.ScreenToWorldPoint(pos1_);
		Vector3 direction_ = (Camera.main.ScreenToWorldPoint(pos2_) - origin_).normalized;

		RaycastHit rayHit_;
		_TouchRay = new Ray(origin_, direction_);
		
		if ( Physics.Raycast(_TouchRay, out rayHit_, 50) )
		{
			return rayHit_.collider.gameObject;
		}

		return null;
	}

	public void OnTouchDown(Vector2 position)
	{
		GameObject touchObjct_ = GetCollider(position);
		if ( touchObjct_ != null )
		{
			OnTouchDown(touchObjct_);
		}
	}

	public void OnTouchUp(Vector2 position)
	{
		GameObject touchObjct_ = GetCollider(position);
		if ( touchObjct_ != null )
		{
			OnTouchUp(touchObjct_);
		}
	}

	void Update () 
	{
		//if (Input.GetTouch

#if UNITY_STANDALONE || UNITY_STANDALONE_OSX
		if ( Input.GetMouseButtonDown(0) )
		{
			OnTouchDown(Input.mousePosition);
		}
		if ( Input.GetMouseButtonUp(0) )
		{
			OnTouchUp(Input.mousePosition);
		}
		
#endif
	}
}
