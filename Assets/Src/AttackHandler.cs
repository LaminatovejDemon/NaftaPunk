using UnityEngine;
using System.Collections;

public class AttackHandler : MonoBehaviour 
{
	public Trooper _Attacker;
	public Trooper _Target;

	public float _Damage;

	float _NextWatchTimestamp = -1;

	public void Attack(Trooper target)
	{
		SetTarget(target);
	}

	void SetTarget(Trooper target)
	{
		if (_Target == target) 
		{
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

		}
	}

	void Update()
	{
		if ( _Target != null )
		{
			_Target._HealthBar._Health -= _Damage * Time.deltaTime;
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

		if ( !SquadManager.GetInstance().HasVisbility(_Attacker, _Target) )
		{
			SetTarget(null);
		}
	}
}
