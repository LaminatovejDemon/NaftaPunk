using UnityEngine;
using System.Collections;

public class HexData : MonoBehaviour 
{
	public bool m_Spawner;

	private GameObject m_SpawnedTrooper = null;

	void Update () 
	{
		UpdateSpawner ();
	}

	void OnDrawGizmos() 
	{
		if( m_Spawner )
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawCube (transform.position, new Vector3(0.3f, 0.5f, 0.3f));
		}
	}

	void UpdateSpawner()
	{
		if( m_Spawner )
		{
			if( m_SpawnedTrooper == null )
			{
				m_SpawnedTrooper = GameObject.Instantiate(SquadManager.GetInstance().EnemyTemplate) as GameObject;
				Trooper trooperComp = m_SpawnedTrooper.GetComponent<Trooper>();
				trooperComp._Fraction = Trooper.Fraction.F_Enemy;
				
				m_SpawnedTrooper.transform.position = transform.position;
			}
		}
	}
}
