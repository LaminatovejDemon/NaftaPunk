using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trooper : MonoBehaviour {

	public const float WALKING_TOLERANCE = 0.01f;

	public float WALKING_SPEED = 1.0f;
	public float HEALTH = 10.0f;
	public float ATTACK = 1.0f;

	public float WATCH_DELTA_TIME = 0.5f;
	public float SHOOT_ANGLE_DOT = 0.8f;
	public string NAME = "Abdul";

	public Transform _GunfireParticle;

	public int _SkillSpeed = 1;
	int _SkillSpeedLocal = -1;

	public int _SkillHealth = 1;
	int _SkillHealthLocal = -1;

	public int _SkillAttack = 1;
	int _SkillAttackLocal = -1;

	public void SkillUpAttack()
	{
		++_SkillAttack;
	}

	public void SkillUpSpeed()
	{
		++_SkillHealth;
	}

	public void SkillUpHealth()
	{
		++_SkillSpeed;
	}

	public Renderer _SkinRenderer;

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
	private HexData m_Spawner;

	private bool m_CarriesGrail = false;
	
	float GetTargetAngle(GameObject target)
	{
		Vector3 targetPos_ = target.transform.position;
		targetPos_.y = transform.position.y;
		float ret_ = Quaternion.LookRotation(transform.position - target.transform.position).eulerAngles.y;                                   

		ret_ = (int)((ret_+30.0f)/60.0f) * 60;

		return ret_;
	}

	float GetTargetAngle(Vector3 zeroPoint)
	{
		Vector3 targetPos_ = zeroPoint;
		targetPos_.y = transform.position.y;
		float ret_ = Quaternion.LookRotation(transform.position - targetPos_).eulerAngles.y;                                   



		ret_ = (int)((ret_+30.0f)/60.0f) * 60 % 360;

		return ret_;
	}

	public Vector3 _TargetPosition {get; private set;}

	public HexData GetSpawner()
	{
		return m_Spawner;
	}

	public void SetSpawner(HexData spawner)
	{
		m_Spawner = spawner;
	}

	public void SetDirection(Vector3 zeroPoint)
	{
		_TargetAngle = GetTargetAngle(zeroPoint);
	}
	
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
			// zastavi postavu
//			Stop ();
			return;
		}

		if ( _Fraction == Fraction.F_Ally )
		{
			SquadManager.GetInstance().TargetHighlight(path[path.Count-1].transform.position);
		}

		_WalkList = path;
		_Walking = false;
	}

	public void Stop()
	{
		_WalkList = new List<GameObject>();
		_WalkList.Add(SquadManager.GetInstance()._Pathfinding.GetTileBelow(transform.position));
		_Walking = false;
	}

	void Start()
	{
		m_EnemyAI = GetComponent<EnemyAI> ();
	}

	void Update()
	{
		UpdateSetting();
		UpdateRotation();
		UpdatePosition();

		UpdateStats();

		if ( Time.time < _NextWatchTimestamp )
		{
			return;
		}		
		_NextWatchTimestamp = Time.time + WATCH_DELTA_TIME;

		if( _Fraction == Fraction.F_Enemy )
		{
			m_EnemyAI.UpdateOnDemand();
		}
		else
		{
			Attack( Watch() );
		}
	}

	void UpdateStats()
	{
		if ( _SkillAttack != _SkillAttackLocal )
		{
			_SkillAttackLocal = Mathf.Clamp(_SkillAttack, 0, 10);
			_SkillAttack = _SkillAttackLocal;
			UIManager.GetInstance().SetStats(this);
		}

		if ( _SkillHealth != _SkillHealthLocal )
		{
			_SkillHealthLocal = Mathf.Clamp(_SkillHealth, 0, 10);
			_SkillHealth = _SkillHealthLocal;
			UIManager.GetInstance().SetStats(this);
		}

		if ( _SkillSpeed != _SkillSpeedLocal )
		{
			_SkillSpeedLocal = Mathf.Clamp(_SkillSpeed, 0, 10);
			_SkillSpeed = _SkillSpeedLocal;
			UIManager.GetInstance().SetStats(this);
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

		if( _FractionLocal == Fraction.F_Ally )
		{
			HexData startPos = Level.GetInstance().GetFreeStartPos();
			if( startPos )
			{
				transform.position = startPos.transform.position;
				startPos.OccupyStartPos(true);
			}
			_TargetPosition = transform.position;
			m_CarriesGrail = false;
		}
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

			if ( _WalkList[0] == null )
			{
				_WalkList = null;
				Debug.Log ("ERROR !!!!");
				return;
			}

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

		Vector3 walkDir = (_TargetPosition - transform.position).normalized;
		GameObject hexInFront = SquadManager.GetInstance ()._Pathfinding.GetTileBelow (transform.position + walkDir * Utils.c_HexRadius * Mathf.Sqrt (3));
		if( SquadManager.GetInstance().IsAnyTrooperOnHex(hexInFront) )
		{
			Stop ();
			PullWalkPoint();
		}

		transform.position = Utils.Slerp(transform.position, _TargetPosition, (float)_SkillSpeed * WALKING_SPEED);

		if ( (transform.position - _TargetPosition).magnitude < WALKING_TOLERANCE )
		{
			_Walking = false;
			PullWalkPoint();
		}
	}

	public Trooper Watch()
	{
		return SquadManager.GetInstance ().GetClosestVisibleTrooper(this, _Fraction == Fraction.F_Ally ? Fraction.F_Enemy : Fraction.F_Ally);
	}

	public void Attack(Trooper target)
	{
		_AttackHandler.Attack( target );
	}

	void UpdateRotation()
	{
		if ( _ActualAngle == _TargetAngle )
		{
			return;
		}
		_ActualAngle = _TargetAngle;
		_AttackHandler.SetTarget(null);

		Texture tex_ = SquadManager.GetInstance().GetComponent<Atlas>().GetTexture((int)_ActualAngle);

		if ( tex_ != null )
		{
			_SkinRenderer.material.mainTexture = tex_;
		}
		
	  	_Body.transform.eulerAngles = new Vector3(0, _ActualAngle, 0);
	}

	public void CarryGrail()
	{
		m_CarriesGrail = true;
	}

	public void DropGrail()
	{
		m_CarriesGrail = true;
	}

	public bool IsCarryingGrail()
	{
		return m_CarriesGrail;
	}

}
