﻿using UnityEngine;
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

	public void OnTouchUpWorld(Vector3 zeroPosition)
	{
		SquadManager.GetInstance().OrderTrooper(zeroPosition);
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
		Gizmos.color = Color.red;
		Gizmos.DrawLine(_UITouchRay.origin, _UITouchRay.origin + _UITouchRay.direction * 20.0f);
	}

	Ray _UITouchRay;

	GameObject GetUICollider(Vector2 position)
	{
		Vector3 origin_ =  UIManager.GetInstance().camera.ScreenToWorldPoint(position);
		Vector3 direction_ = UIManager.GetInstance().camera.transform.forward;
		
		RaycastHit rayHit_;
		_UITouchRay = new Ray(origin_ - direction_ * 10.0f, direction_);
		
		if ( Physics.Raycast(_UITouchRay, out rayHit_, 50, 1<<LayerMask.NameToLayer("UI")) )
		{
			return rayHit_.collider.gameObject;
		}
		
		return null;
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
		
		if ( Physics.Raycast(_TouchRay, out rayHit_, 50, 1<<LayerMask.NameToLayer("Floor") | 1<<LayerMask.NameToLayer("Default")) )
		{
			return rayHit_.collider.gameObject;
		}

		return null;
	}

	Button _TouchDown = null;

	public void OnTouchDown(Vector2 position)
	{
		GameObject touchObjct_ = GetUICollider(position);
		Debug.Log ("We touched object" + touchObjct_);
		if ( touchObjct_ != null && touchObjct_.GetComponent<Button>() != null )
		{
			_TouchDown = touchObjct_.GetComponent<Button>();
			_TouchDown.OnTouchDown();

			return;
		}

		touchObjct_ = GetCollider(position);
		if ( touchObjct_ != null )
		{
			OnTouchDown(touchObjct_);
		}
	}

	public void OnTouchUp(Vector2 position)
	{
		GameObject touchObjct_ = GetUICollider(position);
		if ( _TouchDown != null )
		{
			if ( touchObjct_ != null && touchObjct_.GetComponent<Button>() != null && _TouchDown == touchObjct_.GetComponent<Button>() )
			{
				touchObjct_.GetComponent<Button>().OnTouchUp();
			}
			else
			{
				_TouchDown.OnTouchCancel();
			}

			_TouchDown = null;
			return;
		}

		touchObjct_ = GetCollider(position);
		if ( touchObjct_ != null )
		{
			OnTouchUp(touchObjct_);
		}
		else
		{
			OnTouchUpWorld(GetZeroPosition(position));
		}
	}

	Vector3 GetZeroPosition(Vector2 position)
	{
		Vector3 pos1_ = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 1));
		Vector3 pos2_ = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 2));

		Vector3 delta_ = (pos1_ - pos2_).normalized;
		delta_ *= pos1_.y/delta_.y;

		pos1_ -= delta_;

		return pos1_;
	}

	void Update () 
	{
	

#if UNITY_STANDALONE || UNITY_STANDALONE_OSX || UNITY_EDITOR
		if ( Input.GetMouseButtonDown(0) )
		{
			OnTouchDown(Input.mousePosition);
		}
		if ( Input.GetMouseButtonUp(0) )
		{
			OnTouchUp(Input.mousePosition);
		}
		
#else
		for ( int i = 0; i < Input.touchCount; ++i )
		{
			switch ( Input.GetTouch(i).phase )
			{
			case TouchPhase.Began:
				OnTouchDown(Input.GetTouch(i).position);
				break;
			case TouchPhase.Stationary:
			case TouchPhase.Moved:
				break;
			case TouchPhase.Ended:
			case TouchPhase.Canceled:
				OnTouchUp(Input.GetTouch(i).position);
				break;
			}
		}
#endif
	}
}
