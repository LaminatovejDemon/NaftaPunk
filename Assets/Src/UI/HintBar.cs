using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HintBar : MonoBehaviour 
{	
	public TextMesh _TitleText;

	string [] _SearchTexts = {"FIND GRALOPHONE", "I KNOW YOU CAN DO IT", "FIRST TAP TO WALK", "SECOND TAP TO ROTATE", "DID YOU KNOW YOU CAN WALK BACKWARDS?"};
	
	public void Start()
	{
		OnEnable();
	}

	void OnEnable()
	{
		_TitleText.text = "FIND GRALOPHONE"; 
	}

	void Update()
	{
		Vector3 pos_ = _TitleText.transform.position + Vector3.left * (Time.deltaTime/Time.timeScale) * 3.5f;

		if ( UIManager.GetInstance().camera.WorldToViewportPoint(pos_).x < -0.5f) 
		{
			_TitleText.text = _SearchTexts[Random.Range(0, _SearchTexts.Length)];
			pos_ = UIManager.GetInstance().camera.ViewportToWorldPoint(Vector3.right * 1.5f);
			pos_.y = _TitleText.transform.position.y;
			pos_.z = _TitleText.transform.position.z;
		}

		_TitleText.transform.position = pos_;
	}
}
