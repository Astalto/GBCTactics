using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusManager : singleton<StatusManager>
{
    public Text MessageBox;

    void Start ()
    {
        MessageBox = this.GetComponent<Text>();
    }
	
}
