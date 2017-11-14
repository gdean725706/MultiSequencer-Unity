using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerStep : MonoBehaviour {

    [SerializeField]
    public bool Active = false;

    [SerializeField]
    public bool CurrentStep = false;

    [SerializeField]
    private float DefaultAmp = 0.99f;

    public int stepNumber = 0;

    private Renderer rend;
    private float amp = 0f;

    private Color colorOff = new Color(1f, 1f, 1f, 0.05f);
    private Color colorCurrentStep = new Color(0f,1f,0f, 0.2f);
    private Color colorActive = new Color(1f, 0f, 0f, 0.2f);

    public delegate void PlayStep(int stepNumber);
    public event PlayStep stepOccurred;

    // Use this for initialization
    void Start ()
    {

        rend = transform.GetChild(0).GetComponent<Renderer>();

        BPMTimer.beatOccurred += beatStep;
	}

    // Update is called once per frame
    void Update()
    {
        if (CurrentStep)
        {
            rend.material.SetColor("_Color", colorCurrentStep);
            amp = DefaultAmp;
        }
        else
        {
            Color col = Active ? colorActive : colorOff;
            amp *= amp;
            rend.material.SetColor("_Color", col);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Active = !Active;

            Color col = Active ? Color.red : Color.white;

            rend.material.SetColor("_Color", col);

            Debug.Log("Color change");

            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.CompareTag("Block"))
         //   Debug.Log(gameObject.name + " " + other.gameObject.name);
    }

    void beatStep(int beat)
    {
        if (Active && CurrentStep)
        {
            if (stepOccurred != null)
                stepOccurred(stepNumber);
        }
    }


}
