using UnityEngine;
using System.Collections;

public class HexData : MonoBehaviour 
{
	public bool m_Teleport;
	public bool m_Spawner;
	public bool m_Grail;
	public bool m_SkillPoint;
	public int m_HexType;

	private float m_DistanceOneTile;

	private GameObject m_SpawnedTrooper = null;
	private float m_SpawnDelay;
	private float m_SpawnStart;

	private bool m_StartPosOccupied = false;

	void Awake()
	{
		m_DistanceOneTile = Utils.c_HexRadius * Mathf.Sqrt (3);
	}

	void Start ()
	{
		SetTexture ();

		m_SpawnDelay = Random.Range (SquadManager.ENEMY_SPAWN_DELAY_MIN, SquadManager.ENEMY_SPAWN_DELAY_MAX);
		ResetSpawner ();
	}

	public void SetTexture()
	{
		m_HexType = Random.Range(0,16);
		
		if ( m_HexType == 13 ) m_HexType = 16; // black ceiling 1
		else if ( m_HexType == 12 ) m_HexType = 10; // black ceiling 2
		else if ( m_HexType == 8 ) m_HexType = 6; // grail
		else if ( m_HexType == 4 ) m_HexType = 2; // teleport
		
		if( m_Teleport )
			m_HexType = 4;
		if( m_Grail )
			m_HexType = 8;
		
		Utils.SetUV(renderer.material, m_HexType);
	}

	void Update () 
	{
		UpdateSpawner ();
		UpdateTeleport ();
		UpdateGrail ();
		UpdateSkillPoint ();
	}

	void OnDrawGizmos() 
	{
		if( m_Teleport )
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawCube (transform.position, new Vector3(0.3f, 0.5f, 0.3f));
		}
		if( m_Spawner )
		{
			Gizmos.color = Color.red;
			Gizmos.DrawCube (transform.position, new Vector3(0.3f, 0.5f, 0.3f));
		}
		if( m_Grail )
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere (transform.position, 0.3f);
		}
		if( m_SkillPoint )
		{
			Gizmos.color = Color.green;
			Gizmos.DrawCube (transform.position, new Vector3(0.3f, 0.5f, 0.3f));
		}
	}

	void UpdateSpawner()
	{
		if( !m_Spawner )
			return;

		if( SquadManager.GetInstance().SpawnerVisibleByTroopers(this.gameObject) )
			return;
		
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

	void UpdateTeleport()
	{
		if( !m_Teleport )
			return;

		Trooper tr = SquadManager.GetInstance().GetAllyTrooperOnHex(this.gameObject);
		if( tr == null )
			return;

		if ( (transform.position - tr.transform.position).magnitude > Trooper.WALKING_TOLERANCE )
			return;

		if( tr.IsCarryingGrail() )
		{
			Level.GetInstance().LevelDone();
			m_Teleport = false;
		}
	}
	
	void UpdateGrail()
	{
		if( !m_Grail )
			return;

		for( int i = 0; i < 6; ++i )
		{
			float alpha = (30f + i*60f)*(Mathf.PI/180f);
			float x = m_DistanceOneTile * Mathf.Cos(alpha);
			float z = m_DistanceOneTile * Mathf.Sin(alpha);
			
			Vector3 checkPosition = this.transform.position + new Vector3(x, 0, z);
			GameObject t = SquadManager.GetInstance()._Pathfinding.GetTileBelow(checkPosition);
			if( t == null )
				continue;

			Trooper tr = SquadManager.GetInstance().GetAllyTrooperOnHex(t);
			if( tr == null )
				continue;

			if ( (t.transform.position - tr.transform.position).magnitude < Trooper.WALKING_TOLERANCE )
			{
				tr.CarryGrail();
				m_Grail = false;
				SetTexture();
				return;
			}
		}
	}

	public void UpdateSkillPoint()
	{
		if( !m_SkillPoint )
			return;

		Trooper tr = SquadManager.GetInstance().GetAllyTrooperOnHex(this.gameObject);
		if( tr == null )
			return;
		
		if ( (transform.position - tr.transform.position).magnitude > Trooper.WALKING_TOLERANCE )
			return;

		transform.GetChild (0).renderer.enabled = false;
		Level.GetInstance ().SkillPointPickedUp ();
		m_SkillPoint = false;
	}

	public void ResetSpawner()
	{
		m_SpawnedTrooper = null;
		m_SpawnStart = Time.time;
	}

	public void OccupyStartPos(bool state)
	{
		m_StartPosOccupied = state;
	}

	public bool StartPosOccupied()
	{
		return m_StartPosOccupied;
	}
}
