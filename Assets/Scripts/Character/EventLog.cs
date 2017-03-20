using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EventLog : MonoBehaviour
{

    private List<string> listOfEvents = new List<string>();
    private string printOut = "";

    public int maxLines = 10;

    void OnGUI()
    {
        GUI.Label(new Rect(0, Screen.height - (Screen.height / 4), Screen.width,
            Screen.height / 4), printOut, GUI.skin.textArea);
    }

    //TODO//
    //somehow add color to the text based on what is hapneing
    //i.e attacking = red
    //  healing = green
    public void AddEvent(string eventString)
    {
        listOfEvents.Add(eventString);

        if (listOfEvents.Count >= maxLines)
        {
            listOfEvents.RemoveAt(0);
        }

        printOut = "";

        foreach (string logEvent in listOfEvents)
        {
            printOut += logEvent;
            printOut += "\n";
        }
    }
}
