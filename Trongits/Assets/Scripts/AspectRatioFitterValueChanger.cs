using UnityEngine;
using UnityEngine.UI;

public class AspectRatioFitterValueChanger : MonoBehaviour {

	void Awake()
    {

        gameObject.GetComponent<AspectRatioFitter>().aspectRatio = Camera.main.aspect;

    }

    void Update()
    {
        gameObject.GetComponent<AspectRatioFitter>().aspectRatio = Camera.main.aspect;

    }
}
