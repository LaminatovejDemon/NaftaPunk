using UnityEngine;
using System.Collections;

public class HexData : MonoBehaviour 
{
	public bool m_Spawner;

	private Trooper m_SpawnedTrooper = null;

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
