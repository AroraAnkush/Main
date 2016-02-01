using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Artiificial_Player : MonoBehaviour {


    #region public Attributes

    public string AiName;
    public enum State
    {
        Ante,
        Wait,
        Meld,
        Strategise,
        Dump,
        pick,
        win,
        lost,
        Draw
    }

    public State PlayerState;

    public GameObject[] MyCards;
    public GameObject[] MySelectedCards;
    public GameObject[] MyStrategyCards;


    public GameObject MeldingPoint;
    public GameObject StrategyPoint;
    public GameObject DumpPoint;
    public GameObject DeckPoint;
    public GameObject Melds;



    public bool _Ante = false;
    public bool _IAmDealer = false;
    public bool _Waiting = false;
    public bool _MyTurn = false;
    public bool _PickingCard = false;
    public bool _MeldingCard = false;
    public bool _DumpingCard = false;
    public bool _Win = false;
    public bool _lost = false;
    public bool _Fight = false;



    public bool _CanBeMeld = false;
    public bool _straightflush = false;
    public bool _samekind = false;

    public GameObject NextPlayer, PreviousPlayer;
    public GameObject GenPoint;
    public int MyTongits;

    #endregion


    #region Private Attributes

    List<GameObject> StrategyList = new List<GameObject>();
    GameObject[] StrategyArr;
    GameObject[] selArr;
    List<GameObject> SelectedCards = new List<GameObject>();

    bool _sorting = false;


    #endregion
    #region Functions

    public void Start()
    {
        StartCoroutine(Setup());
    }
    IEnumerator Setup()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerState = State.Ante;
      
        InvokeRepeating("Start_Setup", 0.1f, 0.1f);

       // InvokeRepeating("StartGame", 3f, 0.3f);
    }

    bool _InTheState = false;

    
    public void ClearEveryThing()
    {
        for (int i = 0; i < MeldingPoint.transform.childCount; i++)
        {
            Destroy(MeldingPoint.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < StrategyPoint.transform.childCount; i++)
        {
            Destroy(StrategyPoint.transform.GetChild(i).gameObject);
        }
        for(int i = 0; i < MyCards.Length; i++)
        {
            Destroy(MyCards[i]);
        }

        _Ante = false;
        _IAmDealer = false;
        _Waiting = false;
        _MyTurn = false;
        _PickingCard = false;
        _MeldingCard = false;
        _DumpingCard = false;
        _Win = false;
        _lost = false;
        _Fight = false;



        _CanBeMeld = false;
        _straightflush = false;
        _samekind = false;
        _sorting = false;
        StrategyList.Clear();
        StrategyArr = StrategyList.ToArray();
        SelectedCards.Clear();
        selArr = SelectedCards.ToArray();
        MyTongits = 0;

        StartCoroutine(Setup());

    }

    void Start_Setup()
    {
        if (PlayerGamePlay.Instance._GameStarted)
        {
            if (AiName == "Ai1")
            {
                MyCards = PlayerGamePlay.Instance.CardsGeneratedForAI1;
            }
            else if (AiName == "Ai2")
            {
                MyCards = PlayerGamePlay.Instance.CardsGeneratedForAI2;
            }

            if (GenPoint.transform.childCount == 13)
            {
                _IAmDealer = true;
            }
            else
            {
                _IAmDealer = false;
            }

            //    ManageState();
            CancelInvoke("Start_Setup");
          //  Invoke("SortCards", 1f);
           if(_IAmDealer) Invoke("StartGame", 2f);

        }
    }

    void StartGame()
    {

        
        print("In the start game");
        if (!_InTheState && PlayerGamePlay.Instance._GameStarted)
        {
           
            print("In the start game if condition");
            _InTheState = true;
            StartCoroutine(ChangeStates());

        }
        else
        {
            print("in the start game else condition");
            return;
        }
    }


   
    
   public IEnumerator ChangeStates()
    {
        yield return null;

        if (_IAmDealer)
        {
           

            yield return StartCoroutine(Auto());
            yield return null;
            while (_FoundMeld)
            {
                if(!_InTheMeldFunction) yield return StartCoroutine(Auto());
                yield return null;
                if (!_FoundMeld) break;
            }
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(DumpCard());

            

            //////////////////////////////////////////////////////////////////////////////////////////////////
            /// Check For Melding Repeating
            /// Then Check for Dumping Card
            /// pass the chance
            /// ////////////////////////////////////////////////////////////////////////////////////////////////

        }

        else
        {
            print("in the else condition of change state");
            if (_MyTurn)
            {
                if (GameManager.Instance._DrawCalled)
                {
                    yield return StartCoroutine(Draw_Called());
                }
                else
                {
                    yield return StartCoroutine(CheckForPicking());

                    yield return new WaitForEndOfFrame();

                    yield return StartCoroutine(Check_For_Connect());
                    yield return null;
                    while (_Connecting_Card_Found)
                    {
                       if(!_InTheConnectFunction) yield return StartCoroutine(Check_For_Connect());
                        yield return null;
                        if (!_Connecting_Card_Found) break;
                    }


                    yield return null;
                    yield return StartCoroutine(Auto());
                    yield return null;
                    while (_FoundMeld)
                    {
                       if(!_InTheMeldFunction) yield return StartCoroutine(Auto());
                        yield return null;
                        if (!_FoundMeld) break;
                    }
                    yield return new WaitForSeconds(2f) ;
                    yield return StartCoroutine(DumpCard());


                    ///////////////////////////////////////////////////////////////////////////////////////////////////
                    /// Check for Picking Card
                    /// Check for connecting card
                    /// check for melding repeating
                    /// check for dumping card
                    /// pass the chance
                    /// /////////////////////////////////////////////////////////////////////////////////////////////////
                }
            }
            else
            {
                _Waiting = true;
                PlayerState = State.Wait;
                /// ////////////////////////////////////////////////////////////////////////////////////////////////
                /// Wait for the Chance to come
                /// ////////////////////////////////////////////////////////////////////////////////////////////////
            }
        }
      //  ManageState();
    }

    #endregion

    #region Ienumerators



    IEnumerator Draw_Called()
    {
        yield return new WaitForSeconds(0.1f);
        _Fight = true;
        GameManager.Instance.ChangeChance(gameObject);
    }

    IEnumerator Putting_Ante()
    {
        _InTheState = true;

        if(!_Ante)
        {
            _Ante = true;
        }
        
        yield return null;
        _InTheState = false;
    }

    IEnumerator Waiting()
    {
        _InTheState = true;

         yield return new WaitForEndOfFrame();
       
        yield return null;
        _InTheState = false;
    }


    IEnumerator Melding()
    {
        _InTheState = true;

        if(_MeldingCard)
        {
            print("in the melding start function");
            _MeldingCard = false;
            Check_For_Connect();
            // Auto();
        }
       
        yield return null;
      //  _InTheState = false;
    }

  

    IEnumerator Picking ()
    {
        _InTheState = true;
     //   _MyTurn = false;
        if(_PickingCard && !_IAmDealer)
        {
            
            _PickingCard = false;
            StartCoroutine(CheckForPicking());
        }
       
        yield return null;
     //   _InTheState = false;
    }




    IEnumerator Winning()
    {
        _InTheState = true;
        if (_Win)
        {

        }
       
        yield return null;
      //  _InTheState = false;
    }

    IEnumerator Lossing()
    {
        _InTheState = true;
        if (_lost)
        {

        }
       
        yield return null;
       // _InTheState = false;
    }

    

    IEnumerator SetupPositionWhenArrChanged(GameObject[] CardGeneratedTransfer, int i)
    {


        //yield return null;
        //   MyCards[i].transform.parent = null;

        CardGeneratedTransfer[i].transform.SetSiblingIndex(i);

        if (i > 0)
        {
            
            for (float timer = 0; timer < 1f; timer += Time.deltaTime)
            {
                CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition
                    = Vector3.Lerp(CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition,
                                     new Vector3(CardGeneratedTransfer[i - 1].GetComponent<RectTransform>().localPosition.x + 45, 0, 0),
                                     0.5f * timer);
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
                                 0.5f * timer);

                yield return null;
            }
            
            CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        }


        if (i == MyCards.Length - 1)
        {
            _sorting = false;
            // Auto();

         }
    }

    #endregion



    #region sorting

    void SortCards()
    {

        GameObject[] TempArr = new GameObject[MyCards.Length];

        for (int i = 0; i < MyCards.Length; i++)
        {
            TempArr[i] = MyCards[i];
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
        foreach (GameObject go in cat1)
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
            MyCards[i] = TempArr[i];
        }

        for (int i = 0; i < MyCards.Length; i++)
        {
            StartCoroutine(SetupPosition(MyCards, i));
        }




    }


    IEnumerator SetupPosition(GameObject[] CardGeneratedTransfer, int i)
    {


        yield return null;
        //   MyCards[i].transform.parent = null;

        CardGeneratedTransfer[i].transform.SetSiblingIndex(i);

        if (i > 0)
        {
           /* for (float timer = 0; timer < 3f; timer += Time.deltaTime)
            {
                CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition
                    = Vector3.Lerp(CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition,
                                     new Vector3(CardGeneratedTransfer[i - 1].GetComponent<RectTransform>().localPosition.x + 45, 0, 0),
                                     0.1f * timer);

                if (CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition.x > 1200)
                {
                    CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition = new Vector3(1200, 0, 0);
                }
                yield return null;
            }
            */
            CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition = new Vector3(CardGeneratedTransfer[i - 1].GetComponent<RectTransform>().localPosition.x + 45, 0, 0);
        }

        else
        {
         /*   for (float timer = 0; timer < 3f; timer += Time.deltaTime)
            {
                CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition
                = Vector3.Lerp(CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition,
                                 new Vector3(0, 0, 0),
                                 0.1f * timer);

                yield return null;
            }
            */
            CardGeneratedTransfer[i].GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        }


        if (i == MyCards.Length - 1)
            _sorting = false;
        // _GameStarted = true;
    }


    #endregion

    #region Dumping


    IEnumerator DumpCard()
    {
        List<GameObject> CanBeDumped = new List<GameObject>();
        List<GameObject> MayBeDumped = new List<GameObject>();
        for(int i = 0; i < MyCards.Length; i++)
        {
            CanBeDumped.Add(MyCards[i]);
        }

        GameObject temp;


        for (int i = 0; i < CanBeDumped.Count; i++)
        {
            for(int j = 0; j < CanBeDumped.Count; j++)
            {
                if(CanBeDumped[j].GetComponent<CardManager>().Preference > CanBeDumped[i].GetComponent<CardManager>().Preference)
                {
                    temp = CanBeDumped[i];
                    CanBeDumped[i] = CanBeDumped[j];
                    CanBeDumped[j] = temp;
                }
            }
        }


        GameObject ObjectToBeDumped = CanBeDumped[0];

        List<GameObject> cat1 = new List<GameObject>();
        List<GameObject> cat2 = new List<GameObject>();
        List<GameObject> cat3 = new List<GameObject>();
        List<GameObject> cat4 = new List<GameObject>();

        for(int i = 0; i < CanBeDumped.Count; i++)
        {
            switch(CanBeDumped[i].GetComponent<CardManager>().CategoryWeight)
            {
                case 1:
                    cat1.Add(CanBeDumped[i]);
                    break;

                case 2:
                    cat2.Add(CanBeDumped[i]);
                    break;

                case 3:
                    cat3.Add(CanBeDumped[i]);
                    break;

                case 4:
                    cat4.Add(CanBeDumped[i]);
                    break;
            }
        }

        // sort all the category lists
        if(cat1.Count > 0)
        {
            for (int i = 0; i < cat1.Count; i++)
            {
                for (int j = i; j < cat1.Count; j++)
                {
                    if (cat1[i].GetComponent<CardManager>().Preference > cat1[j].GetComponent<CardManager>().Preference)
                    {
                        temp = cat1[i];
                        cat1[i] = cat1[j];
                        cat1[j] = temp;
                    }
                }
            }
        }
        
        if(cat2.Count > 0)
        {
            for (int i = 0; i < cat2.Count; i++)
            {
                for (int j = i; j < cat2.Count; j++)
                {
                    if (cat2[i].GetComponent<CardManager>().Preference > cat2[j].GetComponent<CardManager>().Preference)
                    {
                        temp = cat2[i];
                        cat2[i] = cat2[j];
                        cat2[j] = temp;
                    }
                }
            }

        }

        if(cat3.Count > 0)
        {
            for (int i = 0; i < cat3.Count; i++)
            {
                for (int j = i; j < cat3.Count; j++)
                {
                    if (cat3[i].GetComponent<CardManager>().Preference > cat3[j].GetComponent<CardManager>().Preference)
                    {
                        temp = cat3[i];
                        cat3[i] = cat3[j];
                        cat3[j] = temp;
                    }
                }
            }

        }
        
        if(cat4.Count > 0)
        {
            for (int i = 0; i < cat4.Count; i++)
            {
                for (int j = i; j < cat4.Count; j++)
                {
                    if (cat4[i].GetComponent<CardManager>().Preference > cat4[j].GetComponent<CardManager>().Preference)
                    {
                        temp = cat4[i];
                        cat4[i] = cat4[j];
                        cat4[j] = temp;
                    }
                }
            }

        }

        // search for objects that can't be dumped

        List<GameObject> CantBeDumped = new List<GameObject>();
        if(cat1.Count > 0)
        {
            for (int i = 0; i < cat1.Count; i++)
            {
                CantBeDumped.Add(cat1[i]);
                for (int j = i; j < cat1.Count; j++)
                {
                    if (cat1[j].GetComponent<CardManager>().Preference == CantBeDumped[CantBeDumped.Count - 1].GetComponent<CardManager>().Preference + 1 && CantBeDumped.Contains(cat1[j]))
                    {
                        CantBeDumped.Add(cat1[j]);
                    }
                }

                if (CantBeDumped.Count == 2)
                {
                    for (int x = 0; x < CantBeDumped.Count; x++)
                    {
                        if (!MayBeDumped.Contains(CantBeDumped[x]))
                            MayBeDumped.Add(CantBeDumped[x]);
                        if (CanBeDumped.Contains(CantBeDumped[x])) CanBeDumped.Remove(CantBeDumped[x]);
                    }

                    CantBeDumped.Clear();
                }
                else if (CantBeDumped.Count >= 3)
                {
                    for (int x = 0; x < CantBeDumped.Count; x++)
                    {
                        if (CanBeDumped.Contains(CantBeDumped[x])) CanBeDumped.Remove(CantBeDumped[x]);
                    }
                    CantBeDumped.Clear();
                }
                else
                {
                    CantBeDumped.Clear();
                }

            }
        }
       



        if(cat2.Count > 0)
        {
            for (int i = 0; i < cat2.Count; i++)
            {
                CantBeDumped.Add(cat2[i]);
                for (int j = i; j < cat2.Count; j++)
                {
                    if (cat2[j].GetComponent<CardManager>().Preference == CantBeDumped[CantBeDumped.Count - 1].GetComponent<CardManager>().Preference + 1 && CantBeDumped.Contains(cat2[j]))
                    {
                        CantBeDumped.Add(cat2[j]);
                    }
                }

                if (CantBeDumped.Count == 2)
                {
                    for (int x = 0; x < CantBeDumped.Count; x++)
                    {
                        if (!MayBeDumped.Contains(CantBeDumped[x]))
                            MayBeDumped.Add(CantBeDumped[x]);
                        if (CanBeDumped.Contains(CantBeDumped[x])) CanBeDumped.Remove(CantBeDumped[x]);
                    }

                    CantBeDumped.Clear();
                }
                else if (CantBeDumped.Count >= 3)
                {
                    for (int x = 0; x < CantBeDumped.Count; x++)
                    {
                        if (CanBeDumped.Contains(CantBeDumped[x])) CanBeDumped.Remove(CantBeDumped[x]);
                    }
                    CantBeDumped.Clear();
                }
                else
                {
                    CantBeDumped.Clear();
                }

            }
        }

      


        if(cat3.Count > 0)
        {
            for (int i = 0; i < cat3.Count; i++)
            {
                CantBeDumped.Add(cat3[i]);
                for (int j = i; j < cat3.Count; j++)
                {
                    if (cat3[j].GetComponent<CardManager>().Preference == CantBeDumped[CantBeDumped.Count - 1].GetComponent<CardManager>().Preference + 1 && CantBeDumped.Contains(cat3[j]))
                    {
                        CantBeDumped.Add(cat3[j]);
                    }
                }

                if (CantBeDumped.Count == 2)
                {
                    for (int x = 0; x < CantBeDumped.Count; x++)
                    {
                        if (!MayBeDumped.Contains(CantBeDumped[x]))
                            MayBeDumped.Add(CantBeDumped[x]);
                        if (CanBeDumped.Contains(CantBeDumped[x])) CanBeDumped.Remove(CantBeDumped[x]);
                    }

                    CantBeDumped.Clear();
                }
                else if (CantBeDumped.Count >= 3)
                {
                    for (int x = 0; x < CantBeDumped.Count; x++)
                    {
                        if (CanBeDumped.Contains(CantBeDumped[x])) CanBeDumped.Remove(CantBeDumped[x]);
                    }
                    CantBeDumped.Clear();
                }
                else
                {
                    CantBeDumped.Clear();
                }

            }
        }

      


        if(cat4.Count > 0)
        {
            for (int i = 0; i < cat4.Count; i++)
            {
                CantBeDumped.Add(cat4[i]);
                for (int j = i; j < cat4.Count; j++)
                {
                    if (cat4[j].GetComponent<CardManager>().Preference == CantBeDumped[CantBeDumped.Count - 1].GetComponent<CardManager>().Preference + 1 && CantBeDumped.Contains(cat4[j]))
                    {
                        CantBeDumped.Add(cat4[j]);
                    }
                }

                if (CantBeDumped.Count == 2)
                {
                    for (int x = 0; x < CantBeDumped.Count; x++)
                    {
                        if (!MayBeDumped.Contains(CantBeDumped[x]))
                            MayBeDumped.Add(CantBeDumped[x]);
                        if (CanBeDumped.Contains(CantBeDumped[x])) CanBeDumped.Remove(CantBeDumped[x]);
                    }

                    CantBeDumped.Clear();
                }
                else if (CantBeDumped.Count >= 3)
                {
                    for (int x = 0; x < CantBeDumped.Count; x++)
                    {
                        if (CanBeDumped.Contains(CantBeDumped[x])) CanBeDumped.Remove(CantBeDumped[x]);
                    }
                    CantBeDumped.Clear();
                }
                else
                {
                    CantBeDumped.Clear();
                }

            }

        }




        for (int i = 0; i < MyCards.Length; i++)
        {
            CantBeDumped.Add(MyCards[i]);
            for (int j = i; j < MyCards.Length; j++)
            {
                if (MyCards[j].GetComponent<CardManager>().Preference == MyCards[i].GetComponent<CardManager>().Preference && !CantBeDumped.Contains(MyCards[j]))
                {
                    CantBeDumped.Add(MyCards[j]);
                }
            }

            if (CantBeDumped.Count >= 3)
            {
                for(int x = 0; x < CantBeDumped.Count; x++)
                {
                    if (CanBeDumped.Contains(CantBeDumped[x]))
                        CanBeDumped.Remove(CantBeDumped[x]);
                }
                CantBeDumped.Clear();
            }
            else if(CantBeDumped.Count == 2)
            {
                for (int x = 0; x < CantBeDumped.Count; x++)
                {
                    if (CanBeDumped.Contains(CantBeDumped[x]))
                        CanBeDumped.Remove(CantBeDumped[x]);
                    if (!MayBeDumped.Contains(CantBeDumped[x]))
                        MayBeDumped.Add(CantBeDumped[x]);
                }

                CantBeDumped.Clear();
            }
            else
            {
                CantBeDumped.Clear();
            }
        }

        // dump the top most prioritized object from the main list

        if(CanBeDumped.Count != 0)
        {
            // Dump from Canbedumped list
            // the first element of the list canbedumped will be dumped
            ObjectToBeDumped = CanBeDumped[0];
            SelectedCards.Add(ObjectToBeDumped);
           yield return StartCoroutine(MoveDumpedCard(PlayerGamePlay.Instance.DumpingPoint, ObjectToBeDumped));
           
        }

        else
        {
            //dump from maybedumped list with maximum priority

            for (int i = 0; i < MayBeDumped.Count; i++)
            {
                for (int j = 0; j < MayBeDumped.Count; j++)
                {
                    if (MayBeDumped[j].GetComponent<CardManager>().Preference > MayBeDumped[i].GetComponent<CardManager>().Preference)
                    {
                        temp = MayBeDumped[i];
                        MayBeDumped[i] = MayBeDumped[j];
                        MayBeDumped[j] = temp;
                    }
                }
            }
            ObjectToBeDumped = MayBeDumped[0];
            SelectedCards.Add(ObjectToBeDumped);
            yield return StartCoroutine(MoveDumpedCard(PlayerGamePlay.Instance.DumpingPoint, ObjectToBeDumped));
           
        }


        yield return null;
    }




    IEnumerator MoveDumpedCard(GameObject MP, GameObject Target)
    {
        //yield return null;
        Target.transform.SetParent(MP.transform, false);
        //moving melded cards
        Debug.Log("in the Dumped function");

        
        for (float timer = 0; timer < 1f; timer += Time.deltaTime)
        {
            Target.GetComponent<RectTransform>().localPosition = Vector3.Lerp(Target.GetComponent<RectTransform>().localPosition,
                                                                                    new Vector3(0, 0, 0), 0.5f * timer);
            yield return null;
        }
        
        PlayerGamePlay.Instance.DumpedCards.Add(Target);

        Target.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

        GameManager.Instance.ChangeChance(gameObject);
        yield return new WaitForSeconds(1f);
        RemoveCardFromArray();
        
    
    }


    #endregion


    bool _FoundMeld = false;
    bool _InTheMeldFunction = false;
    #region Meld State Selection

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Sort the Cards according to the category and then weight in the category
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////


    public IEnumerator Auto()
    {
        _InTheMeldFunction = true;
        SortForAuto();
        if (SelectedCards.Count >= 3)
        {
            yield return StartCoroutine(SelectSelectedCards());
            yield return new WaitForSeconds(1f);
            RemoveCardFromArray();
            _InTheMeldFunction = false;
        }
        else
        {
            SelectedCards.Clear();
            _FoundMeld = false;
            _InTheMeldFunction = false;
        }

        yield return null;

      //  SelectedCards.Clear();
    }




    void SortForAuto()
    {
        
        ////print("sorting for auto");
        if(MyCards.Length > 0)
        {
            GameObject[] TempArr = new GameObject[MyCards.Length];

            for (int i = 0; i < MyCards.Length; i++)
            {
                TempArr[i] = MyCards[i];
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
                    CheckForStraightFlush(cat1);
                }
                if (SelectedCards.Count >= 3)
                {
                    return;
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
                    CheckForStraightFlush(cat2);
                }
                if (SelectedCards.Count >= 3)
                {
                    return;
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
                    CheckForStraightFlush(cat3);
                }
                if (SelectedCards.Count >= 3)
                {
                    return;
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
                    CheckForStraightFlush(cat4);
                }
                if (SelectedCards.Count >= 3)
                {
                    return;
                }
            }


            if (SelectedCards.Count < 3)
            {
                CheckForSameKind();
            }


            if (SelectedCards.Count >= 3)
            {
                return;
            }
            else
            {
                return;
            }
        }
      
    }



    void CheckForStraightFlush(List<GameObject> templist)
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
                break;
            }
            else
            {
                SelectedCards.Clear();
            }

        }
        return;


    }


    void CheckForSameKind()
    {
        print("check for same kind");
        SelectedCards.Clear();
        for (int i = 0; i < MyCards.Length; i++)
        {
            SelectedCards.Add(MyCards[i]);
            for (int j = i; j < MyCards.Length; j++)
            {
                if (MyCards[j].GetComponent<CardManager>().Preference == MyCards[i].GetComponent<CardManager>().Preference && !SelectedCards.Contains(MyCards[j]))
                {
                    SelectedCards.Add(MyCards[j]);
                }
            }

            if (SelectedCards.Count >= 3)
            {
                break;
            }
            else
            {
                SelectedCards.Clear();
            }
        }

        return;
    }

    IEnumerator SelectSelectedCards()
    {
        for (int i = 0; i < SelectedCards.Count; i++)
        {
            SelectedCards[i].GetComponent<CardManager>().Select();
            
        }
        selArr = SelectedCards.ToArray();

        GameObject Meld = (GameObject)Instantiate(Melds, MeldingPoint.transform.position, MeldingPoint.transform.rotation);
        Meld.transform.SetParent(MeldingPoint.transform, false);
        Meld.transform.localPosition = new Vector3(0, MyTongits * 50, 0);

        Meld.name = "Meld" + MyTongits;

        for (int i = 0; i < SelectedCards.Count; i++)
        {
         StartCoroutine(MoveMeldedCards(Meld, i));
        }
        yield return null;
      
        //SelectedCards.Clear();
       
    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Now Check for the conditions of the melding if completed then meld else change the state to dumping
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




    public void RemoveCardFromArray()
    {
        List<GameObject> Temp = new List<GameObject>();

        for (int i = 0; i < MyCards.Length; i++)
        {
            Temp.Add(MyCards[i]);
        }
        foreach (GameObject sc in SelectedCards)
        {
            if (Temp.Contains(sc)) Temp.Remove(sc);
        }

        MyCards = Temp.ToArray();

/*
        for (int i = 0; i < MyCards.Length; i++)
        {
            StartCoroutine(SetupPositionWhenArrChanged(MyCards, i));
        }
        */
        SelectedCards.Clear();
        selArr = SelectedCards.ToArray();
        
    }



    IEnumerator MoveMeldedCards(GameObject MP, int i)
    {
       yield return null;

        selArr[i].transform.SetParent(MP.transform, false);
        MP.transform.SetAsFirstSibling();
        //moving melded cards
        Debug.Log("in the move melded function");

       
        for (float timer = 0; timer < 1f; timer += Time.deltaTime)
        {
            selArr[i].GetComponent<RectTransform>().localPosition = Vector3.Lerp(selArr[i].GetComponent<RectTransform>().localPosition,
                                                                                    new Vector3((i), MyTongits, 0), 0.5f * timer);
            yield return null;
        }
        
        selArr[i].GetComponent<RectTransform>().localPosition = new Vector3((i * 50), 0, 0);

        if (i == SelectedCards.Count - 1)
        {
            MyTongits++;
            _MeldingCard = true;
           
           
            _FoundMeld = true;            
        }
    }



    #endregion


    #region Strategy



    public void Strategy()
    {
       // Auto();
        selArr = SelectedCards.ToArray();

        GameObject Meld = (GameObject)Instantiate(Melds, MeldingPoint.transform.position, MeldingPoint.transform.rotation);
        Meld.transform.SetParent(StrategyPoint.transform, false);
        Meld.transform.localPosition = Vector3.zero;
        Meld.name = "strategy" + MyTongits;

        for (int i = 0; i < selArr.Length; i++)
        {
            StartCoroutine(MoveStrategySelected(Meld, i));
            StrategyList.Add(selArr[i]);
        }

        StrategyArr = StrategyList.ToArray();
        if(AiName == "Ai1")
            PlayerGamePlay.Instance.TongitsMadeByAi1++;
        else
            PlayerGamePlay.Instance.TongitsMadeByAi2++;

        RemoveCardFromArray();
      //  Auto();
    }

    IEnumerator MoveStrategySelected(GameObject MP, int i)
    {
        selArr[i].transform.SetParent(MP.transform, false);
        //moving melded cards
        Debug.Log("in the move strategy function");


        for (float timer = 0; timer < 1f; timer += Time.deltaTime)
        {
            selArr[i].GetComponent<RectTransform>().localPosition = Vector3.Lerp(selArr[i].GetComponent<RectTransform>().localPosition,
                                                                                    new Vector3((i * 70), MyTongits * 70, 0), 0.5f);
            selArr[i].GetComponent<RectTransform>().localScale = Vector3.Lerp(selArr[i].GetComponent<RectTransform>().localScale,
                                                                               new Vector3(0.7f, 0.7f, 0.7f), 0.5f);
            yield return null;
        }

        selArr[i].GetComponent<RectTransform>().localPosition = new Vector3((i * 70), MyTongits * 70, 0);
        selArr[i].GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }





    #endregion


    #region Picking



    IEnumerator CheckForPicking()
    {

        GameObject[] TempArr = new GameObject[MyCards.Length + 1];
        bool _PickFromDump = false;

        if (PlayerGamePlay.Instance.DumpedCards.Count > 0)
        {

            for (int i = 0; i <= MyCards.Length; i++)
            {
                if (i < MyCards.Length)
                    TempArr[i] = MyCards[i];
                else
                {
                    TempArr[i] = PlayerGamePlay.Instance.DumpedCards[PlayerGamePlay.Instance.DumpedCards.Count - 1];
                }
                print("mycards length" + MyCards.Length + "Preference = " + TempArr[i].GetComponent<CardManager>().Preference + " Weight = " + TempArr[i].GetComponent<CardManager>().weight);
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




        if (_PickFromDump)
        {
            StartCoroutine(PickFromDump());
        }
        else
        {
            StartCoroutine(PickFromDeck());
        }



        yield return null;
        _PickingCard = false;
    }



    IEnumerator PickFromDeck()
    {

        if (PlayerGamePlay.Instance.Deck.Count > 0)
        {
            List<GameObject> Temp = new List<GameObject>();

            for (int i = 0; i < MyCards.Length; i++)
            {
                Temp.Add(MyCards[i]);
            }
            GameObject Tempobj = (GameObject)Instantiate(PlayerGamePlay.Instance.CardPrefab, DeckPoint.transform.position, DeckPoint.transform.rotation);
            Tempobj.transform.SetParent(DeckPoint.transform, false);
            Tempobj.transform.localPosition = new Vector3(0, 0, 0);
            Temp.Add(Tempobj);
            Tempobj.GetComponent<Image>().sprite = PlayerGamePlay.Instance.Deck[PlayerGamePlay.Instance.Deck.Count - 1];
            PlayerGamePlay.Instance.Deck.Remove(PlayerGamePlay.Instance.Deck[PlayerGamePlay.Instance.Deck.Count - 1]);
            MyCards = Temp.ToArray();
            MyCards[MyCards.Length - 1].transform.SetParent(GenPoint.transform, false);
            yield return StartCoroutine(SetupPositionWhenArrChanged(MyCards, MyCards.Length - 1));
            yield return new WaitForSeconds(2f);
            _MeldingCard = true;
            PlayerState = State.Meld;
            
        }


        yield return null;
    }

    IEnumerator PickFromDump()
    {


        if (PlayerGamePlay.Instance.DumpedCards.Count > 0)
        {
            List<GameObject> Temp = new List<GameObject>();

            for (int i = 0; i < MyCards.Length; i++)
            {
                Temp.Add(MyCards[i]);
            }

            Temp.Add(PlayerGamePlay.Instance.DumpedCards[PlayerGamePlay.Instance.DumpedCards.Count - 1]);
            PlayerGamePlay.Instance.DumpedCards.Remove(PlayerGamePlay.Instance.DumpedCards[PlayerGamePlay.Instance.DumpedCards.Count - 1]);
            MyCards = Temp.ToArray();
            MyCards[MyCards.Length - 1].transform.SetParent(GenPoint.transform, false);
            yield return StartCoroutine(SetupPositionWhenArrChanged(MyCards, MyCards.Length - 1));
            yield return new WaitForSeconds(2f);
            _MeldingCard = true;
            PlayerState = State.Meld;
           
        }
        yield return null;
    }



    #endregion



    #region Total Calculation For Winning

    int Weight_In_Hand;
    List<GameObject> TempList = new List<GameObject>();
   
   public int Calculate_Total()
    {
       
            for (int i = 0; i < MyCards.Length; i++)
            {
                TempList.Add(MyCards[i]);
            }
       
        Total_Weight();
        SortForTotal();
        Decrease_Weight_Of_Tongits();
        return Weight_In_Hand;
    }

    void Total_Weight()
    {
        Weight_In_Hand = 0;
        for(int i = 0; i < TempList.Count; i++)
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
            for (int j = i; j < TempList.Count; j++)
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


    #region Connect
    GameObject Target;
    bool _Connecting_Card_Found = false;
    bool _InTheConnectFunction = false;
    public IEnumerator Check_For_Connect()
    {

        _InTheConnectFunction = true;
       for(int i = 0; i < GenPoint.transform.childCount; i++)
        {
            for(int j = 0; j < MeldingPoint.transform.childCount; j++)
            {
                for(int k = 0; k < MeldingPoint.transform.GetChild(j).childCount; k++)
                {
                    if(k > 0)
                    {
                        if (GenPoint.transform.GetChild(i).GetComponent<CardManager>().Preference == MeldingPoint.transform.GetChild(j).GetChild(k).GetComponent<CardManager>().Preference + 1
                            && MeldingPoint.transform.GetChild(j).GetChild(k).GetComponent<CardManager>().Preference == MeldingPoint.transform.GetChild(j).GetChild(k - 1).GetComponent<CardManager>().Preference + 1
                        && GenPoint.transform.GetChild(i).GetComponent<CardManager>().CategoryWeight == MeldingPoint.transform.GetChild(j).GetChild(k).GetComponent<CardManager>().CategoryWeight)
                        {
                            SelectedCards.Add(GenPoint.transform.GetChild(i).gameObject);
                            Target = MeldingPoint.transform.GetChild(j).gameObject;
                            break;
                        }
                        else if (k < MeldingPoint.transform.GetChild(j).childCount - 1 &&
                            GenPoint.transform.GetChild(i).GetComponent<CardManager>().Preference == MeldingPoint.transform.GetChild(j).GetChild(k).GetComponent<CardManager>().Preference - 1
                            && MeldingPoint.transform.GetChild(j).GetChild(k).GetComponent<CardManager>().Preference == MeldingPoint.transform.GetChild(j).GetChild(k + 1).GetComponent<CardManager>().Preference - 1
                            && GenPoint.transform.GetChild(i).GetComponent<CardManager>().CategoryWeight == MeldingPoint.transform.GetChild(j).GetChild(k).GetComponent<CardManager>().CategoryWeight)
                        {
                            SelectedCards.Add(GenPoint.transform.GetChild(i).gameObject);
                            Target = MeldingPoint.transform.GetChild(j).gameObject;
                            break;
                        }
                        else if (GenPoint.transform.GetChild(i).GetComponent<CardManager>().Preference == MeldingPoint.transform.GetChild(j).GetChild(k).GetComponent<CardManager>().Preference
                                  && MeldingPoint.transform.GetChild(j).GetChild(k).GetComponent<CardManager>().Preference == MeldingPoint.transform.GetChild(j).GetChild(k - 1).GetComponent<CardManager>().Preference)
                        {
                            SelectedCards.Add(GenPoint.transform.GetChild(i).gameObject);
                            Target = MeldingPoint.transform.GetChild(j).gameObject;
                            break;
                        }
                    }

                  /*  
                    */
                }
            }
        }


       if(SelectedCards.Count == 0)
        {
            for (int i = 0; i < GenPoint.transform.childCount; i++)
            {
                for (int j = 0; j < PreviousPlayer.transform.FindChild("Melding Point").childCount; j++)
                {
                    for (int k = 0; k < PreviousPlayer.transform.FindChild("Melding Point").GetChild(j).childCount; k++)
                    {
                        if( k > 0)
                        {
                            if (GenPoint.transform.GetChild(i).GetComponent<CardManager>().Preference == PreviousPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k).GetComponent<CardManager>().Preference + 1
                                && PreviousPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k).GetComponent<CardManager>().Preference == PreviousPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k - 1).GetComponent<CardManager>().Preference + 1
                            && GenPoint.transform.GetChild(i).GetComponent<CardManager>().CategoryWeight == PreviousPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k).GetComponent<CardManager>().CategoryWeight)
                            {
                                SelectedCards.Add(GenPoint.transform.GetChild(i).gameObject);
                                Target = PreviousPlayer.transform.FindChild("Melding Point").GetChild(j).gameObject;
                                break;
                            }
                            else if (k < PreviousPlayer.transform.FindChild("Melding Point").GetChild(j).childCount -1
                                && GenPoint.transform.GetChild(i).GetComponent<CardManager>().Preference == PreviousPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k).GetComponent<CardManager>().Preference - 1
                                && PreviousPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k).GetComponent<CardManager>().Preference == PreviousPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k + 1).GetComponent<CardManager>().Preference - 1
                                && GenPoint.transform.GetChild(i).GetComponent<CardManager>().CategoryWeight == PreviousPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k).GetComponent<CardManager>().CategoryWeight)
                            {
                                SelectedCards.Add(GenPoint.transform.GetChild(i).gameObject);
                                Target = PreviousPlayer.transform.FindChild("Melding Point").GetChild(j).gameObject;
                                break;
                            }
                            else if (GenPoint.transform.GetChild(i).GetComponent<CardManager>().Preference == PreviousPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k).GetComponent<CardManager>().Preference
                                && PreviousPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k).GetComponent<CardManager>().Preference == PreviousPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k - 1).GetComponent<CardManager>().Preference)
                            {
                                SelectedCards.Add(GenPoint.transform.GetChild(i).gameObject);
                                Target = PreviousPlayer.transform.FindChild("Melding Point").GetChild(j).gameObject;
                                break;
                            }
                        }
                        
                    }
                }
            }
        }


        if(SelectedCards.Count == 0)
        {
            for (int i = 0; i < GenPoint.transform.childCount; i++)
            {
                for (int j = 0; j < NextPlayer.transform.FindChild("Melding Point").childCount; j++)
                {
                    for (int k = 0; k < NextPlayer.transform.FindChild("Melding Point").GetChild(j).childCount; k++)
                    {
                        if( k > 0)
                        {
                            if (GenPoint.transform.GetChild(i).GetComponent<CardManager>().Preference == NextPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k).GetComponent<CardManager>().Preference + 1
                         && NextPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k).GetComponent<CardManager>().Preference == NextPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k - 1).GetComponent<CardManager>().Preference + 1
                          && GenPoint.transform.GetChild(i).GetComponent<CardManager>().CategoryWeight == NextPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k).GetComponent<CardManager>().CategoryWeight)
                            {
                                SelectedCards.Add(GenPoint.transform.GetChild(i).gameObject);
                                Target = NextPlayer.transform.FindChild("Melding Point").GetChild(j).gameObject;
                                break;
                            }
                            else if (k < NextPlayer.transform.FindChild("Melding Point").GetChild(j).childCount - 1
                                && GenPoint.transform.GetChild(i).GetComponent<CardManager>().Preference == NextPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k).GetComponent<CardManager>().Preference - 1
                               && NextPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k).GetComponent<CardManager>().Preference == NextPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k + 1).GetComponent<CardManager>().Preference - 1
                                && GenPoint.transform.GetChild(i).GetComponent<CardManager>().CategoryWeight == NextPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k).GetComponent<CardManager>().CategoryWeight)
                            {
                                SelectedCards.Add(GenPoint.transform.GetChild(i).gameObject);
                                Target = NextPlayer.transform.FindChild("Melding Point").GetChild(j).gameObject;
                                break;
                            }
                            else if (k != 0 && GenPoint.transform.GetChild(i).GetComponent<CardManager>().Preference == NextPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k).GetComponent<CardManager>().Preference
                                && NextPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k).GetComponent<CardManager>().Preference == NextPlayer.transform.FindChild("Melding Point").GetChild(j).GetChild(k - 1).GetComponent<CardManager>().Preference)
                            {
                                SelectedCards.Add(GenPoint.transform.GetChild(i).gameObject);
                                Target = NextPlayer.transform.FindChild("Melding Point").GetChild(j).gameObject;
                                break;
                            }
                        }
                      
                    }
                }
            }
        }


        if (SelectedCards.Count > 0)
        {
            List<GameObject> Connect_List = new List<GameObject>();

            for(int i = 0; i < SelectedCards.Count; i++)
            {
                Connect_List.Add(SelectedCards[i]);
            }
            for(int i = 0; i < Target.transform.childCount; i++)
            {
                Connect_List.Add(Target.transform.GetChild(i).gameObject);
            }
            GameObject TempGO;

            for(int i = 0; i < Connect_List.Count; i++)
            {
                for(int j = i; j < Connect_List.Count; j++)
                {
                    if(Connect_List[i].GetComponent<CardManager>().Preference > Connect_List[j].GetComponent<CardManager>().Preference)
                    {
                        TempGO = Connect_List[i];
                        Connect_List[i] = Connect_List[j];
                        Connect_List[j] = TempGO;
                    }
                }
            }
            selArr = new GameObject[Connect_List.Count];
            selArr = Connect_List.ToArray();

            for (int i = 0; i < selArr.Length; i++)
            {
               StartCoroutine(Move_Connecting_Cards(Target, i));
             
            }

            yield return new WaitForSeconds(1f);
            RemoveCardFromArray();
            _InTheConnectFunction = false;
        }
        else
        {
            SelectedCards.Clear();
            _Connecting_Card_Found = false;
            _InTheConnectFunction = false;
        }
    }

    IEnumerator Move_Connecting_Cards(GameObject Target, int i)
    {
        
      //  yield return null;

        selArr[i].transform.SetParent(Target.transform, false);
        selArr[i].transform.SetAsLastSibling();
        //moving melded cards
        Debug.Log("in the move connect function");



            for (float timer = 0; timer < 1f; timer += Time.deltaTime)
            {
                selArr[i].GetComponent<RectTransform>().localPosition = Vector3.Lerp(selArr[i].GetComponent<RectTransform>().localPosition,
                                                                                        new Vector3((i * 50), 0, 0), 0.5f);
                yield return null;
            }

            selArr[i].GetComponent<RectTransform>().localPosition = new Vector3((i * 50), 0, 0);
        _Connecting_Card_Found = true;
      
      
    }
    #endregion

}
