/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Collections;

/**
 * When creating your message listeners you need to implement these two methods:
 *  - OnMessageArrived
 *  - OnConnectionEvent
 */
public class SampleMessageListener : MonoBehaviour
{

    public int posX = 0;
    public int posY = 0;
    public int item = 0;
    public int offsetx = 0;
    public int offsety = 0;

    // Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
        Debug.Log("Message arrived: " + msg);

        int messageCount = 0;
        int commaPos = 0;
        int lastDataPos = 0;
        int posx = 0;
        int posy = 0;
        int itemNo = 0;

        for (int i = 0; i < msg.Length; ++i)
        {
            if (msg[i] == ',')
            {
                string message;
                if (commaPos != 0)
                    message = msg.Substring(lastDataPos, i - commaPos - 1);
                else
                    message = msg.Substring(lastDataPos, i);
                messageCount++;
                lastDataPos = i + 1;
                commaPos = i;
                if (messageCount == 1)
                    posx = int.Parse(message);
                else
                    posy = int.Parse(message);
            }
        }
        string lastmessage = msg.Substring(lastDataPos, msg.Length - commaPos - 1);
        itemNo = int.Parse(lastmessage);
        ConversionToControllerPos(posx, posy, itemNo);
    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        if (success)
            Debug.Log("Connection established");
        else
            Debug.Log("Connection attempt failed or disconnection detected");
    }

    void ConversionToControllerPos(int posx, int posy, int itemNo)
    {
        posX = posx - offsetx;
        posY = posy - offsety;
        item = itemNo;
    }
}