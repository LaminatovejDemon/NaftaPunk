using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	public List<Transform> _GunfireContainers;

	void SetGunfire(Trooper.Angle angle, bool state)
	{
		_Attacker._GunfireParticle.gameObject.SetActive(state);
		if ( _GunfireContainers.Count > (int)angle )
		{
			_Attacker._GunfireParticle.transform.parent = _GunfireContainers[(int)angle];
			_Attacker._GunfireParticle.transform.localPosition = Vector3.zero;
			_Attacker._GunfireParticle.transform.localRotation = Quaternion.identity;
		}

		if ( state )
		{
			GetComponent<AudioSource>().clip = MusicManager.GetInstance().GetClip(_Attacker._Fraction == GameStateManager.EFractionType.Geographers ? MusicManager.OneShots.Gunfire_Art : MusicManager.OneShots.Gunfire_Gym);
			GetComponent<AudioSource>().loop = true;
			GetComponent<AudioSource>().Play();
		}
		else
		{
			GetComponent<AudioSource>().Stop();
		}
	}

	public void SetTarget(Trooper target)
	{
		if ( target == null )
		{
			SetGunfire(_Attacker._NamedAngle, false);
		}
		if (_Target == target) 
		{

			return;
		}

	/*	if (_Target != null)
		{
			_Target._Body.renderer.material = _Target._Side == Trooper.Side.F_Ally ? SquadManager.GetInstance().TrooperAllyMaterial : SquadManager.GetInstance().TrooperEnemyMaterial;
		}*/

		_Target = target;

		if ( _Target != null )
		{
	//		_Target._Body.renderer.material = SquadManager.GetInstance().TrooperHitMaterial;
			SetGunfire(_Attacker._NamedAngle, true);
		}
	}

	void Update()
	{
		//SetGunfire((Trooper.Angle)(((int)(Time.time)) % 6), true);
		//SetGunfire(Trooper.Angle.Angle_300, true);

		if ( _Target != null )
		{
/*			if( _Target.GetKilled() )
			{
				SetTarget(null);
				return;
			}*/
			if ( _Target._HealthBar._Health <= 0 )
			{
				_Target._BloodParticle.gameObject.SetActive(false);
				SetTarget(null);
				return;
			}

			_Target._BloodParticle.gameObject.SetActive(true);
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
