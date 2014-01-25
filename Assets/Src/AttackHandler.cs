using UnityEngine;
using System.Collections;

public class AttackHandler : MonoBehaviour 
{
	public void Attack(Trooper target)
	{
		if ( target == null )
		{
			return;
		}

		target._Body.renderer.material.color = Color.red;
	}

}
