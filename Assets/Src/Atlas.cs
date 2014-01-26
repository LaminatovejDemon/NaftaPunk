using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Atlas : MonoBehaviour 
{
	public List<Texture>[] _GymList;
	public List<Texture>[] _GeoList;

	public List<Texture>[] _GymLegList;
	public List<Texture>[] _Gramophone;
	public List<Texture>[] _XPAnimated;

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
		CreateAtlas(ref _GeoList, 6);
		CreateAtlas(ref _GymLegList, 6);




		FillList(_GymList[0], 0, "Gymmasters/gymmaster_Left_");
		FillList(_GymList[1], 60, "Gymmasters/gymmaster_Left_");
		FillList(_GymList[2], 120, "Gymmasters/gymmaster_Left_");
		FillList(_GymList[3], 180, "Gymmasters/gymmaster_Left_");
		FillList(_GymList[4], 240, "Gymmasters/gymmaster_Left_");
		FillList(_GymList[5], 300, "Gymmasters/gymmaster_Left_");

		FillList(_GeoList[0], 0, "AntiGyms/antigymmaster_");
		FillList(_GeoList[1], 60, "AntiGyms/antigymmaster_");
		FillList(_GeoList[2], 120, "AntiGyms/antigymmaster_");
		FillList(_GeoList[3], 180, "AntiGyms/antigymmaster_");
		FillList(_GeoList[4], 240, "AntiGyms/antigymmaster_");
		FillList(_GeoList[5], 300, "AntiGyms/antigymmaster_");

		FillLegList(_GymLegList[0], 0);
		FillLegList(_GymLegList[1], 60);
		FillLegList(_GymLegList[2], 120);
		FillLegList(_GymLegList[3], 180);
		FillLegList(_GymLegList[4], 240);
		FillLegList(_GymLegList[5], 300);
	}

	void FillGramophone(List<Texture> target, string path)
	{
		Texture tex_ = null;
		int frame_ = 1;
		while ( true )
		{
			string name_ = string.Format("{0}{1:D2}",path,frame_);

			Debug.Log (name_);


			tex_ = (Texture)Resources.Load(name_);
			
			if ( tex_ == null )
			{
				break;
			}
			
			target.Add(tex_);
			++frame_;
		}
	}

	public List<Texture> GetXPTexture()
	{
		if ( _XPAnimated == null )
		{
			CreateAtlas(ref _XPAnimated, 1);
			FillGramophone(_XPAnimated[0], "xpanimated/xpanimated");
		}
		
		return _XPAnimated[0];
	}

	public List<Texture> GetGrailTexture()
	{
		if ( _Gramophone == null )
		{
			CreateAtlas(ref _Gramophone, 1);
			FillGramophone(_Gramophone[0], "Gymmasters/gramophoneanimated/gramophone");
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

	public List<Texture> GetTexture(int angle, GameStateManager.EFractionType type)
	{
		List<Texture> [] targetList_ = (type == GameStateManager.EFractionType.Gyms ? _GymList : _GeoList);

		if ( angle > 270 )
			return targetList_[5];
		else if ( angle > 210 )
			return targetList_[0];
		else if ( angle > 150 )
			return targetList_[1];
		else if ( angle > 90 )
			return targetList_[2];
		else if ( angle > 30 )
			return targetList_[3];
		else
			return targetList_[4];
	}

	void FillList(List<Texture> target, int angle, string pathBegin)
	{
		Texture tex_ = null;
		int frame_ = 1;
		while ( true )
		{
			string name_ = pathBegin + "Angle" + angle + "_frame" + frame_;
	
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
