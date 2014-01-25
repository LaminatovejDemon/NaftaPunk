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

	public GameObject m_SkillPointTemplate;

	List<GameObject> m_StartPositions = new List<GameObject> ();
	List<GameObject> m_SkillPointPositions = new List<GameObject> ();
	private int m_TroopersKilled = 0;
	private int m_SkillPointsPickedUp = 0;

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

		if( Input.GetKey(KeyCode.Space) )
		{
		}
	}
	
	void Init()
	{
		for( int i = 0; i < transform.childCount; ++i )
		{
			HexData hexData = transform.GetChild(i).GetComponent<HexData>();
			if( hexData != null )
			{
				// troops start positions 
				if( hexData.m_Teleport )
					m_StartPositions.Add(hexData.gameObject);

				// skillpoint positions
				if( hexData.m_SkillPoint )
				{
					m_SkillPointPositions.Add(hexData.gameObject);

					GameObject go = GameObject.Instantiate(m_SkillPointTemplate) as GameObject;
					go.transform.position = hexData.transform.position + 0.5f*Vector3.up;
					go.transform.parent = hexData.transform;
				}
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

		// reset skill pointu
		m_SkillPointsPickedUp = 0;
		for( int i = 0; i < m_StartPositions.Count; ++i )
		{
			HexData pos = m_SkillPointPositions[i].GetComponent<HexData>();
			pos.m_SkillPoint = true;
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

	public void SkillPointPickedUp()
	{
		m_SkillPointsPickedUp++;
	}

	public void LevelDone()
	{
		//UIManager.GetInstance().r
	}

	void GameOver()
	{
	}
}
