using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
    public GameObject contentGO;

    public GameObject ButtonTemplate;
    public GameObject ToggleTemplate;
    public GameObject SliderTemplate;
    public GameObject DropdownTemplate;

    public Button addButton(string Name, UnityAction action)
    {
        Button button = Instantiate(ButtonTemplate, contentGO.transform).GetComponentInChildren<Button>();
        button.onClick.AddListener(action);
        button.GetComponentInChildren<TMP_Text>().text = Name;

        return button;
    }

    public Toggle addToggle(string Name, UnityAction<bool> action, bool value = false) 
    {
        Toggle toggle = Instantiate(ToggleTemplate, contentGO.transform).GetComponentInChildren<Toggle>();
        toggle.isOn = value;
        toggle.onValueChanged.AddListener(action);
        toggle.transform.parent.parent.GetComponentInChildren<TMP_Text>().text = Name;

        return toggle;
    }

    public Slider addSlider(string Name, UnityAction<float> action, float value = 0.0f, float min = -1f, float max = 1f)
    {
        Slider slider = Instantiate(SliderTemplate, contentGO.transform).GetComponentInChildren<Slider>();
        slider.minValue = min;
        slider.maxValue = max;
        slider.value = value;
        slider.onValueChanged.AddListener(action);
        slider.transform.parent.GetComponentInChildren<TMP_Text>().text = Name;

        return slider;
    }

    public TMP_Dropdown addDropdown(string Name, UnityAction<int> action, string[] options, int value = 0)
    {
        TMP_Dropdown dropdown = Instantiate(DropdownTemplate, contentGO.transform).GetComponentInChildren<TMP_Dropdown>();
        dropdown.value = value;
        dropdown.onValueChanged.AddListener(action);
        dropdown.options.Clear();

        foreach (string option in options)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(option));
        }

        dropdown.transform.parent.GetComponentInChildren<TMP_Text>().text = Name;

        return dropdown;
    }
}
