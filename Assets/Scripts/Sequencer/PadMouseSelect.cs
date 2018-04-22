using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to quad of sequencer pad to detect mouse clicks
// Quads are on separate layer above pad colliders to enable priority over mouse clicks
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
