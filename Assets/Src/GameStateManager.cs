using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour 
{
	public const string c_Squad1Trooper1Name = "Name11";
	public const string c_Squad1Trooper2Name = "Name12";
	public const string c_Squad1Trooper3Name = "Name13";
	public const string c_Squad2Trooper1Name = "Name21";
	public const string c_Squad2Trooper2Name = "Name22";
	public const string c_Squad2Trooper3Name = "Name23";
	
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

		m_CharacterStats.Add(c_Squad1Trooper1Name, new TCharStats(1, 1, 1));
		m_CharacterStats.Add(c_Squad1Trooper2Name, new TCharStats(1, 1, 1));
		m_CharacterStats.Add(c_Squad1Trooper3Name, new TCharStats(1, 1, 1));
		m_CharacterStats.Add(c_Squad2Trooper1Name, new TCharStats(1, 1, 1));
		m_CharacterStats.Add(c_Squad2Trooper2Name, new TCharStats(1, 1, 1));
		m_CharacterStats.Add(c_Squad2Trooper3Name, new TCharStats(1, 1, 1));
	}

	public TCharStats GetStats(string charName)
	{
		return m_CharacterStats [charName];
	}

	public void SaveStats(string charName, TCharStats stats)
	{
		m_CharacterStats[charName] = stats;
	}




}
