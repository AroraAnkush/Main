using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardManager : MonoBehaviour {

    public int weight = 0;
    public int CategoryWeight = 0;
    public int Preference = 0;
    public string WhichPlayer;
    void Start()
    {
        ManageWeight();
        ManageCategoryWeight();
        WhichPlayer = "" + transform.parent.name;
    }


    public void ManageWeight()
    {
        string name = gameObject.GetComponent<Image>().sprite.name;

        if (name.Contains("2"))
        {
            weight = 2;
            Preference = 2;
        }
        else if (name.Contains("3"))
        {
            weight = 3;
            Preference = 3;

        }
        else if (name.Contains("4"))
        {
            weight = 4;
            Preference = 4;

        }
        else if(name.Contains("5"))
        {
            weight = 5;
            Preference = 5;

        }
        else if(name.Contains("6"))
        {
            weight = 6;
            Preference = 6;

        }
        else if(name.Contains("7"))
        {
            Preference = 7;
            weight = 7;

        }
        else if(name.Contains("8"))
        {
            weight = 8;
            Preference = 8;

        }
        else if(name.Contains("9"))
        {
            Preference = 9;
            weight = 9;

        }
        else if(name.Contains("10"))
        {
            Preference = 10;
            weight = 10;

        }
        else if(name.Contains("ace"))
        {
            Preference = 1;
            weight = 1;

        }
        else if(name.Contains("Jack"))
        {
            weight = 10;
            Preference = 11;

        }
        else if(name.Contains("King"))
        {
            Preference = 13;
            weight = 10;

        }
        else if(name.Contains("Queen"))
        {
            Preference = 12;
            weight = 10;

        }

    }


    public void ManageCategoryWeight()
    {
        string name = gameObject.GetComponent<Image>().sprite.name;

        if (name.Contains("Spades"))
        {
            CategoryWeight = 1;
            
        }
        else if (name.Contains("Hearts"))
        {
            CategoryWeight = 2;

        }
        else if (name.Contains("Clubs"))
        {
            CategoryWeight = 3;

        }
        else if (name.Contains("Diamonds"))
        {
            CategoryWeight = 4;

        }
    }



    bool _Selected = false;
    bool _InSelection = false;

    public void Select()
    {
        if (transform.parent.parent.name == "Player")
        {
           if(!_InSelection) StartCoroutine(Selection());
        }
        else if(transform.parent.parent.name == "Melding Point")
        {
            PlayerGamePlay.Instance.Check_For_Connect(transform.parent.gameObject);
        }
        
       
    }

    


    public IEnumerator Selection()
    {
        _InSelection = true;
        float y;
        if (!_Selected)
        {
            y = gameObject.GetComponent<RectTransform>().localPosition.y + 25;
            _Selected = true;
          //  

            if(transform.parent.parent.name == "Player")
            {
                PlayerGamePlay.Instance.SelectedCards.Add(gameObject);
            }
            
        }
        else
        {
            y = gameObject.GetComponent<RectTransform>().localPosition.y - 25;
            _Selected = false;
            //   PlayerGamePlay.Instance.SelectedCards.Remove(gameObject);
            if (transform.parent.parent.name == "Player")
            {
                PlayerGamePlay.Instance.SelectedCards.Remove(gameObject);
            }
        }


        for (float i = 0; i < 1f; i+=Time.deltaTime)
        {
            gameObject.GetComponent<RectTransform>().localPosition = Vector3.Lerp(gameObject.GetComponent<RectTransform>().localPosition,
                                                                                  new Vector3(gameObject.GetComponent<RectTransform>().localPosition.x, y,
                                                                                               gameObject.GetComponent<RectTransform>().localPosition.z),
                                                                                  0.8f);

            if (gameObject.GetComponent<RectTransform>().localPosition.y >= y)
            {
                break;
            }
            yield return null;

        }

        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(gameObject.GetComponent<RectTransform>().localPosition.x, y,
                                                                                              gameObject.GetComponent<RectTransform>().localPosition.z);
        _InSelection = false;


    }

}
