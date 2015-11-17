using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
    private UITextList label;
    private int index = 0;
    void Start()
    {
        label = GetComponent<UITextList>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            label.Add("Fuck" + index++); 
    }
}
