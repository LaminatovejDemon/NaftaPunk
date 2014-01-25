using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trooper : MonoBehaviour {

	float WALKING_TOLERANCE = 0.01f;
	public float WALKING_SPEED = 1.0f;
	public float WATCH_DELTA_TIME = 0.5f;

	public enum Fraction
	{
		F_Enemy,
		F_Ally,
		F_Invalid,
	};

	public GameObject _HighlightCircle;
	public GameObject _Body;

	public bool _Selected = false;

	public void OnSelect(bool state)
	{
		collider.enabled = !state;
		_Selected = state;
		_HighlightCircle.SetActive(state);
	}

	float _TargetAngle = 0;
	float _ActualAngle = 0;

	public Fraction _Fraction = Fraction.F_Ally;
	Fraction _FractionLocal = Fraction.F_Invalid;
	
	float GetTargetAngle(GameObject target)
	{
		Vector3 targetPos_ = target.transform.position;
		targetPos_.y = transform.position.y;
		float ret_ = Quaternion.LookRotation(transform.position - target.transform.position).eulerAngles.y;                                   

		ret_ = (int)((ret_+30.0f)/60.0f) * 60;

		return ret_;
	}

	public Vector3 _TargetPosition {get; private set;}

	public void SetDirection(GameObject target)
	{
		_TargetAngle = GetTargetAngle(target);
	}

	public bool HasDirection(GameObject target)
	{
		return _TargetAngle == GetTargetAngle(target);
	}

	public void Walk(List<GameObject> path)
	{
		if ( path == null || path.Count == 0 )
		{
			return;
		}

		GameObject waypoint = path [0];
		waypoint.renderer.material = SquadManager.GetInstance().TrooperEnemyMaterial;
		Vector3 targetPos_ = waypoint.transform.position;
		targetPos_.y = transform.position.y;
		_TargetPosition = targetPos_;

		SquadManager.GetInstance().SetCenterPoint();
	}

	void Start()
	{
		_TargetPosition = transform.position;
	}

	void Update()
	{
		UpdateSetting();
		UpdateRotation();
		UpdatePosition();
	}

	void UpdateSetting()
	{
		SetFraction();
	}

	void SetFraction()
	{
		if ( _FractionLocal == _Fraction )
		{
			return;
		}

		_FractionLocal = _Fraction;
		SquadManager.GetInstance().RegisterTrooper(this, _FractionLocal);
		_Body.renderer.material = _FractionLocal == Fraction.F_Enemy ? SquadManager.GetInstance().TrooperEnemyMaterial : SquadManager.GetInstance().TrooperAllyMaterial;
	}

	float _NextWatchTimestamp = -1;

	void UpdatePosition()
	{
		if ( (transform.position - _TargetPosition).magnitude < WALKING_TOLERANCE )
		{
			return;
		}
	
		Watch();

		transform.position = Utils.Slerp(transform.position, _TargetPosition, WALKING_SPEED);
	}

	void Watch()
	{
		if ( Time.time < _NextWatchTimestamp )
		{
			return;
		}

		_NextWatchTimestamp = Time.time + WATCH_DELTA_TIME;

		Attack(SquadManager.GetInstance().GetClosestVisibleTrooper(this, _Fraction == Fraction.F_Ally ? Fraction.F_Enemy : Fraction.F_Ally));
	}

	void Attack(Trooper target)
	{
		if ( target == null )
		{
			return;
		}

		target._Body.renderer.material.color = Color.red;
		target.Watch();
	}

	void UpdateRotation()
	{
		if ( _ActualAngle == _TargetAngle )
		{
			return;
		}
		_ActualAngle = _TargetAngle;
		
	  	_Body.transform.eulerAngles = new Vector3(0, _ActualAngle, 0);

		Watch();
	}

}
