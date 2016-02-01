using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.iOS;

public class GameManager : Singleton<GameManager> {

    public GameObject[] Players;
    public int[] PlayerWeight;

    public bool _clockwise = true;

    public string WhichPlayerTurn;

    public bool _DrawCalled = false;

    public GameObject Winner;

    public GameObject Conclusion_Panel;

    public Text Winner_Name;

    public Text Loser_1_Name;

    public Text Loser_2_Name;

    public Text Winner_Points;

    public Text Loser_1_Points;

    public Text Loser_2_Points;

   // public Text conclusion_text;


    public GameObject Winner_Gen_Point;

    public GameObject Loser1_Gen_Point;

    public GameObject Loser2_Gen_Point;


    void Start()
    {
        StartUp();
    }

    void StartUp()
    {
        PlayerWeight = new int[Players.Length];

        for(int i = 0; i < Players.Length; i++)
        {
            if(_clockwise)
            {
                switch(i)
                {
                    case 0:
                        if(Players[i].GetComponent<PlayerGamePlay>())
                        {
                            Players[i].GetComponent<PlayerGamePlay>().PreviousPlayer = Players[2];
                            Players[i].GetComponent<PlayerGamePlay>().NextPlayer = Players[1];
                        }
                      
                        break;
                    case 1:
                        if (Players[i].GetComponent<Artiificial_Player>())
                        {
                            Players[i].GetComponent<Artiificial_Player>().PreviousPlayer = Players[0];
                            Players[i].GetComponent<Artiificial_Player>().NextPlayer = Players[2];
                        }

                        break;
                    case 2:
                        if (Players[i].GetComponent<Artiificial_Player>())
                        {
                            Players[i].GetComponent<Artiificial_Player>().PreviousPlayer = Players[1];
                            Players[i].GetComponent<Artiificial_Player>().NextPlayer = Players[0];
                        }

                        break;



                }
               
            }

            else
            {
                switch (i)
                {
                    case 0:
                        if (Players[i].GetComponent<PlayerGamePlay>())
                        {
                            Players[i].GetComponent<PlayerGamePlay>().PreviousPlayer = Players[1];
                            Players[i].GetComponent<PlayerGamePlay>().NextPlayer = Players[2];
                        }

                        break;
                    case 1:
                        if (Players[i].GetComponent<Artiificial_Player>())
                        {
                            Players[i].GetComponent<Artiificial_Player>().PreviousPlayer = Players[2];
                            Players[i].GetComponent<Artiificial_Player>().NextPlayer = Players[0];
                        }

                        break;
                    case 2:
                        if (Players[i].GetComponent<Artiificial_Player>())
                        {
                            Players[i].GetComponent<Artiificial_Player>().PreviousPlayer = Players[0];
                            Players[i].GetComponent<Artiificial_Player>().NextPlayer = Players[1];
                        }

                        break;



                }
            }

            if(Players[i].GetComponent<PlayerGamePlay>())
            {
                if(Players[i].GetComponent<PlayerGamePlay>().GenPoint.childCount == 13)
                {
                    Players[i].GetComponent<PlayerGamePlay>()._MyTurn = true;
                }
            }
            else
            {
                if (Players[i].GetComponent<Artiificial_Player>().GenPoint.transform.childCount == 13)
                {
                    Players[i].GetComponent<Artiificial_Player>()._MyTurn = true;
                }
            }
        }

       // if (PlayerGamePlay.Instance._GameStarted)
    }


   public void ChangeChance(GameObject Sender)
    {
        Winning_Check();

        if(Winner == null)
        {
            if (Sender.GetComponent<PlayerGamePlay>())
            {
                if (Sender.GetComponent<PlayerGamePlay>().NextPlayer.GetComponent<Artiificial_Player>())
                {
                    Sender.GetComponent<PlayerGamePlay>()._MyTurn = false;
                    Sender.GetComponent<PlayerGamePlay>().NextPlayer.GetComponent<Artiificial_Player>()._MyTurn = true;
                    StartCoroutine(Sender.GetComponent<PlayerGamePlay>().NextPlayer.GetComponent<Artiificial_Player>().ChangeStates());
                    Invoke("Sender.GetComponent<PlayerGamePlay>().NextPlayer.GetComponent<Artiificial_Player>().StartGame", 0.1f);
                   
                }
                else
                {
                    Sender.GetComponent<PlayerGamePlay>()._MyTurn = false;
                    Sender.GetComponent<PlayerGamePlay>().NextPlayer.GetComponent<PlayerGamePlay>()._MyTurn = true;
                    Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<PlayerGamePlay>().Picking_Option_Buttons.SetActive(true);
                    if (!Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<PlayerGamePlay>().Last_Meld_turn) Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<PlayerGamePlay>().Draw_Button.SetActive(true);
                    else Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<PlayerGamePlay>().Draw_Button.SetActive(false);
                    if (Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<PlayerGamePlay>().Check_Condition_For_Picking()) Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<PlayerGamePlay>().Pick_Dump_Button.SetActive(true);
                    else Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<PlayerGamePlay>().Pick_Dump_Button.SetActive(false);
                }
            }
            else
            {
                if (Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<Artiificial_Player>())
                {
                    Sender.GetComponent<Artiificial_Player>()._MyTurn = false;
                    Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<Artiificial_Player>()._MyTurn = true;
                    StartCoroutine(Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<Artiificial_Player>().ChangeStates());
                    Invoke("Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<Artiificial_Player>().StartGame", 0.1f);

                }
                else
                {
                    Sender.GetComponent<Artiificial_Player>()._MyTurn = false;
                    Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<PlayerGamePlay>()._MyTurn = true;
                    Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<PlayerGamePlay>().Picking_Option_Buttons.SetActive(true);
                    if (!Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<PlayerGamePlay>().Last_Meld_turn) Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<PlayerGamePlay>().Draw_Button.SetActive(true);
                    else Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<PlayerGamePlay>().Draw_Button.SetActive(false);
                    if (Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<PlayerGamePlay>().Check_Condition_For_Picking()) Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<PlayerGamePlay>().Pick_Dump_Button.SetActive(true);
                    else Sender.GetComponent<Artiificial_Player>().NextPlayer.GetComponent<PlayerGamePlay>().Pick_Dump_Button.SetActive(false);

                }
            }
        }
       
    }

   
    public void Ante_Apply()
    {
        Players[1].GetComponent<Artiificial_Player>()._Ante = true;
        Players[2].GetComponent<Artiificial_Player>()._Ante = true;
        PlayerGamePlay.Instance.Distribute_Cards();
       


    }


    void Winning_Check()
    {
        if (PlayerGamePlay.Instance._GameStarted)
        {
            
            
                if (PlayerGamePlay.Instance.Deck.Count == 0)
                {
                    Winning_By_DeckRunOut();
                }
                if (Players[0].GetComponent<PlayerGamePlay>().GenPoint.transform.childCount == 0 || Players[1].GetComponent<Artiificial_Player>().GenPoint.transform.childCount == 0 || Players[2].GetComponent<Artiificial_Player>().GenPoint.transform.childCount == 0)
                {

                    print("in the winning check else condition");
                    Winning_By_Tongits();
                }
            
        }
        
        
        
    }

    void Winning_By_Tongits()
    {
        PlayerGamePlay.Instance._GameStarted = false;
        Players[0].GetComponent<PlayerGamePlay>()._MyTurn = false;
        Players[1].GetComponent<Artiificial_Player>()._MyTurn = false;
        Players[2].GetComponent<Artiificial_Player>()._MyTurn = false;

        print("in the winning by tongits");

        playertemp = new GameObject[Players.Length];
        for (int i = 0; i < Players.Length; i++)
        {
            playertemp[i] = Players[i];
        }

        for (int i = 0; i < Players.Length; i++)
        {
            Calculate_InHand_Weight(Players[i], i);
        }

        for (int i = 0; i < PlayerWeight.Length; i++)
            {
                for (int j = i; j < PlayerWeight.Length; j++)
                {
                    if (PlayerWeight[j] < PlayerWeight[i])
                    {
                        int x = PlayerWeight[i];
                        PlayerWeight[i] = PlayerWeight[j];
                        PlayerWeight[j] = x;

                        GameObject temp = playertemp[i];
                        playertemp[i] = playertemp[j];
                        playertemp[j] = temp;
                    }
                }
            }

            // playertemp[0] is winner
            Winner = playertemp[0];
           // conclusion_text.text = "Win By Tongits";
             Show_Conclusion();
            CancelInvoke("Winning_Check");

      

    }


    public GameObject[] playertemp;
    void Winning_By_DeckRunOut()
    {
        playertemp = new GameObject[Players.Length];

        for(int i =0; i < Players.Length; i++)
        {
            playertemp[i] = Players[i];
        }

        if(PlayerGamePlay.Instance.Deck.Count == 0)
        {
            for(int i = 0; i < Players.Length; i++)
            {
                Calculate_InHand_Weight(Players[i], i);
            }

            for(int i = 0; i < PlayerWeight.Length; i++)
            {
                for(int j= i; j < PlayerWeight.Length; j++)
                {
                    if(PlayerWeight[j] < PlayerWeight[i])
                    {
                        int x = PlayerWeight[i];
                        PlayerWeight[i] = PlayerWeight[j];
                        PlayerWeight[j] = x;

                        GameObject temp = playertemp[i];
                        playertemp[i] = playertemp[j];
                        playertemp[j] = temp;
                    }
                }
            }

            // playertemp[0] is winner
            Winner = playertemp[0];
            PlayerGamePlay.Instance.DeckPoint.SetActive(false);

            Show_Conclusion();
         //   conclusion_text.text = "Win By Deck Run Out";

            CancelInvoke("Winning_Check");

        }
    }



   public void Draw_Handler(GameObject DrawCaller)
    {
        Winning_By_Draw(DrawCaller);
    }
    void Winning_By_Draw(GameObject DrawCaller)
    {
        playertemp = new GameObject[Players.Length];

        for (int i = 0; i < Players.Length; i++)
        {
            playertemp[i] = Players[i];
        }
        
        for (int i = 0; i < Players.Length; i++)
        {
            Calculate_InHand_Weight(Players[i], i);
        }

        for (int i = 0; i < PlayerWeight.Length; i++)
        {
            for (int j = i; j < PlayerWeight.Length; j++)
            {
                if (PlayerWeight[j] > PlayerWeight[i])
                {
                    int x = PlayerWeight[i];
                    PlayerWeight[i] = PlayerWeight[j];
                    PlayerWeight[j] = x;

                    GameObject temp = playertemp[i];
                    playertemp[i] = playertemp[j];
                    playertemp[j] = temp;
                }
            }
        }
        int caller_hand = 0;

        for(int i = 0; i < playertemp.Length; i++)
        {
            if(playertemp[i] == DrawCaller)
            {
                caller_hand = PlayerWeight[i];
            }
        }

        for(int i = 0; i < playertemp.Length; i++)
        {

            for(int j= i; j < playertemp.Length; j++)
            {
                if(PlayerWeight[j] < PlayerWeight[i])
                {
                    int tempint = PlayerWeight[i];
                    PlayerWeight[i] = PlayerWeight[j];
                    PlayerWeight[j] = tempint;

                    GameObject temp = playertemp[i];
                    playertemp[i] = playertemp[j];
                    playertemp[j] = temp;
                }

                else if(PlayerWeight[j] == PlayerWeight[i] && playertemp[i] == DrawCaller)
                {

                }
                else if(PlayerWeight[j] == PlayerWeight[i] && playertemp[j] == DrawCaller)
                {
                    int tempint = PlayerWeight[i];
                    PlayerWeight[i] = PlayerWeight[j];
                    PlayerWeight[j] = tempint;

                    GameObject temp = playertemp[i];
                    playertemp[i] = playertemp[j];
                    playertemp[j] = temp;
                }
                
            }
  
        }
        Winner = playertemp[0];
        Show_Conclusion();
       // conclusion_text.text = "Win By Draw";

        DrawCaller.GetComponent<PlayerGamePlay>().CancelInvoke("Check_For_Draw_Return");
    }


    void Calculate_InHand_Weight(GameObject Target, int i)
    {
        PlayerWeight[i] = 0;
        if(Target.name == "Player")
        {

            // auto functionality of player
            PlayerWeight[i] = Target.GetComponent<PlayerGamePlay>().Calculate_Total();

        }

        else
        {
            // auto functionality for ai
            PlayerWeight[i] = Target.GetComponent<Artiificial_Player>().Calculate_Total();
        }

    }

    void Show_Conclusion()
    {
        Conclusion_Panel.SetActive(true);
        Winner_Name.text = playertemp[0].name;
        Loser_1_Name.text = playertemp[1].name;
        Loser_2_Name.text = playertemp[2].name;

        Winner_Points.text = PlayerWeight[0].ToString();
        Loser_1_Points.text = PlayerWeight[1].ToString();
        Loser_2_Points.text = PlayerWeight[2].ToString();


        for(int i = 0; i < playertemp.Length; i++)
        {
            if(playertemp[i].GetComponent<PlayerGamePlay>())
            {
                int pos = 0;
                for(int j = 0; j < playertemp[i].GetComponent< PlayerGamePlay>().MeldingPoint.transform.childCount; j++)
                {
                   
                    for(int k = 0; k < playertemp[i].GetComponent<PlayerGamePlay>().MeldingPoint.transform.GetChild(j).childCount; k++)
                    {
                        if (i == 0)
                        {
                            GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<PlayerGamePlay>().MeldingPoint.transform.GetChild(j).GetChild(k).gameObject, new Vector3(Winner_Gen_Point.transform.position.x + (k * 50) + pos, Winner_Gen_Point.transform.position.y, Winner_Gen_Point.transform.position.z), Quaternion.identity);
                            temp.transform.SetParent(Winner_Gen_Point.transform, false);
                            temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                        }
                        else if (i == 1)
                        {
                            GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<PlayerGamePlay>().MeldingPoint.transform.GetChild(j).GetChild(k).gameObject, new Vector3(Loser1_Gen_Point.transform.position.x + (k * 50) + pos, Loser1_Gen_Point.transform.position.y, Loser1_Gen_Point.transform.position.z), Quaternion.identity);
                            temp.transform.SetParent(Loser1_Gen_Point.transform, false);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                            temp.transform.localScale = new Vector3(1, 1, 1);
                        }
                        else if (i == 2)
                        {
                            GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<PlayerGamePlay>().MeldingPoint.transform.GetChild(j).GetChild(k).gameObject, new Vector3(Loser2_Gen_Point.transform.position.x + (k * 50) + pos, Loser2_Gen_Point.transform.position.y, Loser2_Gen_Point.transform.position.z), Quaternion.identity);
                            temp.transform.SetParent(Loser2_Gen_Point.transform, false);
                            temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                        }
                    }

                    pos += 100;

                   

                }

                pos = 0;
                for (int j = 0; j < playertemp[i].GetComponent<PlayerGamePlay>().StrategyPoint.transform.childCount; j++)
                {

                    for (int k = 0; k < playertemp[i].GetComponent<PlayerGamePlay>().StrategyPoint.transform.GetChild(j).childCount; k++)
                    {
                        if (i == 0)
                        {
                            GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<PlayerGamePlay>().StrategyPoint.transform.GetChild(j).GetChild(k).gameObject, new Vector3(Winner_Gen_Point.transform.position.x + (Winner_Gen_Point.transform.childCount * 50 + 50), Winner_Gen_Point.transform.position.y, Winner_Gen_Point.transform.position.z), Quaternion.identity);
                            temp.transform.SetParent(Winner_Gen_Point.transform, false);
                            temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                        }
                        else if (i == 1)
                        {
                            GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<PlayerGamePlay>().StrategyPoint.transform.GetChild(j).GetChild(k).gameObject, new Vector3(Loser1_Gen_Point.transform.position.x + (Loser1_Gen_Point.transform.childCount * 50 + 50), Loser1_Gen_Point.transform.position.y, Loser1_Gen_Point.transform.position.z), Quaternion.identity);
                            temp.transform.SetParent(Loser1_Gen_Point.transform, false);
                            temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                        }
                        else if (i == 2)
                        {
                            GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<PlayerGamePlay>().StrategyPoint.transform.GetChild(j).GetChild(k).gameObject, new Vector3(Loser2_Gen_Point.transform.position.x + (Loser2_Gen_Point.transform.childCount * 50 + 50), Loser2_Gen_Point.transform.position.y, Loser2_Gen_Point.transform.position.z), Quaternion.identity);
                            temp.transform.SetParent(Loser2_Gen_Point.transform, false);
                            temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                        }
                    }

                    pos += 100;
                   

                }

                for (int j = 0; j < playertemp[i].GetComponent<PlayerGamePlay>().GenPoint.transform.childCount; j++)
                {
                    if (i == 0)
                    {
                        GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<PlayerGamePlay>().GenPoint.transform.GetChild(j).gameObject, new Vector3(Winner_Gen_Point.transform.position.x + (Winner_Gen_Point.transform.childCount * 50 + 100)  , Winner_Gen_Point.transform.position.y, Winner_Gen_Point.transform.position.z), Quaternion.identity);
                        temp.transform.SetParent(Winner_Gen_Point.transform, false);
                        temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                    }
                    else if (i == 1)
                    {
                        GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<PlayerGamePlay>().GenPoint.transform.GetChild(j).gameObject, new Vector3(Loser1_Gen_Point.transform.position.x + (Loser1_Gen_Point.transform.childCount * 50 + 100)  , Loser1_Gen_Point.transform.position.y, Loser1_Gen_Point.transform.position.z), Quaternion.identity);
                        temp.transform.SetParent(Loser1_Gen_Point.transform, false);
                        temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                    }
                    else if (i == 2)
                    {
                        GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<PlayerGamePlay>().GenPoint.transform.GetChild(j).gameObject, new Vector3(Loser2_Gen_Point.transform.position.x + (Loser2_Gen_Point.transform.childCount * 50 + 100)  , Loser2_Gen_Point.transform.position.y, Loser2_Gen_Point.transform.position.z), Quaternion.identity);
                        temp.transform.SetParent(Loser2_Gen_Point.transform, false);
                        temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                    }

                }
            }
            else
            {
                int pos = 0;
                for (int j = 0; j < playertemp[i].GetComponent<Artiificial_Player>().MeldingPoint.transform.childCount; j++)
                {

                    for(int k = 0; k < playertemp[i].GetComponent<Artiificial_Player>().MeldingPoint.transform.GetChild(j).childCount; k++)
                    {
                        if (i == 0)
                        {
                            GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<Artiificial_Player>().MeldingPoint.transform.GetChild(j).GetChild(k).gameObject, new Vector3(Winner_Gen_Point.transform.position.x + (k * 50) + pos, Winner_Gen_Point.transform.position.y, Winner_Gen_Point.transform.position.z), Quaternion.identity);
                            temp.transform.SetParent(Winner_Gen_Point.transform, false);
                            temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                        }
                        else if (i == 1)
                        {
                            GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<Artiificial_Player>().MeldingPoint.transform.GetChild(j).GetChild(k).gameObject, new Vector3(Loser1_Gen_Point.transform.position.x + (k * 50) + pos, Loser1_Gen_Point.transform.position.y, Loser1_Gen_Point.transform.position.z), Quaternion.identity);
                            temp.transform.SetParent(Loser1_Gen_Point.transform, false);
                            temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                        }
                        else if (i == 2)
                        {
                            GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<Artiificial_Player>().MeldingPoint.transform.GetChild(j).GetChild(k).gameObject, new Vector3(Loser2_Gen_Point.transform.position.x + (k * 50) + pos, Loser2_Gen_Point.transform.position.y, Loser2_Gen_Point.transform.position.z), Quaternion.identity);
                            temp.transform.SetParent(Loser2_Gen_Point.transform, false);
                            temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                        }
                    }


                    pos += 100;
                }

                pos = 0;
                for (int j = 0; j < playertemp[i].GetComponent<Artiificial_Player>().StrategyPoint.transform.childCount; j++)
                {

                    for (int k = 0; k < playertemp[i].GetComponent<Artiificial_Player>().StrategyPoint.transform.GetChild(j).childCount; k++)
                    {
                        if (i == 0)
                        {
                            GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<Artiificial_Player>().StrategyPoint.transform.GetChild(j).GetChild(k).gameObject, new Vector3(Winner_Gen_Point.transform.position.x + (Winner_Gen_Point.transform.childCount * 50 + 50 + pos), Winner_Gen_Point.transform.position.y, Winner_Gen_Point.transform.position.z), Quaternion.identity);
                            temp.transform.SetParent(Winner_Gen_Point.transform, false);
                            temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                        }
                        else if (i == 1)
                        {
                            GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<Artiificial_Player>().StrategyPoint.transform.GetChild(j).GetChild(k).gameObject, new Vector3(Loser1_Gen_Point.transform.position.x + (Loser1_Gen_Point.transform.childCount * 50 + 50 + pos), Loser1_Gen_Point.transform.position.y, Loser1_Gen_Point.transform.position.z), Quaternion.identity);
                            temp.transform.SetParent(Loser1_Gen_Point.transform, false);
                            temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                        }
                        else if (i == 2)
                        {
                            GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<Artiificial_Player>().StrategyPoint.transform.GetChild(j).GetChild(k).gameObject, new Vector3(Loser2_Gen_Point.transform.position.x + (Loser2_Gen_Point.transform.childCount * 50 + 50 + pos), Loser2_Gen_Point.transform.position.y, Loser2_Gen_Point.transform.position.z), Quaternion.identity);
                            temp.transform.SetParent(Loser2_Gen_Point.transform, false);
                            temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                        }
                    }

                    pos += 100;
                }

                for (int j = 0; j < playertemp[i].GetComponent<Artiificial_Player>().GenPoint.transform.childCount; j++)
                {
                    if (i == 0)
                    {
                        GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<Artiificial_Player>().GenPoint.transform.GetChild(j).gameObject, new Vector3(Winner_Gen_Point.transform.position.x + (Winner_Gen_Point.transform.childCount * 50 + 100)  , Winner_Gen_Point.transform.position.y, Winner_Gen_Point.transform.position.z), Quaternion.identity);
                        temp.transform.SetParent(Winner_Gen_Point.transform, false);
                        temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                    }
                    else if (i == 1)
                    {
                        GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<Artiificial_Player>().GenPoint.transform.GetChild(j).gameObject, new Vector3(Loser1_Gen_Point.transform.position.x + (Loser1_Gen_Point.transform.childCount * 50 + 100)  , Loser1_Gen_Point.transform.position.y, Loser1_Gen_Point.transform.position.z), Quaternion.identity);
                        temp.transform.SetParent(Loser1_Gen_Point.transform, false);
                        temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                    }
                    else if (i == 2)
                    {
                        GameObject temp = (GameObject)Instantiate(playertemp[i].GetComponent<Artiificial_Player>().GenPoint.transform.GetChild(j).gameObject, new Vector3(Loser2_Gen_Point.transform.position.x + (Loser2_Gen_Point.transform.childCount * 50 + 100)  , Loser2_Gen_Point.transform.position.y, Loser2_Gen_Point.transform.position.z), Quaternion.identity);
                        temp.transform.SetParent(Loser2_Gen_Point.transform, false);
                        temp.transform.localScale = new Vector3(1, 1, 1);
                            temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, 0, temp.transform.localPosition.z);
                    }

                }
            }
        }        
       
    }


    public void OnConclusionCross()
    {
        GameObject.Find("left_top panel").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("right_top panel").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("left_bottom panel").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("right_bottom panel").transform.localScale = new Vector3(0, 0, 0);
		if (Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone5S || 
		    Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6 || 
		    Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6Plus)
		{

			GameObject.Find("GamePlay IPhone/Side Panels/left_bottom").GetComponent<Panel_Script>()._InEnum = false;
			GameObject.Find("GamePlay IPhone/Side Panels/right_top").GetComponent<Panel_Script>()._InEnum = false;
			GameObject.Find("GamePlay IPhone/Side Panels/left_top").GetComponent<Panel_Script>()._InEnum = false;
			GameObject.Find("GamePlay IPhone/Side Panels/right_bottom").GetComponent<Panel_Script>()._InEnum = false;
		}
		
		else
		{
			
			
			GameObject.Find("GamePlay IPhone/Side Panels/left_bottom").GetComponent<Panel_Script>()._InEnum = false;
			GameObject.Find("GamePlay IPhone/Side Panels/right_top").GetComponent<Panel_Script>()._InEnum = false;
			GameObject.Find("GamePlay IPhone/Side Panels/left_top").GetComponent<Panel_Script>()._InEnum = false;
			GameObject.Find("GamePlay IPhone/Side Panels/right_bottom").GetComponent<Panel_Script>()._InEnum = false;
			
			/*
            IPhoneManager.SetActive(false);
            IPhoneGameManager.SetActive(false);
            IPhoneScreen.SetActive(false);
            IPadScreen.SetActive(true);
            IPadManager.SetActive(true);
            IPadGameManager.SetActive(true);
            */
		}
       
        _DrawCalled = false;
        WhichPlayerTurn = "";
        PlayerGamePlay.Instance.ResetEveryThing();
        Winner = null;
        GameObject.Find("Ai1").GetComponent<Artiificial_Player>().ClearEveryThing();
        GameObject.Find("Ai2").GetComponent<Artiificial_Player>().ClearEveryThing();
     
        Conclusion_Panel.SetActive(false);
    }
}
