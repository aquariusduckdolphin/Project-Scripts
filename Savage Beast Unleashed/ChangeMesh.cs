using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMesh : MonoBehaviour
{

    [SerializeField] private GameObject[] models = new GameObject[3];

    public int modelIndex = 0;

    void Start()
    {

        TurnOfModels();

        SingleModelOn(modelIndex);

    }

    public void TurnOfModels()
    {

        foreach(GameObject go in models)
        {

            go.SetActive(false);

        }

    }

    public void SingleModelOn(int value)
    {

        models[value].SetActive(true);

    }

}
