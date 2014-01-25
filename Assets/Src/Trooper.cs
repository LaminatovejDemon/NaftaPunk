using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trooper : MonoBehaviour {

	float WALKING_TOLERANCE = 0.01f;
	public float WALKING_SPEED = 1.0f;
	public float WATCH_DELTA_TIME = 0.5f;
	public float SHOOT_ANGLE_DOT = 0.8f;

	public enum Fraction
	{
		F_Enemy,
		F_Ally,
		F_Invalid,
	};

	public GameObject _HighlightCircle;
	public GameObject _Body;
	public AttackHandler _AttackHandler;
	public HealthBar _HealthBar;

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

	private EnemyAI m_EnemyAI;
	
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

	public float GetDot(GameObject target)
	{
		float dot_ = Vector3.Dot(_Body.transform.rotation * Vector3.forward, (transform.position - target.transform.position).normalized);
		return dot_;
	}

	List<GameObject> _WalkList;

	public void Walk(List<GameObject> path)
	{
		if ( path == null || path.Count == 0 )
		{
			return;
		}

		_WalkList = path;
		_Walking = false;
	}

	void Start()
	{
		m_EnemyAI = GetComponent<EnemyAI> ();
		_TargetPosition = transform.position;
	}

	void Update()
	{
		UpdateSetting();
		UpdateRotation();
		UpdatePosition();

		if( _Fraction == Fraction.F_Enemy )
		{
			m_EnemyAI.UpdateOnDemand();
		}
		else
		{
			Watch();
		}
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
		collider.enabled = _FractionLocal == Fraction.F_Ally;

		SquadManager.GetInstance().RegisterTrooper(this, _FractionLocal);
		_Body.renderer.material = _FractionLocal == Fraction.F_Enemy ? SquadManager.GetInstance().TrooperEnemyMaterial : SquadManager.GetInstance().TrooperAllyMaterial;
	}

	float _NextWatchTimestamp = -1;
	bool _Walking = false;

	void PullWalkPoint()
	{
		if ( _WalkList != null && _WalkList.Count > 0 && !_Walking)
		{
			_Walking = true;

			Vector3 targetPos_ = _WalkList[0].transform.position;
			targetPos_.y = transform.position.y;
			_TargetPosition = targetPos_;
			_WalkList.RemoveAt(0);
			SquadManager.GetInstance().SetCenterPoint();

			if ( _WalkList.Count == 0 )
			{
				_WalkList = null;
			}
		}
	}

	void UpdatePosition()
	{
		PullWalkPoint();

		transform.position = Utils.Slerp(transform.position, _TargetPosition, WALKING_SPEED);

		if ( (transform.position - _TargetPosition).magnitude < WALKING_TOLERANCE )
		{
			_Walking = false;
			PullWalkPoint();
		}
	}

	void Watch()
	{
		if ( Time.time < _NextWatchTimestamp )
		{
			return;
		}

		_NextWatchTimestamp = Time.time + WATCH_DELTA_TIME;

		_AttackHandler.Attack(SquadManager.GetInstance().GetClosestVisibleTrooper(this, _Fraction == Fraction.F_Ally ? Fraction.F_Enemy : Fraction.F_Ally));
	}

	void UpdateRotation()
	{
		if ( _ActualAngle == _TargetAngle )
		{
			return;
		}
		_ActualAngle = _TargetAngle;
		
	  	_Body.transform.eulerAngles = new Vector3(0, _ActualAngle, 0);
	}

}
