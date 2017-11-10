using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerStep : MonoBehaviour {

    [SerializeField]
    private bool active = false;

    [SerializeField]
    public bool CurrentStep = false;

    [SerializeField]
    private float DefaultAmp = 0.99f;

    private Renderer rend;
    private float amp = 0f;
    private static ulong randomSeed;

    // Use this for initialization
    void Start ()
    {
        rend = GetComponent<Renderer>();	
	}

    // Update is called once per frame
    void Update()
    {
        if (CurrentStep)
        {
            rend.material.SetColor("_Color", Color.green);
            amp = DefaultAmp;
        }
        else
        {
            Color col = active ? Color.red : Color.white;
            amp *= amp;
            rend.material.SetColor("_Color", col);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            active = !active;

            Color col = active ? Color.red : Color.white;

            rend.material.SetColor("_Color", col);

            Debug.Log("Color change");

            collision.gameObject.SetActive(false);
        }
    }

    float GetRandom()
    {
        const float scale = 1.0f / (float)0x7FFFFFFF;
        randomSeed = randomSeed * 69069 + 1;
        return (((randomSeed >> 16) ^ randomSeed) & 0x7FFFFFFF) * scale;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        for (int j = 0; j < data.Length; j += channels)
        {
            float t = GetRandom() * 0.5f;

            for (int i = 0; i < channels; i++)
                data[j + i] += t * amp;
        }
    }
}
