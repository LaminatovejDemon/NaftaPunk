using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour 
{
	public GameObject _Target;
	public string _Message;

	public void OnTouchDown()
	{
		renderer.material.color = Color.black;
	}

	public void OnTouchUp()
	{
		renderer.material.color = Color.white;
		if ( _Target != null && _Message != null )
		{
			_Target.SendMessage(_Message, SendMessageOptions.RequireReceiver);
		}
	}

	public void OnTouchCancel()
	{
		renderer.material.color = Color.white;
	}

}
