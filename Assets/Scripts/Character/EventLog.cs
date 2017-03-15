using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EventLog : MonoBehaviour {

    private List<string> listOfEvents = new List<string>();
    private string guiText = "";

    public int maxLines = 7;

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

        int newLineCounter = 0;

        foreach (string logEvent in listOfEvents)
        {
<<<<<<< HEAD
            guiText += logEvent;
            guiText += "\n";
=======
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
>>>>>>> 5441021dc6652775af3ea55f34817cc2bc7e1842
        }
    }
}
