using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadMouseSelect : MonoBehaviour
{

    private SequencerPad seqPad;

    private void Awake()
    {
        seqPad = GetComponentInParent<SequencerPad>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        if (seqPad == null)
        {
            seqPad = GetComponentInParent<SequencerPad>();
        }

        if (seqPad != null)
        {
            Debug.Log("Clicked " + seqPad.name);
        }

    }
}
