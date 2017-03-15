using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EventLog : MonoBehaviour {

    private List<string> listOfEvents = new List<string>();
    private string printOut = "";

    public int maxLines = 7;

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

        int newLineCounter = 0;

        foreach (string logEvent in listOfEvents)
        {
            newLineCounter++;
            printOut += logEvent;
            printOut += "\n";
            //Since each player character will create 4 lines in eventLog (where they move, what they are doing, who they are attacking, enemy hp left)
            //Our counter is set to 4 lines, the next 4 lines (another character eventLog) will be a separate paragraph
            if (newLineCounter == 4)
            {
                //Print out an additonal linespace
                printOut += "\n";
                //Reset the counter
                newLineCounter = 0;
            }
        }
    }
}
