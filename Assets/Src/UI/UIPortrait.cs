using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIPortrait : MonoBehaviour 
{
	Trooper _Owner;

	public TextMesh _Name;
	public Transform _AttackSkillContainer;
	public Transform _HealthSkillContainer;
	public Transform _SpeedSkillContainer;

	public Renderer _AttackBackground;
	public Renderer _HealthBackground;
	public Renderer _SpeedBackground;
	public Renderer _NameBackground;
	public Renderer _PortraitRenderer;

	public Button _HealthSkillButton;
	public Button _AttackSkillButton;
	public Button _SpeedSkillButton;

	public List<Texture> _GeoPortraits;
	public List<Texture> _GymPortraits;

	int _AttackLocal;
	public int _Attack
	{
		set{
			_AttackLocal = value;
			ShowStats(_AttackSkillContainer, value);
		}
		get{
			return _AttackLocal;
		}
	}

	public void SetButtonVisibility(bool state)
	{
		_HealthSkillButton.gameObject.SetActive(state);
		_SpeedSkillButton.gameObject.SetActive(state);
		_AttackSkillButton.gameObject.SetActive(state);
	}


	int _SpeedLocal;
	public int _Speed
	{
		set{
			_SpeedLocal = value;
			ShowStats(_SpeedSkillContainer, value);
		}
		get{
			return _SpeedLocal;
		}
	}

	int _HealthLocal;
	public int _Health
	{
		set{
			_HealthLocal = value;
			ShowStats(_HealthSkillContainer, value);
		}
		get{
			return _HealthLocal;
		}
	}

	public void OnSelect(bool state)
	{
		Material target_ = state ? UIManager.GetInstance().SelectedPortrait : UIManager.GetInstance().DefaultPortrait;

		Color nameColor_ = state ? UIManager.GetInstance().SelectedPortrait.GetColor("_TintColor") : Color.white;
		nameColor_.a = 1.0f;
		_Name.color = nameColor_;

		_Name.fontStyle = state ? FontStyle.Bold : FontStyle.Normal;

		_AttackBackground.material = target_;
		_HealthBackground.material = target_;
		_SpeedBackground.material = target_;
		_NameBackground.material = target_;
	}

	void ShowStats(Transform container, int number)
	{
		int i = 0;
		for ( ; i < number; ++i )
		{
			container.GetChild(i).gameObject.SetActive(true);
		}
		for ( ; i < container.childCount; ++i )
		{
			container.GetChild(i).gameObject.SetActive(false);
		}
	}
	
	public void SetStats(Trooper owner, int index = -1)
	{
		_Owner = owner;
		_AttackSkillButton._Target = owner.gameObject;
		_AttackSkillButton._Message = "SkillUpAttack";
		_SpeedSkillButton._Target = owner.gameObject;
		_SpeedSkillButton._Message = "SkillUpSpeed";
		_HealthSkillButton._Target = owner.gameObject;
		_HealthSkillButton._Message = "SkillUpHealth";

		_Name.text = _Owner.NAME;
		_Health = _Owner._SkillHealth;
		_Speed = _Owner._SkillSpeed;
		_Attack = _Owner._SkillAttack;

		if ( index != -1 )
		{
			_PortraitRenderer.material.mainTexture = 
				owner._Fraction == GameStateManager.EFractionType.Geographers ? _GeoPortraits[index % _GeoPortraits.Count] : _GymPortraits[index % _GymPortraits.Count]; 
		}

		_PortraitRenderer.GetComponent<Button>()._Target = owner.gameObject;
		_PortraitRenderer.GetComponent<Button>()._Message = "Select";

	}

}
