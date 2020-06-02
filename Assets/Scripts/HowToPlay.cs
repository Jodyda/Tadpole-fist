using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : MonoBehaviour
{

	public static bool isHowToOpen = false;

	public GameObject howToGUI;

    public void openHowTo() {
    	howToGUI.SetActive(true);
    }

    public void closeHowTo() {
    	howToGUI.SetActive(false);
    }
}
