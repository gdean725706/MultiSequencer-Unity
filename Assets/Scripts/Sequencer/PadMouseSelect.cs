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

    private void OnMouseDown()
    {
        if (seqPad == null)
        {
            seqPad = GetComponentInParent<SequencerPad>();
        }

        if (seqPad != null)
        {
            seqPad.SpawnBlock();
        }

    }
    
}
