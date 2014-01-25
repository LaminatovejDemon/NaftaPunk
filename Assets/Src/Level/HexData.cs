using UnityEngine;
using System.Collections;

public class HexData : MonoBehaviour 
{
	public bool m_Spawner;
	public int m_HexType;

	private Trooper m_SpawnedTrooper = null;

	void Start ()
	{
		m_HexType = Random.Range(0,16);


		if ( m_HexType == 13 ) m_HexType = 16; // black ceiling 1
		else if ( m_HexType == 12 ) m_HexType = 10; // black ceiling 2
		else if ( m_HexType == 8 ) m_HexType = 6; // grail
		else if ( m_HexType == 4 ) m_HexType = 2; // teleport

		Utils.SetUV(renderer.material, m_HexType);
	}

	// Update is called once per frame
	void Update () 
	{
		if( m_SpawnedTrooper == null )
		{
		}
	}

	void OnDrawGizmos() 
	{
		if( m_Spawner )
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawCube (transform.position, new Vector3(0.3f, 0.5f, 0.3f));
		}
	}
}
