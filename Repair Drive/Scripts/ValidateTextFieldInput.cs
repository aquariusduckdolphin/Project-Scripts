using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValidateTextFieldInput : MonoBehaviour
{

    public TMP_InputField userInput;

    public PlayerInfo playerSens;

    public Slider mouseSensitivitySlider;

    private int minMaxValue;

    private void Start()
    {

        userInput = GetComponent<TMP_InputField>();

        mouseSensitivitySlider = GameObject.Find("Adjust Mouse Sensitivity Slider").GetComponent<Slider>();

        userInput.text = playerSens.mouseSensitivity.ToString();

    }

    public void ValidateInput()
    {

        int result = 0;

        if(int.TryParse(userInput.text, out result))
        {

            minMaxValue = result;

        }
        else
        {

            print("Not Working");

        }

        if(minMaxValue < 500)
        {

            minMaxValue = 500;

            userInput.text = "500";

            playerSens.mouseSensitivity = minMaxValue;

        }

        if(minMaxValue > 2000)
        {

            minMaxValue = 2000;

            userInput.text = "2000";

            playerSens.mouseSensitivity = minMaxValue;

        }

        playerSens.mouseSensitivity = minMaxValue;

        mouseSensitivitySlider.value = minMaxValue;

    }

    private void Update()
    {

        playerSens.mouseSensitivity = minMaxValue;

    }

}
