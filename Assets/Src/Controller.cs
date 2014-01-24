using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour 
{
	private static Controller _Instance = null;

	public static Controller GetInstance()
	{
		if ( _Instance == null )
		{
			_Instance = new GameObject().AddComponent<Controller>();
		}

		return _Instance;
	}

	public void OnTouchDown(GameObject source)
	{
		Debug.Log (source + " OnTouchDown");
	}

	public void OnTouchUp(GameObject source)
	{
		Debug.Log (source + " OnTouchUp");
	}

	public void OnTouchDown(Vector3 position)
	{

		RaycastHit rayHit_;
		Ray ray_ = new Ray(Camera.main.ScreenToWorldPoint(position) - Camera.main.transform.rotation * Vector3.back * 10.0f, Camera.main.transform.forward);

	//	if ( Physics.ray
	}

	public void OnTouchUp(Vector3 position)
	{

	}

	void Update () 
	{
		//if (Input.GetTouch

#if UNITY_STANDALONE || UNITY_STANDALONE_OSX
		if ( Input.GetMouseButtonDown(0) )
		{
			OnTouchDown(Input.mousePosition);
		}
		if ( Input.GetMouseButtonUp(0) )
		{
			OnTouchUp(Input.mousePosition);
		}
		
#endif
	}
}
