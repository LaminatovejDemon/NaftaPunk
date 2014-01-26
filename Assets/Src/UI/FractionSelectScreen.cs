using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FractionSelectScreen : MonoBehaviour 
{	
	public TextMesh _TitleShadow;
	public TextMesh _TitleShadow2;

	public void Start()
	{
		OnEnable();
	}

	public void OnEnable()
	{
		Time.timeScale = 0.0001f;
		UIManager.GetInstance()._ChapterScreen.gameObject.SetActive(false);

		_TitleShadow.animation["TitleShadow"].speed = 1.0f/Time.timeScale;
		_TitleShadow.animation.Play();
		_TitleShadow2.animation["TitleShadow2"].speed = 1.0f/Time.timeScale;
		_TitleShadow2.animation.Play();
	}

	public void SelectGym()
	{
		gameObject.SetActive(false);

		UIManager.GetInstance()._ChapterScreen.gameObject.SetActive(true);
		UIManager.GetInstance()._ChapterScreen.InitFraction(GameStateManager.EFractionType.Gyms);
	}

	public void SelectGeo()
	{
		gameObject.SetActive(false);

		UIManager.GetInstance()._ChapterScreen.gameObject.SetActive(true);
		UIManager.GetInstance()._ChapterScreen.InitFraction(GameStateManager.EFractionType.Geographers);
	}
}
