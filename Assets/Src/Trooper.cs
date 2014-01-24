using UnityEngine;
using System.Collections;

public class Trooper : MonoBehaviour {

	public GameObject _HighlightCircle;

	public bool _Selected = false;

	public void OnSelect(bool state)
	{
		_Selected = state;
		_HighlightCircle.SetActive(state);
	}

	public float _TargetAngle = 0;
	public float _ActualAngle = 0;

	float GetTargetAngle(GameObject target)
	{
		Vector3 targetPos_ = target.transform.position;
		targetPos_.y = transform.position.y;
		return Quaternion.LookRotation(transform.position - target.transform.position).eulerAngles.y;                                   
	}

	public void SetDirection(GameObject target)
	{
		_TargetAngle = GetTargetAngle(target);
	}

	public bool HasDirection(GameObject target)
	{
		return _TargetAngle == GetTargetAngle(target);
	}

	public void Walk(GameObject target)
	{

	}

	void Update()
	{

		_ActualAngle = _TargetAngle;

		transform.eulerAngles = new Vector3(0, _ActualAngle, 0);
	}



}
