using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HintBar : MonoBehaviour 
{	
	public TextMesh _TitleText;

	string [] _SearchTexts = {"FIND GRALOPHONE", "I KNOW YOU CAN DO IT","FIND GRALOPHONE", "FIRST TAP TO ROTATE","FIND GRALOPHONE", "SECOND TAP TO WALK","FIND GRALOPHONE", "DID YOU KNOW YOU CAN WALK BACKWARDS?"};
	string [] _FoundGrail = {"GO BACK TO THE SHIP", "I KNEW YOU CAN DO IT","GO BACK TO THE SHIP", "EVERY LIVING BEING TO TELEPORTS, PLEASE","GO BACK TO THE SHIP", "DON'T DROP PHONOGRAIL, PLEASE", "GO BACK TO THE SHIP"};
	string [] _Won = {"YOU'RE AWESOME", "YOU WON!", "DID YOU FIND ANY SKILL POINT?","YOU WON!", "YEAH, YEAH... HEALTH, ATTACK and SPEED","YOU WON!", "FAIRLY SPLITTED POINTS PREVENTS REVOLTS"};
	string [] _Lost = {"I NEVER BELIEVED IN YOU.", "HEY MAN, YOU'RE REALLY BAD", "DID YOU THAT CARNAGE? ..uh sorry.", "YOUR SQUAD IS ... DEAD"};

	public void Start()
	{
		OnEnable();
	}

	void OnEnable()
	{
		_TitleText.text = "FIND GRALOPHONE"; 
	}

	public enum State
	{
		Search,
		FoundGrail,
		Won,
		Lost,
		Invalid,
	};

	State _State;

	public void SetState(State state)
	{
		_State = state;
		Reset();
	}

	void Reset()
	{
		_TitleText.transform.position += Vector3.right * 10.0f;
	}

	string GetText()
	{
		switch(_State)
		{
		case State.Search:
			return _SearchTexts[Random.Range(0, _SearchTexts.Length)];
		case State.FoundGrail:
			return _FoundGrail[Random.Range(0, _FoundGrail.Length)];
		case State.Won:
			return _Won[Random.Range(0, _Won.Length)];
		case State.Lost:
			return _Lost[Random.Range(0, _Lost.Length)];
		}

		return "";
	}

	void Update()
	{
		Vector3 pos_ = _TitleText.transform.position + Vector3.left * (Time.deltaTime/Time.timeScale) * 3.5f;

		if ( UIManager.GetInstance().camera.WorldToViewportPoint(pos_).x < -0.5f) 
		{
			_TitleText.text = GetText();
			pos_ = UIManager.GetInstance().camera.ViewportToWorldPoint(Vector3.right * 1.5f);
			pos_.y = _TitleText.transform.position.y;
			pos_.z = _TitleText.transform.position.z;
		}

		_TitleText.transform.position = pos_;
	}
}
