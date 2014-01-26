using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trooper : MonoBehaviour {

	public const float WALKING_TOLERANCE = 0.01f;

	public float DELTA_DISTANCE_PER_LEG_FRAME = 1.0f;
	public float WALKING_SPEED = 1.0f;
	public float HEALTH = 10.0f;
	public float ATTACK = 1.0f;

	public float WATCH_DELTA_TIME = 0.5f;
	public float SHOOT_ANGLE_DOT = 0.8f;
	public string NAME = "Abdul";
	public GameStateManager.EFractionType _Fraction = GameStateManager.EFractionType.Gyms;

	public Transform _GunfireParticle;

	public int _SkillSpeed = 1;
	int _SkillSpeedLocal = -1;

	public int _SkillHealth = 1;
	int _SkillHealthLocal = -1;

	public int _SkillAttack = 1;
	int _SkillAttackLocal = -1;

	public enum Angle
	{
		Angle_0,
		Angle_60,
		Angle_120,
		Angle_180,
		Angle_240,
		Angle_300,
	};

	public void SkillUpAttack()
	{
		if ( Level.GetInstance().SkillPointAmount() > 0 )
		{
			Level.GetInstance().DecreaseSkillPoints();
			++_SkillAttack;
			UIManager.GetInstance()._RewardScreen.UpdateLevelUpStatus();
		}
	}

	public void SkillUpSpeed()
	{
		if ( Level.GetInstance().SkillPointAmount() > 0 )
		{
			Level.GetInstance().DecreaseSkillPoints();
			++_SkillSpeed;
			UIManager.GetInstance()._RewardScreen.UpdateLevelUpStatus();
		}
	}

	public void SkillUpHealth()
	{
		if ( Level.GetInstance().SkillPointAmount() > 0 )
		{
			Level.GetInstance().DecreaseSkillPoints();
			++_SkillHealth;
			UIManager.GetInstance()._RewardScreen.UpdateLevelUpStatus();
		}
	}

	public Renderer _BodySkinRenderer;
	public Renderer _LegsSkinRenderer;

	public enum Side
	{
		F_Enemy,
		F_Ally,
		F_Invalid,
	};

	public GameObject _HighlightCircle;
	public GameObject _Body;
	public AttackHandler _AttackHandler;
	public HealthBar _HealthBar;
	public GameObject _Grail;

	public bool _Selected = false;

	public void OnSelect(bool state)
	{
		collider.enabled = !state;
		_Selected = state;
		_HighlightCircle.SetActive(state);
	}

	public Angle _NamedAngle { get; private set;}
	float _TargetAngle = 0;
	float _ActualAngle = -1;

	public Side _Side = Side.F_Ally;
	Side _SideLocal = Side.F_Invalid;

	private EnemyAI m_EnemyAI;
	private HexData m_Spawner;

	private bool m_CarriesGrail = false;

	private List<Texture> _BodyTextureSet;
	private List<Texture> _LegsTextureSet;

	public void SetRpgProperties(string name, int health, int speed, int attack)
	{
		Debug.Log ("Setting stats to trooper " + name + " H: " + health + ", S: " + speed + " A: " + attack);

		NAME = name;
		_SkillHealth = health;
		_SkillAttack = attack;
		_SkillSpeed = speed;
	}

	Angle GetNamedAngle(float angle)
	{
		if ( angle > 270 )
			return Angle.Angle_300;
		else if ( angle > 210 )
			return Angle.Angle_240;
		else if ( angle > 150 )
			return Angle.Angle_180;
		else if ( angle > 90 )
			return Angle.Angle_120;
		else if ( angle > 30 )
			return Angle.Angle_60;
		else
			return Angle.Angle_0;
	}
	
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

	public void InvalidateSide()
	{
		_SideLocal = Side.F_Invalid;
	}

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
		_NamedAngle = GetNamedAngle(_TargetAngle);
	}
	
	public void SetDirection(GameObject target)
	{
		_TargetAngle = GetTargetAngle(target);
		_NamedAngle = GetNamedAngle(_TargetAngle);
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

		if ( _Side == Side.F_Ally )
		{
			SquadManager.GetInstance().TargetHighlight(path[path.Count-1].transform.position);
		}

		_WalkList = path;
		_Walking = false;
	}

	public void Stop()
	{
		_WalkList = new List<GameObject>();
		GameObject tile = SquadManager.GetInstance ()._Pathfinding.GetTileBelow (transform.position);
		if( tile )
			_WalkList.Add(tile);
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

		if( _Side == Side.F_Enemy )
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
			_SkillAttackLocal = Mathf.Clamp(_SkillAttack, 1, 10);
			_SkillAttack = _SkillAttackLocal;
			UIManager.GetInstance().SetStats(this);
		}

		if ( _SkillHealth != _SkillHealthLocal )
		{
			_SkillHealthLocal = Mathf.Clamp(_SkillHealth, 1, 10);
			_SkillHealth = _SkillHealthLocal;
			UIManager.GetInstance().SetStats(this);
		}

		if ( _SkillSpeed != _SkillSpeedLocal )
		{
			_SkillSpeedLocal = Mathf.Clamp(_SkillSpeed, 1, 10);
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
		if ( _SideLocal == _Side )
		{
			return;
		}

		_SideLocal = _Side;
		collider.enabled = _SideLocal == Side.F_Ally;

		if( _SideLocal == Side.F_Ally )
		{
			Level.GetInstance().InitTrooper(this);
			m_CarriesGrail = false;
		}
		_TargetPosition = transform.position;
		SquadManager.GetInstance().RegisterTrooper(this, _SideLocal);
		_Body.renderer.material = _SideLocal == Side.F_Enemy ? SquadManager.GetInstance().TrooperEnemyMaterial : SquadManager.GetInstance().TrooperAllyMaterial;
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

	Vector3 _LastLegUpdatePosition = Vector3.zero;

	void UpdatePosition()
	{
		TrySetLegUpdatePosition();

		PullWalkPoint();

		if ( _TargetPosition == transform.position )
		{
			return;
		}

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
		return SquadManager.GetInstance ().GetClosestVisibleTrooper(this, _Side == Side.F_Ally ? Side.F_Enemy : Side.F_Ally);
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

		_BodyTextureSet = SquadManager.GetInstance().GetComponent<Atlas>().GetTexture((int)_ActualAngle, _Fraction);
		_LegsTextureSet = SquadManager.GetInstance().GetComponent<Atlas>().GetLegsTexture((int)_ActualAngle);

		if ( _BodyTextureSet.Count > 0 )
		{
			_BodySkinRenderer.material.mainTexture = _BodyTextureSet[0];
		}

		SetLegTextureIndex(0);

	  	_Body.transform.eulerAngles = new Vector3(0, _ActualAngle, 0);
	}

	int _CurrentLegTextureIndex = -1;

	void SetLegTextureIndex(int index)
	{
		if ( _LegsTextureSet.Count > index )
		{
			_CurrentLegTextureIndex = index;
			_LegsSkinRenderer.material.mainTexture = _LegsTextureSet[_CurrentLegTextureIndex];
		}
	}

	void SetNextLegTextureIndex()
	{
		if ( _LegsTextureSet.Count == 0 )
		{
			return;
		}

		_CurrentLegTextureIndex = (_CurrentLegTextureIndex + 1) % _LegsTextureSet.Count;

		if ( _LegsTextureSet.Count > _CurrentLegTextureIndex )
		{
			_LegsSkinRenderer.material.mainTexture = _LegsTextureSet[_CurrentLegTextureIndex];
		}
	}

	void TrySetLegUpdatePosition()
	{
		if ( (transform.position - _LastLegUpdatePosition).magnitude > DELTA_DISTANCE_PER_LEG_FRAME )
		{
			_LastLegUpdatePosition = transform.position;
			SetNextLegTextureIndex();
		}
	}

	public void CarryGrail(GameObject grail)
	{
		m_CarriesGrail = true;

		grail.transform.parent = _Grail.transform;
		grail.transform.localScale = Vector3.one;
		grail.transform.localPosition = Vector3.zero;
	}

	public void DropGrail()
	{
		if( !m_CarriesGrail )
			return;

		m_CarriesGrail = false;

		GameObject tile = SquadManager.GetInstance ()._Pathfinding.GetTileBelow (transform.position);
		HexData tileData = tile.GetComponent<HexData> ();
		GameObject grail = Level.GetInstance ().GetGrailInstance ();
		grail.transform.parent = tile.transform;
		grail.transform.localScale = Vector3.one;
		grail.transform.position = tile.transform.position + Vector3.up * 0.83f;
		tileData.SetContainsGrail (true);
	}

	public bool IsCarryingGrail()
	{
		return m_CarriesGrail;
	}

}
