using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour 
{
	private static Level _Instance = null;
	public static Level GetInstance()
	{
		return _Instance;
	}

	List<GameObject> m_StartPositions = new List<GameObject> ();
	private int m_TroopersKilled = 0;

	void Awake()
	{
		_Instance = this;
	}

	void Start()
	{
		Init ();
	}

	void Update()
	{
		// test na game over - restart?
		if( m_TroopersKilled == 3 )
		{
		}

		// test na splneni levelu
	}
	
	void Init()
	{
		// troops start position 
		for( int i = 0; i < transform.childCount; ++i )
		{
			HexData hexData = transform.GetChild(i).GetComponent<HexData>();
			if( hexData != null )
			{
				if( hexData.m_StarterHex )
					m_StartPositions.Add(hexData.gameObject);
			}
		}
	}

	void Reset()
	{
		// reset ukoncovacich podminek
		m_TroopersKilled = 0;

		// reset startovacich pozic
		for( int i = 0; i < m_StartPositions.Count; ++i )
		{
			HexData pos = m_StartPositions[i].GetComponent<HexData>();
			pos.OccupyStartPos(false);
		}
	}

	public HexData GetFreeStartPos()
	{
		for( int i = 0; i < m_StartPositions.Count; ++i )
		{
			HexData pos = m_StartPositions[i].GetComponent<HexData>();
			if( !pos.StartPosOccupied() )
				return pos;
		}

		return null;
	}

	void GameOver()
	{
	}
}
