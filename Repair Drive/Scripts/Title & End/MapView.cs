using UnityEngine;
using UnityEngine.SceneManagement;

public class MapView : MonoBehaviour
{

    public GameObject[] backgroundImages;

    public int currentMap = 0;

    [Tooltip("Only needed for the selection")]
    public GameObject leftButton;

    public GameObject rightButton;

    public void Start()
    {

        backgroundImages[0].SetActive(false);
        backgroundImages[1].SetActive(false);
        backgroundImages[0].SetActive(true);

    }

    public void RightButton()
    {

        backgroundImages[currentMap].gameObject.SetActive(false);

        currentMap++;

        if (currentMap > backgroundImages.Length - 1) { currentMap = 0; }

        backgroundImages[currentMap].gameObject.SetActive(true);

    }

    public void LeftButton()
    {

        backgroundImages[currentMap].gameObject.SetActive(false);

        currentMap--;

        if (currentMap < 0) { currentMap = backgroundImages.Length -1; }

        backgroundImages[currentMap].gameObject.SetActive(true);

    }

    public void SelectEnvironment()
    {

        currentMap++;

        SceneManager.LoadScene(currentMap);

    }

}
