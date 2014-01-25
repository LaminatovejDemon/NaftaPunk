using UnityEngine;
using System.Collections;

public class AttackHandler : MonoBehaviour 
{
	public Trooper _Attacker;
	public Trooper _Target { get; private set;}

	//public float _Damage;

	float _NextWatchTimestamp = -1;

	public void Attack(Trooper target)
	{
		SetTarget(target);
	}

	public void SetTarget(Trooper target)
	{
		if (_Target == target) 
		{
			if ( target == null )
			{
				_Attacker._GunfireParticle.gameObject.SetActive(false);
			}
			
			return;
		}

		if (_Target != null)
		{
			_Target._Body.renderer.material = _Target._Fraction == Trooper.Fraction.F_Ally ? SquadManager.GetInstance().TrooperAllyMaterial : SquadManager.GetInstance().TrooperEnemyMaterial;

		}

		_Target = target;

		if ( _Target != null )
		{
			_Target._Body.renderer.material = SquadManager.GetInstance().TrooperHitMaterial;
			_Attacker._GunfireParticle.gameObject.SetActive(true);
		}
	}

	void Update()
	{
		if ( _Target != null )
		{
			if ( _Target._HealthBar._Health <= 0 )
			{
				SetTarget(null);
				return;
			}

			_Target._HealthBar._Health -= _Attacker.ATTACK * (float)_Attacker._SkillAttack * Time.deltaTime;
			_Target._HealthBar.UpdateHealth();
		}
		else
		{
			return;
		}


		if ( Time.time < _NextWatchTimestamp )
		{
			return;
		}

		_NextWatchTimestamp = Time.time + _Attacker.WATCH_DELTA_TIME;

		if ( !SquadManager.GetInstance().HasVisibility(_Attacker, _Target.gameObject) )
		{
			SetTarget(null);
		}
	}
}
