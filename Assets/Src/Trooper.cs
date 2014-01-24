using UnityEngine;
using System.Collections;

public class Trooper : MonoBehaviour {

	float WALKING_TOLERANCE = 0.01f;
	public float WALKING_SPEED = 1.0f;

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
		return Quaternion.LookRotation(transform.position - target.transform.position).eulerAngles.y;                                   
	}

	Vector3 _TargetPosition;

	public void SetDirection(GameObject target)
	{
		_TargetAngle = GetTargetAngle(target);
	}

	public bool HasDirection(GameObject target)
	{
		return _TargetAngle == GetTargetAngle(target);
	}

	public void Walk(GameObject target)
	{
		_TargetPosition = target.transform.localPosition;
		_TargetPosition.y = transform.position.y;
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

	void UpdatePosition()
	{
		if ( (transform.position - _TargetPosition).magnitude < WALKING_TOLERANCE )
		{
			return;
		}

		Vector3 delta_ = (_TargetPosition - transform.position).normalized * WALKING_SPEED * Time.deltaTime;

		Debug.Log ("dot: " + Vector3.Dot(_TargetPosition - transform.position, _TargetPosition - transform.position - delta_));

		if ( Vector3.Dot(_TargetPosition - transform.position, _TargetPosition - transform.position + delta_) <= 0 )
		{
			transform.position = _TargetPosition;
		}
		else
		{
			transform.position += delta_;
		}
	}

	void UpdateRotation()
	{
		_ActualAngle = _TargetAngle;
		
		transform.eulerAngles = new Vector3(0, _ActualAngle, 0);
	}

}
