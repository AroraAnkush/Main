using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerGamePlay : Singleton<PlayerGamePlay> {

    #region Public Attributes
    /// <summary>
    /// all the cards sprites array for assigning
    /// </summary>
    public Sprite[] Cards;

    /// <summary>
    /// prefab of the gameobject that is to be generated for the card distribution
    /// </summary>
	public GameObject CardPrefab;

    /// <summary>
    /// Gen point of the player cards
    /// </summary>
	public RectTransform GenPoint;

    /// <summary>
    /// Gen point of the AI 1 cards
    /// </summary>
    public RectTransform GenPointForAI1;

    /// <summary>
    /// Gen point of the AI 2 Cards
    /// </summary>
    public RectTransform GenPointForAI2;

	 /// <summary>
     /// the array of generated cards for the player
     /// </summary>
	public GameObject[] CardsGenerated;

    /// <summary>
    /// array of generated cards for the ai1
    /// </summary>
    public GameObject[] CardsGeneratedForAI1;

    /// <summary>
    /// array of generated cards for the ai2
    /// </summary>
    public GameObject[] CardsGeneratedForAI2;

    /// <summary>
    /// list of cards available in deck
    /// at starting it is equal to the cards array and is assigned the same
    /// and distribution is done by this list only
    /// </summary>
    public List<Sprite> Deck = new List<Sprite>();


    /// <summary>
    /// the melding point for the player
    /// </summary>
    public GameObject MeldingPoint;
    public GameObject DumpingPoint;
    public GameObject DeckPoint;
    public GameObject StrategyPoint;
    public GameObject Ante_Option_Buttons;
    public GameObject Picking_Option_Buttons;
    public GameObject Melding_Option_Buttons;
    public GameObject Auto_Button;
    public GameObject Draw_Button;
    public GameObject Pick_Dump_Button;
    public GameObject Melds;


    /// <summary>
    /// list of cards that are selected for the melding or dumping
    /// </summary>
    public List<GameObject> SelectedCards = new List<GameObject>();
    public List<GameObject> DumpedCards = new List<GameObject>();

    public int TongitsMadeByPlayer, TongitsMadeByAi1, TongitsMadeByAi2;

    public bool _CanBeMeld = false;
    public bool _straightflush = false;
    public bool _samekind = false;
    public bool _Ante = false;

  //  [HideInInspector]
    public bool _GameStarted = false;

    [HideInInspector]
    public bool _MyTurn = false;

    public GameObject PreviousPlayer, NextPlayer;
    public bool Last_Meld_turn = true;
    #endregion



    #region Private Attributes

    GameObject[] selArr;
    bool _sorting = false;
    List<GameObject> AutoSelect = new List<GameObject>();
    List<GameObject> StrategyList = new List<GameObject>();
    GameObject[] StrategyArr;
    bool _IAmDealer = false;
    

    #endregion
    // Use this for initialization
    void Start () 
	{


        SetUpForNewGame();

    }


   public void ResetEveryThing()
    {
        foreach (GameObject go in CardsGenerated)
        {
            Destroy(go);
        }
        foreach (GameObject go in DumpedCards)
        {
            Destroy(go);
        }
        for (int i = 0; i < MeldingPoint.transform.childCount; i++)
        {
            Destroy(MeldingPoint.transform.GetChild(i).gameObject);
        }
        Deck.Clear();
        _GameStarted = false;
        _IAmDealer = false;
        _MyTurn = false;
        _Ante = false;
        _CanBeMeld = false;
        _samekind = false;
        _sorting = false;
        _straightflush = false;
        Draw_Button.SetActive(false);


        SetUpForNewGame();

    }


    public void SetUpForNewGame()
    {
        for (int i = 0; i < Cards.Length; i++)
        {
            Deck.Add(Cards[i]);
        }
        
        if (GenPoint.transform.childCount == 13)
        {
            _IAmDealer = true;
        }
        else
        {
            _IAmDealer = false;
        }

        Ante_Option_Buttons.SetActive(true);
        Melding_Option_Buttons.SetActive(false);
        Picking_Option_Buttons.SetActive(false);
    }


   

    public void OnAnte()
    {
        _Ante = true;
        GameManager.Instance.Ante_Apply();
        Ante_Option_Buttons.SetActive(false);
    }

    public void OnOptOut()
    {
        // Opt Out

    }
    public void Distribute_Cards()
    {

        switch (Random.Range(1, 3))
        {
            case 1:
                CardGenerationForPlayer(13);
                CardGenerationForAI1(12);
                CardGenerationForAI2(12);
                Melding_Option_Buttons.SetActive(true);
                Auto_Button.SetActive(true);
                _MyTurn = true;


                break;
            case 2:
                CardGenerationForPlayer(12);
                CardGenerationForAI1(13);
                CardGenerationForAI2(12);

                break;
            case 3:
                CardGenerationForPlayer(12);
                CardGenerationForAI1(12);
                CardGenerationForAI2(13);

                break;
        }
    }

    public void OnDraw()
    {
        GameManager.Instance._DrawCalled = true;
        GameManager.Instance.ChangeChance(gameObject);
        InvokeRepeating("Check_For_Draw_Return", 0.1f, 0.1f);
    }


    void Check_For_Draw_Return()
    {
       
        if(_MyTurn && GameManager.Instance._DrawCalled)
        GameManager.Instance.Draw_Handler(gameObject);
    }


    #region Change_the_Turn

    void ChangeTurn()
    {
        if (PreviousPlayer.GetComponent<Artiificial_Player>())
        {
            if (!PreviousPlayer.GetComponent<Artiificial_Player>()._MyTurn && !_MyTurn)
            {
                NextPlayer.GetComponent<Artiificial_Player>()._MyTurn = true;
               
            }

        }
        
    }

    #endregion

    #region Card Distribution

    void CardGenerationForPlayer(int n)
    {
        List<GameObject> TempList = new List<GameObject>();
        for (int i = 0; i < n; i++)
        {

            GameObject Temp = (GameObject)Instantiate(CardPrefab, GenPoint.position, GenPoint.rotation);
            Temp.transform.SetParent(GenPoint.transform, false);
            TempList.Add(Temp);
            CardsAssign(Temp);
            CardsGenerated = TempList.ToArray();
            Temp.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            StartCoroutine(SetupPosition(CardsGenerated,i));

        }

    }


    void CardGenerationForAI1(int n)
    {
        List<GameObject> TempList = new List<GameObject>();
        for (int i = 0; i < n; i++)
        {

            GameObject Temp = (GameObject)Instantiate(CardPrefab, GenPointForAI1.position, GenPointForAI1.rotation);
            Temp.transform.SetParent(GenPointForAI1.transform, false);
            TempList.Add(Temp);
            CardsAssign(Temp);
            CardsGeneratedForAI1 = TempList.ToArray();
            Temp.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            StartCoroutine(SetupPosition(CardsGeneratedForAI1,i));

        }

    }

    void CardGenerationForAI2(int n)
    {
        List<GameObject> TempList = new List<GameObject>();
        for (int i = 0; i < n; i++)
        {

            GameObject Temp = (GameObject)Instantiate(CardPrefab, GenPointForAI2.position, GenPointForAI2.rotation);
            Temp.transform.SetParent(GenPointForAI2.transform, false);
            TempList.Add(Temp);
            CardsAssign(Temp);
            CardsGeneratedForAI2 = TempList.ToArray();
            Temp.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            StartCoroutine(SetupPosition(CardsGeneratedForAI2,i));

        }

    }


    void CardsAssign(GameObject Temp)
    {
        Sprite[] CardsTempArr = Deck.ToArray();
        int i = Random.Range(0, CardsTempArr.Length);
        Temp.GetComponent<Image>().sprite = CardsTempArr[i];
        Deck.Remove(CardsTempArr[i]);
    }

    #endregion


    #region Ienumerators

    IEnumerator SetupPosition(GameObject[] CardGeneratedTransfer,int i)
	{


		yield return null;
        //   CardsGenerated[i].transform.parent = null;

        CardGeneratedTransfer[i].transform.SetSiblingIndex(i);

        if (i > 0)
			{
				for(float timer = 0; timer < 3f; timer += Time.deltaTime)
				{
                CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition
					= Vector3.Lerp(CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition,
	                                 new Vector3(CardGeneratedTransfer[i-1].GetComponent<RectTransform>().localPosition.x + 45, 0,0),
                                     0.1f * timer);
                
                if(CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition.x>1200)
                {
                    CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition = new Vector3(1200, 0, 0);
                }
					yield return null;
				}

            CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition = new Vector3(CardGeneratedTransfer[i-1].GetComponent<RectTransform>().localPosition.x + 45, 0,0);
			}

            else
        {
            for (float timer = 0; timer < 3f; timer += Time.deltaTime)
            {
                CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition
                = Vector3.Lerp(CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition,
                                 new Vector3(0, 0, 0),
                                 0.1f * timer);

                yield return null;
            }

            CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        }

        
       
        yield return new WaitForSeconds(2f);
        if (i == CardGeneratedTransfer.Length - 1)
            _sorting = false;
        _GameStarted = true;
    }

    IEnumerator SetupPositionWhenSort(GameObject[] CardGeneratedTransfer, int i)
    {


        yield return null;
        //   CardsGenerated[i].transform.parent = null;

        CardGeneratedTransfer[i].transform.SetSiblingIndex(i);

        if (i > 0)
        {
          
            CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition = new Vector3(CardGeneratedTransfer[i - 1].GetComponent<RectTransform>().localPosition.x + 45, 0, 0);
        }

        else
        {
           
            CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        }



        yield return null ;
        if (i == CardGeneratedTransfer.Length - 1)
            _sorting = false;
        _GameStarted = true;
    }



    IEnumerator MoveMeldedCards(GameObject MP, int i)
    {


        MP.transform.localScale = new Vector3(1,1,1);
        selArr[i].transform.SetParent(MP.transform, false);
        MP.transform.SetAsFirstSibling();
        //moving melded cards
        Debug.Log("in the move melded function");


        for (float timer = 0; timer < 1f; timer += Time.deltaTime)
        {
            selArr[i].GetComponent<RectTransform>().localPosition = Vector3.Lerp(selArr[i].GetComponent<RectTransform>().localPosition,
                                                                                    new Vector3((i * 50),0,0), 0.5f);
            yield return null;
        }

        selArr[i].GetComponent<RectTransform>().localPosition = new Vector3((i * 50),0, 0);
        Last_Meld_turn = true;
        Draw_Button.SetActive(false);
    }


    int counter = 0;
    IEnumerator MoveDumpedCards(GameObject MP, int i)
    {
        selArr[i].transform.SetParent(MP.transform);
        //moving melded cards
        Debug.Log("in the Dumped function");


        for (float timer = 0; timer < 1f; timer += Time.deltaTime)
        {
            selArr[i].GetComponent<RectTransform>().localPosition = Vector3.Lerp(selArr[i].GetComponent<RectTransform>().localPosition,
                                                                                    new Vector3(0, 0, 0), 0.5f);
            yield return null;
        }

        selArr[i].GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

        if(Last_Meld_turn)
        {
            if (counter < 2)
            {
                counter++;
            }
            else
            {
                counter = 0;
                Last_Meld_turn = false;
            }
        }
        
        GameManager.Instance.ChangeChance(gameObject);

    }


    IEnumerator SetupPositionWhenArrChanged(GameObject[] CardGeneratedTransfer, int i)
    {


        yield return null;
        //   CardsGenerated[i].transform.parent = null;

        CardGeneratedTransfer[i].transform.SetSiblingIndex(i);

        if (i > 0)
        {
            for (float timer = 0; timer < 1f; timer += Time.deltaTime)
            {
                CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition
                    = Vector3.Lerp(CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition,
                                     new Vector3(CardGeneratedTransfer[i - 1].GetComponent<RectTransform>().localPosition.x + 45, 0, 0),
                                     0.5f);
                yield return null;
            }

            CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition = new Vector3(CardGeneratedTransfer[i - 1].GetComponent<RectTransform>().localPosition.x + 45, 0, 0);
        }

        else
        {
            for (float timer = 0; timer < 1f; timer += Time.deltaTime)
            {
                CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition
                = Vector3.Lerp(CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition,
                                 new Vector3(0, 0, 0),
                                 0.5f);

                yield return null;
            }

            CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        }


        if (i == CardsGenerated.Length - 1)
            _sorting = false;

    }






    #endregion


    #region Sorting

   

    public void SortByWeight()
    {

      //  StopAllCoroutines();

        if (!_sorting && _GameStarted && _MyTurn)
        {
            _sorting = true;

            GameObject[] TempArr = new GameObject[CardsGenerated.Length];

            for (int i = 0; i < CardsGenerated.Length; i++)
            {
                TempArr[i] = CardsGenerated[i];
                Debug.Log("" + TempArr[i].gameObject.name);
            }
            GameObject TempObj;

            for (int i = 0; i < TempArr.Length; i++)
            {
                for (int j = i + 1; j < TempArr.Length; j++)
                {
                    if (TempArr[i].GetComponent<CardManager>().Preference > TempArr[j].GetComponent<CardManager>().Preference)
                    {
                        TempObj = TempArr[i];
                        TempArr[i] = TempArr[j];
                        TempArr[j] = TempObj;

                    }
                    else if (TempArr[i].GetComponent<CardManager>().Preference == TempArr[j].GetComponent<CardManager>().Preference)
                    {

                    }
                    else if (TempArr[i].GetComponent<CardManager>().Preference < TempArr[j].GetComponent<CardManager>().Preference)
                    {

                    }
                }
            }

            for (int i = 0; i < TempArr.Length; i++)
            {
                CardsGenerated[i] = TempArr[i];
            }

            for (int i = 0; i < CardsGenerated.Length; i++)
            {
                StartCoroutine(SetupPositionWhenSort(CardsGenerated,i));
            }

           
        }
    }


    public void SortByCategory()
    {
    //    StopAllCoroutines();


        if (!_sorting && _GameStarted && _MyTurn)
        {
            _sorting = true;

            GameObject[] TempArr = new GameObject[CardsGenerated.Length];

            for (int i = 0; i < CardsGenerated.Length; i++)
            {
                TempArr[i] = CardsGenerated[i];
            }

            GameObject TempObj;
          
            List<GameObject> cat1 = new List<GameObject>();
            List<GameObject> cat2 = new List<GameObject>();
            List<GameObject> cat3 = new List<GameObject>();
            List<GameObject> cat4 = new List<GameObject>();

            for (int i = 0; i < TempArr.Length; i++)
            {
                switch (TempArr[i].GetComponent<CardManager>().CategoryWeight)
                {
                    

                    case 1:
                        cat1.Add(TempArr[i]);
                        break;
                    case 2:
                        cat2.Add(TempArr[i]);

                        break;
                    case 3:
                        cat3.Add(TempArr[i]);

                        break;
                    case 4:
                        cat4.Add(TempArr[i]);

                        break;

                }
            }


            for (int i = 0; i < cat1.Count; i++)
            {
                for (int j = 0; j < cat1.Count; j++)
                {
                    if (cat1[i].GetComponent<CardManager>().Preference < cat1[j].GetComponent<CardManager>().Preference)
                    {
                        TempObj = cat1[i];
                        cat1[i] = cat1[j];
                        cat1[j] = TempObj;
                    }
                }
            }

           


            for (int i = 0; i < cat2.Count; i++)
            {
                for (int j = 0; j < cat2.Count; j++)
                {
                    if (cat2[i].GetComponent<CardManager>().Preference < cat2[j].GetComponent<CardManager>().Preference)
                    {
                        TempObj = cat2[i];
                        cat2[i] = cat2[j];
                        cat2[j] = TempObj;
                    }
                }
            }

           


            for (int i = 0; i < cat3.Count; i++)
            {
                for (int j = 0; j < cat3.Count; j++)
                {
                    if (cat3[i].GetComponent<CardManager>().Preference < cat3[j].GetComponent<CardManager>().Preference)
                    {
                        TempObj = cat3[i];
                        cat3[i] = cat3[j];
                        cat3[j] = TempObj;
                    }
                }
            }

          


            for (int i = 0; i < cat4.Count; i++)
            {
                for (int j = 0; j < cat4.Count; j++)
                {
                    if (cat4[i].GetComponent<CardManager>().Preference < cat4[j].GetComponent<CardManager>().Preference)
                    {
                        TempObj = cat4[i];
                        cat4[i] = cat4[j];
                        cat4[j] = TempObj;
                    }
                }
            }

            List<GameObject> temp = new List<GameObject>();
            foreach(GameObject go in cat1)
            {
                temp.Add(go);
            }
            foreach (GameObject go in cat2)
            {
                temp.Add(go);
            }
            foreach (GameObject go in cat3)
            {
                temp.Add(go);
            }
            foreach (GameObject go in cat4)
            {
                temp.Add(go);
            }

            TempArr = temp.ToArray();
            for (int i = 0; i < TempArr.Length; i++)
            {
                CardsGenerated[i] = TempArr[i];
            }

            for (int i = 0; i < CardsGenerated.Length; i++)
            {
                StartCoroutine(SetupPositionWhenSort(CardsGenerated,i));
            }

        
        }
    }



    #endregion

    #region Melding

  

    public void Melding()
    {

        if(SelectedCards.Count >= 3)
        {
            GameObject[] Temp = SelectedCards.ToArray();

            GameObject TempObj;

            for (int i = 0; i < Temp.Length; i++)
            {
                for (int j = i + 1; j < Temp.Length; j++)
                {
                    if (Temp[i].GetComponent<CardManager>().Preference > Temp[j].GetComponent<CardManager>().Preference)
                    {
                        TempObj = Temp[i];
                        Temp[i] = Temp[j];
                        Temp[j] = TempObj;

                    }
                    else if (Temp[i].GetComponent<CardManager>().Preference == Temp[j].GetComponent<CardManager>().Preference)
                    {

                    }
                    else if (Temp[i].GetComponent<CardManager>().Preference < Temp[j].GetComponent<CardManager>().Preference)
                    {

                    }
                }
            }


            for (int i = 0; i < Temp.Length; i++)
            {
                if (i > 0 && Temp[i].GetComponent<CardManager>().Preference == Temp[i - 1].GetComponent<CardManager>().Preference)
                {
                    //Three or four of a kind
                    _CanBeMeld = true;
                    _samekind = true;
                    _straightflush = false;
                }

                else if (i > 0 && Temp[i].GetComponent<CardManager>().Preference == Temp[i - 1].GetComponent<CardManager>().Preference + 1 && Temp[i].GetComponent<CardManager>().CategoryWeight == Temp[i-1].GetComponent<CardManager>().CategoryWeight)
                {
                    //straight flush
                    _CanBeMeld = true;
                    _samekind = false;
                    _straightflush = true;
                }

                else
                {
                    _CanBeMeld = false;
                    _straightflush = false;
                    _samekind = false;
                }
            }

            if (_CanBeMeld && _straightflush)
            {
                StraightFlush();
            }
            else if (_CanBeMeld && _samekind)
            {
                if (SelectedCards.Count == 3)
                {
                    ThreeOfaKind();
                }
                else if (SelectedCards.Count == 4)
                {
                    FourOfaKind();
                }
        }

        }

        

    }

    public void StraightFlush()
    {
        Debug.Log("in the Straight flush function");
        int n = SelectedCards.Count;
        selArr = SelectedCards.ToArray();

        GameObject Meld = (GameObject)Instantiate(Melds, MeldingPoint.transform.position, MeldingPoint.transform.rotation);
        Meld.transform.SetParent(MeldingPoint.transform, false);
        Meld.transform.localScale = new Vector3(1,1,1);
        Meld.transform.localPosition = new Vector3(0, TongitsMadeByPlayer * 50, 0);
        Meld.name = "Meld" + TongitsMadeByPlayer;

        for (int i = 0; i < n; i++)
        {
        Debug.Log("in the Straight flush function for loop");
            StartCoroutine(MoveMeldedCards(Meld, i));
        }
        TongitsMadeByPlayer++;
       RemoveCardFromArray();
    }

    public void ThreeOfaKind()
    {
        Debug.Log("in the three of a kind function");
        int n = SelectedCards.Count;
        selArr = SelectedCards.ToArray();

        GameObject Meld = (GameObject)Instantiate(Melds, MeldingPoint.transform.position, MeldingPoint.transform.rotation);
        Meld.transform.SetParent(MeldingPoint.transform);
        Meld.transform.localPosition = new Vector3(0, TongitsMadeByPlayer * 50, 0);
        Meld.name = "Meld" + TongitsMadeByPlayer;

        for (int i = 0; i < n; i++)
        {
        Debug.Log("in the three of a kind function for loop");
            StartCoroutine(MoveMeldedCards(Meld, i));
        }
        TongitsMadeByPlayer++;
       RemoveCardFromArray();

    }

    public void FourOfaKind()
    {
        Debug.Log("in the four of a kind function");
        int n = SelectedCards.Count;
        selArr = SelectedCards.ToArray();

        GameObject Meld = (GameObject)Instantiate(Melds, MeldingPoint.transform.position, MeldingPoint.transform.rotation);
        Meld.transform.SetParent(MeldingPoint.transform, false);
        Meld.transform.localPosition = new Vector3(0, TongitsMadeByPlayer * 50, 0);
        Meld.name = "Meld" + TongitsMadeByPlayer;

        for (int i = 0; i < n; i++)
        {
        Debug.Log("in the four of a kind function for loop");
            StartCoroutine(MoveMeldedCards(Meld, i));
        }
        TongitsMadeByPlayer++;
       RemoveCardFromArray();

    }



    public void RemoveCardFromArray()
    {
        List<GameObject> Temp = new List<GameObject>();

        for(int i = 0; i < CardsGenerated.Length; i++)
        {
            Temp.Add(CardsGenerated[i]);
        }
       foreach(GameObject sc in SelectedCards)
        {
            if (Temp.Contains(sc)) Temp.Remove(sc);
        }
        
        CardsGenerated = Temp.ToArray();
     

        for(int i = 0; i < CardsGenerated.Length; i++)
        {
            StartCoroutine(SetupPositionWhenArrChanged(CardsGenerated, i));
        }

        SelectedCards.Clear();
    }


    #endregion


    #region Dumping

    public void Dumping()
    {
        if(SelectedCards.Count == 0)
        {
            Debug.Log("please select a card");

        }

        else if(SelectedCards.Count == 1)
        {
            selArr = SelectedCards.ToArray();
            Auto_Button.SetActive(false);
            Melding_Option_Buttons.SetActive(false);
            StartCoroutine(MoveDumpedCards(DumpingPoint, 0));
            DumpedCards.Add(selArr[0]);
            RemoveCardFromArray();

        }
        else
        {
            Debug.Log("please select only one card");
        }
    }


    #endregion

    #region Add Cards 
    public void AddCardsFromDeck()
    {
        //Generate Random Card At DeckPosition
        //Move Card From DeckPosition to cardarray position
        //add card to cardarray
        if (Deck.Count > 0)
        {
            List<GameObject> Temp = new List<GameObject>();

            for (int i = 0; i < CardsGenerated.Length; i++)
            {
                Temp.Add(CardsGenerated[i]);
            }
            GameObject Tempobj = (GameObject)Instantiate(CardPrefab, DeckPoint.transform.position, DeckPoint.transform.rotation);
            Tempobj.transform.SetParent(DeckPoint.transform, false);
            Tempobj.transform.localPosition = new Vector3(0, 0, 0);
            Temp.Add(Tempobj);
            Tempobj.GetComponent<Image>().sprite = Deck[Deck.Count - 1];
            Deck.Remove(Deck[Deck.Count - 1]);
            CardsGenerated = Temp.ToArray();
            CardsGenerated[CardsGenerated.Length - 1].transform.SetParent(GenPoint.transform);
            Picking_Option_Buttons.SetActive(false);
            Melding_Option_Buttons.SetActive(true);
            Auto_Button.SetActive(true);
            for(int i = 0; i < CardsGenerated.Length; i++)
            {
                CardsGenerated[i].GetComponent<CardManager>().enabled = true;
            }
            StartCoroutine(SetupPositionWhenArrChanged(CardsGenerated, CardsGenerated.Length - 1));
        }
       
    }

    public void AddCardsFromDump()
    {
        //Move Last Card From DumpPosition to cardarrayPosition
        //add card to cardarray
        if (DumpedCards.Count > 0)
        {
            List<GameObject> Temp = new List<GameObject>();

            for (int i = 0; i < CardsGenerated.Length; i++)
            {
                Temp.Add(CardsGenerated[i]);
            }

            Temp.Add(DumpedCards[DumpedCards.Count - 1]);
            DumpedCards.Remove(DumpedCards[DumpedCards.Count - 1]);
            CardsGenerated = Temp.ToArray();
            Picking_Option_Buttons.SetActive(false);
            Melding_Option_Buttons.SetActive(true);
            Auto_Button.SetActive(true);
            for (int i = 0; i < CardsGenerated.Length; i++)
            {
                CardsGenerated[i].GetComponent<CardManager>().enabled = true;
            }
            CardsGenerated[CardsGenerated.Length - 1].transform.SetParent(GenPoint.transform);
            StartCoroutine(SetupPositionWhenArrChanged(CardsGenerated, CardsGenerated.Length - 1));
        }
    }

    #endregion


    #region AutoFunctionality

    public void Auto()
    {
        SortForAuto();
        SelectAutoSelectedCards();
        AutoSelect.Clear();
    }


   

    void SortForAuto()
    {

        ////print("sorting for auto");
        GameObject[] TempArr = new GameObject[CardsGenerated.Length];

        for (int i = 0; i < CardsGenerated.Length; i++)
        {
            TempArr[i] = CardsGenerated[i];
        }

        GameObject TempObj;
        List<GameObject> cat1 = new List<GameObject>();
        List<GameObject> cat2 = new List<GameObject>();
        List<GameObject> cat3 = new List<GameObject>();
        List<GameObject> cat4 = new List<GameObject>();

        for (int i = 0; i < TempArr.Length; i++)
        {
            switch (TempArr[i].GetComponent<CardManager>().CategoryWeight)
            {
                case 1:
                    cat1.Add(TempArr[i]);
                    break;
                case 2:
                    cat2.Add(TempArr[i]);

                    break;
                case 3:
                    cat3.Add(TempArr[i]);

                    break;
                case 4:
                    cat4.Add(TempArr[i]);

                    break;
            }
        }


        for (int i = 0; i < cat1.Count; i++)
        {
            for (int j = 0; j < cat1.Count; j++)
            {
                if (cat1[i].GetComponent<CardManager>().Preference < cat1[j].GetComponent<CardManager>().Preference)
                {
                    TempObj = cat1[i];
                    cat1[i] = cat1[j];
                    cat1[j] = TempObj;
                }
            }
        }

        if(cat1.Count >= 3 && AutoSelect.Count < 3)
        {
            CheckForStraightFlush(cat1);
        }
        if(AutoSelect.Count >=3)
        {
            return;
        }


        for (int i = 0; i < cat2.Count; i++)
        {
            for (int j = 0; j < cat2.Count; j++)
            {
                if (cat2[i].GetComponent<CardManager>().Preference < cat2[j].GetComponent<CardManager>().Preference)
                {
                    TempObj = cat2[i];
                    cat2[i] = cat2[j];
                    cat2[j] = TempObj;
                }
            }
        }

        if (cat2.Count >= 3 && AutoSelect.Count < 3)
        {
            CheckForStraightFlush(cat2);
        }
        if (AutoSelect.Count >= 3)
        {
            return;
        }

        for (int i = 0; i < cat3.Count; i++)
        {
            for (int j = 0; j < cat3.Count; j++)
            {
                if (cat3[i].GetComponent<CardManager>().Preference < cat3[j].GetComponent<CardManager>().Preference)
                {
                    TempObj = cat3[i];
                    cat3[i] = cat3[j];
                    cat3[j] = TempObj;
                }
            }
        }

        if (cat3.Count >= 3 && AutoSelect.Count < 3)
        {
            CheckForStraightFlush(cat3);
        }
        if (AutoSelect.Count >= 3)
        {
            return;
        }

        for (int i = 0; i < cat4.Count; i++)
        {
            for (int j = 0; j < cat4.Count; j++)
            {
                if (cat4[i].GetComponent<CardManager>().Preference < cat4[j].GetComponent<CardManager>().Preference)
                {
                    TempObj = cat4[i];
                    cat4[i] = cat4[j];
                    cat4[j] = TempObj;
                }
            }
        }

        if (cat4.Count >= 3 && AutoSelect.Count < 3)
        {
            CheckForStraightFlush(cat4);
        }
        if (AutoSelect.Count >= 3)
        {
            return;
        }

        if (AutoSelect.Count < 3)
        {
            CheckForSameKind();
        }
    }


    void CheckForStraightFlush(List<GameObject> templist)
    {

        //print("check for straight flush");
        for(int i = 0; i < templist.Count; i++)
        {

            AutoSelect.Add(templist[i]);
           
            for (int j = i; j < templist.Count; j++)
            {
                if (templist[j].GetComponent<CardManager>().Preference == (AutoSelect[AutoSelect.Count - 1].GetComponent<CardManager>().Preference + 1) && !AutoSelect.Contains(templist[j]))
                {
                    AutoSelect.Add(templist[j]);
                }
            }

            if (AutoSelect.Count >= 3)
            {
                break;
            }
            else
            {
                AutoSelect.Clear();
            }

        }

       
    }


    void CheckForSameKind()
    {
        //print("check for same kind");
        for(int i = 0; i < CardsGenerated.Length; i++)
        {
            AutoSelect.Add(CardsGenerated[i]);
            for(int j = i; j < CardsGenerated.Length; j++)
            {
                if(CardsGenerated[j].GetComponent<CardManager>().Preference == CardsGenerated[i].GetComponent<CardManager>().Preference && !AutoSelect.Contains(CardsGenerated[j]))
                {
                    AutoSelect.Add(CardsGenerated[j]);
                }
            }

            if(AutoSelect.Count >=3)
            {
                break;
            }
            else
            {
                AutoSelect.Clear();
            }
        }
    }

    void SelectAutoSelectedCards()
    {
        for(int i = 0; i < AutoSelect.Count; i++)
        {
            AutoSelect[i].GetComponent<CardManager>().Select();
        }
    }

    #endregion

    #region Strategy

   


   public void Strategy()
    {
       // Auto();
        selArr = SelectedCards.ToArray();


        GameObject Strat = (GameObject)Instantiate(Melds, MeldingPoint.transform.position, MeldingPoint.transform.rotation);
        Strat.transform.SetParent(StrategyPoint.transform, false);
        Strat.transform.localPosition = new Vector3(0, TongitsMadeByPlayer * 20, 0);

        Strat.name = "Strategy" + TongitsMadeByPlayer;

        for (int i = 0; i < selArr.Length; i++)
        {
            StartCoroutine(MoveStrategySelected(Strat, i));
            StrategyList.Add(selArr[i]);
        }

        StrategyArr = StrategyList.ToArray();
        TongitsMadeByPlayer++;
        RemoveCardFromArray();
    }

    IEnumerator MoveStrategySelected(GameObject MP, int i)
    {
        selArr[i].transform.SetParent(MP.transform);
        //moving melded cards
        Debug.Log("in the move strategy function");


        for (float timer = 0; timer < 1f; timer += Time.deltaTime)
        {
            selArr[i].GetComponent<RectTransform>().localPosition = Vector3.Lerp(selArr[i].GetComponent<RectTransform>().localPosition,
                                                                                    new Vector3((i * 70), 0, 0), 0.5f);
            selArr[i].GetComponent<RectTransform>().localScale = Vector3.Lerp(selArr[i].GetComponent<RectTransform>().localScale,
                                                                               new Vector3(0.7f, 0.7f, 0.7f), 0.5f);
            yield return null;
        }

        selArr[i].GetComponent<RectTransform>().localPosition = new Vector3((i * 70), 0, 0);
        selArr[i].GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    #endregion

    #region Connect
   

  public void Check_For_Connect(GameObject Meld_Point)
    {
        List<GameObject> ConnectTemplist = new List<GameObject>();
        if(SelectedCards.Count != 0)
        {
            for(int i = 0; i < SelectedCards.Count; i++)
            {
                ConnectTemplist.Add(SelectedCards[i]);
            }

            for(int i = 0; i < Meld_Point.transform.childCount; i++)
            {
                ConnectTemplist.Add(Meld_Point.transform.GetChild(i).gameObject);
            }

            bool _CanBeConnected = false;
            GameObject TempGO;

            for(int i = 0; i < ConnectTemplist.Count; i++)
            {
                for(int j = i; j < ConnectTemplist.Count; j++)
                {
                    if(ConnectTemplist[j].GetComponent<CardManager>().Preference < ConnectTemplist[i].GetComponent<CardManager>().Preference)
                    {
                        TempGO = ConnectTemplist[i];
                        ConnectTemplist[i] = ConnectTemplist[j];
                        ConnectTemplist[j] = TempGO;
                    }
                }
            }

            for(int i = 0; i < ConnectTemplist.Count; i++)
            {
                if(i > 0)
                {
                    if(ConnectTemplist[i].GetComponent<CardManager>().Preference == ConnectTemplist[i-1].GetComponent<CardManager>().Preference)
                    {
                        _CanBeConnected = true;
                    }
                    else if (ConnectTemplist[i].GetComponent<CardManager>().Preference == ConnectTemplist[i - 1].GetComponent<CardManager>().Preference + 1 && ConnectTemplist[i].GetComponent<CardManager>().CategoryWeight == ConnectTemplist[i - 1].GetComponent<CardManager>().CategoryWeight)
                    {
                        _CanBeConnected = true;
                    }
                    else
                    {
                        _CanBeConnected = false;
                        break;
                    }
                }
            }

            if(_CanBeConnected)
            {
                selArr = ConnectTemplist.ToArray();
                for(int i = 0; i < selArr.Length; i++)
                {
                    StartCoroutine(Move_Connecting_Cards(Meld_Point, i));
                }
                for(int i = 0; i < selArr.Length; i++)
                {
                    selArr[i].transform.SetSiblingIndex(i);
                }
                RemoveCardFromArray();

            }
        }

        
    }

    IEnumerator Move_Connecting_Cards(GameObject Target, int i)
    {
        yield return null;

        selArr[i].transform.SetParent(Target.transform);
        //moving melded cards
        Debug.Log("in the move connect function");


        for (float timer = 0; timer < 1f; timer += Time.deltaTime)
        {
            selArr[i].GetComponent<RectTransform>().localPosition = Vector3.Lerp(selArr[i].GetComponent<RectTransform>().localPosition,
                                                                                    new Vector3((i * 50), 0, 0), 0.5f);
            yield return null;
        }

        selArr[i].GetComponent<RectTransform>().localPosition = new Vector3((i * 50), 0, 0);
       
       
        SelectedCards.Clear();
       

    }
    #endregion

   public bool Check_Condition_For_Picking()
    {
        GameObject[] TempArr = new GameObject[CardsGenerated.Length + 1];
        bool _PickFromDump = false;

        if (PlayerGamePlay.Instance.DumpedCards.Count > 0)
        {

            for (int i = 0; i <= CardsGenerated.Length; i++)
            {
                if (i < CardsGenerated.Length)
                    TempArr[i] = CardsGenerated[i];
                else
                {
                    TempArr[i] = PlayerGamePlay.Instance.DumpedCards[PlayerGamePlay.Instance.DumpedCards.Count - 1];
                }
                print("CardsGenerated length" + CardsGenerated.Length + "Preference = " + TempArr[i].GetComponent<CardManager>().Preference + " Weight = " + TempArr[i].GetComponent<CardManager>().weight);
            }


            GameObject TempObj;

            List<GameObject> cat1 = new List<GameObject>();
            List<GameObject> cat2 = new List<GameObject>();
            List<GameObject> cat3 = new List<GameObject>();
            List<GameObject> cat4 = new List<GameObject>();

            for (int i = 0; i < TempArr.Length; i++)
            {
                switch (TempArr[i].GetComponent<CardManager>().CategoryWeight)
                {
                    case 1:
                        cat1.Add(TempArr[i]);
                        break;
                    case 2:
                        cat2.Add(TempArr[i]);

                        break;
                    case 3:
                        cat3.Add(TempArr[i]);

                        break;
                    case 4:
                        cat4.Add(TempArr[i]);

                        break;
                }
            }


            List<GameObject> CheckSelect = new List<GameObject>();
            GameObject target = TempArr[TempArr.Length - 1];
            switch (target.GetComponent<CardManager>().CategoryWeight)
            {
                case 1:


                    for (int i = 0; i < cat1.Count; i++)
                    {
                        for (int j = 0; j < cat1.Count; j++)
                        {
                            if (cat1[i].GetComponent<CardManager>().Preference < cat1[j].GetComponent<CardManager>().Preference)
                            {
                                TempObj = cat1[i];
                                cat1[i] = cat1[j];
                                cat1[j] = TempObj;
                            }
                        }
                    }


                    for (int i = 0; i < cat1.Count; i++)
                    {
                        print("Preference = " + cat1[i].GetComponent<CardManager>().Preference + " Weight = " + cat1[i].GetComponent<CardManager>().weight);
                    }


                    if (cat1.Count >= 3 && CheckSelect.Count < 3)
                    {
                        for (int i = 0; i < cat1.Count; i++)
                        {

                            CheckSelect.Add(cat1[i]);

                            for (int j = i; j < cat1.Count; j++)
                            {
                                if (cat1[j].GetComponent<CardManager>().Preference == (CheckSelect[CheckSelect.Count - 1].GetComponent<CardManager>().Preference + 1) && !CheckSelect.Contains(cat1[j]))
                                {
                                    CheckSelect.Add(cat1[j]);
                                }
                            }

                            if (CheckSelect.Count >= 3 && CheckSelect.Contains(target))
                            {
                                _PickFromDump = true;
                                break;
                            }
                            else
                            {
                                _PickFromDump = false;
                                CheckSelect.Clear();
                            }

                        }
                    }



                    break;

                case 2:


                    for (int i = 0; i < cat2.Count; i++)
                    {
                        for (int j = 0; j < cat2.Count; j++)
                        {
                            if (cat2[i].GetComponent<CardManager>().Preference < cat2[j].GetComponent<CardManager>().Preference)
                            {
                                TempObj = cat2[i];
                                cat2[i] = cat2[j];
                                cat2[j] = TempObj;
                            }
                        }
                    }

                    for (int i = 0; i < cat2.Count; i++)
                    {
                        print("Preference = " + cat2[i].GetComponent<CardManager>().Preference + " Weight = " + cat2[i].GetComponent<CardManager>().weight);
                    }

                    if (cat2.Count >= 3 && CheckSelect.Count < 3)
                    {
                        for (int i = 0; i < cat2.Count; i++)
                        {

                            CheckSelect.Add(cat2[i]);

                            for (int j = i; j < cat2.Count; j++)
                            {
                                if (cat2[j].GetComponent<CardManager>().Preference == (CheckSelect[CheckSelect.Count - 1].GetComponent<CardManager>().Preference + 1) && !CheckSelect.Contains(cat2[j]))
                                {
                                    CheckSelect.Add(cat2[j]);
                                }
                            }

                            if (CheckSelect.Count >= 3 && CheckSelect.Contains(target))
                            {
                                _PickFromDump = true;
                                break;
                            }
                            else
                            {
                                _PickFromDump = false;

                                CheckSelect.Clear();
                            }

                        }
                    }



                    break;

                case 3:



                    for (int i = 0; i < cat3.Count; i++)
                    {
                        for (int j = 0; j < cat3.Count; j++)
                        {
                            if (cat3[i].GetComponent<CardManager>().Preference < cat3[j].GetComponent<CardManager>().Preference)
                            {
                                TempObj = cat3[i];
                                cat3[i] = cat3[j];
                                cat3[j] = TempObj;
                            }
                        }
                    }

                    for (int i = 0; i < cat3.Count; i++)
                    {
                        print("Preference = " + cat3[i].GetComponent<CardManager>().Preference + " Weight = " + cat3[i].GetComponent<CardManager>().weight);
                    }

                    if (cat3.Count >= 3 && CheckSelect.Count < 3)
                    {
                        for (int i = 0; i < cat3.Count; i++)
                        {

                            CheckSelect.Add(cat3[i]);

                            for (int j = i; j < cat3.Count; j++)
                            {
                                if (cat3[j].GetComponent<CardManager>().Preference == (CheckSelect[CheckSelect.Count - 1].GetComponent<CardManager>().Preference + 1) && !CheckSelect.Contains(cat3[j]))
                                {
                                    CheckSelect.Add(cat3[j]);
                                }
                            }

                            if (CheckSelect.Count >= 3 && CheckSelect.Contains(target))
                            {
                                _PickFromDump = true;
                                break;
                            }
                            else
                            {
                                _PickFromDump = false;
                                CheckSelect.Clear();
                            }

                        }
                    }



                    break;

                case 4:



                    for (int i = 0; i < cat4.Count; i++)
                    {
                        for (int j = 0; j < cat4.Count; j++)
                        {
                            if (cat4[i].GetComponent<CardManager>().Preference < cat4[j].GetComponent<CardManager>().Preference)
                            {
                                TempObj = cat4[i];
                                cat4[i] = cat4[j];
                                cat4[j] = TempObj;
                            }
                        }
                    }

                    for (int i = 0; i < cat4.Count; i++)
                    {
                        print("Preference = " + cat4[i].GetComponent<CardManager>().Preference + " Weight = " + cat4[i].GetComponent<CardManager>().weight);
                    }
                    if (cat4.Count >= 3 && CheckSelect.Count < 3)
                    {
                        for (int i = 0; i < cat4.Count; i++)
                        {

                            CheckSelect.Add(cat4[i]);

                            for (int j = i; j < cat4.Count; j++)
                            {
                                if (cat4[j].GetComponent<CardManager>().Preference == (CheckSelect[CheckSelect.Count - 1].GetComponent<CardManager>().Preference + 1) && !CheckSelect.Contains(cat4[j]))
                                {
                                    CheckSelect.Add(cat4[j]);
                                }
                            }

                            if (CheckSelect.Count >= 3 && CheckSelect.Contains(target))
                            {
                                _PickFromDump = true;
                                break;
                            }
                            else
                            {
                                _PickFromDump = false;
                                CheckSelect.Clear();
                            }

                        }
                    }



                    break;

            }




            if (CheckSelect.Count < 3)
            {

                CheckSelect.Add(TempArr[TempArr.Length - 1]);
                for (int j = 0; j < TempArr.Length; j++)
                {
                    if (TempArr[j].GetComponent<CardManager>().Preference == CheckSelect[0].GetComponent<CardManager>().Preference && !CheckSelect.Contains(TempArr[j]))
                    {
                        CheckSelect.Add(TempArr[j]);
                    }
                }

                if (CheckSelect.Count >= 3)
                {
                    _PickFromDump = true;
                }
                else
                {
                    _PickFromDump = false;
                    CheckSelect.Clear();
                }
            }

        }
        else
        {
            _PickFromDump = false;
        }

        return _PickFromDump;
    }


    #region Total Calculation For Winning

    int Weight_In_Hand;
    List<GameObject> TempList = new List<GameObject>();

    public int Calculate_Total()
    {
        TempList.Clear();
        for (int i = 0; i < CardsGenerated.Length; i++)
        {
            TempList.Add(CardsGenerated[i]);
        }

        Total_Weight();
        SortForTotal();
        Decrease_Weight_Of_Tongits();
        return Weight_In_Hand;
    }

    void Total_Weight()
    {
        Weight_In_Hand = 0;
        for (int i = 0; i < TempList.Count; i++)
        {
            Weight_In_Hand += TempList[i].GetComponent<CardManager>().weight;
        }

    }


    void Decrease_Weight_Of_Tongits()
    {
        for (int i = 0; i < SelectedCards.Count; i++)
        {
            Weight_In_Hand -= SelectedCards[i].GetComponent<CardManager>().weight;
        }
        SelectedCards.Clear();

    }


    void SortForTotal()
    {

        ////print("sorting for auto");
        if (TempList.Count > 0)
        {
            GameObject[] TempArr = new GameObject[TempList.Count];

            for (int i = 0; i < TempList.Count; i++)
            {
                TempArr[i] = TempList[i];
            }

            GameObject TempObj;
            List<GameObject> cat1 = new List<GameObject>();
            List<GameObject> cat2 = new List<GameObject>();
            List<GameObject> cat3 = new List<GameObject>();
            List<GameObject> cat4 = new List<GameObject>();

            for (int i = 0; i < TempArr.Length; i++)
            {
                switch (TempArr[i].GetComponent<CardManager>().CategoryWeight)
                {
                    case 1:
                        cat1.Add(TempArr[i]);
                        break;
                    case 2:
                        cat2.Add(TempArr[i]);

                        break;
                    case 3:
                        cat3.Add(TempArr[i]);

                        break;
                    case 4:
                        cat4.Add(TempArr[i]);

                        break;
                }
            }

            if (cat1.Count > 0)
            {
                for (int i = 0; i < cat1.Count; i++)
                {
                    for (int j = 0; j < cat1.Count; j++)
                    {
                        if (cat1[i].GetComponent<CardManager>().Preference < cat1[j].GetComponent<CardManager>().Preference)
                        {
                            TempObj = cat1[i];
                            cat1[i] = cat1[j];
                            cat1[j] = TempObj;
                        }
                    }
                }

                if (cat1.Count >= 3 && SelectedCards.Count < 3)
                {
                    CheckForStraightFlushForTotal(cat1);
                }


            }

            if (cat2.Count > 0)
            {
                for (int i = 0; i < cat2.Count; i++)
                {
                    for (int j = 0; j < cat2.Count; j++)
                    {
                        if (cat2[i].GetComponent<CardManager>().Preference < cat2[j].GetComponent<CardManager>().Preference)
                        {
                            TempObj = cat2[i];
                            cat2[i] = cat2[j];
                            cat2[j] = TempObj;
                        }
                    }
                }

                if (cat2.Count >= 3 && SelectedCards.Count < 3)
                {
                    CheckForStraightFlushForTotal(cat2);
                }


            }

            if (cat3.Count > 0)
            {
                for (int i = 0; i < cat3.Count; i++)
                {
                    for (int j = 0; j < cat3.Count; j++)
                    {
                        if (cat3[i].GetComponent<CardManager>().Preference < cat3[j].GetComponent<CardManager>().Preference)
                        {
                            TempObj = cat3[i];
                            cat3[i] = cat3[j];
                            cat3[j] = TempObj;
                        }
                    }
                }

                if (cat3.Count >= 3 && SelectedCards.Count < 3)
                {
                    CheckForStraightFlushForTotal(cat3);
                }

            }

            if (cat4.Count > 0)
            {
                for (int i = 0; i < cat4.Count; i++)
                {
                    for (int j = 0; j < cat4.Count; j++)
                    {
                        if (cat4[i].GetComponent<CardManager>().Preference < cat4[j].GetComponent<CardManager>().Preference)
                        {
                            TempObj = cat4[i];
                            cat4[i] = cat4[j];
                            cat4[j] = TempObj;
                        }
                    }
                }

                if (cat4.Count >= 3 && SelectedCards.Count < 3)
                {
                    CheckForStraightFlushForTotal(cat4);
                }

            }



            CheckForSameKindForTotal();



            return;
        }

    }


    void CheckForStraightFlushForTotal(List<GameObject> templist)
    {

        print("check for straight flush");
        for (int i = 0; i < templist.Count; i++)
        {

            SelectedCards.Add(templist[i]);

            for (int j = i; j < templist.Count; j++)
            {
                if (templist[j].GetComponent<CardManager>().Preference == (SelectedCards[SelectedCards.Count - 1].GetComponent<CardManager>().Preference + 1) && !SelectedCards.Contains(templist[j]))
                {
                    SelectedCards.Add(templist[j]);
                }
            }

            if (SelectedCards.Count >= 3)
            {
                //   break;
            }
            else
            {
                SelectedCards.Clear();
            }

        }
        return;


    }


    void CheckForSameKindForTotal()
    {
        print("check for same kind");
        SelectedCards.Clear();
        for (int i = 0; i < TempList.Count; i++)
        {
            if (!SelectedCards.Contains(TempList[i])) SelectedCards.Add(TempList[i]);
            for (int j = i; j < CardsGenerated.Length; j++)
            {
                if (TempList[j].GetComponent<CardManager>().Preference == TempList[i].GetComponent<CardManager>().Preference && !SelectedCards.Contains(TempList[j]))
                {
                    SelectedCards.Add(TempList[j]);
                }
            }

            if (SelectedCards.Count >= 3)
            {
                //break;
            }
            else
            {
                SelectedCards.Clear();
            }
        }

        return;
    }



    #endregion
}

