using UnityEngine;
using UnityEngine.UI;

public class TextSliderValue : MonoBehaviour
{

    Text textComponent;

    [SerializeField]
    string suffix = "";

    [SerializeField]
    bool round = true;

    [SerializeField]
    int offset = 0;

	// Use this for initialization
	void Start ()
    {
        textComponent = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void UpdateTextValue(float sliderValue)
    {
        sliderValue += offset;

        if (round)
        {
            textComponent.text = Mathf.Round(sliderValue).ToString() + " " + suffix;
        }
        else
            textComponent.text = sliderValue.ToString("f2") + " " + suffix;
    }
}
