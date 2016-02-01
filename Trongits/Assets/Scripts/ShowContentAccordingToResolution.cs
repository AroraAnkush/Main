using UnityEngine;
using System.Collections;
using UnityEngine.iOS;

public class ShowContentAccordingToResolution : MonoBehaviour {

    public GameObject IPadScreen;
    public GameObject IPadManager;
    public GameObject IPhoneScreen;
    public GameObject IPhoneManager;
    public GameObject IPhoneGameManager;
    public GameObject IPadGameManager;



	void Awake()
    {

        if (Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone5S || 
            Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6 || 
            Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6Plus )
        {
            IPhoneManager.SetActive(true);
            IPhoneGameManager.SetActive(true);
            IPhoneScreen.SetActive(true);
            IPadScreen.SetActive(false);
            IPadManager.SetActive(false);
            IPadGameManager.SetActive(false);

        }

        else
        {


            IPhoneManager.SetActive(true);
            IPhoneGameManager.SetActive(true);
            IPhoneScreen.SetActive(true);
            IPadScreen.SetActive(false);
            IPadManager.SetActive(false);
            IPadGameManager.SetActive(false);

            /*
            IPhoneManager.SetActive(false);
            IPhoneGameManager.SetActive(false);
            IPhoneScreen.SetActive(false);
            IPadScreen.SetActive(true);
            IPadManager.SetActive(true);
            IPadGameManager.SetActive(true);
            */
        }
            
    }
}
