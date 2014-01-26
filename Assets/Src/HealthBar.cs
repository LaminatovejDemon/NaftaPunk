using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour 
{
	public Trooper _Owner;
	public GameObject _Container;
	public GameObject _Bar;

	public float _MaxHealth;
	public float _Health;

	float _HealthLocal = -1;

	void Start()
	{
		_Container.SetActive(true);
		_Container.transform.localPosition = Vector3.up * 1.7f;
		_Container.transform.rotation = Camera.main.transform.rotation;
	}

	void Update()
	{
		UpdateHealth();
	}

	public void UpdateHealth()
	{
		_MaxHealth = _Owner.HEALTH * (float)_Owner._SkillHealth;

		if ( _HealthLocal != _Health )
		{
			_Health = Mathf.Max(0, _Health);
			_Health = Mathf.Min(_MaxHealth, _Health);
			
			_HealthLocal = _Health;
			Vector3 scale_ = _Bar.transform.localScale;
			scale_.x = 0.95f * (_HealthLocal / _MaxHealth);
			_Bar.transform.localScale = scale_;

			if ( _HealthLocal <= 0 )
			{
				SquadManager.GetInstance().OnKilled(_Owner);
			}
		}
		else
		{
			_Owner._BloodParticle.gameObject.SetActive(false);
		}
	}

}
