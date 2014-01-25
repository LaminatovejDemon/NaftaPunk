using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MenuAddWalls : EditorWindow 
{
	static CeilingTop _CeilingTop;
	static CeilingBottom _CeilingBottom;
	static CeilingRight _CeilingRight;

	[@MenuItem("NaftaPunk/Add Walls %&s")]
	static void AddWalls()
	{
		Level levelRoot = FindObjectOfType(typeof(Level)) as Level;
		_CeilingTop = FindObjectOfType(typeof(CeilingTop)) as CeilingTop;
		_CeilingBottom = FindObjectOfType(typeof(CeilingBottom)) as CeilingBottom;
		_CeilingRight = FindObjectOfType(typeof(CeilingRight)) as CeilingRight;

		for ( int i = 0; i < levelRoot.transform.childCount; ++i )
		{
			TryAddCeiling(levelRoot.transform.GetChild(i), 300, _CeilingTop.CeilingPrefab, _CeilingTop.transform);
			TryAddCeiling(levelRoot.transform.GetChild(i), 0, _CeilingTop.CeilingPrefab, _CeilingTop.transform);
		}

		for ( int i = 0; i < levelRoot.transform.childCount; ++i )
		{
			TryAddCeiling(levelRoot.transform.GetChild(i), 60, _CeilingRight.CeilingPrefab, _CeilingRight.transform);
		}

		for ( int i = 0; i < levelRoot.transform.childCount; ++i )
		{
			TryAddCeiling(levelRoot.transform.GetChild(i), 120, _CeilingBottom.CeilingPrefab, _CeilingBottom.transform);
			TryAddCeiling(levelRoot.transform.GetChild(i), 180, _CeilingBottom.CeilingPrefab, _CeilingBottom.transform);
			TryAddCeiling(levelRoot.transform.GetChild(i), 240, _CeilingBottom.CeilingPrefab, _CeilingBottom.transform);
		}
	}

	static void TryAddCeiling(Transform source, int angle, GameObject ceilingTemplate, Transform parent)
	{
		Vector3 pos_ = GetPosition(source, angle);
		if ( !GetTileBelow(pos_) )
		{
			AddCeiling(pos_, ceilingTemplate, parent);
		}
	}

	static void AddCeiling(Vector3 position, GameObject template, Transform parent)
	{
		GameObject new_ = (GameObject)GameObject.Instantiate(template);
		new_.transform.parent = parent;
		new_.transform.position = position + Vector3.up * 0.5f;
		//new_.renderer.material.mainTextureOffset = new Vector2(0, 0.75f);
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
