using UnityEngine;
using System.Collections;

public class TestReload : MonoBehaviour {

    public void Reload()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
