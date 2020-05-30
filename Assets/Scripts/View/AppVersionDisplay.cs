using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppVersionDisplay : MonoBehaviour
{

    void Awake()
    {
        Text text = GetComponent<Text>();
        if (text != null) 
            text.text = Application.version;
    }
}
