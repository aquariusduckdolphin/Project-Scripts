using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScreen : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("CreditsScreen");
    }
}
