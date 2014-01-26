using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grail : MonoBehaviour 
{
	List<Texture> _List;
	public float _AnimationTime = 0.1f;
	float _LastAnimationSwitch = -1;
	int index_ = 0;
	
	void Start () 
	{
		_List = SquadManager.GetInstance().GetComponent<Atlas>().GetGrailTexture();
	}
	
	void Update () 
	{
		if ( _List == null )
		{
			return;
		}

		if ( Time.time - _LastAnimationSwitch > _AnimationTime )
		{
			renderer.material.mainTexture = _List[index_];
			_LastAnimationSwitch = Time.time;

			index_ = (index_ +1)%_List.Count;
		}
	}
}
