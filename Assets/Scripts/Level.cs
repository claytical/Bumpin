﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour {
	public GameObject LevelCompletePanel;
	public GameObject LevelFailPanel;
	public GameObject WorldCompletePanel;
	public Text nextWorldName;
	private string selectedScene;
	private AsyncOperation AO;
	private GameState gameState;
	private int nextLevel;
	private string currentWorld;

	// Use this for initialization
	void Start () {
		gameState = (GameState)FindObjectOfType (typeof(GameState));	
		if (gameState != null) {
			currentWorld = gameState.SelectedWorld;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void ScanForCompletion() {

		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Disappearing");
		if (gos.Length > 0) {
			Debug.Log ("Still bumpable objects");

		} else {
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			//set stars for current level
			if (gameState != null) {
				PlayerPrefs.SetInt (gameState.SelectedWorld + "_" + gameState.SelectedLevel, player.GetComponent<PlayerControl> ().balls + 1);
				if (int.Parse (gameState.SelectedLevel) >= 10) {
					//unlock next world
					//WORLD COMPLETE UI
					gameState.SelectedWorld = nextWorldName.text;
					nextLevel = 1;
					PlayerPrefs.SetInt (gameState.SelectedWorld, 1);
					WorldCompletePanel.SetActive (true);
				} else {
					//unlock next level
					nextLevel = int.Parse (gameState.SelectedLevel) + 1;
					PlayerPrefs.SetInt (gameState.SelectedWorld + "_" + nextLevel, 0);
					LevelCompletePanel.SetActive (true);
					if (LevelCompletePanel.GetComponent<LevelComplete> () != null) {
						LevelCompletePanel.GetComponent<LevelComplete> ().SetStars (player.GetComponent<PlayerControl> ().balls + 1);
					}
					BallHolder ballHolder = (BallHolder)FindObjectOfType(typeof(BallHolder));
					Ball[] balls = ballHolder.GetComponentsInChildren<Ball> ();
					for (int i = 0; i < balls.Length; i++) {
						Destroy(balls [i].gameObject);
					}
				}
			} else {
			//Debugging level
				BallHolder ballHolder = (BallHolder)FindObjectOfType(typeof(BallHolder));
				Ball[] balls = ballHolder.GetComponentsInChildren<Ball> ();
				for (int i = 0; i < balls.Length; i++) {
					Destroy(balls [i].gameObject);
				}
				LevelCompletePanel.SetActive(true);
				LevelCompletePanel.GetComponent<LevelComplete> ().SetStars (2);
			}
		}
	}
	public void NextLevel() {
		if (gameState != null) {
			gameState.SelectedLevel = nextLevel.ToString ();
			selectedScene = gameState.SelectedWorld + "_" + nextLevel;
			SelectScene ();
		}
	}

	public void RetryLevel() {
		if (gameState) {
			selectedScene = currentWorld + "_" + gameState.SelectedLevel;
			SelectScene ();
		} else {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}

	public void SetSceneToLoad(string s) {
		selectedScene = s;
	}

	public void SelectScene() {
		StartCoroutine("loadScene");
	}

	IEnumerator loadScene() {
		AO = SceneManager.LoadSceneAsync (selectedScene, LoadSceneMode.Single);
		AO.allowSceneActivation = false;
		while (AO.progress < 0.9f) {
			yield return null;
		}
		AO.allowSceneActivation = true;
	}

}
