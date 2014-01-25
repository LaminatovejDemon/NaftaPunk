using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour 
{
	private const float ATTACK_DELTA_TIME = 0.5f;

	private Trooper m_Owner;
	private GameObject m_MoveTarget = null;
	private float m_NextAttackTimestamp = -1;

	void Start () 
	{
		m_Owner = GetComponent<Trooper> ();
	}
	
	public void UpdateOnDemand () 
	{
		Trooper enemy = m_Owner.Watch();
		if( enemy )
		{
			if ( Time.time < m_NextAttackTimestamp )
			{
				return;
			}		
			m_Owner.Attack(enemy);
		}
		else
		{
			m_NextAttackTimestamp = -1;
			m_MoveTarget = FindTarget();
			if( m_MoveTarget )
			{
				SquadManager.GetInstance().OrderEnemyTrooper(m_Owner, m_MoveTarget);
			}
		}
	}

	private GameObject FindTarget()
	{
		return SquadManager.GetInstance().GetMoveTargetForEnemy(m_Owner);
	}
}
