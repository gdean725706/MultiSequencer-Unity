﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Attached to quad of sequencer pad to detect mouse clicks
// Quads are on separate layer above pad colliders to enable priority over mouse clicks
public class PadMouseSelect : MonoBehaviour
{

    private SequencerPad seqPad;

    bool spawned = false;
    

    private void Awake()
    {
        seqPad = GetComponentInParent<SequencerPad>();
    }



    private void OnMouseEnter()
    {
        if (seqPad == null)
        {
            seqPad = GetComponentInParent<SequencerPad>();
        }

        return;

    }

    private void OnMouseOver()
    {
        if (seqPad != null && !EventSystem.current.IsPointerOverGameObject())
        {
            if (GameOfLife.MousePaintMode)
            {
                if (Input.GetMouseButton(0))
                    seqPad.State = 1;
                else if (Input.GetMouseButton(1))
                    seqPad.State = 0;
            }

            if (BlockSpawnManager.BrushMode)
            {
                if (Input.GetMouseButton(0) && !spawned)
                {
                    seqPad.SpawnBlock();
                    spawned = true;
                }
            }
        }
    }

    private void OnMouseExit()
    {
        if (BlockSpawnManager.BrushMode)
            spawned = false;
    }

    private void OnMouseDown()
    {
        // Check mouse isn't over UI
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (seqPad == null)
            {
                seqPad = GetComponentInParent<SequencerPad>();
            }

            if (!BlockSpawnManager.BrushMode)
            {
                if (seqPad != null)
                {
                    if (!GameOfLife.MousePaintMode)
                        seqPad.SpawnBlock();
                }
            }
        }

    }
    
}
