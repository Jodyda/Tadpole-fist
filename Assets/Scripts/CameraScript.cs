using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    public Image rink;

    // Use this for initialization
    void Start()
    {
        RectTransform rt = rink.GetComponent<RectTransform>();

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = rt.rect.width / rt.rect.height;

        if (screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = rt.rect.height / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = rt.rect.height / 2 * differenceInSize;
        }
    }
}