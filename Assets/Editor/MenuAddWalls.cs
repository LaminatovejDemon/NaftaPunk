using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MenuAddWalls : EditorWindow 
{
	static CeilingTop _CeilingTop;
	static CeilingBottom _CeilingBottom;

	[@MenuItem("NaftaPunk/Add Walls %&s")]
	static void AddWalls()
	{
		Level levelRoot = FindObjectOfType(typeof(Level)) as Level;
		_CeilingTop = FindObjectOfType(typeof(CeilingTop)) as CeilingTop;
		_CeilingBottom = FindObjectOfType(typeof(CeilingBottom)) as CeilingBottom;

		for ( int i = 0; i < levelRoot.transform.childCount; ++i )
		{
			TryAddCeiling(levelRoot.transform.GetChild(i), 60);
			TryAddCeiling(levelRoot.transform.GetChild(i), 300);
			TryAddCeiling(levelRoot.transform.GetChild(i), 0);
		}

		for ( int i = 0; i < levelRoot.transform.childCount; ++i )
		{
			TryAddCeilingBottom(levelRoot.transform.GetChild(i), 120);
			TryAddCeilingBottom(levelRoot.transform.GetChild(i), 180);
			TryAddCeilingBottom(levelRoot.transform.GetChild(i), 240);
		}
	}

	static void TryAddCeilingBottom(Transform source, int angle)
	{
		Vector3 pos_ = GetPosition(source, angle);
		if ( !GetTileBelow(pos_) )
		{
			AddCeilingBottom(pos_);
		}
	}

	static void TryAddCeiling(Transform source, int angle)
	{
		Vector3 pos_ = GetPosition(source, angle);
		if ( !GetTileBelow(pos_) )
		{
			AddCeiling(pos_);
		}
	}

	static void AddCeiling(Vector3 position)
	{
		GameObject new_ = (GameObject)GameObject.Instantiate(_CeilingTop.CeilingPrefab);
		new_.transform.parent = _CeilingTop.transform;
		new_.transform.position = position + Vector3.up * 0.5f;
		//new_.renderer.material.mainTextureOffset = new Vector2(0.25f, 0.75f);
		new_.name = "CeilingTile";
	}

	static void AddCeilingBottom(Vector3 position)
	{
		GameObject new_ = (GameObject)GameObject.Instantiate(_CeilingBottom.CeilingPrefab);
		new_.transform.parent = _CeilingBottom.transform;
		new_.transform.position = position + Vector3.up * 0.5f;
		//new_.renderer.material.mainTextureOffset = new Vector2(0, 0.75f);
		new_.name = "CeilingBottomTile";
	}

	static Vector3 GetPosition(Transform source, int angle)
	{
		return source.position + Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * (Utils.c_HexRadius * Mathf.Sqrt(3));
	}

	static Ray _Ray;

	static GameObject GetTileBelow(Vector3 position)
	{
		_Ray = new Ray(position + Vector3.up * 2.0f, Vector3.down);

		RaycastHit rayHit_;
		
		if ( Physics.Raycast(_Ray, out rayHit_, 3, 1<<LayerMask.NameToLayer("Floor") | 1<<LayerMask.NameToLayer("Ceiling")) )
		{
			return rayHit_.collider.gameObject;
		}
		
		return null;
	}
}
