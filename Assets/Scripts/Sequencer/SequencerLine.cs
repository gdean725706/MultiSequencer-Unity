using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SequencerLine : MonoBehaviour {

    public int NumberOfSteps = 16;
    public Vector3 Spacing = new Vector3 (2, 0, 0);

    public bool Regenerate = false;

    public AudioMixerGroup mixer;
    public AudioClip silence;

    private GameObject[] Steps;
    private BPMTimer Clock;

    public enum Instrument
    {
        Kick,
        Hat,
        Snare,
        Cymbal
    };

    public Instrument DrumType;

    private void Awake()
    {
        Steps = new GameObject[NumberOfSteps];
    }

    // Use this for initialization
    void Start ()
    {
        Clock = GameObject.Find("Timer").GetComponent<BPMTimer>();
        GenerateSteps(NumberOfSteps);

	}
	
	// Update is called once per frame
	void Update () {
		
        if (Regenerate)
        {
            foreach (var step in Steps)
            {
                Destroy(step);
            }

            GenerateSteps(NumberOfSteps);

            Regenerate = false;
        }

        for (int i = 0; i < NumberOfSteps; ++i)
        {
            Steps[i].GetComponent<SequencerPad>().CurrentStep = (Clock.GetBeat() % NumberOfSteps == i);
        }
	}

    void GenerateSteps(int number)
    {
        GameObject[] steps = new GameObject[number];
        for (int i = 0; i < steps.Length; i++)
        {
            steps[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            steps[i].transform.parent = transform;
            steps[i].transform.localPosition = Vector3.zero;
            steps[i].transform.localPosition = new Vector3(i * Spacing.x, i*Spacing.y, i*Spacing.z);

            var rb = steps[i].AddComponent<Rigidbody>();
            rb.mass = 30;

            var audio = steps[i].AddComponent<AudioSource>();
            audio.outputAudioMixerGroup = mixer;
            audio.clip = silence;
            audio.spatialBlend = 1f;
            audio.loop = true;

            steps[i].AddComponent<SequencerPad>();
            Renderer rend = steps[i].GetComponent<Renderer>();
            rend.material = new Material(Shader.Find("Standard"));
        }

        Steps = steps;
    }
}
