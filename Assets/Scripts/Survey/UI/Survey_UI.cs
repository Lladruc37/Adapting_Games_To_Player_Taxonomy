using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Survey_UI : MonoBehaviour
{
	public Image background;
	public Canvas canvas;
	public Slider answerSlider;
	public Text sliderText;
	public Text question;
	public PlayerProfileSurvey_UI playerProfileQuestion;
	public Button submitButton;
	public GameObject endOfSurvey;

	public string profileQuestion;
	public string question1;
	public string question2;
	public string question3;

	public int questionNumber;

	private void OnEnable()
	{
		background.color = new Color(0, 0, 0, 1f);
		canvas.enabled = true;
	}

	private void OnDisable()
	{
		background.color = new Color(0, 0, 0, 0);
		canvas.enabled = false;
	}

	public void OpenSurvey()
	{
		questionNumber = SurveyController.Instance.profileQuestion ? 0 : 1;
		SetUpQuestion();
	}

	public void OnSliderChanged()
	{
		sliderText.text = System.Convert.ToInt32(answerSlider.value).ToString();
	}

	public void SetUpQuestion()
	{
		answerSlider.gameObject.SetActive(questionNumber != 0);
		sliderText.gameObject.SetActive(questionNumber != 0);
		playerProfileQuestion.gameObject.SetActive(questionNumber == 0);
		playerProfileQuestion.OnSliderChanged();
		var transform = (RectTransform)question.transform;
		transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, questionNumber == 0 ? 880f : 540f);

		question.text = questionNumber switch
		{
			0 => profileQuestion,
			1 => question1,
			2 => question2,
			3 => question3,
			_ => "",
		};

		answerSlider.value = 5;
		OnSliderChanged();
	}

	public void SubmitAnswer()
	{
		var data = SurveyController.Instance.surveyData.runs.Last();

		switch (questionNumber)
		{
			case 0:
				data.question0 = new PlayerProfileData(playerProfileQuestion.profileDataAnswer);
				break;
			case 1:
				data.question1 = System.Convert.ToInt32(answerSlider.value);
				break;
			case 2:
				data.question2 = System.Convert.ToInt32(answerSlider.value);
				break;
			case 3: //survey over
				data.question3 = System.Convert.ToInt32(answerSlider.value);
				data.finalProfile = new PlayerProfileData(PluginController.Instance.PlayerProfileData);
				DataPersistenceManager.Instance.SaveSurveyRun(data);
				//TODO: save game stats here

				if (SurveyController.Instance.RunNumber == 2)
				{
					endOfSurvey.SetActive(true);
					answerSlider.gameObject.SetActive(false);
					sliderText.gameObject.SetActive(false);
					question.gameObject.SetActive(false);
					playerProfileQuestion.gameObject.SetActive(false);
					submitButton.gameObject.SetActive(false);
					return;
				}

				Debug.Log("Setting up next run...");
				this.gameObject.SetActive(false);
				SurveyController.Instance.SetupNextRun();
				//TODO: Restart game here
				break;
			default:
				break;
		}

		questionNumber++;
		SetUpQuestion();
	}

	public void Quit()
	{
		Application.Quit();
	}
}
