using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppVersionDisplay : MonoBehaviour
{

    void Awake()
    {
        Text text = GetComponent<Text>();
        if (text == null) return;
        text.text = Application.version;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
