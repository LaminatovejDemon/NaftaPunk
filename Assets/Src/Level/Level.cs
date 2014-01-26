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

	public GameObject m_GymTroopTemplate;
	public GameObject m_GeographerTroopTemplate;

	public GameObject m_SkillPointTemplate;
	public GameObject m_GrailTemplate;
	public GameObject m_TeleportParticle;

	List<Trooper> m_Troopers = new List<Trooper> ();
	List<GameObject> m_StartPositions = new List<GameObject> ();
	List<GameObject> m_SkillPointPositions = new List<GameObject> ();
	List<GameObject> m_SpawnPositions = new List<GameObject> ();
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
		if( GameStateManager.GetInstance().WasGameStarted() )
			Init ();
		else
			UIManager.GetInstance()._FractionSelectScreen.gameObject.SetActive(true);
	}

	void Update()
	{
		// test na game over - restart?
		if( m_TroopersKilled == 3 )
		{
			GameOver();
		}

		if( Input.GetKeyUp(KeyCode.Space) )
		{
			for (int i = m_Troopers.Count-1; i >= 0; --i)
			{
				SquadManager.GetInstance ().OnKilled (m_Troopers [i]);
			}
				//Reset ();
			//SquadManager.GetInstance().KillGrailCarrier();
		}
	}

	public GameObject GetGrailInstance()
	{
		return m_GrailInstance;
	}
	
	public void Init()
	{
		// instancovani postav
		GameObject trooperTemplate = null;
		string[] trooperNames = null;
		GameStateManager.EFractionType fraction = GameStateManager.GetInstance ().GetFraction ();
		switch( fraction )
		{
		case GameStateManager.EFractionType.Gyms:
			trooperTemplate = m_GymTroopTemplate;
			SquadManager.GetInstance().EnemyTemplate = m_GeographerTroopTemplate;
			trooperNames = GameStateManager.Fraction1Names;
			break;
		case GameStateManager.EFractionType.Geographers:
			trooperTemplate = m_GeographerTroopTemplate;
			SquadManager.GetInstance().EnemyTemplate = m_GymTroopTemplate;
			trooperNames = GameStateManager.Fraction2Names;
			break;
		}

		for( int i = 0; i < 3; ++i )
		{
			GameObject go = GameObject.Instantiate(trooperTemplate) as GameObject;
			go.name = "Trooper" + (i+1).ToString();

			string trooperName = trooperNames[i];
			GameStateManager.TCharStats stats = GameStateManager.GetInstance().GetStats(trooperName);
			Trooper t = go.GetComponent<Trooper>();
			t.SetRpgProperties(trooperName, stats.Health, stats.Speed, stats.Attack);
		}

		// inicializace hexu
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
					go.transform.localPosition = Vector3.down * 0.21f;
					go.transform.localRotation = Quaternion.AngleAxis(-90, Vector3.left);
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

				// grail
				if( hexData.m_Grail )
				{
					m_GrailPosition = hexData.gameObject;

					m_GrailInstance = GameObject.Instantiate(m_GrailTemplate) as GameObject;
					m_GrailInstance.transform.parent = hexData.transform;

					m_GrailInstance.transform.position = m_GrailPosition.transform.position + Vector3.up*0.5f;
					m_GrailInstance.transform.rotation = Camera.main.transform.rotation;
				}

				// spawn positions
				if( hexData.m_Spawner )
				{
					m_SpawnPositions.Add(hexData.gameObject);
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
			SquadManager.GetInstance ().OnKilled (m_Troopers [i], true);
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

	public void EnableSpawners(bool state)
	{
		for( int i = 0; i < m_SpawnPositions.Count; ++i )
		{
			m_SpawnPositions[i].GetComponent<HexData>().m_Spawner = state;
		}
	}

	public void SkillPointPickedUp()
	{
		m_SkillPointsPickedUp++;
	}

	public void DecreaseSkillPoints()
	{
		-- m_SkillPointsPickedUp;
	}

	public int SkillPointAmount()
	{
		return m_SkillPointsPickedUp;
	}

	public void AddKilledTrooper()
	{
		m_TroopersKilled++;
		Debug.Log ("Trooper killed");
	}

	public void LevelDone()
	{
		Debug.Log ("Level done");
		UIManager.GetInstance()._RewardScreen._Enabled = true;
		//UIManager.GetInstance().r
	}

	void GameOver()
	{
		Debug.Log ("Game over");
	}
}
