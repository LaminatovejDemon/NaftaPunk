using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOverScreen : MonoBehaviour 
{	
	public TextMesh _DescriptionText;

	List <string> _FailTexts = new List<string>();

	public void Start()
	{
		string t1_ = "2044 A.D.\n\nWhat Nietzsche started,\n Oprah Winfrey and CBS finally finished. Relativization of truth\n is at its highpoint. People struggle to find one universal belief,\n a beacon of light that would unify the humanity. Incomprehension,\n uncertanity, permanent revolution... chaos rules the Earth.";
		string t2_ = "2172 A.D.\n\nThe day of the big breakdown: Ron Paul finally succeeds\n in his 162nd run for US president. He shuts down\n the Ministry of Education. Once famous Universities are\n collapsing. In a few years, one last hope of many\n - education is lost. Bands of unemployed teachers\n and scientists scavange the country, hoping to find food,\n water and possibly free sex (if they get really lucky). The end\n of the world as we know it is drawing near like never before.";
		string t3_ = "2179 A.D.\n\nIn the darkest hours of humankind a glimmer of light appears.\n Tom Cruise finally decrypts one of\n the original Mormon's secret text. The message follows:\n\n \"When the darkest hour comes, set on the voyage for\n Vinyl Holyphone. It speaks the truth and one truth only.\n This truth will unify the Earth in peace, once and for all...\"";
		string t4_ = "2180 A.D.\n\nNow bands of former teachers of physical education\n are forming special UTOPEA (unemployed teachers of physical\n education assembled) units in the quest for the Holyphone.\n They hope that finding the Holyphone first will enable them\n to transmit the truth according their personal beliefs\n to what is left of humanity.\n\n The life of all would turn into permanent\n game of baseball, basketball, icehockey and worst\nof all... soccer.\n\n\nBut in their quest, they are not alone...";

		_FailTexts.Add(t1_);
		_FailTexts.Add(t2_);
		_FailTexts.Add(t3_);
		_FailTexts.Add(t4_);

		_DescriptionText.text = _FailTexts[Random.Range(0, _FailTexts.Count)];
	}

	public void Restart()
	{
		gameObject.SetActive(false);
		Level.GetInstance ().Reset ();
	}
}
