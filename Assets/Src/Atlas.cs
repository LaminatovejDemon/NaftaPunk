using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Atlas : MonoBehaviour 
{
	public List<Texture>[] _GymList;
	public List<Texture>[] _GymLegList;
	public List<Texture>[] _Gramophone;

	void CreateAtlas(ref List<Texture>[] list, int dimension)
	{
		list = new List<Texture>[6];

		for ( int i = 0; i < list.Length; ++i )
		{
			list[i] = new List<Texture>();
		}
	}

	void Start()
	{
		CreateAtlas(ref _GymList, 6);
		CreateAtlas(ref _GymLegList, 6);




		FillList(_GymList[0], 0);
		FillList(_GymList[1], 60);
		FillList(_GymList[2], 120);
		FillList(_GymList[3], 180);
		FillList(_GymList[4], 240);
		FillList(_GymList[5], 300);

		FillLegList(_GymLegList[0], 0);
		FillLegList(_GymLegList[1], 60);
		FillLegList(_GymLegList[2], 120);
		FillLegList(_GymLegList[3], 180);
		FillLegList(_GymLegList[4], 240);
		FillLegList(_GymLegList[5], 300);
	}

	void FillGramophone(List<Texture> target)
	{
		Texture tex_ = null;
		int frame_ = 1;
		while ( true )
		{
			string name_ = string.Format("Gymmasters/gramophoneanimated/gramophone{0:D2}", frame_);

			tex_ = (Texture)Resources.Load(name_);
			
			if ( tex_ == null )
			{
				break;
			}
			
			target.Add(tex_);
			++frame_;
		}
	}

	public List<Texture> GetGrailTexture()
	{
		if ( _Gramophone == null )
		{
			CreateAtlas(ref _Gramophone, 1);
			FillGramophone(_Gramophone[0]);
		}

		return _Gramophone[0];
	}
	
	public List<Texture> GetLegsTexture(int angle)
	{
		if ( angle > 270 )
			return _GymLegList[5];
		else if ( angle > 210 )
			return _GymLegList[0];
		else if ( angle > 150 )
			return _GymLegList[1];
		else if ( angle > 90 )
			return _GymLegList[2];
		else if ( angle > 30 )
			return _GymLegList[3];
		else
			return _GymLegList[4];
	}

	public List<Texture> GetTexture(int angle)
	{
		if ( angle > 270 )
			return _GymList[5];
		else if ( angle > 210 )
			return _GymList[0];
		else if ( angle > 150 )
			return _GymList[1];
		else if ( angle > 90 )
			return _GymList[2];
		else if ( angle > 30 )
			return _GymList[3];
		else
			return _GymList[4];
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

	void FillLegList(List<Texture> target, int angle)
	{
		Texture tex_ = null;
		int frame_ = 1;
		while ( true )
		{
			string name_ = "Gymmasters/legs_Angle" + angle + "_frame" + frame_;
			
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
