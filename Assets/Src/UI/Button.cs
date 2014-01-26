using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour 
{
	public GameObject _Target;
	public string _Message;

	public Color _SelectedColor = new Color(0.3f,0.3f,0.3f,1);
	public Color _StandardColor = new Color(0.5f,0.5f,0.5f,1);

	public void OnTouchDown()
	{
		renderer.material.SetColor("_TintColor", _SelectedColor);
	}

	public void OnTouchUp()
	{
		renderer.material.SetColor("_TintColor", _StandardColor);
		if ( _Target != null && _Message != null )
		{
			_Target.SendMessage(_Message, SendMessageOptions.RequireReceiver);
		}
	}

	public void OnTouchCancel()
	{
		renderer.material.SetColor("_TintColor", _StandardColor);
	}

}
