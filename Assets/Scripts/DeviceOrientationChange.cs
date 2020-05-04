using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DeviceOrientationChange : MonoBehaviour
{
	public UnityEvent OnResolutionChange = new UnityEvent();
	public UnityEvent OnOrientationChange = new UnityEvent();
	public static float CheckDelay = 0.5f;        // How long to wait until we check again.

	public static Vector2 resolution;                    // Current Resolution
	public static DeviceOrientation orientation;        // Current Device Orientation
	static bool isAlive = true;                    // Keep this script running?

	public Camera cam;      //attaching script to camera itself...could be any gameobject

	void Start()
	{
		cam = GetComponent<Camera>();

		if (OnResolutionChange == null)
			OnResolutionChange = new UnityEvent();
		OnResolutionChange.AddListener(Ping);

		if (OnOrientationChange == null)
			OnOrientationChange = new UnityEvent();
		OnOrientationChange.AddListener(Ping);

		StartCoroutine(CheckForChange());
	}

	void Ping()
	{
		Debug.Log("Ping");
		if (Screen.width < Screen.height)
		{
			cam.orthographicSize = 10f;
		}
		else
		{
			cam.orthographicSize = 5f;
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