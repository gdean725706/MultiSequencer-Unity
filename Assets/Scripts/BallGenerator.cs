using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallGenerator : MonoBehaviour
{

    public GameObject bouncyBallPrefab;
    public int numberOfBalls = 10;
    public Vector3 ImpactForce = new Vector3(-100f, 50f, 0f);
    private GameObject[] bouncyBalls;

    public float Power = 5f;

    public bool Refresh = false;
    public bool AddBall = false;

    public bool GenerateEnabled = true;

    public bool BeatOccurred = false;

    private int previousTick = -1;

    private BPMTimer Quantiser;

    private void Awake()
    {
        Quantiser = GameObject.Find("Timer").GetComponent<BPMTimer>();
    }
    // Use this for initialization
    void Start()
    {
        GenerateBalls(numberOfBalls);

    }

    // Update is called once per frame
    void Update()
    {

        if (Refresh)
        {
            DestroyAllBalls();
            Refresh = !Refresh;
        }
        if (GenerateEnabled)
        {
            if (previousTick != Quantiser.GetBar())
            {
                GenerateBalls(1);
            }
        }
        previousTick = Quantiser.GetBar();

        if (AddBall)
        {
            GenerateBalls(1);
            AddBall = !AddBall;
        }

        //Debug.Log(AudioSettings.dspTime + " next beat time = " + Quantiser.GetNextBeatTime());
    }

    private void FixedUpdate()
    {
        if (Refresh)
        {
            GenerateBalls(numberOfBalls);
        }
    }


    void DestroyAllBalls()
    {
        var balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (var ball in balls)
            Destroy(ball);
    }

    void GenerateBalls(int number)
    {
        GameObject[] balls = new GameObject[numberOfBalls];
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i] = Instantiate(bouncyBallPrefab, transform, false);
            //balls [i].transform.localPosition = new Vector3 (balls [i].transform.localPosition.x + 5, balls [i].transform.localPosition.y, balls [i].transform.localPosition.z);
            balls[i].GetComponent<Rigidbody>().AddForce(ImpactForce, ForceMode.Impulse);
        }
        //bouncyBalls = balls;
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 150, 100), "Bouncy Ball Menu");
        if (GUI.Button(new Rect(20, 40, 100, 20), "Refresh Balls"))
        {
            Refresh = true;
        }

        //string numBalls = GUI.TextField (new Rect (80, 60, 50, 50), "");

        if (GUI.Button(new Rect(20, 60, 100, 20), "Add More Balls"))
        {
            GenerateBalls(10);
        }

        if (GUI.Button(new Rect(20, 80, 100, 20), "Cleanup Balls"))
        {
            DestroyAllBalls();
        }
    }
}
    
