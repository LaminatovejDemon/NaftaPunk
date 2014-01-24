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

	public void Walk(GameObject target)
	{
		if ( target == null )
		{
			return;
		}

		target.renderer.material = SquadManager.GetInstance().TrooperEnemyMaterial;
		Vector3 targetPos_ = target.transform.position;
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

	void UpdatePosition()
	{
		if ( (transform.position - _TargetPosition).magnitude < WALKING_TOLERANCE )
		{
			return;
		}

		transform.position = Utils.Slerp(transform.position, _TargetPosition, WALKING_SPEED);
	}

	void UpdateRotation()
	{
		_ActualAngle = _TargetAngle;
		
	  	_Body.transform.eulerAngles = new Vector3(0, _ActualAngle, 0);
	}

}
