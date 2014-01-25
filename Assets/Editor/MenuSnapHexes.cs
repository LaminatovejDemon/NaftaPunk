using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MenuSnapHexes : EditorWindow 
{
	private const float c_HexRadius = 0.5f;
	private const float c_CosThreshold = 0.9397f;

	private static List<GameObject> s_SnappedHexes = new List<GameObject>();
	private static Queue<GameObject> s_Queue = new Queue<GameObject> (); 

	[@MenuItem("NaftaPunk/Snap Hexes %&a")]
	static void SnapHexes()
	{
		s_SnappedHexes.Clear ();

		Level levelRoot = FindObjectOfType(typeof(Level)) as Level;
		GameObject firstElement = levelRoot.transform.GetChild(0).gameObject;
		s_SnappedHexes.Add (firstElement);
		s_Queue.Enqueue (firstElement);
		while( s_Queue.Count > 0 )
		{
			GameObject h = s_Queue.Dequeue();
			SnapHexesAround(h);
		}
	}

	static void SnapHexesAround(GameObject refHex)
	{
		Vector3 refHexPos = refHex.transform.position;
		Collider[] hexesAround = Physics.OverlapSphere (refHexPos, 1.5f*c_HexRadius);
		List<int> forbiddenIndices = new List<int> ();
		foreach( Collider hex in hexesAround )
		{
			if( hex.GetComponent<HexData>() == null )
				continue;
			if( s_SnappedHexes.Contains(hex.gameObject) )
				continue;

			Vector3 hexPosition = hex.transform.position;
			Vector3 dir = (hexPosition - refHexPos).normalized;

			GameObject nearestHex = null;
			float minDist = float.MaxValue;
			Vector3 targetPos = Vector3.zero;
			int foundIndex = -1;
			for( int i = 0; i < 6; ++i )
			{
				if( forbiddenIndices.Contains(i) )
					continue;

				float alpha = (30f + i*60f)*(Mathf.PI/180f);

				float x = c_HexRadius * (Mathf.Sqrt (3) / 2) * Mathf.Cos(alpha);
				float z = c_HexRadius * (Mathf.Sqrt (3) / 2) * Mathf.Sin(alpha);

				Vector3 predDir = new Vector3(x, 0, z).normalized;
				float dot = Vector3.Dot(dir, predDir);
				if( dot > c_CosThreshold )
				{
					float distSqr = (hexPosition - refHexPos).sqrMagnitude;
					if( distSqr < minDist )
					{
						minDist = distSqr;
						nearestHex = hex.gameObject;
						targetPos = refHexPos + new Vector3(2*x, 0, 2*z);
						foundIndex = i;
					}
				}
			}
			if( nearestHex != null )
			{
				nearestHex.transform.position = targetPos;
				s_SnappedHexes.Add(nearestHex);
				s_Queue.Enqueue(nearestHex);
				forbiddenIndices.Add(foundIndex);
			}
		}
	}
}
