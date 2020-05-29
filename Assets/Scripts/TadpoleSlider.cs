using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TadpoleSlider : MonoBehaviour
{

    public Slider slider;
    
    public void SetMaxCount(int tadpoles)
    {
        slider.maxValue = tadpoles;
        slider.value = tadpoles;
    }
    public void SetCount(int tadpoles)
    {
        slider.value = tadpoles;
    }
}
