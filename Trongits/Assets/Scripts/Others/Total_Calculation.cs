using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Total_Calculation : MonoBehaviour {

    public int total = 0;
    public Text TotalText;
	// Update is called once per frame
	

    public void Calculate()
    {
        total = 0;
        for(int i = 0; i < PlayerGamePlay.Instance.CardsGenerated.Length; i++)
        {
            total += PlayerGamePlay.Instance.CardsGenerated[i].GetComponent<CardManager>().weight;
        }

        TotalText.text = total.ToString();
    }
}