using System;
using System.Collections.Generic;

[Serializable]
public class SurveyData
{
	public List<SurveyDataRun> runs;
	public char testGroup;

	public SurveyData()
	{
		runs = new List<SurveyDataRun>();
		testGroup = '-';
	}
	public SurveyData(SurveyData data)
	{
		runs = new List<SurveyDataRun>();
		testGroup = data != null ? data.testGroup : '-';

		if (data == null)
		{
			foreach (var i in data.runs)
				runs.Add(i);
		}
	}
}

[Serializable]
public class SurveyDataRun
{
	public PlayerProfileData initialProfile;
	public PlayerProfileData finalProfile;
	//public GameStatistic gameStats;
	public PlayerProfileData question0;
	public int question1;
	public int question2;
	public int question3;
}
