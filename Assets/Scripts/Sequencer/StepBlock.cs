using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class StepBlock : MonoBehaviour
{
    [SerializeField]
    bool play = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pads"))
        {
            Debug.Log("Adding " + other.gameObject.name + "to " + gameObject.name);
            var obj = other.gameObject.GetComponent<SequencerStep>();
            obj.stepOccurred += playStepSound;
            obj.Active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pads"))
        {
            Debug.Log("Removing " + other.gameObject.name + "to " + gameObject.name);
            var obj = other.gameObject.GetComponent<SequencerStep>();
            obj.stepOccurred -= playStepSound;
            obj.Active = false;
        }
    }

    void playStepSound(int stepNumber)
    {
        Debug.Log("Playing step sound for" + stepNumber);
    }

}
