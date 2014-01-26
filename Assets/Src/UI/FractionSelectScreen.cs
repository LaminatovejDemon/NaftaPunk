using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FractionSelectScreen : MonoBehaviour 
{	
	public void SelectGym()
	{
		gameObject.SetActive(false);
		GameStateManager.GetInstance ().StartGame (GameStateManager.EFractionType.Gyms);
	}

	public void SelectGeo()
	{
		gameObject.SetActive(false);
		GameStateManager.GetInstance ().StartGame (GameStateManager.EFractionType.Geographers);
	}
}
