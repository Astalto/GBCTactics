using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EventLog : MonoBehaviour
{

    private List<string> listOfEvents = new List<string>();
    private string printOut = "";

    public int maxLines = 12;

    void OnGUI()
    {
        GUI.Label(new Rect(0, Screen.height - (Screen.height / 4), Screen.width, Screen.height / 4), printOut, GUI.skin.textArea);
    }

    public void Initialize()
    {
        listOfEvents.Clear();
        printOut = "";
    }


    //TODO//
    //somehow add color to the text based on what is hapneing
    //i.e attacking = red
    //  healing = green
    public void AddEvent(string eventString)
    {
        //if (eventString.Contains("hit"))
        //{
        //    GUI.color = Color.red;
        //    print("COLOR RED");
        //}

        //else if(eventString.Contains("moving"))
        //{
        //    GUI.color = Color.cyan;
        //    print("COLOR CYAN");
        //}

        //else
        //{
        //    GUI.color = Color.white;
        //}

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
