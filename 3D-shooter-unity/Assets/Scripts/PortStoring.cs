using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortStoring : MonoBehaviour
{

    public static PortStoring instance;

    public string portNumber = "";
    // Start is called before the first frame update
    void Start()
    {

		//if there is additional soundmanager created,destroy it
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			//set it to not being destroyed when going to different scene
			DontDestroyOnLoad(gameObject);
		}
	}

    public void UpdateString(string portnumber)
    {
		portNumber = "COM" + portnumber;
    }
}
