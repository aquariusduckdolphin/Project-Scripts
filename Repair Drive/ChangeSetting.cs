using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeSetting : MonoBehaviour
{

    public PlayerCamera[] mouseSensitivity = new PlayerCamera[2];

    public PlayerInfo playerSens;

    private Slider mouseSensitivitySlider;

    private TMP_InputField inputFieldText;

    public bool sliderChange = false;

    private void Start()
    {

        mouseSensitivitySlider = GetComponent<Slider>();

        inputFieldText = GameObject.Find("Number&Text").GetComponent<TMP_InputField>();

        mouseSensitivitySlider.value = playerSens.mouseSensitivity;

        inputFieldText.text = mouseSensitivitySlider.value.ToString();

    }

    public void ChangeSensitivity()
    {

        playerSens.mouseSensitivity = mouseSensitivitySlider.value;

        inputFieldText.text = mouseSensitivitySlider.value.ToString();

    }

}
