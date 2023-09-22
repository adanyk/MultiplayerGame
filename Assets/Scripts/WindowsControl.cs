using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WindowsControl : MonoBehaviour
{    
    // Actions
    [SerializeField] public GameObject ActionsWindow;
    public static GameObject actionsWindow;

        // Throw Dices
    [SerializeField] Button ThrowDicesButton;

        // Bail out
    [SerializeField] GameObject BailObject;
    public static GameObject bailObject;
    [SerializeField] Button BailMeOutButton;

        // Break free
    [SerializeField] GameObject BreakFreeObject;
    public static GameObject breakFreeObject;
    [SerializeField] Button BreakFreeButton;

        // Buy houses/hotels
    [SerializeField] GameObject BuyHousesObject;
    public static GameObject buyHousesObject;
    [SerializeField] Button BuyHousesButton;

    [SerializeField] public GameObject BuyHousesWindow;
    public static GameObject buyHousesWindow;
    [SerializeField] GameObject[] BuyHousesTexts;
    public static GameObject buyList;
    public static GameObject cash;
    public static GameObject price;
    public static GameObject rest;
    [SerializeField] GameObject ConfirmBuyHousesObject;
    public static GameObject confirmBuyHousesObject;
    [SerializeField] Button ConfirmBuyHousesButton;
    public static Button confirmBuyHousesButton;


        // Buy property
    [SerializeField] GameObject BuyWindow;
    public static GameObject buyWindow;
    [SerializeField] Button YesBuyButton;
    public static Button yesBuyButton;
    [SerializeField] Button NoBuyButton;
    

    // Chance and Treasure Chest
    [SerializeField] GameObject[] ChanceWindows;
    public static GameObject[] chanceWindows;
    [SerializeField] GameObject[] ChestWindows;
    public static GameObject[] chestWindows;
    [SerializeField] GameObject[] ChanceChestObjects;
    public static GameObject[] chanceChestObjects;
    [SerializeField] Button[] ChanceChestButtons;


    // Both Free cards
    [SerializeField] GameObject FreeCardsWindow;
    public static GameObject freeCardsWindow;
    [SerializeField] Button FreeChanceButton;
    [SerializeField] Button FreeChestButton;


    // Houses and Hotels
    [SerializeField] GameObject[] Houses1;
    [SerializeField] GameObject[] Houses2;
    [SerializeField] GameObject[] Houses3;
    [SerializeField] GameObject[] Houses4;
    [SerializeField] GameObject[] Hotels;
    public static GameObject[] houses1;
    public static GameObject[] houses2;
    public static GameObject[] houses3;
    public static GameObject[] houses4;
    public static GameObject[] hotels;
    public static List<GameObject[]> houses;
    
    [SerializeField] GameObject[] HouseObjects;
    public static GameObject[] houseObjects;
    [SerializeField] Button[] HouseButtons;


    // Mortgage and/or Sell houses
    [SerializeField] GameObject[] DeedObjects;
    public static GameObject[] deedObjects;
    [SerializeField] Button[] DeedButtons;
    [SerializeField] GameObject[] MortgageObjects;
    public static GameObject[] mortgageObjects;

    // Shortage
    [SerializeField] public GameObject ShortageWindow;
    public static GameObject shortageWindow;
    [SerializeField] GameObject[] ShortageTexts;
    public static GameObject sellList;
    public static GameObject sum;
    public static GameObject toPay;
    public static GameObject left;
    [SerializeField] Button ConfirmShortageButton;
    public static Button confirmShortageButton;


    // Auction
    [SerializeField] GameObject AuctionWindow;
    public static GameObject auctionWindow;
    [SerializeField] GameObject[] BiddersAndBidObjects;
    public static GameObject[,] biddersAndBidObjects;
    [SerializeField] Button[] BidButtons;
    public static Button[,] bidButtons;
    [SerializeField] GameObject[] HighestBidder;
    public static GameObject[] highestBidder;
    public static List<int> bides;

        // Unmortgage
    [SerializeField] GameObject UnmortgageObject;
    public static GameObject unmortgageObject;
    [SerializeField] Button UnmortgageButton;

    [SerializeField] public GameObject UnmortgageWindow;
    public static GameObject unmortgageWindow;
    [SerializeField] GameObject[] UnmortgageTexts;
    public static GameObject unmortgageList;
    public static GameObject unmCash;
    public static GameObject unmPrice;
    public static GameObject unmRest;
    [SerializeField] GameObject ConfirmUnmortgageObject;
    public static GameObject confirmUnmortgageObject;
    [SerializeField] Button ConfirmUnmortgageButton;
    public static Button confirmUnmortgageButton;

    // Bankruptcy
    [SerializeField] public GameObject BankruptcyWindow;
    public static GameObject bankruptcyWindow;
    [SerializeField] public GameObject BankruptcyMessage;
    public static GameObject bankruptcyMessage;
    [SerializeField] public GameObject BankruptcyOKObject;
    [SerializeField] Button BankruptcyOKButton;
    
    // Game Over
    [SerializeField] public GameObject GameOverWindow;
    public static GameObject gameOverWindow;
    [SerializeField] public GameObject WinnerMessage;
    public static GameObject winnerMessage;
    [SerializeField] public GameObject Highscore;
    public static GameObject highscore;
    [SerializeField] Button GameOverOKButton;

    // Are you sure?
    [SerializeField] public GameObject AreYouSureWindow;
    public static GameObject areYouSureWindow;
    [SerializeField] public GameObject AreYouSureMessage;
    public static GameObject areYouSureMessage;
    [SerializeField] Button YesButton;
    [SerializeField] Button NoButton;

    // Side Buttons
    [SerializeField] Button GameOverButton;
    [SerializeField] Button QuitButton;
    [SerializeField] Button ShowCardsButton;
    [SerializeField] Button TradeButton;






    // Windows bools
    public static bool actionsWindowOpen, buyWindowOpen, chanceWindowOpen, chestWindowOpen, freeWindowOpen, buyHousesWindowOpen, chanceToTake, chestToTake, shortageWindowOpen, auctionWindowOpen, unmortgageWindowOpen, bankruptcyWindowOpen, gameOverWindowOpen, areYouSureWindowOpen;

    // Buttons bools
    public static bool throwDices, yesBuy, noBuy, chanceReading, chestReading, chanceOK, chestOK, chestPay, takeChance, bailMeOut, breakFree, freeChanceB, freeChestB, buyHouses, confirmBuyHouses, confirmShortage, unmortgage, confirmUnmortgage, bankruptcyOK, gameOverOK, yes, no, gameOver, quit, showCards, trade;
        // Houses buttons bools
    public static bool AHouse, mortgaging;
    public static List<bool> housebools;
    public static List<bool> mortgagebools;
    public static bool[,] bidbools;



    void Start()
    {
        // Action
        actionsWindowOpen = false;
        actionsWindow = ActionsWindow;
        throwDices = false;
        bailObject = BailObject;
        breakFreeObject = BreakFreeObject;
        bailMeOut = false;
        breakFree = false;

// Buy houses/hotels================================================================================================
        buyHousesWindowOpen = false;
        buyHousesObject = BuyHousesObject;
        buyHouses = false;
        buyHousesWindow = BuyHousesWindow;
        buyList = BuyHousesTexts[0];
        cash = BuyHousesTexts[1];
        price = BuyHousesTexts[2];
        rest = BuyHousesTexts[3];
        confirmBuyHouses = false;
        houseObjects = HouseObjects;
        confirmBuyHousesObject = ConfirmBuyHousesObject;
        confirmBuyHousesButton = ConfirmBuyHousesButton;

        housebools = new List<bool>();
        for(int i = 0; i < 41; i++)
        {
            housebools.Add(false);
        }

        houses1 = Houses1;
        houses2 = Houses2;
        houses3 = Houses3;
        houses4 = Houses4;
        hotels = Hotels;
        houses = new List<GameObject[]>();
        houses.Add(houses1);
        houses.Add(houses2);
        houses.Add(houses3);
        houses.Add(houses4);
        houses.Add(hotels);
// End of buy houses/hotels================================================================================================


        // Buy property
        buyWindowOpen = false;
        buyWindow = BuyWindow;
        yesBuy = false;
        noBuy = false;
        yesBuyButton = YesBuyButton;


        // Chance and Treasure Chest
        chanceToTake = false;
        chestToTake = false;
        chanceWindowOpen = false;
        chestWindowOpen = false;

        chanceWindows = ChanceWindows;
        chestWindows = ChestWindows;
        chanceChestObjects = ChanceChestObjects;

        chanceReading = false;
        chestReading = false;
        chanceOK = false;
        chestOK = false;
        // Pay or take Chance
        chestPay = false;
        takeChance = false;


        // Both Free cards
        freeWindowOpen = false;
        freeCardsWindow = FreeCardsWindow;
        freeChanceB = false;
        freeChestB = false;
        

        // Shortage
        shortageWindow = ShortageWindow;
        deedObjects = DeedObjects;
        mortgageObjects = MortgageObjects;
        mortgaging = false;
        shortageWindowOpen = false;
        confirmShortage = false;
        confirmShortageButton = ConfirmShortageButton;
        sellList = ShortageTexts[0];
        sum = ShortageTexts[1];
        toPay = ShortageTexts[2];
        left = ShortageTexts[3];
        mortgagebools = new List<bool>();
        for(int i = 0; i < 41; i++)
        {
            mortgagebools.Add(false);
        }
        
        // Auction
        auctionWindow = AuctionWindow;
        auctionWindowOpen = false;
        biddersAndBidObjects = new GameObject[6, 8];
        for(int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                biddersAndBidObjects[i, j] = BiddersAndBidObjects[i * 8 + j];
            }
        }
        bidButtons = new Button[6, 7];
        for(int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 7; j++)
            {
                bidButtons[i, j] = BidButtons[i * 7 + j];
            }
        }
        highestBidder = HighestBidder;
        bidbools = new bool[6, 7];
        for(int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 7; j++)
            {
                bidbools[i, j] = false;
            }
        }
        bides = new List<int>{1, 5, 10, 50, 100};

        // Unmortgage
        unmortgageWindowOpen = false;
        unmortgageObject = UnmortgageObject;
        unmortgageWindow = UnmortgageWindow;
        unmortgageList = UnmortgageTexts[0];
        unmCash = UnmortgageTexts[1];
        unmPrice = UnmortgageTexts[2];
        unmRest = UnmortgageTexts[3];
        confirmUnmortgage = false;
        houseObjects = HouseObjects;
        confirmUnmortgageObject = ConfirmUnmortgageObject;
        confirmUnmortgageButton = ConfirmUnmortgageButton;
        unmortgage = false;

        // Bankruptcy
        bankruptcyWindow = BankruptcyWindow;
        bankruptcyMessage = BankruptcyMessage;
        bankruptcyWindowOpen = false;
        bankruptcyOK = false;

        // GameOver
        gameOverWindow = GameOverWindow;
        winnerMessage = WinnerMessage;
        highscore = Highscore;
        gameOverWindowOpen = false;
        gameOverOK = false;

        // Are you sure?
        areYouSureWindow = AreYouSureWindow;
        areYouSureMessage = AreYouSureMessage;
        areYouSureWindowOpen = false;
        yes = false;
        no = false;

        // Side Buttons
        gameOver = false;
        quit = false;
        showCards = false;
        trade = false;
    }


    void Update()
    {
        // Side Buttons
        GameOverButton.onClick.AddListener(GameOverClicked);
        QuitButton.onClick.AddListener(QuitClicked);
        ShowCardsButton.onClick.AddListener(ShowCardsClicked);

        // Actions
        if(actionsWindowOpen)
        {
            ThrowDicesButton.onClick.AddListener(ThrowDicesClicked);
            BailMeOutButton.onClick.AddListener(BailMeOutClicked);
            BreakFreeButton.onClick.AddListener(BreakFreeClicked);
            BuyHousesButton.onClick.AddListener(BuyHousesClicked);
            UnmortgageButton.onClick.AddListener(UnmortgageClicked);
            TradeButton.onClick.AddListener(TradeClicked);
        }
        else if(shortageWindowOpen || buyHousesWindowOpen || unmortgageWindowOpen)
        {

            if(buyHousesWindowOpen) // Confirm buying houses
            {
                ConfirmBuyHousesButton.onClick.AddListener(ConfirmBuyHousesClicked);
            }
            else if(!mortgaging) // Can mean mortgaging or unmortgaging
            {
                ConfirmShortageButton.onClick.AddListener(ConfirmShortageClicked);
                ConfirmUnmortgageButton.onClick.AddListener(ConfirmUnmortgageClicked);
                
                DeedButtons[2].onClick.AddListener(deed2Clicked);
                DeedButtons[4].onClick.AddListener(deed4Clicked);

                DeedButtons[7].onClick.AddListener(deed7Clicked);
                DeedButtons[9].onClick.AddListener(deed9Clicked);
                DeedButtons[10].onClick.AddListener(deed10Clicked);

                DeedButtons[12].onClick.AddListener(deed12Clicked);
                DeedButtons[14].onClick.AddListener(deed14Clicked);
                DeedButtons[15].onClick.AddListener(deed15Clicked);

                DeedButtons[17].onClick.AddListener(deed17Clicked);
                DeedButtons[19].onClick.AddListener(deed19Clicked);
                DeedButtons[20].onClick.AddListener(deed20Clicked);

                DeedButtons[22].onClick.AddListener(deed22Clicked);
                DeedButtons[24].onClick.AddListener(deed24Clicked);
                DeedButtons[25].onClick.AddListener(deed25Clicked);

                DeedButtons[27].onClick.AddListener(deed27Clicked);
                DeedButtons[28].onClick.AddListener(deed28Clicked);
                DeedButtons[30].onClick.AddListener(deed30Clicked);

                DeedButtons[32].onClick.AddListener(deed32Clicked);
                DeedButtons[33].onClick.AddListener(deed33Clicked);
                DeedButtons[35].onClick.AddListener(deed35Clicked);

                DeedButtons[38].onClick.AddListener(deed38Clicked);
                DeedButtons[40].onClick.AddListener(deed40Clicked);


                DeedButtons[6].onClick.AddListener(deed6Clicked);
                DeedButtons[16].onClick.AddListener(deed16Clicked);
                DeedButtons[26].onClick.AddListener(deed26Clicked);
                DeedButtons[36].onClick.AddListener(deed36Clicked);
                
                DeedButtons[13].onClick.AddListener(deed13Clicked);
                DeedButtons[29].onClick.AddListener(deed29Clicked);
            }

            // House Buttons
            if(!AHouse && !confirmBuyHouses && !GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().povertyON) // && !confirmBuyHouses - just a precaution
            {
                HouseButtons[2].onClick.AddListener(House2Clicked);
                HouseButtons[4].onClick.AddListener(House4Clicked);

                HouseButtons[7].onClick.AddListener(House7Clicked);
                HouseButtons[9].onClick.AddListener(House9Clicked);
                HouseButtons[10].onClick.AddListener(House10Clicked);

                HouseButtons[12].onClick.AddListener(House12Clicked);
                HouseButtons[14].onClick.AddListener(House14Clicked);
                HouseButtons[15].onClick.AddListener(House15Clicked);

                HouseButtons[17].onClick.AddListener(House17Clicked);
                HouseButtons[19].onClick.AddListener(House19Clicked);
                HouseButtons[20].onClick.AddListener(House20Clicked);

                HouseButtons[22].onClick.AddListener(House22Clicked);
                HouseButtons[24].onClick.AddListener(House24Clicked);
                HouseButtons[25].onClick.AddListener(House25Clicked);

                HouseButtons[27].onClick.AddListener(House27Clicked);
                HouseButtons[28].onClick.AddListener(House28Clicked);
                HouseButtons[30].onClick.AddListener(House30Clicked);

                HouseButtons[32].onClick.AddListener(House32Clicked);
                HouseButtons[33].onClick.AddListener(House33Clicked);
                HouseButtons[35].onClick.AddListener(House35Clicked);

                HouseButtons[38].onClick.AddListener(House38Clicked);
                HouseButtons[40].onClick.AddListener(House40Clicked);
            }
        }
        else if(buyWindowOpen)
        {
            // Buy property
            YesBuyButton.onClick.AddListener(YesBuyClicked);
            NoBuyButton.onClick.AddListener(NoBuyClicked);
        }
            // Chance and Treasure Chest
        else if(chanceToTake)
        {
            ChanceChestButtons[0].onClick.AddListener(ChanceButtonClicked);
        }
        else if(chestToTake)
        {
            ChanceChestButtons[1].onClick.AddListener(ChestButtonClicked);
        }
        else if(chanceWindowOpen)
        {
            ChanceChestButtons[2].onClick.AddListener(ChanceOKClicked);
        }
        else if(chestWindowOpen)
        {
            ChanceChestButtons[3].onClick.AddListener(ChestOKClicked);
            ChanceChestButtons[4].onClick.AddListener(ChestOKClicked);
            ChanceChestButtons[5].onClick.AddListener(TakeChanceClicked);
        }
            // End of Chance and Treasure Chest
        else if(freeWindowOpen)
        {
            // Both Free cards
            FreeChanceButton.onClick.AddListener(FreeChanceClicked);
            FreeChestButton.onClick.AddListener(FreeChestClicked);
        }
        //Auction
        else if(auctionWindowOpen)
        {
            bidButtons[0,0].onClick.AddListener(Bid10kClicked1);
            bidButtons[0,1].onClick.AddListener(Bid50kClicked1);
            bidButtons[0,2].onClick.AddListener(Bid100kClicked1);
            bidButtons[0,3].onClick.AddListener(Bid500kClicked1);
            bidButtons[0,4].onClick.AddListener(Bid1MClicked1);
            bidButtons[0,5].onClick.AddListener(PassClicked1);
            bidButtons[0,6].onClick.AddListener(BuyClicked1);

            bidButtons[1,0].onClick.AddListener(Bid10kClicked2);
            bidButtons[1,1].onClick.AddListener(Bid50kClicked2);
            bidButtons[1,2].onClick.AddListener(Bid100kClicked2);
            bidButtons[1,3].onClick.AddListener(Bid500kClicked2);
            bidButtons[1,4].onClick.AddListener(Bid1MClicked2);
            bidButtons[1,5].onClick.AddListener(PassClicked2);
            bidButtons[1,6].onClick.AddListener(BuyClicked2);
            
            bidButtons[2,0].onClick.AddListener(Bid10kClicked3);
            bidButtons[2,1].onClick.AddListener(Bid50kClicked3);
            bidButtons[2,2].onClick.AddListener(Bid100kClicked3);
            bidButtons[2,3].onClick.AddListener(Bid500kClicked3);
            bidButtons[2,4].onClick.AddListener(Bid1MClicked3);
            bidButtons[2,5].onClick.AddListener(PassClicked3);
            bidButtons[2,6].onClick.AddListener(BuyClicked3);
            
            bidButtons[3,0].onClick.AddListener(Bid10kClicked4);
            bidButtons[3,1].onClick.AddListener(Bid50kClicked4);
            bidButtons[3,2].onClick.AddListener(Bid100kClicked4);
            bidButtons[3,3].onClick.AddListener(Bid500kClicked4);
            bidButtons[3,4].onClick.AddListener(Bid1MClicked4);
            bidButtons[3,5].onClick.AddListener(PassClicked4);
            bidButtons[3,6].onClick.AddListener(BuyClicked4);
            
            bidButtons[4,0].onClick.AddListener(Bid10kClicked5);
            bidButtons[4,1].onClick.AddListener(Bid50kClicked5);
            bidButtons[4,2].onClick.AddListener(Bid100kClicked5);
            bidButtons[4,3].onClick.AddListener(Bid500kClicked5);
            bidButtons[4,4].onClick.AddListener(Bid1MClicked5);
            bidButtons[4,5].onClick.AddListener(PassClicked5);
            bidButtons[4,6].onClick.AddListener(BuyClicked5);
            
            bidButtons[5,0].onClick.AddListener(Bid10kClicked6);
            bidButtons[5,1].onClick.AddListener(Bid50kClicked6);
            bidButtons[5,2].onClick.AddListener(Bid100kClicked6);
            bidButtons[5,3].onClick.AddListener(Bid500kClicked6);
            bidButtons[5,4].onClick.AddListener(Bid1MClicked6);
            bidButtons[5,5].onClick.AddListener(PassClicked6);
            bidButtons[5,6].onClick.AddListener(BuyClicked6);
        }
        // Bankruptcy
        else if(bankruptcyWindowOpen)
        {
            BankruptcyOKButton.onClick.AddListener(BankruptcyOKClicked);
        }
        // Game Over
        else if(gameOverWindowOpen)
        {
            GameOverOKButton.onClick.AddListener(GameOverOKClicked);
        }

        // Are you sure?
        if(areYouSureWindowOpen)
        {
            YesButton.onClick.AddListener(YesClicked);
            NoButton.onClick.AddListener(NoClicked);
        }
    }


    // Actions
    public static void OpenActionsWindow()
    {
        actionsWindow.SetActive(true);
        actionsWindowOpen = true;
    }
    public static void CloseActionsWindow()
    {
        actionsWindow.SetActive(false);
        actionsWindowOpen = false;
    }
        // Throw Dices
    void ThrowDicesClicked()
    {
        throwDices = true;
    }
        // Bail out
    void BailMeOutClicked()
    {
        bailMeOut = true;
    }
        // Break free
    void BreakFreeClicked()
    {
        breakFree = true;
    }
        // Buy houses/hotels
    void BuyHousesClicked()
    {
        buyHouses = true;
    }
    public static void OpenBuyHousesWindow()
    {
        buyHousesWindow.SetActive(true);
        buyHousesWindowOpen = true;
    }
    public static void CloseBuyHousesWindow()
    {
        buyHousesWindow.SetActive(false);
        buyHousesWindowOpen = false;
    }
    public static void OpenHouseButtons(bool buyAfterUnmortgaging=false)
    {
        if(!buyAfterUnmortgaging)
        {
            GameControl.TempColorOwners();
        }
        if(GameControl.tempColorOwners[0] == GameControl.whoseTurn) // Browns
        {
            if(GameControl.titleDeedCards[2].numHouses < 5)
            {
                houseObjects[2].SetActive(true);
            }
            if(GameControl.titleDeedCards[4].numHouses < 5)
            {
                houseObjects[4].SetActive(true);
            }
        }
        if(GameControl.tempColorOwners[1] == GameControl.whoseTurn) // Lightblues
        {
            if(GameControl.titleDeedCards[7].numHouses < 5)
            {
                houseObjects[7].SetActive(true);
            }
            if(GameControl.titleDeedCards[9].numHouses < 5)
            {
                houseObjects[9].SetActive(true);
            } 
            if(GameControl.titleDeedCards[10].numHouses < 5)
            {
                houseObjects[10].SetActive(true);
            } 
        }
        if(GameControl.tempColorOwners[2] == GameControl.whoseTurn) // Roses
        {
            if(GameControl.titleDeedCards[12].numHouses < 5)
            {
                houseObjects[12].SetActive(true);
            }
            if(GameControl.titleDeedCards[14].numHouses < 5)
            {
                houseObjects[14].SetActive(true);
            } 
            if(GameControl.titleDeedCards[15].numHouses < 5)
            {
                houseObjects[15].SetActive(true);
            } 
        }
        if(GameControl.tempColorOwners[3] == GameControl.whoseTurn) // Oranges
        {
            if(GameControl.titleDeedCards[17].numHouses < 5)
            {
                houseObjects[17].SetActive(true);
            }
            if(GameControl.titleDeedCards[19].numHouses < 5)
            {
                houseObjects[19].SetActive(true);
            } 
            if(GameControl.titleDeedCards[20].numHouses < 5)
            {
                houseObjects[20].SetActive(true);
            }
        }
        if(GameControl.tempColorOwners[4] == GameControl.whoseTurn) // Reds
        {
            if(GameControl.titleDeedCards[22].numHouses < 5)
            {
                houseObjects[22].SetActive(true);
            }
            if(GameControl.titleDeedCards[24].numHouses < 5)
            {
                houseObjects[24].SetActive(true);
            } 
            if(GameControl.titleDeedCards[25].numHouses < 5)
            {
                houseObjects[25].SetActive(true);
            }
        }
        if(GameControl.tempColorOwners[5] == GameControl.whoseTurn) // Yellows
        {
            if(GameControl.titleDeedCards[27].numHouses < 5)
            {
                houseObjects[27].SetActive(true);
            }
            if(GameControl.titleDeedCards[28].numHouses < 5)
            {
                houseObjects[28].SetActive(true);
            } 
            if(GameControl.titleDeedCards[30].numHouses < 5)
            {
                houseObjects[30].SetActive(true);
            }
        }
        if(GameControl.tempColorOwners[6] == GameControl.whoseTurn) // Greens
        {
            if(GameControl.titleDeedCards[32].numHouses < 5)
            {
                houseObjects[32].SetActive(true);
            }
            if(GameControl.titleDeedCards[33].numHouses < 5)
            {
                houseObjects[33].SetActive(true);
            } 
            if(GameControl.titleDeedCards[35].numHouses < 5)
            {
                houseObjects[35].SetActive(true);
            }
        }
        if(GameControl.tempColorOwners[7] == GameControl.whoseTurn) // Blues
        {
            if(GameControl.titleDeedCards[38].numHouses < 5)
            {
                houseObjects[38].SetActive(true);
            }
            if(GameControl.titleDeedCards[40].numHouses < 5)
            {
                houseObjects[40].SetActive(true);
            }
        }
    }
    public static void ConfirmBuyHousesClicked()
    {
        confirmBuyHouses = true;
    }

        // House Buttons
    public static void House2Clicked()
    {
        housebools[2] = true;
        AHouse = true;
    }
    public static void House4Clicked()
    {
        housebools[4] = true;
        AHouse = true;
    }

    public static void House7Clicked()
    {
        housebools[7] = true;
        AHouse = true;
    }
    public static void House9Clicked()
    {
        housebools[9] = true;
        AHouse = true;
    }
    public static void House10Clicked()
    {
        housebools[10] = true;
        AHouse = true;
    }

    public static void House12Clicked()
    {
        housebools[12] = true;
        AHouse = true;
    }
    public static void House14Clicked()
    {
        housebools[14] = true;
        AHouse = true;
    }
    public static void House15Clicked()
    {
        housebools[15] = true;
        AHouse = true;
    }

    public static void House17Clicked()
    {
        housebools[17] = true;
        AHouse = true;
    }
    public static void House19Clicked()
    {
        housebools[19] = true;
        AHouse = true;
    }
    public static void House20Clicked()
    {
        housebools[20] = true;
        AHouse = true;
    }

    public static void House22Clicked()
    {
        housebools[22] = true;
        AHouse = true;
    }
    public static void House24Clicked()
    {
        housebools[24] = true;
        AHouse = true;
    }
    public static void House25Clicked()
    {
        housebools[25] = true;
        AHouse = true;
    }

    public static void House27Clicked()
    {
        housebools[27] = true;
        AHouse = true;
    }
    public static void House28Clicked()
    {
        housebools[28] = true;
        AHouse = true;
    }
    public static void House30Clicked()
    {
        housebools[30] = true;
        AHouse = true;
    }

    public static void House32Clicked()
    {
        housebools[32] = true;
        AHouse = true;
    }
    public static void House33Clicked()
    {
        housebools[33] = true;
        AHouse = true;
    }
    public static void House35Clicked()
    {
        housebools[35] = true;
        AHouse = true;
    }

    public static void House38Clicked()
    {
        housebools[38] = true;
        AHouse = true;
    }
    public static void House40Clicked()
    {
        housebools[40] = true;
        AHouse = true;
    }
    
    

    // Buy property
    public static void OpenBuyWindow()
    {   
        price.GetComponent<TextMeshProUGUI>().text = "0";
        buyWindow.SetActive(true);
        buyWindowOpen = true;
    }
    public static void CloseBuyWindow()
    {
        buyWindow.SetActive(false);
        buyWindowOpen = false;
    }
    public void YesBuyClicked()
    {
        yesBuy = true;
    }
    public void NoBuyClicked()
    {
        noBuy = true;
    }

    // Chance and Treasure Chest
    public static void ChanceButtonClicked()
    {
        chanceReading = true;
    }
    public static void ChestButtonClicked()
    {
        chestReading = true;
    }
    public static void ChanceOKClicked()
    {
        chanceOK = true;
    }
    public static void ChestOKClicked()
    {
        chestOK = true;
    }
    // Pay or take Chance
    public static void TakeChanceClicked()
    {
        takeChance = true;
    }
    
    // Both Free cards
    public static void OpenFreeCardsWindow()
    {
        freeWindowOpen = true;
        freeCardsWindow.SetActive(true);
    }
    public static void CloseFreeCardsWindow()
    {
        freeWindowOpen = false;
        freeCardsWindow.SetActive(false);
    }
    void FreeChanceClicked()
    {
        freeChanceB = true;
    }
    void FreeChestClicked()
    {
        freeChestB = true;
    }

    // Shortage
    void ConfirmShortageClicked()
    {
        confirmShortage = true;
    }
    public static void OpenShortageWindow(bool poverty, int player=0)
    {
        if(player == 0) // Useful in case it is not bidding winner's turn, and he run out of cash
        {
            player = GameControl.whoseTurn;
        }
        shortageWindow.SetActive(true);
        shortageWindowOpen = true;
        if(!poverty)
        {
            OpenSellHouseButtons(player);
            OpenDeedButtons(player);
        }
    }
    public static void CloseShortageWindow()
    {
        shortageWindow.SetActive(false);
        shortageWindowOpen = false;
    }
    static void OpenSellHouseButtons(int player)
    {
        if(GameControl.colorOwners[0] == player) // Browns
        {
            if(GameControl.titleDeedCards[2].numHouses > 0)
            {
                houseObjects[2].SetActive(true);
            }
            if(GameControl.titleDeedCards[4].numHouses > 0)
            {
                houseObjects[4].SetActive(true);
            }
        }if(GameControl.colorOwners[1] == player) // Lightblues
        {
            if(GameControl.titleDeedCards[7].numHouses > 0)
            {
                houseObjects[7].SetActive(true);
            }
            if(GameControl.titleDeedCards[9].numHouses > 0)
            {
                houseObjects[9].SetActive(true);
            } 
            if(GameControl.titleDeedCards[10].numHouses > 0)
            {
                houseObjects[10].SetActive(true);
            } 
        }
        if(GameControl.colorOwners[2] == player) // Roses
        {
            if(GameControl.titleDeedCards[12].numHouses > 0)
            {
                houseObjects[12].SetActive(true);
            }
            if(GameControl.titleDeedCards[14].numHouses > 0)
            {
                houseObjects[14].SetActive(true);
            } 
            if(GameControl.titleDeedCards[15].numHouses > 0)
            {
                houseObjects[15].SetActive(true);
            } 
        }
        if(GameControl.colorOwners[3] == player) // Oranges
        {
            if(GameControl.titleDeedCards[17].numHouses > 0)
            {
                houseObjects[17].SetActive(true);
            }
            if(GameControl.titleDeedCards[19].numHouses > 0)
            {
                houseObjects[19].SetActive(true);
            } 
            if(GameControl.titleDeedCards[20].numHouses > 0)
            {
                houseObjects[20].SetActive(true);
            }
        }
        if(GameControl.colorOwners[4] == player) // Reds
        {
            if(GameControl.titleDeedCards[22].numHouses > 0)
            {
                houseObjects[22].SetActive(true);
            }
            if(GameControl.titleDeedCards[24].numHouses > 0)
            {
                houseObjects[24].SetActive(true);
            } 
            if(GameControl.titleDeedCards[25].numHouses > 0)
            {
                houseObjects[25].SetActive(true);
            }
        }
        if(GameControl.colorOwners[5] == player) // Yellows
        {
            if(GameControl.titleDeedCards[27].numHouses > 0)
            {
                houseObjects[27].SetActive(true);
            }
            if(GameControl.titleDeedCards[28].numHouses > 0)
            {
                houseObjects[28].SetActive(true);
            } 
            if(GameControl.titleDeedCards[30].numHouses > 0)
            {
                houseObjects[30].SetActive(true);
            }
        }
        if(GameControl.colorOwners[6] == player) // Greens
        {
            if(GameControl.titleDeedCards[32].numHouses > 0)
            {
                houseObjects[32].SetActive(true);
            }
            if(GameControl.titleDeedCards[33].numHouses > 0)
            {
                houseObjects[33].SetActive(true);
            } 
            if(GameControl.titleDeedCards[35].numHouses > 0)
            {
                houseObjects[35].SetActive(true);
            }
        }
        if(GameControl.colorOwners[7] == player) // Blues
        {
            if(GameControl.titleDeedCards[38].numHouses > 0)
            {
                houseObjects[38].SetActive(true);
            }
            if(GameControl.titleDeedCards[40].numHouses > 0)
            {
                houseObjects[40].SetActive(true);
            }
        }
    }

    public static void OpenSellDeedButtons(int player)
    {
        for(int i = 2; i < 41; i++ )
        {
            if((GameControl.titleDeedCardsOwners[i] * (-1)) == player)
            {
                deedObjects[i].SetActive(true);
            }
        }
    }

    static void OpenDeedButtons(int player)
    {
        for(int i = 0; i < 41; i++ )
        {   // First, open buttons for all title deed cards Player owns
            if(GameControl.titleDeedCardsOwners[i] == player)
            {
                deedObjects[i].SetActive(true);
            }
        }
        for(int i = 0; i < 8; i++)
        {   // Then, close buttons if there are any houses on the color
            if(GameControl.colorOwners[i] == player && GameControl.colorHouses[i] > 0)
            {
                if(i == 0)
                {
                    deedObjects[2].SetActive(false);
                    deedObjects[4].SetActive(false);
                }
                else if(i == 1)
                {
                    deedObjects[7].SetActive(false);
                    deedObjects[9].SetActive(false);
                    deedObjects[10].SetActive(false);
                }
                else if(i == 2)
                {
                    deedObjects[12].SetActive(false);
                    deedObjects[14].SetActive(false);
                    deedObjects[15].SetActive(false);
                }
                else if(i == 3)
                {
                    deedObjects[17].SetActive(false);
                    deedObjects[19].SetActive(false);
                    deedObjects[20].SetActive(false);
                }
                else if(i == 4)
                {
                    deedObjects[22].SetActive(false);
                    deedObjects[24].SetActive(false);
                    deedObjects[25].SetActive(false);
                }
                else if(i == 5)
                {
                    deedObjects[27].SetActive(false);
                    deedObjects[28].SetActive(false);
                    deedObjects[30].SetActive(false);
                }
                else if(i == 6)
                {
                    deedObjects[32].SetActive(false);
                    deedObjects[33].SetActive(false);
                    deedObjects[35].SetActive(false);
                }
                else if(i == 7)
                {
                    deedObjects[38].SetActive(false);
                    deedObjects[40].SetActive(false);
                }
            }
        }
    }

    // Mortgaging or selling deed or unmortgaging
    public static void deed2Clicked()
    {
        mortgagebools[2] = true;
        mortgaging = true;
    }
    public static void deed4Clicked()
    {
        mortgagebools[4] = true;
        mortgaging = true;
    }
    public static void deed6Clicked()
    {
        mortgagebools[6] = true;
        mortgaging = true;
    }
    public static void deed7Clicked()
    {
        mortgagebools[7] = true;
        mortgaging = true;
    }
    public static void deed9Clicked()
    {
        mortgagebools[9] = true;
        mortgaging = true;
    }
    public static void deed10Clicked()
    {
        mortgagebools[10] = true;
        mortgaging = true;
    }
    public static void deed12Clicked()
    {
        mortgagebools[12] = true;
        mortgaging = true;
    }
    public static void deed13Clicked()
    {
        mortgagebools[13] = true;
        mortgaging = true;
    }
    public static void deed14Clicked()
    {
        mortgagebools[14] = true;
        mortgaging = true;
    }
    public static void deed15Clicked()
    {
        mortgagebools[15] = true;
        mortgaging = true;
    }
    public static void deed16Clicked()
    {
        mortgagebools[16] = true;
        mortgaging = true;
    }
    public static void deed17Clicked()
    {
        mortgagebools[17] = true;
        mortgaging = true;
    }
    public static void deed19Clicked()
    {
        mortgagebools[19] = true;
        mortgaging = true;
    }
    public static void deed20Clicked()
    {
        mortgagebools[20] = true;
        mortgaging = true;
    }
    public static void deed22Clicked()
    {
        mortgagebools[22] = true;
        mortgaging = true;
    }
    public static void deed24Clicked()
    {
        mortgagebools[24] = true;
        mortgaging = true;
    }
    public static void deed25Clicked()
    {
        mortgagebools[25] = true;
        mortgaging = true;
    }
    public static void deed26Clicked()
    {
        mortgagebools[26] = true;
        mortgaging = true;
    }
    public static void deed27Clicked()
    {
        mortgagebools[27] = true;
        mortgaging = true;
    }
    public static void deed28Clicked()
    {
        mortgagebools[28] = true;
        mortgaging = true;
    }
    public static void deed29Clicked()
    {
        mortgagebools[29] = true;
        mortgaging = true;
    }
    public static void deed30Clicked()
    {
        mortgagebools[30] = true;
        mortgaging = true;
    }
    public static void deed32Clicked()
    {
        mortgagebools[32] = true;
        mortgaging = true;
    }
    public static void deed33Clicked()
    {
        mortgagebools[33] = true;
        mortgaging = true;
    }
    public static void deed35Clicked()
    {
        mortgagebools[35] = true;
        mortgaging = true;
    }
    public static void deed36Clicked()
    {
        mortgagebools[36] = true;
        mortgaging = true;
    }
    public static void deed38Clicked()
    {
        mortgagebools[38] = true;
        mortgaging = true;
    }
    public static void deed40Clicked()
    {
        mortgagebools[40] = true;
        mortgaging = true;
    }


    // Auction
    public static void OpenAuctionWindow()
    {
        GameControl.bid = 0;
        GameControl.activeBidders = GameControl.activePlayers.Count;
        highestBidder[2].GetComponent<TextMeshProUGUI>().text = "0";
        highestBidder[0].GetComponent<SpriteRenderer>().color = Color.white;
        highestBidder[1].GetComponent<SpriteRenderer>().color = Color.white;
        for(int i = 0; i < GameControl.activePlayers.Count; i++)
        {
            GameControl.players[GameControl.activePlayers[i]].GetComponent<PlayerControl>().settingUpBids = true;
            GameControl.players[GameControl.activePlayers[i]].GetComponent<PlayerControl>().myBid = -1;
            GameControl.players[GameControl.activePlayers[i]].GetComponent<PlayerControl>().botIsInBidding = true;
        }
        auctionWindow.SetActive(true);
        auctionWindowOpen = true;
    }
    public static void CloseAuctionWindow()
    {
        auctionWindowOpen = false;
        auctionWindow.SetActive(false);
        for(int i = 0; i < GameControl.activePlayers.Count; i++)
        {
            biddersAndBidObjects[GameControl.activePlayers[i] - 1, 0].SetActive(false);
        }
    }
    void Bid10kClicked1()
    {
        bidbools[0,0] = true;
    }
    void Bid50kClicked1()
    {
        bidbools[0,1] = true;
    }
    void Bid100kClicked1()
    {
        bidbools[0,2] = true;
    }
    void Bid500kClicked1()
    {
        bidbools[0,3] = true;
    }
    void Bid1MClicked1()
    {
        bidbools[0,4] = true;
    }
    void PassClicked1()
    {
        bidbools[0,5] = true;
    }
    void BuyClicked1()
    {
        bidbools[0,6] = true;
    }
    void Bid10kClicked2()
    {
        bidbools[1,0] = true;
    }
    void Bid50kClicked2()
    {
        bidbools[1,1] = true;
    }
    void Bid100kClicked2()
    {
        bidbools[1,2] = true;
    }
    void Bid500kClicked2()
    {
        bidbools[1,3] = true;
    }
    void Bid1MClicked2()
    {
        bidbools[1,4] = true;
    }
    void PassClicked2()
    {
        bidbools[1,5] = true;
    }
    void BuyClicked2()
    {
        bidbools[1,6] = true;
    }
    void Bid10kClicked3()
    {
        bidbools[2,0] = true;
    }
    void Bid50kClicked3()
    {
        bidbools[2,1] = true;
    }
    void Bid100kClicked3()
    {
        bidbools[2,2] = true;
    }
    void Bid500kClicked3()
    {
        bidbools[2,3] = true;
    }
    void Bid1MClicked3()
    {
        bidbools[2,4] = true;
    }
    void PassClicked3()
    {
        bidbools[2,5] = true;
    }
    void BuyClicked3()
    {
        bidbools[2,6] = true;
    }
    void Bid10kClicked4()
    {
        bidbools[3,0] = true;
    }
    void Bid50kClicked4()
    {
        bidbools[3,1] = true;
    }
    void Bid100kClicked4()
    {
        bidbools[3,2] = true;
    }
    void Bid500kClicked4()
    {
        bidbools[3,3] = true;
    }
    void Bid1MClicked4()
    {
        bidbools[3,4] = true;
    }
    void PassClicked4()
    {
        bidbools[3,5] = true;
    }
    void BuyClicked4()
    {
        bidbools[3,6] = true;
    }
    void Bid10kClicked5()
    {
        bidbools[4,0] = true;
    }
    void Bid50kClicked5()
    {
        bidbools[4,1] = true;
    }
    void Bid100kClicked5()
    {
        bidbools[4,2] = true;
    }
    void Bid500kClicked5()
    {
        bidbools[4,3] = true;
    }
    void Bid1MClicked5()
    {
        bidbools[4,4] = true;
    }
    void PassClicked5()
    {
        bidbools[4,5] = true;
    }
    void BuyClicked5()
    {
        bidbools[4,6] = true;
    }
    void Bid10kClicked6()
    {
        bidbools[5,0] = true;
    }
    void Bid50kClicked6()
    {
        bidbools[5,1] = true;
    }
    void Bid100kClicked6()
    {
        bidbools[5,2] = true;
    }
    void Bid500kClicked6()
    {
        bidbools[5,3] = true;
    }
    void Bid1MClicked6()
    {
        bidbools[5,4] = true;
    }
    void PassClicked6()
    {
        bidbools[5,5] = true;
    }
    void BuyClicked6()
    {
        bidbools[5,6] = true;
    }

    // Unmortgage
    void UnmortgageClicked()
    {
        unmortgage = true;
    }
    public static void OpenUnmortgageWindow()
    {
        unmortgageWindow.SetActive(true);
        unmortgageWindowOpen = true;
    }
    public static void CloseUnmortgageWindow()
    {
        unmortgageWindow.SetActive(false);
        unmortgageWindowOpen = false;
    }
    void ConfirmUnmortgageClicked()
    {
        confirmUnmortgage = true;
    }
    
    // Bankruptcy
    void BankruptcyOKClicked()
    {
        bankruptcyOK = true;
    }
    // GameOver
    void GameOverOKClicked()
    {
        gameOverOK = true;
    }

    // Are you sure?
    void YesClicked()
    {
        yes = true;
    }
    void NoClicked()
    {
        no = true;
    }

    // Side buttons
    void GameOverClicked()
    {
        gameOver = true;
    }
    void QuitClicked()
    {
        quit = true;
    }
    void ShowCardsClicked()
    {
        showCards = true;
    }
    void TradeClicked()
    {
        trade = true;
    }
}