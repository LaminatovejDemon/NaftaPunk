using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour 
{
	public enum EFractionType
	{
		Gyms,
		Geographers
	}

	public const string c_Squad1Trooper1Name = "Kamil Lhotak";
	public const string c_Squad1Trooper2Name = "Name12";
	public const string c_Squad1Trooper3Name = "Name13";
	public const string c_Squad2Trooper1Name = "Name21";
	public const string c_Squad2Trooper2Name = "Name22";
	public const string c_Squad2Trooper3Name = "Name23";

	public static string[] Fraction1Names = {c_Squad1Trooper1Name, c_Squad1Trooper2Name, c_Squad1Trooper3Name};
	public static string[] Fraction2Names = {c_Squad2Trooper1Name, c_Squad2Trooper2Name, c_Squad2Trooper3Name};

	private bool m_GameStarted = false;
	private EFractionType m_ActFraction = EFractionType.Gyms;
	
	public struct TCharStats
	{
		public int Health;
		public int Speed;
		public int Attack;

		public TCharStats(int h, int s, int a)
		{
			Health = h;
			Speed = s;
			Attack = a;
		}
	}
	private Dictionary<string, TCharStats> m_CharacterStats = new Dictionary<string, TCharStats>();

	private static GameStateManager _Instance = null;
	public static GameStateManager GetInstance()
	{
		return _Instance;
	}

	void Awake()
	{
		if( _Instance != null )
		{
			Destroy(this.gameObject);
			return;
		}

		DontDestroyOnLoad(this);
		_Instance = this;

		ResetGame ();
	}

	public TCharStats GetStats(string charName)
	{
		return m_CharacterStats [charName];
	}

	public void SaveAllStats()
	{
		for ( int i = 0; i < SquadManager.GetInstance()._AllyList.Count; ++i )
		{
			Trooper ally_ = SquadManager.GetInstance()._AllyList[i];
			SaveStats(ally_.NAME, new TCharStats(ally_._SkillHealth, ally_._SkillSpeed, ally_._SkillAttack));
		}
	}

	public void SaveStats(string charName, TCharStats stats)
	{
		m_CharacterStats[charName] = stats;
	}

	int _SpreadStatsindex = -1;

	public void SetTrooperStats(Trooper target)
	{
		_SpreadStatsindex = (_SpreadStatsindex + 1) % 3;

		string name_ = target._Fraction == EFractionType.Gyms ? Fraction1Names[_SpreadStatsindex] : Fraction2Names[_SpreadStatsindex];

		int penalty_ = Random.Range(0,2);
		target.SetRpgProperties(name_,  m_CharacterStats[name_].Health - penalty_, m_CharacterStats[name_].Speed - 1, m_CharacterStats[name_].Attack - (2-penalty_) ); 
	}

	public void ResetGame()
	{
		m_GameStarted = false;
		m_CharacterStats.Clear ();

		m_CharacterStats.Add(c_Squad1Trooper1Name, new TCharStats(3, 1, 2));
		m_CharacterStats.Add(c_Squad1Trooper2Name, new TCharStats(2, 1, 3));
		m_CharacterStats.Add(c_Squad1Trooper3Name, new TCharStats(2, 3, 2));
		m_CharacterStats.Add(c_Squad2Trooper1Name, new TCharStats(2, 3, 3));
		m_CharacterStats.Add(c_Squad2Trooper2Name, new TCharStats(2, 2, 2));
		m_CharacterStats.Add(c_Squad2Trooper3Name, new TCharStats(3, 2, 3));
	}

	public void SetFraction(EFractionType ft)
	{
		m_ActFraction = ft;
	}

	public EFractionType GetFraction()
	{
		return m_ActFraction;
	}

	public void SwitchFraction()
	{
		if( m_ActFraction == EFractionType.Gyms )
			m_ActFraction = EFractionType.Geographers;
		else
			m_ActFraction = EFractionType.Gyms;
	}

	public bool WasGameStarted()
	{
		return m_GameStarted;
	}

	public void StartGame(EFractionType initFraction)
	{
		m_ActFraction = initFraction;
		m_GameStarted = true;

		Level.GetInstance().Init ();
	}

}
