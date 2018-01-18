using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleSelectSlider : MonoBehaviour
{

    private List<GameObject> _spawnedSteps = new List<GameObject>();

    private int _currentSampleValue = 0;
    private int _currentStep = 0;

    [SerializeField]
    private Text _textValue;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateSample(float number)
    {
        _spawnedSteps[_currentStep].GetComponent<StepBlock>().currentSample = (int)number;
    }
   

    public void UpdateCurrentStep(int currentStep)
    {
        _currentStep = Mathf.Clamp(currentStep, 0, _spawnedSteps.Count);
        _currentSampleValue = _spawnedSteps[_currentStep].GetComponent<StepBlock>().currentSample;
        _textValue.text = _currentSampleValue.ToString();
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
