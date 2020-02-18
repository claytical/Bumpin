﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BallHolder : MonoBehaviour {
//	public GameObject ball;
    public GameObject[] ships;
    public GameObject ship;
	public PlayerControl player;
    public GameObject textOverlay;
    public GameObject uiCanvas;
    public int maxEnergyPlaytime;
    private int seedsCollected;
    private bool maxEnergy;
    private float maxEnergyTime;
	private int score;
    public Grid grid;
    public GameObject touchPoint;
    private List<GameObject> touchPoints;
    private bool[] startedTouching;
    public int energy;
    public Text energyUI;
    private float force;
    private bool useTilt;
    // Use this for initialization

    void Start()
    {
        //		setBallDisplay ();
        energy = 0;
        seedsCollected = 0;
        maxEnergy = false;
        /*
        touchPoints = new List<GameObject>();

        GameState gameState = (GameState)FindObjectOfType(typeof(GameState));

        switch (gameState.ship)
        {
            case GameState.Ship.Boomerang:
                ship = Instantiate(ships[0], transform);
                break;
            case GameState.Ship.Rocket:
                ship = Instantiate(ships[1], transform);

                break;

        }
        
            
        force = ship.GetComponent<Ball>().force;
        */
        if (PlayerPrefs.GetInt("tilt") == 1)
        {
            useTilt = true;
        }
        else
        {
            useTilt = false;
        }

        /*
        inkJar.SetActive(player.level.inkEnabled);
        */
    }
        public int increaseEnergy(int amount)
    {
        //        bool speedUp = false;
        if (amount > 0)
        {
            int speedState = 0;
            if (energy < grid.energyRequiredForPowerUp)
            {
                energy+=amount;
                //update score
                score += score;
//                feverBar.increaseFever(amount);
                speedState = 1;
            }
            else if(!maxEnergy && energy >= grid.energyRequiredForPowerUp) {
                //EVENT #3 - FEVER REACHED
                speedState = 2;
            }
            return speedState;
        }
        return -1;
    }

    public void MaxEnergy()
    {
        AddSeed();
        player.level.CreatePowerUp();
        Debug.Log("MAX ENERGY!");
        grid.currentSet.BroadcastMessage("MaxEnergy", SendMessageOptions.DontRequireReceiver);
        maxEnergy = true;
        energy = 0;
//        maxEnergyTime = Time.time + maxEnergyPlaytime;

    }
    public bool HasFullEnergy()
    {
        return maxEnergy;
    }

    void CheckMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Making Mouse Touch Point");
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0;
            GameObject go = (GameObject)Instantiate(touchPoint, touchPosition, transform.rotation);
            go.transform.parent = transform;

            touchPoints.Add(go);
        }
        if (Input.GetMouseButtonUp(0))
        {
            GameObject tP = touchPoints[0];
            touchPoints.RemoveAt(0);
            Destroy(tP, 5);
        }

        if (Input.GetMouseButton(0))
        {

            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (ship)
            {
                Vector2 direction = (Vector2)touchPosition - (Vector2)ship.transform.position;
                direction.Normalize();

                ship.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
            }
            /*            touchPosition.z = ball.transform.position.z - Camera.main.transform.position.z;
                        touchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                        touchPosition.y = ball.transform.position.y;
                        */

            //            Vector2 dir = lastKnown - (Vector2)(player.transform.position);
        }
        else
        {
        }

    }
    void CheckControl()
    {
        if (Input.mousePresent)
        {
            CheckMouse();
        }
//        CheckTouches();

    }

    public void AddSeed()
    {
        seedsCollected++;
        energyUI.text = seedsCollected.ToString();
        energyUI.GetComponent<Animator>().SetTrigger("add");
        energyUI.GetComponent<AudioSource>().Play();

    }

    // Update is called once per frame
    void Update()
    {

        if (energy > 25)
        {
            MaxEnergy();
        }
        else
        {

        }

        if (!useTilt)
        {
//            CheckControl();
        }



	}

	public void DeadBall() {
        player.GameOver(seedsCollected);
    }

	public void Drop() {
        Time.timeScale = 1f;
//        button.GetComponent<Animator>().SetTrigger("pop");
        ship.GetComponent<Ball>().inPlay = true;
        grid.currentSet.Starting();

    }
    public void removePoints() {
		score = 0;
	}
	public void addPoints(int points) {
		score += points;
	}

	public void Reset() {
		score = 0;
	}


}
