using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grail : MonoBehaviour 
{
	List<Texture> _List;
	public float _AnimationTime = 0.1f;
	float _LastAnimationSwitch = -1;
	int index_ = 0;

	public bool _XP = false; // Too tired to do better, used for grail and xp
	
	void Start () 
	{
		if ( !_XP )
		{
			_List = SquadManager.GetInstance().GetComponent<Atlas>().GetGrailTexture();
		}
		else
		{
			_List = SquadManager.GetInstance().GetComponent<Atlas>().GetXPTexture();
		}

		transform.rotation = Camera.main.transform.rotation;
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
