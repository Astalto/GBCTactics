using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusManager : singleton<StatusManager>
{
    public Text text;
    // Use this for initialization
    void Start () {
        text = this.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
    }
}
