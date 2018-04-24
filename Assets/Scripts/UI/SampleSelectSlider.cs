using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleSelectSlider : MonoBehaviour
{

    private Slider sliderObj;

    private List<GameObject> _spawnedSteps = new List<GameObject>();

    private int _currentSampleValue = 0;    
    private int _currentStep = 0;

    [SerializeField]
    private Text _textValue;
    [SerializeField]
    private Text _sampleNameText;

    [SerializeField]
    private Dropdown modeDropdown;

    // Use this for initialization
    void Start()
    {
        sliderObj = GetComponent<Slider>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Change currently selected cube's sample
    public void UpdateSample(float number)
    {

        if (_spawnedSteps.Count > 0)
        {
            _spawnedSteps[_currentStep].GetComponent<StepBlock>().UpdateSample((int)number);

            _sampleNameText.text = _spawnedSteps[_currentStep].GetComponent<StepBlock>().Samples[(int)number].name;
        }
    }
   
    // Called when dropdown is clicked
    // Updates slider value to currently selected step block
    public void UpdateCurrentStep(int currentStep)
    {
        _currentStep = Mathf.Clamp(currentStep, 0, _spawnedSteps.Count);
        _currentSampleValue = _spawnedSteps[_currentStep].GetComponent<StepBlock>().currentSample;
        _textValue.text = _currentSampleValue.ToString();
        sliderObj.value = _currentSampleValue;
        _sampleNameText.text = _spawnedSteps[_currentStep].GetComponent<StepBlock>().Samples[_currentSampleValue].name;


        if (modeDropdown != null)
        {
            modeDropdown.value = (int)_spawnedSteps[_currentStep].GetComponent<StepBlock>().GetMode();
        }
    }

    public void UpdateMode(int mode)
    {
        if (_spawnedSteps.Count > 0)
        {
            _spawnedSteps[_currentStep].GetComponent<StepBlock>().UpdateMode((StepBlock.Mode)mode);
        }
    }

    public void AddStep(GameObject obj)
    {
        _spawnedSteps.Add(obj);
    }

    public void RemoveLastStep()
    {
        _spawnedSteps.RemoveAt(_spawnedSteps.Count - 1);
    }

    
}
