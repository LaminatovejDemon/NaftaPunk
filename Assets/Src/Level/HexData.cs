using UnityEngine;
using System.Collections;

public class HexData : MonoBehaviour 
{
	public bool m_Spawner;
	public int m_HexType;

	private GameObject m_SpawnedTrooper = null;
	private float m_SpawnDelay;
	private float m_SpawnStart;

	void Start ()
	{
		m_SpawnDelay = Random.Range (1f, 3f);
		m_HexType = Random.Range(0,16);
		
		if ( m_HexType == 13 ) m_HexType = 16; // black ceiling 1
		else if ( m_HexType == 12 ) m_HexType = 10; // black ceiling 2
		else if ( m_HexType == 8 ) m_HexType = 6; // grail
		else if ( m_HexType == 4 ) m_HexType = 2; // teleport

		Utils.SetUV(renderer.material, m_HexType);

		ResetSpawner ();
	}

	// Update is called once per frame
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
				if( Time.time < m_SpawnStart + m_SpawnDelay )
					return;

				m_SpawnedTrooper = GameObject.Instantiate(SquadManager.GetInstance().EnemyTemplate) as GameObject;
				Trooper trooperComp = m_SpawnedTrooper.GetComponent<Trooper>();
				trooperComp._Fraction = Trooper.Fraction.F_Enemy;
				trooperComp.SetSpawner(this);
				
				m_SpawnedTrooper.transform.position = transform.position;
			}
		}
	}

	public void ResetSpawner()
	{
		m_SpawnedTrooper = null;
		m_SpawnStart = Time.time;
	}
}
