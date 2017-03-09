using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EventLog : MonoBehaviour {

    private List<string> listOfEvents = new List<string>();
    private string guiText = "";

    public int maxLines = 10;

	void OnGUI()
    {
        GUI.Label(new Rect(0, Screen.height - (Screen.height / 4), Screen.width,
            Screen.height / 4), guiText, GUI.skin.textArea);
    }

    public void AddEvent(string eventString)
    {
        listOfEvents.Add(eventString);

        if (listOfEvents.Count >= maxLines)
        {
            listOfEvents.RemoveAt(0);
        }

        guiText = "";

        foreach (string logEvent in listOfEvents)
        {
            guiText += logEvent;
            guiText += "\n";
        }
    }
}
