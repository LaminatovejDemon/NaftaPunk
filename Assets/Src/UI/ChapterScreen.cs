using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChapterScreen : MonoBehaviour 
{	
	public TextMesh _TitleText;
	public TextMesh _TitleShadow;
	public TextMesh _TitleShadow2;
	public TextMesh _DescriptionText;

	List <string> _FailTexts = new List<string>();
	string[] _ChapterNumbers = {"XII", "XVII", "CXVI", "XIII", "VI", "III", "IX", "IV", "DIV", "MMVI", "XXV"};
	string _GeoDescription = "You are very rachetic.\n And also art historian.\n But now, you need to save\nthe UNIVERSE.\nAt least it's History!.\n\nGo and get Phonograil!\n";
	string _GymDescription = "Those skinny idiots\n don't even know\n how important\ndevice phono...\n phon'o'thing is!\n ...They said.\n\nGET IT!\n";

	public void OnEnable()
	{
		Time.timeScale = 0.0001f;
		_TitleShadow.animation["TitleShadow"].speed = 1.0f/Time.timeScale;
		_TitleShadow.animation.Play();
		_TitleShadow2.animation["TitleShadow2"].speed = 1.0f/Time.timeScale;
		_TitleShadow2.animation.Play();

		//UIManager.GetInstance().SetStatsVisibility(false);
	}

	public void Start()
	{
		if ( _Fraction == GameStateManager.EFractionType.Invalid )
		{
			_TitleText.text = _TitleShadow.text = _TitleShadow2.text = _ChapterNumbers[Random.Range(0, _ChapterNumbers.Length)] + ". ACT"; 

			_DescriptionText.text = "bla bla, vsichni spolu valci\nbla bla, vsichni spolu valci bla\nbla bla, vsichni spolu valci valci\nbla bla, vsichni spolu.";
		}

		OnEnable();
	}

	GameStateManager.EFractionType _Fraction = GameStateManager.EFractionType.Invalid;

	public void InitFraction(GameStateManager.EFractionType fraction)
	{
		_Fraction = fraction;
		_TitleText.text = _TitleShadow.text = _TitleShadow2.text = "I. ACT";
		_DescriptionText.text = fraction == GameStateManager.EFractionType.Geographers ? _GeoDescription : _GymDescription;
	}

	public void Continue()
	{
		gameObject.SetActive(false);
		Time.timeScale = 1.0f;

		if ( _Fraction != GameStateManager.EFractionType.Invalid )
		{
			GameStateManager.GetInstance ().StartGame (_Fraction);
			_Fraction = GameStateManager.EFractionType.Invalid;
		}

		UIManager.GetInstance()._HintBar.gameObject.SetActive(true);
		UIManager.GetInstance()._HintBar.SetState(HintBar.State.Search);

	}
}
