using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{

    public int selectedWeapon = 0;

    private const string mouseWheel = "Mouse ScrollWheel";

    void Start()
    {

        SelectWeapon();
        
    }

    void Update()
    {

        int previousSelectedWeapon = selectedWeapon;

        if(Input.GetAxis(mouseWheel) > 0f)
        {

            if(selectedWeapon >= transform.childCount - 1f)
            {

                selectedWeapon = 0;

            }
            else
            {

                selectedWeapon++;


            }

        }

        if(Input.GetAxis(mouseWheel) < 0f)
        {

            if (selectedWeapon <= 0f)
            {

                selectedWeapon = transform.childCount - 1;

            }
            else
            {

                selectedWeapon--;

            }
        }
        
        if(previousSelectedWeapon !=  selectedWeapon)
        {

            SelectWeapon();

        }

    }

    void SelectWeapon()
    {

        int i = 0;

        foreach(Transform weapon in transform)
        {

            if(i == selectedWeapon)
            {
                
                weapon.gameObject.SetActive(true);

            }
            else
            {

                weapon.gameObject.SetActive(false);

            }

            i++;

        }

    }

    void NumberKey(KeyCode inputKey, int currentChild)
    {

        if(Input.GetKeyDown(inputKey) && transform.childCount >= currentChild)
        {

            selectedWeapon = currentChild + 1;

        }

    }

}
