using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LevelManager : MonoBehaviour
{
	[Header("Config")]
	public float otherPlayerInfluenceTime = 5f;

	[Header("Player 1")]
	public PlayerInputController playerLEFT;
	public int playerLEFTScore = 0;
	public TextMeshProUGUI playerLEFTScorePanel;

	[Header("Player 2")]
	public PlayerInputController playerRIGHT;
	public int playerRIGHTScore = 0;
	public TextMeshProUGUI playerRIGHTScorePanel;

	[Header("Other")]
	public TextMeshProUGUI timeCounter;
	public bool immunity;

    // Start is called before the first frame update
    void Start()
    {
		foreach (CollisionHandler handler in playerLEFT.wristColliders)
		{
			handler.OnTouchedGround += () =>
			{
				if (!immunity)
				{
					if (playerLEFT.timeSinceLastContact < otherPlayerInfluenceTime)
					{
						playerRIGHTScore += 3;
					}
					else
					{
						playerRIGHTScore += 1;
					}
					playerRIGHTScorePanel.text = playerRIGHTScore.ToString();
					StartCoroutine(Respawm(3));
				}
			};

			handler.OnLeftBounds += () =>
			{
				if (!immunity)
				{
					if (playerLEFT.timeSinceLastContact < otherPlayerInfluenceTime)
					{
						playerRIGHTScore += 3;
					}
					else
					{
						playerRIGHTScore += 1;
					}
					playerRIGHTScorePanel.text = playerRIGHTScore.ToString();
					StartCoroutine(Respawm(3));
				}
			};
		}

		foreach (CollisionHandler handler in playerRIGHT.wristColliders)
		{
			handler.OnTouchedGround += () =>
			{
				Debug.Log("time since last contact:" + playerRIGHT.timeSinceLastContact);

				if (!immunity)
				{
					if (playerRIGHT.timeSinceLastContact < otherPlayerInfluenceTime)
					{
						Debug.Log("Adding 3 points" + playerRIGHT.timeSinceLastContact);
						playerLEFTScore += 3;
					}
					else
					{

						playerLEFTScore += 1;
					}

					playerLEFTScorePanel.text = playerLEFTScore.ToString();
					StartCoroutine(Respawm(3));
				}
			};

			handler.OnLeftBounds += () =>
			{
				Debug.Log("time since last contact:" + playerRIGHT.timeSinceLastContact);

				if (!immunity)
				{
					if (playerRIGHT.timeSinceLastContact < otherPlayerInfluenceTime)
					{
						playerLEFTScore += 3;
					}
					else
					{

						playerLEFTScore += 1;
					}

					playerLEFTScorePanel.text = playerLEFTScore.ToString();
					StartCoroutine(Respawm(3));
				}
			};
		}
	}

	IEnumerator Respawm(int seconds)
	{
		immunity = true;

		timeCounter.gameObject.SetActive(true);
		timeCounter.text = seconds.ToString();

		while (seconds > 0)
		{
			yield return new WaitForSeconds(1f);
			seconds--;
			timeCounter.text = seconds.ToString();
		}

		immunity = false;
		timeCounter.gameObject.SetActive(false);
		playerLEFT.Reset();
		playerRIGHT.Reset();
	}

}
