using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Atlas : MonoBehaviour 
{
	public List<Texture>[] _GymList;

	void Start()
	{
		_GymList = new List<Texture>[6];

		for ( int i = 0; i < _GymList.Length; ++i )
		{
			_GymList[i] = new List<Texture>();
		}

		FillList(_GymList[0], 0);
		FillList(_GymList[1], 60);
		FillList(_GymList[2], 120);
		FillList(_GymList[3], 180);
		FillList(_GymList[4], 240);
		FillList(_GymList[5], 300);
	}

	public Texture GetTexture(int angle)
	{
		if ( angle > 270 )
			return _GymList[5][0];
		else if ( angle > 210 )
			return _GymList[0][0];
		else if ( angle > 150 )
			return _GymList[1][0];
		else if ( angle > 90 )
			return _GymList[2][0];
		else if ( angle > 30 )
			return _GymList[3][0];
		else
			return _GymList[4][0];
	}

	void FillList(List<Texture> target, int angle)
	{
		Texture tex_ = null;
		int frame_ = 1;
		while ( true )
		{
			string name_ = "Gymmasters/gymmaster_Left_Angle" + angle + "_frame" + frame_;
	
			tex_ = (Texture)Resources.Load(name_);

			if ( tex_ == null )
			{
				break;
			}

			target.Add(tex_);
			++frame_;
		}
	}

}
