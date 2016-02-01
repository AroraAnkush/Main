using UnityEngine;
using System.Collections;

public class Dumped_Panel_Called : MonoBehaviour {


    GameObject Panel;
    GameObject[] cards;

    void Start()
    {
        Panel = transform.gameObject;
    }
	public void OnDumpedButton()
    {
		StartCoroutine (ShowPanel ());
    }

	public void OnCloseButton()
	{
		StartCoroutine (HidePanel ());
	}
    
    IEnumerator ShowPanel()
    {
		Generate_Cards ();
        for (float timer = 0; timer < 1.0f; timer += Time.deltaTime)
        {
            Panel.transform.localScale = Vector3.Lerp(Panel.transform.localScale, new Vector3(1, 1, 1), 0.5f * timer);
            yield return null;
        }
        
    }

	IEnumerator HidePanel()
	{
		Destroy_Cards();
		for (float timer = 0; timer < 1.0f; timer += Time.deltaTime)
		{
			Panel.transform.localScale = Vector3.Lerp(Panel.transform.localScale, new Vector3(0,0,0), 0.5f * timer);
			yield return null;
		}

		
	}



    void Generate_Cards()
    {
		cards = PlayerGamePlay.Instance.DumpedCards.ToArray ();
		int x = -250;
		int y = 100;
		int z = 0;

        for(int i = 0; i < cards.Length; i++)
        {

			x += 50;
			GameObject go = (GameObject) Instantiate(cards[i], Vector3.zero, Quaternion.identity);
			go.transform.SetParent(transform, false);
			go.GetComponent<RectTransform>().localPosition = new Vector3(x, y, z);
			go.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

			if(i != 0 && i % 5 == 0)
			{
				x = -250;
				y -=100;
			}


        }
    }


	void Destroy_Cards()
	{
		for (int i = 0; i < transform.childCount; i++) {
			if(transform.GetChild(i).name.Contains("Card"))
			{
				Destroy(transform.GetChild(i).gameObject);
			}
		}
	}
}
