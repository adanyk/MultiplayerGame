using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GameControl : MonoBehaviour
{
    public static int whoseTurn;
    public static float speed;
    public static float step;
    public static List<int> activePlayers;
    public static int[,] occupiedFields;
    [SerializeField] GameObject[] Players;
    public static GameObject[] players;
    [SerializeField] GameObject[] FieldMarkers;
    public static GameObject[] fieldmarkers;

    public static int[] titleDeedCardsOwners;
    public static TitleDeedCard[] titleDeedCards;
    
    [SerializeField] public GameObject[] PlayerIcons;
    public static GameObject[] playerIcons;
    [SerializeField] public GameObject MoneyStartObject;

    // Money transfer
    public static bool transferingMoney, transferingActivated;
    static bool tempTransferingMoney;

    // Auction
    public static int activeBidders, bid;

    // Chance and Treasure Chest
    [SerializeField] GameObject[] ChanceAndChest;
    public static bool chancefinished, chestfinished;
    
    List<int> randomList;
    private static System.Random StaticRandom;
    public static Queue<int> chanceIndices, chestIndices, turnQ;
    public static int tempIndex;

    // Both Free cards
    public static bool freeFinished;

    // Control color owners
    public static List<int> colorOwners;
    public static List<int> tempColorOwners; // Used to prevent form buying houses on newly unmortgaged color
    public static List<int> colorHouses; // Number of houses on colors


    // DEBUG
    public int WHOSETURN;
    public List<int> ACTIVEPLAYERS, COLOROWNERS, COLORHOUSES, TEMPCOLOROWNERS;
    public int[] TITLEDEEDCARDSOVNERS;


    


    void Start()
    {
        speed = 20; //20
        step = speed * Time.deltaTime;
        activePlayers = new List<int>();
        turnQ = new Queue<int>();
        players = Players;
        playerIcons = PlayerIcons;
        for(int i = 1; i <= GameStart.numberOfPlayers; i++)
        {
            activePlayers.Add(i);
            turnQ.Enqueue(i);
            players[i].SetActive(true);
            players[i].GetComponent<PlayerControl>().myTurn = false;
            PlayerIcons[i].SetActive(true);
            players[i].GetComponent<PlayerControl>().myIndex = i;
        }
        occupiedFields = new int[41, 6];
        fieldmarkers = FieldMarkers;

        titleDeedCardsOwners = new int[41];
        for(int i = 0; i < 40; i++)
        {
            if(i == 0 || i == 1 || i == 3 || i == 5 || i == 8 || i == 11 || i == 18 || i == 21 || i == 23 || i == 31 || i == 34 || i == 37 || i == 39)
            {
                titleDeedCardsOwners[i] = -10;
            }
        }

        colorOwners = new List<int>{0,0,0,0,0,0,0,0};
        tempColorOwners = new List<int>{0,0,0,0,0,0,0,0};
        colorHouses = new List<int>{0,0,0,0,0,0,0,0};

        // Money transfer
        transferingMoney = false;
        transferingActivated = false;

        // Auction
        activeBidders = 0;
        bid = 0;

        // Chance and Treasue Chest
        ChanceAndChest[0].SetActive(true);
        ChanceAndChest[1].SetActive(true);
        chancefinished = true;
        chestfinished = true;
        
        StaticRandom = new System.Random();
        chanceIndices = new Queue<int>();
        chestIndices = new Queue<int>();

        randomList = Enumerable.Range(0, 15).OrderBy(_ => StaticRandom.Next()).ToList();
        foreach(int i in randomList)
        {
            chanceIndices.Enqueue(i);
        }
        randomList = Enumerable.Range(0, 15).OrderBy(_ => StaticRandom.Next()).ToList();
        foreach(int i in randomList)
        {
            chestIndices.Enqueue(i);
        }

        // Both Free cards
        freeFinished = true;

        makeTitleDeedCards();
        MoneyStartObject.SetActive(true);
        firstTurn();

        
        // DEBUG
        WHOSETURN = whoseTurn;
        ACTIVEPLAYERS = activePlayers;
        COLOROWNERS = colorOwners;
        COLORHOUSES = colorHouses;
        TITLEDEEDCARDSOVNERS = titleDeedCardsOwners;
        TEMPCOLOROWNERS = tempColorOwners;
    }

    void Update()
    {
        // DEBUG
        WHOSETURN = whoseTurn;
        ACTIVEPLAYERS = activePlayers;
        COLOROWNERS = colorOwners;
        COLORHOUSES = colorHouses;
        TITLEDEEDCARDSOVNERS = titleDeedCardsOwners;
        TEMPCOLOROWNERS = tempColorOwners;



        // Money transfer
        if(transferingActivated)
        {
            tempTransferingMoney = false;
            for(int i = 0; i < activePlayers.Count; i++)
            {
                if(players[activePlayers[i]].GetComponent<PlayerControl>().addingMoney || players[activePlayers[i]].GetComponent<PlayerControl>().subingMoney)
                {
                    tempTransferingMoney = true;
                    break;
                }
            }
            if(tempTransferingMoney)
            {
                transferingActivated = true;
            }
            else
            {
                transferingActivated = false;
            }
        }
        // Auction
        else if(activeBidders == 1)
        {
            //Debug.Log("activeBidders == 1");
            for(int i = 0; i < activePlayers.Count; i++)
            {
                if(players[activePlayers[i]].GetComponent<PlayerControl>().stillInBidding)
                {
                    Debug.Log("Player " + activePlayers[i] + " won the auction.");
                    players[activePlayers[i]].GetComponent<PlayerControl>().biddingWinner = true;
                    players[activePlayers[i]].GetComponent<PlayerControl>().stillInBidding = false;
                    activeBidders = 0;
                    break;
                }
            }
        }
    }


    void firstTurn()
    {
        whoseTurn = turnQ.Dequeue();
        players[1].GetComponent<PlayerControl>().myTurn = true;
        players[1].GetComponent<PlayerControl>().TurnMarkerOn();
    }

    public static void nextTurn(int bankrupt=0)
    {
        players[whoseTurn].GetComponent<PlayerControl>().myTurn = false;
        players[whoseTurn].GetComponent<PlayerControl>().TurnMarkerOff();
        if(bankrupt != 0)
        {
            occupiedFields[players[bankrupt].GetComponent<PlayerControl>().index, players[bankrupt].GetComponent<PlayerControl>().previousPosition] = 0;            
            players[bankrupt].SetActive(false);
            activePlayers.Remove(bankrupt);
            if(activePlayers.Count == 1)    // End of game
            {
                WindowsControl.winnerMessage.GetComponent<TextMeshProUGUI>().text = "The winner is Player " + activePlayers[0] + ".";
                WindowsControl.highscore.GetComponent<TextMeshProUGUI>().text = "1. Player " + activePlayers[0] + " - " + MoneyText(players[activePlayers[0]].GetComponent<PlayerControl>().worth) + "\n";
                WindowsControl.gameOverWindow.SetActive(true);
                WindowsControl.gameOverWindowOpen = true;
                whoseTurn = turnQ.Dequeue(); // The last player turn - only to click OK in game over window
            }
            else if(bankrupt == whoseTurn)
            {
                whoseTurn = turnQ.Dequeue();
            }
            else
            {
                for(int i = 0; i <= activePlayers.Count; i++)
                {
                    if(whoseTurn != bankrupt)
                    {
                        turnQ.Enqueue(whoseTurn);
                    }
                    whoseTurn = turnQ.Dequeue();
                }
            }
        }
        else
        {
            turnQ.Enqueue(whoseTurn);
            whoseTurn = turnQ.Dequeue();
        }
        players[whoseTurn].GetComponent<PlayerControl>().myTurn = true;
        players[whoseTurn].GetComponent<PlayerControl>().TurnMarkerOn();
    }

    // Cards making
    void makeTitleDeedCards()
    {
        titleDeedCards = new TitleDeedCard[41];
        titleDeedCards[2] = new TitleDeedCard(2, 60, 2, 50, 10, 30, 90, 160, 250, "Swietochlowice", "city", "brown");
        titleDeedCards[4] = new TitleDeedCard(4, 60, 4, 50, 20, 60, 180, 320, 450, "Belchatow", "city", "brown");
        titleDeedCards[7] = new TitleDeedCard(7, 100, 6, 50, 30, 90, 270, 400, 550, "Warszawa", "city", "lightblue");
        titleDeedCards[9] = new TitleDeedCard(9, 100, 6, 50, 30, 90, 270, 400, 550, "Lublin", "city", "lightblue");
        titleDeedCards[10] = new TitleDeedCard(10, 120, 8, 50, 40, 100, 300, 450, 600, "Katowice", "city", "lightblue");
        titleDeedCards[12] = new TitleDeedCard(12, 140, 10, 100, 50, 150, 450, 625, 750, "Torun", "city", "rose");
        titleDeedCards[14] = new TitleDeedCard(14, 140, 10, 100, 50, 150, 450, 625, 750, "Rybnik", "city", "rose");
        titleDeedCards[15] = new TitleDeedCard(15, 160, 12, 100, 60, 180, 500, 700, 900, "Lodz", "city", "rose");
        titleDeedCards[17] = new TitleDeedCard(17, 180, 14, 100, 70, 200, 550, 750, 900, "Chojnice", "city", "orange");
        titleDeedCards[19] = new TitleDeedCard(19, 180, 14, 100, 70, 200, 550, 750, 900, "Elblag", "city", "orange");
        titleDeedCards[20] = new TitleDeedCard(20, 200, 16, 100, 80, 220, 600, 800, 1000, "Szczecin", "city", "orange");
        titleDeedCards[22] = new TitleDeedCard(22, 220, 18, 150, 90, 250, 700, 875, 1050, "Zielona Gora", "city", "red");
        titleDeedCards[24] = new TitleDeedCard(24, 220, 18, 150, 90, 250, 700, 875, 1050, "Bydgoszcz", "city", "red");
        titleDeedCards[25] = new TitleDeedCard(25, 240, 20, 150, 100, 300, 750, 925, 1100, "Tarnow", "city", "red");
        titleDeedCards[27] = new TitleDeedCard(27, 260, 22, 150, 110, 330, 800, 975, 1150, "Piotrkow Trybunalski", "city", "yellow");
        titleDeedCards[28] = new TitleDeedCard(28, 260, 22, 150, 110, 330, 800, 975, 1150, "Wroclaw", "city", "yellow");
        titleDeedCards[30] = new TitleDeedCard(30, 280, 24, 150, 120, 360, 850, 1025, 1200, "Kalisz", "city", "yellow");
        titleDeedCards[32] = new TitleDeedCard(32, 300, 26, 200, 130, 390, 900, 1100, 1275, "Krakow", "city", "green");
        titleDeedCards[33] = new TitleDeedCard(33, 300, 26, 200, 130, 390, 900, 1100, 1275, "Gdynia", "city", "green");
        titleDeedCards[35] = new TitleDeedCard(35, 320, 28, 200, 150, 450, 1000, 1200, 1400, "Poznan", "city", "green");
        titleDeedCards[38] = new TitleDeedCard(38, 350, 35, 200, 175, 500, 1100, 1300, 1500, "Gorzow", "city", "blue");
        titleDeedCards[40] = new TitleDeedCard(40, 400, 50, 200, 200, 600, 1400, 1700, 2000, "Bialystok", "city", "blue");
        titleDeedCards[6] = new TitleDeedCard(6, 200, 25, 0,0,0,0,0,0, "Dworzec Kolejowy", "transport");
        titleDeedCards[16] = new TitleDeedCard(16, 200, 25, 0,0,0,0,0,0, "Lotnisko", "transport");
        titleDeedCards[26] = new TitleDeedCard(26, 200, 25, 0,0,0,0,0,0, "Dworzec Metra", "transport");
        titleDeedCards[36] = new TitleDeedCard(36, 200, 25, 0,0,0,0,0,0, "Port Morski", "transport");
        titleDeedCards[13] = new TitleDeedCard(13, 150, 0,0,0,0,0,0,0, "Telefonia Komorkowa", "utility");
        titleDeedCards[29] = new TitleDeedCard(29, 150, 0,0,0,0,0,0,0, "Portal Spolecznosciowy", "utility");
    }

    public static void playerUpdate(int player)
    {
        players[player].GetComponent<PlayerControl>().moneyText = MoneyText(players[player].GetComponent<PlayerControl>().money);
        players[player].GetComponent<PlayerControl>().NameAndMoney[4].GetComponent<TextMeshProUGUI>().text = players[player].GetComponent<PlayerControl>().moneyText;
    }

    
    public static string MoneyText(int num)
    {
        if(num >= 100)
        {
            return ((double)num/100).ToString() + " M";
        }
        else
        {
            return (num * 10).ToString() + " k";
        }
    }


    // Chance and Chest functions
    public static void ChanceFunction(int i)
    {
        if(i != 11)
        {
            chanceIndices.Enqueue(i);
        }
        if(i == 0)
        {
            if(players[whoseTurn].GetComponent<PlayerControl>().index > 11)
            {
                players[whoseTurn].GetComponent<PlayerControl>().money += 200;
                players[whoseTurn].GetComponent<PlayerControl>().capital += 200;
                players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling += 200;
                players[whoseTurn].GetComponent<PlayerControl>().worth += 200;
                players[whoseTurn].GetComponent<PlayerControl>().AddMoney(200);
            }
            players[whoseTurn].GetComponent<PlayerControl>().waypointIndex = 11;
            players[whoseTurn].GetComponent<PlayerControl>().move = 0;
            players[whoseTurn].GetComponent<PlayerControl>().MakeMove();
        }
        else if(i == 1)
        {
            players[whoseTurn].GetComponent<PlayerControl>().money += 200;
            players[whoseTurn].GetComponent<PlayerControl>().capital += 200;
            players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling += 200;
            players[whoseTurn].GetComponent<PlayerControl>().worth += 200;
            players[whoseTurn].GetComponent<PlayerControl>().AddMoney(200);
            players[whoseTurn].GetComponent<PlayerControl>().waypointIndex = 0;
            players[whoseTurn].GetComponent<PlayerControl>().move = 0;
            players[whoseTurn].GetComponent<PlayerControl>().MakeMove();
        }
        else if(i == 2)
        {
            players[whoseTurn].GetComponent<PlayerControl>().money += 150;
            players[whoseTurn].GetComponent<PlayerControl>().capital += 150;
            players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling += 150;
            players[whoseTurn].GetComponent<PlayerControl>().worth += 150;
            players[whoseTurn].GetComponent<PlayerControl>().AddMoney(150);
            nextTurn();
        }
        else if(i == 3)
        {
            if(15 > players[whoseTurn].GetComponent<PlayerControl>().capital)
            {
                players[whoseTurn].GetComponent<PlayerControl>().Bankruptcy(15);
            }
            else if(15 > players[whoseTurn].GetComponent<PlayerControl>().money)
            {
                players[whoseTurn].GetComponent<PlayerControl>().shortageReason = "Chance";
                players[whoseTurn].GetComponent<PlayerControl>().Shortage(15);
            }
            else
            {
                players[whoseTurn].GetComponent<PlayerControl>().capital -= 15;
                players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling -= 15;
                players[whoseTurn].GetComponent<PlayerControl>().worth -= 15;
                players[whoseTurn].GetComponent<PlayerControl>().money -= 15;
                players[whoseTurn].GetComponent<PlayerControl>().SubMoney(15);
                nextTurn();
            }
        }
        else if(i == 4)
        {
            players[whoseTurn].GetComponent<PlayerControl>().previousWaypointindex = players[whoseTurn].GetComponent<PlayerControl>().index;
            players[whoseTurn].GetComponent<PlayerControl>().moving = true;
            players[whoseTurn].GetComponent<PlayerControl>().index = 40;
            players[whoseTurn].GetComponent<PlayerControl>().waypointIndex = 10;
            players[whoseTurn].GetComponent<PlayerControl>().inJailLeft = 3;
            players[whoseTurn].GetComponent<PlayerControl>().fieldOccupied = false;
            players[whoseTurn].GetComponent<PlayerControl>().Go();
        }
        else if(i == 5)
        {
            var var1 = 25 * players[whoseTurn].GetComponent<PlayerControl>().totalNumOfHouses + 100 * players[whoseTurn].GetComponent<PlayerControl>().totalNumOfHotels;
            if(var1 > 0)
            {
                if(var1 > players[whoseTurn].GetComponent<PlayerControl>().capital)
                {
                    players[whoseTurn].GetComponent<PlayerControl>().Bankruptcy(var1);
                }
                else if(var1 > players[whoseTurn].GetComponent<PlayerControl>().money)
                {
                    players[whoseTurn].GetComponent<PlayerControl>().shortageReason = "Chance";
                    players[whoseTurn].GetComponent<PlayerControl>().Shortage(var1);
                }
                else
                {
                    players[whoseTurn].GetComponent<PlayerControl>().capital -= var1;
                    players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling -= var1;
                    players[whoseTurn].GetComponent<PlayerControl>().worth -= var1;
                    players[whoseTurn].GetComponent<PlayerControl>().money -= var1;
                    players[whoseTurn].GetComponent<PlayerControl>().SubMoney(var1);
                    nextTurn();
                }
            }
            else
            {
                nextTurn();
            }
        }
        else if(i == 6)
        {
            if(20 > players[whoseTurn].GetComponent<PlayerControl>().capital)
            {
                players[whoseTurn].GetComponent<PlayerControl>().Bankruptcy(20);
            }
            else if(20 > players[whoseTurn].GetComponent<PlayerControl>().money)
            {
                players[whoseTurn].GetComponent<PlayerControl>().shortageReason = "Chance";
                players[whoseTurn].GetComponent<PlayerControl>().Shortage(20);
            }
            else
            {
                players[whoseTurn].GetComponent<PlayerControl>().capital -= 20;
                players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling -= 20;
                players[whoseTurn].GetComponent<PlayerControl>().worth -= 20;
                players[whoseTurn].GetComponent<PlayerControl>().money -= 20;
                players[whoseTurn].GetComponent<PlayerControl>().SubMoney(20);
                nextTurn();
            }
        }
        else if(i == 7)
        {
            players[whoseTurn].GetComponent<PlayerControl>().waypointIndex = 39;
            players[whoseTurn].GetComponent<PlayerControl>().move = 0;
            players[whoseTurn].GetComponent<PlayerControl>().MakeMove();
        }
        else if(i == 8)
        {
            players[whoseTurn].GetComponent<PlayerControl>().money += 50;
            players[whoseTurn].GetComponent<PlayerControl>().capital += 50;
            players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling += 50;
            players[whoseTurn].GetComponent<PlayerControl>().worth += 50;
            players[whoseTurn].GetComponent<PlayerControl>().AddMoney(50);
            nextTurn();
        }
        else if(i == 9)
        {
            if(players[whoseTurn].GetComponent<PlayerControl>().waypointIndex == 2)
            {
                players[whoseTurn].GetComponent<PlayerControl>().waypointIndex += 78; // 38 + Go passed
            }
            else
            {
                players[whoseTurn].GetComponent<PlayerControl>().waypointIndex -= 3;
            }
            players[whoseTurn].GetComponent<PlayerControl>().move = 0;
            players[whoseTurn].GetComponent<PlayerControl>().MakeMove();
        }
        else if(i == 10)
        {
            players[whoseTurn].GetComponent<PlayerControl>().money += 100;
            players[whoseTurn].GetComponent<PlayerControl>().capital += 100;
            players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling += 100;
            players[whoseTurn].GetComponent<PlayerControl>().worth += 100;
            players[whoseTurn].GetComponent<PlayerControl>().AddMoney(100);
            nextTurn();
        }
        else if(i == 11)
        {
            players[whoseTurn].GetComponent<PlayerControl>().freeChance = true;
            nextTurn();
        }
        else if(i == 12)
        {
            if(150 > players[whoseTurn].GetComponent<PlayerControl>().capital)
            {
                players[whoseTurn].GetComponent<PlayerControl>().Bankruptcy(150);
            }
            else if(150 > players[whoseTurn].GetComponent<PlayerControl>().money)
            {
                players[whoseTurn].GetComponent<PlayerControl>().shortageReason = "Chance";
                players[whoseTurn].GetComponent<PlayerControl>().Shortage(150);
            }
            else
            {
                players[whoseTurn].GetComponent<PlayerControl>().capital -= 150;
                players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling -= 150;
                players[whoseTurn].GetComponent<PlayerControl>().worth -= 150;
                players[whoseTurn].GetComponent<PlayerControl>().money -= 150;
                players[whoseTurn].GetComponent<PlayerControl>().SubMoney(150);
                nextTurn();
            }
        }
        else if(i == 13)
        {
            var var2 = 40 * players[whoseTurn].GetComponent<PlayerControl>().totalNumOfHouses + 150 * players[whoseTurn].GetComponent<PlayerControl>().totalNumOfHotels;
            if(var2 > 0)
            {
                if(var2 > players[whoseTurn].GetComponent<PlayerControl>().capital)
                {
                    players[whoseTurn].GetComponent<PlayerControl>().Bankruptcy(var2);
                }
                else if(var2 > players[whoseTurn].GetComponent<PlayerControl>().money)
                {
                    players[whoseTurn].GetComponent<PlayerControl>().shortageReason = "Chance";
                    players[whoseTurn].GetComponent<PlayerControl>().Shortage(var2);
                }
                else
                {
                    players[whoseTurn].GetComponent<PlayerControl>().capital -= var2;
                    players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling -= var2;
                    players[whoseTurn].GetComponent<PlayerControl>().worth -= var2;
                    players[whoseTurn].GetComponent<PlayerControl>().money -= var2;
                    players[whoseTurn].GetComponent<PlayerControl>().SubMoney(var2);
                    nextTurn();
                }
            }
            else
            {
                nextTurn();
            }
        }
        else if(i == 14)
        {
            if(players[whoseTurn].GetComponent<PlayerControl>().index > 15)
            {                
                players[whoseTurn].GetComponent<PlayerControl>().money += 200;
                players[whoseTurn].GetComponent<PlayerControl>().capital += 200;
                players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling += 200;
                players[whoseTurn].GetComponent<PlayerControl>().worth += 200;
                players[whoseTurn].GetComponent<PlayerControl>().AddMoney(200);
            }
            players[whoseTurn].GetComponent<PlayerControl>().waypointIndex = 15;
            players[whoseTurn].GetComponent<PlayerControl>().move = 0;
            players[whoseTurn].GetComponent<PlayerControl>().MakeMove();
        }
        else if(i == 15)
        {
            if(players[whoseTurn].GetComponent<PlayerControl>().index > 25)
            {                
                players[whoseTurn].GetComponent<PlayerControl>().money += 200;
                players[whoseTurn].GetComponent<PlayerControl>().capital += 200;
                players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling += 200;
                players[whoseTurn].GetComponent<PlayerControl>().worth += 200;
                players[whoseTurn].GetComponent<PlayerControl>().AddMoney(200);
            }
            players[whoseTurn].GetComponent<PlayerControl>().waypointIndex = 25;
            players[whoseTurn].GetComponent<PlayerControl>().move = 0;
            players[whoseTurn].GetComponent<PlayerControl>().MakeMove();
        }
        GameControl.chancefinished = true;
    }
    public static void ChestFunction(int i)
    {
        if(i != 2)
        {
            chestIndices.Enqueue(i);
        }
        if(i == 0)
        {
            players[whoseTurn].GetComponent<PlayerControl>().money += 25;
            players[whoseTurn].GetComponent<PlayerControl>().capital += 25;
            players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling += 25;
            players[whoseTurn].GetComponent<PlayerControl>().worth += 25;
            players[whoseTurn].GetComponent<PlayerControl>().AddMoney(25);
            nextTurn();
        }
        else if(i == 1)
        {
            players[whoseTurn].GetComponent<PlayerControl>().previousWaypointindex = players[whoseTurn].GetComponent<PlayerControl>().index;
            players[whoseTurn].GetComponent<PlayerControl>().moving = true;
            players[whoseTurn].GetComponent<PlayerControl>().index = 40;
            players[whoseTurn].GetComponent<PlayerControl>().waypointIndex = 10;
            players[whoseTurn].GetComponent<PlayerControl>().inJailLeft = 3;
            players[whoseTurn].GetComponent<PlayerControl>().fieldOccupied = false;
            players[whoseTurn].GetComponent<PlayerControl>().Go();
        }
        else if(i == 2)
        {
            players[whoseTurn].GetComponent<PlayerControl>().freeChest = true;
            nextTurn();
        }
        else if(i == 3)
        {
            players[whoseTurn].GetComponent<PlayerControl>().waypointIndex = 1;
            players[whoseTurn].GetComponent<PlayerControl>().move = 0;
            players[whoseTurn].GetComponent<PlayerControl>().MakeMove();
        }
        else if(i == 4)
        {
            players[whoseTurn].GetComponent<PlayerControl>().money += 10;
            players[whoseTurn].GetComponent<PlayerControl>().capital += 10;
            players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling += 10;
            players[whoseTurn].GetComponent<PlayerControl>().worth += 10;
            players[whoseTurn].GetComponent<PlayerControl>().AddMoney(10);
            nextTurn();
        }
        else if(i == 5)
        {
            //Zapłać 100k grzywny lub pobierz kartę "SZANSA".
            if(WindowsControl.takeChance)
            {
                WindowsControl.takeChance = false;
                WindowsControl.chanceChestObjects[0].SetActive(true);
                WindowsControl.chanceToTake = true;
                players[whoseTurn].GetComponent<PlayerControl>().chanceChestOpportunity = true;
                chancefinished = false;
            }
            else
            {
                if(10 > players[whoseTurn].GetComponent<PlayerControl>().capital)
                {
                    players[whoseTurn].GetComponent<PlayerControl>().Bankruptcy(10);
                }
                else if(10 > players[whoseTurn].GetComponent<PlayerControl>().money)
                {
                    players[whoseTurn].GetComponent<PlayerControl>().shortageReason = "Chest";
                    players[whoseTurn].GetComponent<PlayerControl>().Shortage(10);
                }
                else
                {
                    players[whoseTurn].GetComponent<PlayerControl>().capital -= 10;
                    players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling -= 10;
                    players[whoseTurn].GetComponent<PlayerControl>().worth -= 10;
                    players[whoseTurn].GetComponent<PlayerControl>().money -= 10;
                    players[whoseTurn].GetComponent<PlayerControl>().SubMoney(10);
                    nextTurn();
                }
            }
        }
        else if(i == 6)
        {
            players[whoseTurn].GetComponent<PlayerControl>().money += 20;
            players[whoseTurn].GetComponent<PlayerControl>().capital += 20;
            players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling += 20;
            players[whoseTurn].GetComponent<PlayerControl>().worth += 20;
            players[whoseTurn].GetComponent<PlayerControl>().AddMoney(20);
            nextTurn();
        }
        else if(i == 7)
        {
            if(50 > players[whoseTurn].GetComponent<PlayerControl>().capital)
            {
                players[whoseTurn].GetComponent<PlayerControl>().Bankruptcy(50);
            }
            else if(50 > players[whoseTurn].GetComponent<PlayerControl>().money)
            {
                players[whoseTurn].GetComponent<PlayerControl>().shortageReason = "Chest";
                players[whoseTurn].GetComponent<PlayerControl>().Shortage(50);
            }
            else
            {
                players[whoseTurn].GetComponent<PlayerControl>().capital -= 50;
                players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling -= 50;
                players[whoseTurn].GetComponent<PlayerControl>().worth -= 50;
                players[whoseTurn].GetComponent<PlayerControl>().money -= 50;
                players[whoseTurn].GetComponent<PlayerControl>().SubMoney(50);
                nextTurn();
            }
        }
        else if(i == 8)
        {
            players[whoseTurn].GetComponent<PlayerControl>().money += 200;
            players[whoseTurn].GetComponent<PlayerControl>().capital += 200;
            players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling += 200;
            players[whoseTurn].GetComponent<PlayerControl>().worth += 200;
            players[whoseTurn].GetComponent<PlayerControl>().AddMoney(200);
            players[whoseTurn].GetComponent<PlayerControl>().waypointIndex = 0;
            players[whoseTurn].GetComponent<PlayerControl>().move = 0;
            players[whoseTurn].GetComponent<PlayerControl>().MakeMove();
        }
        else if(i == 9)
        {
            if(50 > players[whoseTurn].GetComponent<PlayerControl>().capital)
            {
                players[whoseTurn].GetComponent<PlayerControl>().Bankruptcy(50);
            }
            else if(50 > players[whoseTurn].GetComponent<PlayerControl>().money)
            {
                players[whoseTurn].GetComponent<PlayerControl>().shortageReason = "Chest";
                players[whoseTurn].GetComponent<PlayerControl>().Shortage(50);
            }
            else
            {
                players[whoseTurn].GetComponent<PlayerControl>().capital -= 50;
                players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling -= 50;
                players[whoseTurn].GetComponent<PlayerControl>().worth -= 50;
                players[whoseTurn].GetComponent<PlayerControl>().money -= 50;
                players[whoseTurn].GetComponent<PlayerControl>().SubMoney(50);
                nextTurn();
            }
        }
        else if(i == 10)
        {
            if(100 > players[whoseTurn].GetComponent<PlayerControl>().capital)
            {
                players[whoseTurn].GetComponent<PlayerControl>().Bankruptcy(100);
            }
            else if(100 > players[whoseTurn].GetComponent<PlayerControl>().money)
            {
                players[whoseTurn].GetComponent<PlayerControl>().shortageReason = "Chest";
                players[whoseTurn].GetComponent<PlayerControl>().Shortage(100);
            }
            else
            {
                players[whoseTurn].GetComponent<PlayerControl>().capital -= 100;
                players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling -= 100;
                players[whoseTurn].GetComponent<PlayerControl>().worth -= 100;
                players[whoseTurn].GetComponent<PlayerControl>().money -= 100;
                players[whoseTurn].GetComponent<PlayerControl>().SubMoney(100);
                nextTurn();
            }
        }
        else if(i == 11)
        {
            players[whoseTurn].GetComponent<PlayerControl>().money += 100;
            players[whoseTurn].GetComponent<PlayerControl>().capital += 100;
            players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling += 100;
            players[whoseTurn].GetComponent<PlayerControl>().worth += 100;
            players[whoseTurn].GetComponent<PlayerControl>().AddMoney(100);
            nextTurn();
        }
        else if(i == 12)
        {
            players[whoseTurn].GetComponent<PlayerControl>().money += 50;
            players[whoseTurn].GetComponent<PlayerControl>().capital += 50;
            players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling += 50;
            players[whoseTurn].GetComponent<PlayerControl>().worth += 50;
            players[whoseTurn].GetComponent<PlayerControl>().AddMoney(50);
            nextTurn();
        }
        else if(i == 13)
        {
            players[whoseTurn].GetComponent<PlayerControl>().money += 10 * (activePlayers.Count - 1);
            players[whoseTurn].GetComponent<PlayerControl>().capital += 10 * (activePlayers.Count - 1);
            players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling += 10 * (activePlayers.Count - 1);
            players[whoseTurn].GetComponent<PlayerControl>().worth += 10 * (activePlayers.Count - 1);
            players[whoseTurn].GetComponent<PlayerControl>().AddMoney(10 * (activePlayers.Count - 1));
            for(int j = 1; j <= activePlayers.Count; j++)
            {
                if(j != whoseTurn)
                {
                    players[j].GetComponent<PlayerControl>().money -= 10;
                    players[j].GetComponent<PlayerControl>().capital -= 10;
                    players[j].GetComponent<PlayerControl>().capitalWithoutSelling -= 10;
                    players[j].GetComponent<PlayerControl>().worth -= 10;
                    players[j].GetComponent<PlayerControl>().SubMoney(10);
                }
            }
            nextTurn();
        }
        else if(i == 14)
        {
            players[whoseTurn].GetComponent<PlayerControl>().money += 100;
            players[whoseTurn].GetComponent<PlayerControl>().capital += 100;
            players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling += 100;
            players[whoseTurn].GetComponent<PlayerControl>().worth += 100;
            players[whoseTurn].GetComponent<PlayerControl>().AddMoney(100);
            nextTurn();
        }
        else if(i == 15)
        {
            players[whoseTurn].GetComponent<PlayerControl>().money += 200;
            players[whoseTurn].GetComponent<PlayerControl>().capital += 200;
            players[whoseTurn].GetComponent<PlayerControl>().capitalWithoutSelling += 200;
            players[whoseTurn].GetComponent<PlayerControl>().worth += 200;
            players[whoseTurn].GetComponent<PlayerControl>().AddMoney(200);
            nextTurn();
        }
        GameControl.chestfinished = true;
    }


    public static void ColorUpdate(string reason, int deedIndex=0, int whoBuys=0)
    {
        if(reason == "BuyNewDeed")
        {
            if(deedIndex == 2 || deedIndex == 4)
            {
                if(titleDeedCardsOwners[2] == titleDeedCardsOwners[4])
                {
                    colorOwners[0] = whoBuys;
                }
            }
            else if(deedIndex == 7 || deedIndex == 9 || deedIndex == 10)
            {
                if(titleDeedCardsOwners[7] == titleDeedCardsOwners[9] && titleDeedCardsOwners[7] == titleDeedCardsOwners[10])
                {
                    colorOwners[1] = whoBuys;
                }
            }
            else if(deedIndex == 12 || deedIndex == 14 || deedIndex == 15)
            {
                if(titleDeedCardsOwners[12] == titleDeedCardsOwners[14] && titleDeedCardsOwners[12] == titleDeedCardsOwners[15])
                {
                    colorOwners[2] = whoBuys;
                }
            }
            else if(deedIndex == 17 || deedIndex == 19 || deedIndex == 20)
            {
                if(titleDeedCardsOwners[17] == titleDeedCardsOwners[19] && titleDeedCardsOwners[17] == titleDeedCardsOwners[20])
                {
                    colorOwners[3] = whoBuys;
                }
            }
            else if(deedIndex == 22 || deedIndex == 24 || deedIndex == 25)
            {
                if(titleDeedCardsOwners[22] == titleDeedCardsOwners[24] && titleDeedCardsOwners[22] == titleDeedCardsOwners[25])
                {
                    colorOwners[4] = whoBuys;
                }
            }
            else if(deedIndex == 27 || deedIndex == 28 || deedIndex == 30)
            {
                if(titleDeedCardsOwners[27] == titleDeedCardsOwners[28] && titleDeedCardsOwners[27] == titleDeedCardsOwners[30])
                {
                    colorOwners[5] = whoBuys;
                }
            }
            else if(deedIndex == 32 || deedIndex == 33 || deedIndex == 35)
            {
                if(titleDeedCardsOwners[32] == titleDeedCardsOwners[33] && titleDeedCardsOwners[32] == titleDeedCardsOwners[35])
                {
                    colorOwners[6] = whoBuys;
                }
            }
            else if(deedIndex == 38 || deedIndex == 40)
            {
                if(titleDeedCardsOwners[38] == titleDeedCardsOwners[40])
                {
                    colorOwners[7] = whoBuys;
                }
            }
            Debug.Log("Color Owners = " + colorOwners[0]+","+colorOwners[1]+","+colorOwners[2]+","+colorOwners[3]+","+colorOwners[4]+","+colorOwners[5]+","+colorOwners[6]+","+colorOwners[7]);
        }
        else if(reason == "Mortgage")
        {
            if(deedIndex == 2 || deedIndex == 4)
            {
                if(colorOwners[0] > 0)
                {
                    colorOwners[0] = 0;
                }
            }
            else if(deedIndex == 7 || deedIndex == 9 || deedIndex == 10)
            {
                if(colorOwners[1] > 0)
                {
                    colorOwners[1] = 0;
                }
            }
            else if(deedIndex == 12 || deedIndex == 14 || deedIndex == 15)
            {
                if(colorOwners[2] > 0)
                {
                    colorOwners[2] = 0;
                }
            }
            else if(deedIndex == 17 || deedIndex == 19 || deedIndex == 20)
            {
                if(colorOwners[3] > 0)
                {
                    colorOwners[3] = 0;
                }
            }
            else if(deedIndex == 22 || deedIndex == 24 || deedIndex == 25)
            {
                if(colorOwners[4] > 0)
                {
                    colorOwners[4] = 0;
                }
            }
            else if(deedIndex == 27 || deedIndex == 28 || deedIndex == 30)
            {
                if(colorOwners[5] > 0)
                {
                    colorOwners[5] = 0;
                }
            }
            else if(deedIndex == 32 || deedIndex == 33 || deedIndex == 35)
            {
                if(colorOwners[6] > 0)
                {
                    colorOwners[6] = 0;
                }
            }
            else if(deedIndex == 38 || deedIndex == 40)
            {
                if(colorOwners[7] > 0)
                {
                    colorOwners[7] = 0;
                }
            }
            Debug.Log("Color Owners = " + colorOwners[0]+","+colorOwners[1]+","+colorOwners[2]+","+colorOwners[3]+","+colorOwners[4]+","+colorOwners[5]+","+colorOwners[6]+","+colorOwners[7]);
        }
    }

    public static void ColorHousesUpdate()
    {
        colorHouses[0] = titleDeedCards[2].numHouses + titleDeedCards[4].numHouses;
        colorHouses[1] = titleDeedCards[7].numHouses + titleDeedCards[9].numHouses + titleDeedCards[10].numHouses;
        colorHouses[2] = titleDeedCards[12].numHouses + titleDeedCards[14].numHouses + titleDeedCards[15].numHouses;
        colorHouses[3] = titleDeedCards[17].numHouses + titleDeedCards[19].numHouses + titleDeedCards[20].numHouses;
        colorHouses[4] = titleDeedCards[22].numHouses + titleDeedCards[24].numHouses + titleDeedCards[25].numHouses;
        colorHouses[5] = titleDeedCards[27].numHouses + titleDeedCards[28].numHouses + titleDeedCards[30].numHouses;
        colorHouses[6] = titleDeedCards[32].numHouses + titleDeedCards[33].numHouses + titleDeedCards[35].numHouses;
        colorHouses[7] = titleDeedCards[38].numHouses + titleDeedCards[40].numHouses;
        Debug.Log("Color Houses = " + colorHouses[0]+","+colorHouses[1]+","+colorHouses[2]+","+colorHouses[3]+","+colorHouses[4]+","+colorHouses[5]+","+colorHouses[6]+","+colorHouses[7]);
    }


    public static List<int> SortActivePlayers()
    {
        return activePlayers.OrderByDescending(player => players[player].GetComponent<PlayerControl>().worth).ToList();
    }

    public static int Price10(int price)
    {
        var priceint = price * 11;
        if(priceint % 10 == 0)
        {
            return (int)(priceint) / 10;
        }
        else
        {
            return (int)(priceint + 5) / 10;
        }
    }

    public static void TempColorOwners()
    {
        for(int i = 0; i < 8; i++)
        {
            tempColorOwners[i] = colorOwners[i];
        }
    }
}
