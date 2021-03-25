using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerScore : MonoBehaviour
{
    private Settings settings { get { if (Settings.instance != null) return Settings.instance; else return FindObjectOfType<Settings>(); } }
    public int points;

	[Header("References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI counter;
	public Animator scoreAnimator;
	[HideInInspector] public PlayerInstance thisPlayer;
	[HideInInspector] public PlayerInstance otherPlayer;

	Coroutine countdownRoutine;

	public System.Action<float> OnFellDown;
	public System.Action OnGotUp;
	public System.Action OnTimerUp;
	public System.Action<float> OnLeftBounds;

	private bool isDown;
	private bool IsDown { get { return isDown; } set { thisPlayer.activeAvatar.isDown = value;	isDown = value; } }
	private bool isOut;
	private bool IsOut { get { return isOut; } set { thisPlayer.activeAvatar.isOut = value; isOut = value; } }

	private float remainingCountDownTime;
	private float timeSicneLastContactAtFallTime;


	public void Initialize()
	{
		remainingCountDownTime = settings.coundDownTime;

		//prep event invoking relating to colliders
		foreach (CollisionHandler handler in thisPlayer.activeAvatar.wristTriggers)
		{
			handler.OnTouchedGround += () =>
			{
				if (!IsDown)
				{
					OnFellDown?.Invoke(handler.thisPlayer.timeSinceLastContact);
				}
			};

			handler.OnTouchedWristHeight += () =>
			{
				if (IsDown) 
				{
					OnGotUp?.Invoke();
				}
			};
		}

		foreach (CollisionHandler handler in thisPlayer.activeAvatar.childHandlers)
		{ 
			handler.OnLeftBounds += () =>
			{
				if (!IsOut && !otherPlayer.activeAvatar.isOut)
				{					
					OnLeftBounds?.Invoke(handler.thisPlayer.timeSinceLastContact);
				}
			};

			handler.OnFalling += () =>
			{
				thisPlayer.activeAvatar.SoftenFingers();
				thisPlayer.inputController.SofterTorso();
			};
		}

		//wire events
		OnFellDown += (timeSinceContact) =>
		{
			IsDown = true;
			StartCountdown();
			SetContactTime(timeSinceContact);
		};

		OnGotUp += () =>
		{
			IsDown = false;
			StopCountdown();
		};

		OnLeftBounds += (timeSinceContact) =>
		{
			Debug.Log("On Left bounds");
			IsOut = true;

			SetContactTime(timeSinceContact);

			StartCoroutine(CallDelayed(() => {
				OnCountersUp();
			}, 1f));

		};


		


	}


	private IEnumerator CountDown(float timeToCount)
	{
		timeToCount = Mathf.Ceil(timeToCount); // round the time up
		counter.text = timeToCount.ToString();

		int nextValue = (int)timeToCount - 1;
		// manage changing of the numbers
		// yielding each frame instead of wait for seconds so that I can stop the coroutine at any moment
		while (timeToCount > 0)
		{
			timeToCount -= Time.deltaTime;

			yield return null;

			if (timeToCount <= nextValue && nextValue != 0)
			{
				ChangeCounterValue(nextValue);
				nextValue--;
			}

			else if (timeToCount <= nextValue && nextValue == 0)
			{
				OnCountersUp();
			}
		}
	}

	private void StartCountdown()
	{
		countdownRoutine = StartCoroutine(CountDown(remainingCountDownTime));
		ShowCounter();
	}

	private void StopCountdown()
	{	
		if (countdownRoutine != null)
		{
			StopCoroutine(countdownRoutine);
			HideCounter();
		}
	}

	private void ShowCounter() 
	{
		counter.gameObject.SetActive(true);
	}

	private void HideCounter() 
	{
		counter.gameObject.SetActive(false);
	}

	private void ChangeCounterValue(int value) 
	{
		counter.text = value.ToString();
	}

	private void OnCountersUp()
	{
		HideCounter();

		//fell through other player
		if (timeSicneLastContactAtFallTime < Settings.instance.otherPlayerInfluenceTime)
		{
			otherPlayer.score.AddPoints(3);
		}
		//fell because your stupid
		else
		{
			otherPlayer.score.AddPoints(1);
		}

		otherPlayer.score.UpdateScoreValue();

		//reset both players
		ResetState();
		otherPlayer.score.ResetState();
	}

	public void AddPoints(int value) 
	{
		points += value;
	}

	public void UpdateScoreValue() 
	{
		scoreText.text = points.ToString();
		scoreAnimator.Play("ScoreIncrease");
	}

	private void SetContactTime(float value) 
	{
		timeSicneLastContactAtFallTime = value;
	}

	public void ResetState() 
	{
		StopCountdown();
		thisPlayer.inputController.ResetPosition();
		IsDown = false;
		IsOut = false;
	}
	private IEnumerator CallDelayed(System.Action method, float delay) 
	{
		Debug.Log("starting fall routine");
		yield return new WaitForSeconds(delay);
		method.Invoke();

	} 
}
