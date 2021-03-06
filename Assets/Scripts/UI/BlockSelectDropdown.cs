﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BlockSelectDropdown : MonoBehaviour
{
    private List<GameObject> activeCubes = new List<GameObject>();
    private List<string> activeCubesNames = new List<string>();

    private Dropdown drop;
    private int currentSelection = 0;

    [SerializeField]
    private Slider startFreq;
    [SerializeField]
    private Slider endFreq;
    [SerializeField]
    private Slider pitchDecay;
    [SerializeField]
    private Slider ampDecay;
    [SerializeField]
    private Slider sinSqMix;
    [SerializeField]
    private Slider fmModSlider;
    [SerializeField]
    private Slider fmFreqSlider;
    [SerializeField]
    private Dropdown channelDropDown;
    [SerializeField]
    private Slider lowPassCutoffSlider;
    [SerializeField]
    private InputField lpCutoffInput;
    [SerializeField]
    private Slider lowPassResoSlider;
    [SerializeField]
    private InputField oscIDInput;

    private DrumVoiceParams linkedDrumParams = new DrumVoiceParams();

    bool firstTimeIn = true;
    
    // Use this for initialization
    void Start ()
    {
        drop = GetComponent<Dropdown>();

	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void UpdateStartFrequency(float frequency)
    {
        if (linkedDrumParams == null) return;
        frequency = Mathf.Clamp(frequency, 0.0f, 20000.0f);
        linkedDrumParams.StartFrequency = frequency;
    }

    public void UpdateEndFrequency(float frequency)
    {
        if (linkedDrumParams == null) return;
        frequency = Mathf.Clamp(frequency, 0.0f, 20000.0f);
        linkedDrumParams.EndFrequency = frequency;
    }

    public void UpdatePitchDecayRate(float rate)
    {
        if (linkedDrumParams == null) return;
        rate = Mathf.Clamp(rate, 0.0f, 1.0f);
        linkedDrumParams.PitchDecayRate = rate;
    }

    public void UpdateAmpDecayRate(float rate)
    {
        if (linkedDrumParams == null) return;
        rate = Mathf.Clamp(rate, 0.0f, 1.0f);
        linkedDrumParams.AmpDecayRate = rate;
    }

    public void UpdateModIndex(float index)
    {
        if (linkedDrumParams == null) return;
        index = Mathf.Clamp(index, 0f, 1000f);
        linkedDrumParams.FMModIndex = index;
    }

    public void UpdateFMFrequency(float frequency)
    {
        if (linkedDrumParams == null) return;
        frequency = Mathf.Clamp(frequency, 0f, 8000f);
        linkedDrumParams.FMFrequency = frequency;
    }

    public void UpdateSineSquareMix(float mix)
    {
        if (linkedDrumParams == null) return;
        mix = Mathf.Clamp(mix, 0f, 1f);
        linkedDrumParams.SinSqMix = mix;
    }

    public void UpdateLPFFreq(string strFreq)
    {
        float f = 0;
        float.TryParse(strFreq, out f);
        lowPassCutoffSlider.value = f;
        UpdateLPFFreq(f);

    }
    public void UpdateLPFFreq(float freq)
    {
        if (activeCubes.Count == 0) return;
        freq = Mathf.Clamp(freq, 20f, 22000f);
        activeCubes[currentSelection].GetComponent<StepBlock>().GetVoice().SetLPFCutoff(freq);
    }
    public void UpdateLPFRes(float res)
    {
        if (activeCubes.Count == 0) return;
        res = Mathf.Clamp(res, 1f, 10f);
        activeCubes[currentSelection].GetComponent<StepBlock>().GetVoice().SetLPFRes(res);
    }

    public void UpdateOSCID(string id)
    {
        if (activeCubes.Count == 0) return;
        activeCubes[currentSelection].GetComponent<StepBlock>().UpdateOSCID(id);
    }

    private void updateToSliders()
    {
        startFreq.value = linkedDrumParams.StartFrequency;
        endFreq.value = linkedDrumParams.EndFrequency;
        pitchDecay.value = linkedDrumParams.PitchDecayRate;
        ampDecay.value = linkedDrumParams.AmpDecayRate;
        fmModSlider.value = linkedDrumParams.FMModIndex;
        fmFreqSlider.value = linkedDrumParams.FMFrequency;
        sinSqMix.value = linkedDrumParams.SinSqMix;

    }

    public void SetMode(int mode)
    {
        switch ((StepBlock.Mode)mode)
        {
            case StepBlock.Mode.Sample:
                setAudioSlidersState(false);
                break;
            case StepBlock.Mode.Voice:
                setAudioSlidersState(true);
                break;
            case StepBlock.Mode.Control:
                setAudioSlidersState(false);

                break;

        }

        oscIDInput.interactable = (StepBlock.Mode)mode == StepBlock.Mode.Control;
    }

    void setAudioSlidersState(bool state)
    {
        startFreq.interactable = state;
        endFreq.interactable = state;
        pitchDecay.interactable = state;
        ampDecay.interactable = state;
        fmModSlider.interactable = state;
        fmFreqSlider.interactable = state;
        sinSqMix.interactable = state;
        channelDropDown.interactable = state;
        lowPassCutoffSlider.interactable = state;
        lowPassResoSlider.interactable = state;
        lpCutoffInput.interactable = state;
    }

    // Called when a step is spawned
    // Adds to dropdown
    public void AddStep(GameObject obj)
    {
        activeCubes.Add(obj);
        activeCubesNames.Add(obj.name);
        
        if (firstTimeIn)
        {
            currentSelection = 0;
            ValueChanged(0);
            firstTimeIn = false;
        }
        refreshDropdown();
    }

    // called when step is deleted
    public void RemoveLastStep()
    {
        // Safely empty list if about to delete final remaining cube
        if (activeCubes.Count <= 1)
        {
            activeCubes.Clear();
            activeCubesNames.Clear();
            // Add "None" to list
            drop.ClearOptions();
            List<string> str = new List<string> { "None" };
            drop.AddOptions(str);
            return;
        }

        activeCubes.RemoveAt(activeCubes.Count - 1);
        activeCubesNames.RemoveAt(activeCubes.Count - 1);
        refreshDropdown();
    }

    public void RemoveStep(GameObject step)
    {
        // Safely empty list if about to delete final remaining cube
        if (activeCubes.Count <= 1)
        {
            activeCubes.Clear();
            activeCubesNames.Clear();
            // Add "None" to list
            drop.ClearOptions();
            List<string> str = new List<string> { "None" };
            drop.AddOptions(str);
            return;
        }

        if (activeCubes.Contains(step))
        {
            activeCubes.Remove(step);
            activeCubesNames.Remove(step.name);
            refreshDropdown();
        }
    }
    
    public void ValueChanged(int value)
    {
        currentSelection = value;
        var block = activeCubes[value].GetComponent<StepBlock>();
        linkedDrumParams = block.GetVoice().GetParams();
        channelDropDown.value = block.getVoiceChannel();
        SetMode((int)block.PlaybackMode);

        float cut = 0f, res = 0f;
        block.GetVoice().GetLPFValues(out cut, out res);
        lowPassCutoffSlider.value = cut;
        lowPassResoSlider.value = res;
        lpCutoffInput.text = cut.ToString();
        oscIDInput.text = block.getOSCID();

        updateToSliders();
    }

    public void SelectionChanged(GameObject block)
    {
        if (activeCubes.Contains(block))
        {
            ValueChanged(activeCubes.IndexOf(block));
            drop.value = currentSelection;
        }
    }

    private void refreshDropdown()
    {
        drop.ClearOptions();
        drop.AddOptions(activeCubesNames);
    }

    public void UpdateSelectedChannel(int value)
    {
        if (activeCubes.Count > 0)
        {
            activeCubes[currentSelection].GetComponent<StepBlock>().setVoiceChannel(value);
            ValueChanged(currentSelection);
        }
    }
}
