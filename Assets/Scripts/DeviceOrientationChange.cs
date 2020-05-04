using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeviceOrientationChange : MonoBehaviour
{
	public static float CheckDelay = 0.5f;        // How long to wait until we check again.

	public static Vector2 resolution;                    // Current Resolution
	public static DeviceOrientation orientation;        // Current Device Orientation
	static bool isAlive = true;                    // Keep this script running?

	public Camera cam;      //attaching script to camera itself...could be any gameobject
    public Image board;
    
	private UnityEvent OnResolutionChange = new UnityEvent();
	private UnityEvent OnOrientationChange = new UnityEvent();

	void Start()
	{
		cam = GetComponent<Camera>();

		if (OnResolutionChange == null)
			OnResolutionChange = new UnityEvent();
		OnResolutionChange.AddListener(Ping);

		if (OnOrientationChange == null)
			OnOrientationChange = new UnityEvent();
		OnOrientationChange.AddListener(Ping);

		Ping();
		isAlive = true;
		StartCoroutine(CheckForChange());
	}

	void Ping()
	{
		RectTransform rt = board.GetComponent<RectTransform>();

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


	IEnumerator CheckForChange()
	{

		resolution = new Vector2(Screen.width, Screen.height);
		orientation = Input.deviceOrientation;

		while (isAlive)
		{
			// Check for a Resolution Change
			if (resolution.x != Screen.width || resolution.y != Screen.height)
			{
				resolution = new Vector2(Screen.width, Screen.height);
				OnResolutionChange.Invoke();
			}

			// Check for an Orientation Change
			switch (Input.deviceOrientation)
			{
				case DeviceOrientation.Unknown:            // Ignore
				case DeviceOrientation.FaceUp:            // Ignore
				case DeviceOrientation.FaceDown:        // Ignore
					break;
				default:
					if (orientation != Input.deviceOrientation)
					{
						orientation = Input.deviceOrientation;
						OnOrientationChange.Invoke();
					}
					break;
			}

			yield return new WaitForSeconds(CheckDelay);
		}
	}

	void OnDestroy()
	{
		isAlive = false;
	}

}