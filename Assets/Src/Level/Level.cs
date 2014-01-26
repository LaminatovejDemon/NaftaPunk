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
	public GameObject m_GrailTemplate;
	public GameObject m_TeleportParticle;

	List<Trooper> m_Troopers = new List<Trooper> ();
	List<GameObject> m_StartPositions = new List<GameObject> ();
	List<GameObject> m_SkillPointPositions = new List<GameObject> ();
	GameObject m_GrailPosition;

	private GameObject m_GrailInstance;

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

		if( Input.GetKeyUp(KeyCode.Space) )
		{
			Reset ();
			//SquadManager.GetInstance().KillGrailCarrier();
		}
	}

	public GameObject GetGrailInstance()
	{
		return m_GrailInstance;
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
				{
					m_StartPositions.Add(hexData.gameObject);

					GameObject go = GameObject.Instantiate(m_TeleportParticle) as GameObject;
					go.transform.parent = hexData.transform;
					go.transform.position = hexData.transform.position;
					go.transform.rotation = Quaternion.identity;
				}

				// skillpoint positions
				if( hexData.m_SkillPoint )
				{
					m_SkillPointPositions.Add(hexData.gameObject);

					GameObject go = GameObject.Instantiate(m_SkillPointTemplate) as GameObject;
					go.transform.position = hexData.transform.position + 0.5f*Vector3.up;
					go.transform.rotation = Quaternion.identity;
					go.transform.parent = hexData.transform;
				}

				if( hexData.m_Grail )
				{
					m_GrailPosition = hexData.gameObject;

					m_GrailInstance = GameObject.Instantiate(m_GrailTemplate) as GameObject;
					m_GrailInstance.transform.parent = hexData.transform;

					m_GrailInstance.transform.position = m_GrailPosition.transform.position + Vector3.up*0.5f;
					m_GrailInstance.transform.rotation = Camera.main.transform.rotation;
				}
			}
		}

	}

	void Reset()
	{
		// zabiti enemaku
		SquadManager.GetInstance ().KillAllEnemies ();

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
		for( int i = 0; i < m_SkillPointPositions.Count; ++i )
		{
			HexData pos = m_SkillPointPositions[i].GetComponent<HexData>();
			pos.m_SkillPoint = true;
			pos.transform.GetChild(0).renderer.enabled = true;
		}

		// reset gralu
		SquadManager.GetInstance ().DropGrailFromAnyone ();
		for( int i = 0; i < transform.childCount; ++i )
		{
			HexData hexData = transform.GetChild(i).GetComponent<HexData>();
			if( hexData != null )
			{
				hexData.SetContainsGrail(false);
			}
		}
		m_GrailInstance.transform.parent = m_GrailPosition.transform;
		m_GrailInstance.transform.position = m_GrailPosition.transform.position + Vector3.up*0.5f;
		m_GrailPosition.GetComponent<HexData> ().SetContainsGrail (true);

		// obnova trooperu
		for (int i = m_Troopers.Count-1; i >= 0; --i)
		{
			SquadManager.GetInstance ().OnKilled (m_Troopers [i]);
			m_Troopers[i].gameObject.SetActive(true);
		}
	}

	public void InitTrooper(Trooper t)
	{
		HexData startPos = GetFreeStartPos();
		startPos.OccupyStartPos(true);
		t.transform.position = startPos.transform.position;

		if( !m_Troopers.Contains(t) )
			m_Troopers.Add(t);
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

	public void AddKilledTrooper()
	{
		m_TroopersKilled++;
	}

	public void LevelDone()
	{
		//UIManager.GetInstance().r
	}

	void GameOver()
	{
	}
}
