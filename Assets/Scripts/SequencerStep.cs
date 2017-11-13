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

    public int stepNumber = 0;

    private Renderer rend;
    private float amp = 0f;

    private Color colorOff = new Color(1f, 1f, 1f, 0.05f);
    private Color colorCurrentStep = new Color(0f,1f,0f, 0.2f);
    private Color colorActive = new Color(1f, 0f, 0f, 0.2f);

    // Use this for initialization
    void Start ()
    {

        rend = transform.GetChild(0).GetComponent<Renderer>();	
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
            Color col = active ? colorActive : colorOff;
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
    
    
}
