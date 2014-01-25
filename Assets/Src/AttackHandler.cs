using UnityEngine;
using System.Collections;

public class AttackHandler : MonoBehaviour 
{
	public Trooper _Attacker;
	public Trooper _Target;

	public void Attack(Trooper target)
	{
		_Target = target;
	}

	void Update()
	{

	}

}
