using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPortNumber : MonoBehaviour
{
    // Update is called once per frame
    public void UpdatePort(string portnumber)
    {
        PortStoring.instance.UpdateString(portnumber);
    }
}
