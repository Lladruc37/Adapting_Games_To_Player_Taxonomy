using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileSurvey_UI : MonoBehaviour
{
	public Slider answerSlider1;
	public Text sliderText1;
	public Slider answerSlider2;
	public Text sliderText2;
	public Slider answerSlider3;
	public Text sliderText3;
	public Slider answerSlider4;
	public Text sliderText4;
	public Slider answerSlider5;
	public Text sliderText5;
	public Slider answerSlider6;
	public Text sliderText6;

	public PlayerProfileData profileDataAnswer;

	public void OnSliderChanged()
	{
		sliderText1.text = $"Achiever: {System.Convert.ToInt32(answerSlider1.value)}";
		sliderText2.text = $"Free Spirit: {System.Convert.ToInt32(answerSlider2.value)}";
		sliderText3.text = $"Philanthropist: {System.Convert.ToInt32(answerSlider3.value)}";
		sliderText4.text = $"Player: {System.Convert.ToInt32(answerSlider4.value)}";
		sliderText5.text = $"Socialiser: {System.Convert.ToInt32(answerSlider5.value)}";
		sliderText6.text = $"Disruptor: {System.Convert.ToInt32(answerSlider6.value)}";

		profileDataAnswer = new PlayerProfileData();
		profileDataAnswer.Profile[PlayerTypes.ACHIEVER] = System.Convert.ToInt32(answerSlider1.value);
		profileDataAnswer.Profile[PlayerTypes.FREE_SPIRIT] = System.Convert.ToInt32(answerSlider2.value);
		profileDataAnswer.Profile[PlayerTypes.PHILANTHROPIST] = System.Convert.ToInt32(answerSlider3.value);
		profileDataAnswer.Profile[PlayerTypes.PLAYER] = System.Convert.ToInt32(answerSlider4.value);
		profileDataAnswer.Profile[PlayerTypes.SOCIALISER] = System.Convert.ToInt32(answerSlider5.value);
		profileDataAnswer.Profile[PlayerTypes.DISRUPTOR] = System.Convert.ToInt32(answerSlider6.value);
	}
}
