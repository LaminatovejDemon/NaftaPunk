using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewardScreen : MonoBehaviour 
{	
	public Transform _LeftContainer;
	public Transform _MiddleContainer;
	public Transform _RightContainer;

	public TextMesh _SkillPointsToSpendMesh;

	public GameObject _TintScreen;

	public bool _Enabled = false;
	bool _EnabledLocal = false;

	void Update()
	{
		if ( _Enabled != _EnabledLocal )
		{
			_EnabledLocal = _Enabled;
			SetVisibility(_EnabledLocal);
		}
	}

	public void OnContinue()
	{
		if ( Level.GetInstance().SkillPointAmount() <= 0 )
		{
			_Enabled = false;
		}
	}

	public void UpdateLevelUpStatus()
	{
		SetVisibility(true);
	}

	public void SetVisibility(bool state)
	{
		List<UIPortrait> portraitList_ = UIManager.GetInstance().GetPortraits();

		_TintScreen.SetActive(state);

		int skillPointAmount_ = Level.GetInstance().SkillPointAmount();

		if (skillPointAmount_ > 0)
		{
			_SkillPointsToSpendMesh.gameObject.SetActive(true);
			_SkillPointsToSpendMesh.text = "YOU HAVE ALSO " + skillPointAmount_ + " SKILLPOINT"+ (skillPointAmount_ > 1 ? "S" : "") +" TO SPEND...";
		}

		if ( state) 
		{

			if ( portraitList_.Count > 0 )
			{
				portraitList_[0].transform.parent = _LeftContainer;
				portraitList_[0].transform.localScale = Vector3.one;
				portraitList_[0].transform.localPosition = Vector3.zero;
				portraitList_[0].SetButtonVisibility(skillPointAmount_ > 0);
			}

			if ( portraitList_.Count > 1 )
			{
				portraitList_[1].transform.parent = _MiddleContainer;
				portraitList_[1].transform.localScale = Vector3.one;
				portraitList_[1].transform.localPosition = Vector3.zero;
				portraitList_[1].SetButtonVisibility(skillPointAmount_ > 0);
			}

			if ( portraitList_.Count > 2 )
			{
				portraitList_[2].transform.parent = _RightContainer;
				portraitList_[2].transform.localScale = Vector3.one;
				portraitList_[2].transform.localPosition = Vector3.zero;
				portraitList_[2].SetButtonVisibility(skillPointAmount_ > 0);
			}
		}
		else
		{
			for ( int i = 0 ; i < portraitList_.Count; ++i )
			{
				portraitList_[i].transform.parent = UIManager.GetInstance().transform;
				portraitList_[i].transform.localScale = Vector3.one;
				portraitList_[i].SetButtonVisibility(false);
				UIManager.GetInstance().SortPortraits();
			}
		}
	}
}
