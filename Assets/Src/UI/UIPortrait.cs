using UnityEngine;
using System.Collections;

public class UIPortrait : MonoBehaviour 
{
	Trooper _Owner;

	public TextMesh _Name;
	public Transform _AttackSkillContainer;
	public Transform _HealthSkillContainer;
	public Transform _SpeedSkillContainer;

	public int _AttackLocal;
	public int _Attack
	{
		set{
			_AttackLocal = value;
			ShowStats(_AttackSkillContainer, value);
		}
		get{
			return _AttackLocal;
		}
	}

	public int _SpeedLocal;
	public int _Speed
	{
		set{
			_SpeedLocal = value;
			ShowStats(_SpeedSkillContainer, value);
		}
		get{
			return _SpeedLocal;
		}
	}

	public int _HealthLocal;
	public int _Health
	{
		set{
			_HealthLocal = value;
			ShowStats(_HealthSkillContainer, value);
		}
		get{
			return _HealthLocal;
		}
	}

	void ShowStats(Transform container, int number)
	{
		int i = 0;
		for ( ; i < number; ++i )
		{
			container.GetChild(i).gameObject.SetActive(true);
		}
		for ( ; i < container.childCount; ++i )
		{
			container.GetChild(i).gameObject.SetActive(false);
		}
	}
	
	public void SetStats(Trooper owner)
	{
		_Owner = owner;
		_Name.text = _Owner.NAME;
		_Health = _Owner._SkillHealth;
		_Speed = _Owner._SkillSpeed;
		_Attack = _Owner._SkillAttack;
	}

}
