using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreditScroll : MonoBehaviour
{

    public float scrollSpeed = 10f;

    public float textStartPosition = -505.1586f;

    public float boundaryEnd = 772f;

    private RectTransform rectTransform;

    private bool loop = true;

    // Start is called before the first frame update
    void Start()
    {

        rectTransform = GetComponent<RectTransform>();

        StartCoroutine(AutoScroll());
        
    }

    IEnumerator AutoScroll()
    {

        while(rectTransform.localPosition.y < boundaryEnd)
        {

            rectTransform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);

            if(rectTransform.localPosition.y > boundaryEnd)
            {

                if (loop)
                {

                    rectTransform.localPosition = Vector3.up * textStartPosition;

                }
                else
                {

                    break;

                }

            }

            yield return null;

        }

    }
}
