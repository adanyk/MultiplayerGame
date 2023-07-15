using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] Transform InitialWaypoint;
    [SerializeField] public GameObject addMoneyText;
    [SerializeField] public GameObject subMoneyText;

    public int myIndex, waypointIndex, index, move, inJailLeft, previousWaypointindex, previousPosition;
    public int money, totalNumOfHouses, totalNumOfHotels, capital, payee, capitalWithoutSelling, worth;
    public bool myTurn, secondRoll, moving, fieldOccupied, buyingOpportunity, chanceChestOpportunity, chanceChestOK, buyingHouses, buyingHousesObjectActivated, shortageON, povertyON, unmortgaging, housesBought, deedsUnmortgaged, buildingConditionsChecked, unmortgageConditionsChecked;
    public bool freeChance, freeChest; // Go out of jail cards
    Vector3 target = new Vector3();
    Vector3 shift = new Vector3();

    // Get updated in GameControl.playerUpdate()
    [SerializeField] public GameObject[] NameAndMoney;
    public string moneyText;

    [SerializeField] public GameObject TurnMarker;

    // Rent to pay/collect
    public int rent;


    // Buy houses window temp values    AND  Mortgage and/or Sell houses    AND  Unmortgage
    public string tempBuyList, tempSellList, tempSellHousesList, tempMortgageList, tempUnmortgageList;
    public int tempPrice, tempRest, whichHouse, tempSum, tempToPay, tempSumSolgt, tempSolgt, tempSumMortgaged, tempMortgaged, whichDeed, housesLeft, tempPrice10;
    public List<bool> tempHouses, tempMortgage, tempUnmortgage;
    public List<int> tempSellHouses, tempColorHouses;
    public string shortageReason;



    // Money transaction
    public string moneyToAdd, moneyToSub;
    Vector3 addingMoneyTarget, subingMoneyTarget;
    public bool addingMoney, subingMoney;
    public bool addStartMoney;

    // Auction
    public bool stillInBidding, biddingWinner, settingUpBids;
    
    







    // Start is called before the first frame update;
    void Start()
    {
        waypointIndex = -1;
        inJailLeft = 0;
        secondRoll = false;
        moving = false;
        transform.position = InitialWaypoint.transform.position;
        index = 0;
        previousWaypointindex = 0;
        previousPosition = 0;
        fieldOccupied = false;
        shift = Vector3.zero;
        move = 0;
        freeChance = false;
        freeChest = false;
        totalNumOfHouses = 0;
        totalNumOfHotels = 0;

        money = 1500;
        capital = money;
        capitalWithoutSelling = money;
        worth = money;
        buyingOpportunity = false;
        chanceChestOpportunity = false;
        chanceChestOK = false;

        buyingHouses = false;
        buyingHousesObjectActivated = false;
        unmortgaging = false;
        housesBought = false;
        deedsUnmortgaged = false;
        buildingConditionsChecked = false;
        unmortgageConditionsChecked = false;
        
        tempHouses = new List<bool>();
        tempMortgage = new List<bool>();
        tempSellHouses = new List<int>();
        tempUnmortgage = new List<bool>();
        for(int i = 0; i < 41; i++)
        {
            tempHouses.Add(false);
            tempMortgage.Add(false);
            tempSellHouses.Add(0);
            tempUnmortgage.Add(false);
        }
        whichHouse = -1;
        shortageON = false;
        povertyON = false;
   
        // Money transaction
        addingMoney = false;
        subingMoney = false;
        addStartMoney = false;

        // Auction
        stillInBidding = false;
        biddingWinner = false;
        settingUpBids = false;
    }

    // Update is called once per frame
    void Update()
    {
/********************************************************************************************************************************************************

                EVENTS BEYOND PLAYER'S TURN

*********************************************************************************************************************************************************/
        if(addingMoney)
        {
            GoAddMoney();
        }
        if(subingMoney)
        {
            GoSubMoney();
        }
        if(WindowsControl.auctionWindowOpen)
        {
            if(stillInBidding)
            {
                if(WindowsControl.bidbools[(myIndex-1), 5] || capital - GameControl.bid < 0) // Out of auction
                {
                    WindowsControl.bidbools[(myIndex-1), 5] = false;
                    stillInBidding = false;
                    GameControl.activeBidders--;
                    for(int i = 0; i < 6; i++)
                    {
                        //WindowsControl.bidButtons[(myIndex-1), i].enabled = false;
                        WindowsControl.bidButtons[(myIndex-1), i].interactable = false;
                    }
                }
                else
                {
                    for(int i = 0; i < 5; i++)
                    {
                        if(capital - GameControl.bid < WindowsControl.bides[i])
                        {
                            //WindowsControl.bidButtons[(myIndex-1), i].enabled = false;
                            WindowsControl.bidButtons[(myIndex-1), i].interactable = false;
                        }
                    }

                    for(int i = 0; i < 5; i++)
                    {
                        if(WindowsControl.bidbools[(myIndex-1), i])
                        {
                            WindowsControl.bidbools[(myIndex-1), i] = false;
                            for(int j = 1; j <= GameControl.activePlayers.Count; j++) // Make sure the previous highest bidder gets back his "Pass" button
                            {
                                if(GameControl.players[j].GetComponent<PlayerControl>().stillInBidding)
                                {
                                    //WindowsControl.bidButtons[(j-1), 5].enabled = true;
                                    WindowsControl.bidButtons[(j-1), 5].interactable = true;
                                }
                            }
                            //WindowsControl.bidButtons[(myIndex-1), 5].enabled = false; // Take away Player's "Pass" button
                            WindowsControl.bidButtons[(myIndex-1), 5].interactable = false;
                            GameControl.bid += WindowsControl.bides[i];
                            WindowsControl.highestBidder[2].GetComponent<TextMeshProUGUI>().text = GameControl.MoneyText(GameControl.bid);
                            WindowsControl.highestBidder[0].GetComponent<SpriteRenderer>().color = this.GetComponent<SpriteRenderer>().color;
                            WindowsControl.highestBidder[1].GetComponent<SpriteRenderer>().color = this.GetComponent<SpriteRenderer>().color;
                        }
                    }
                }
            }
            else if(biddingWinner)
            {
                for(int i = 0; i < 5; i++) // Even though Player won, bid has to be > 0 to buy
                {
                    if(WindowsControl.bidbools[(myIndex-1), i])
                    {
                        WindowsControl.bidbools[(myIndex-1), i] = false;
                        GameControl.bid += WindowsControl.bides[i];
                        WindowsControl.highestBidder[2].GetComponent<TextMeshProUGUI>().text = GameControl.MoneyText(GameControl.bid);
                        WindowsControl.highestBidder[0].GetComponent<SpriteRenderer>().color = this.GetComponent<SpriteRenderer>().color;
                        WindowsControl.highestBidder[1].GetComponent<SpriteRenderer>().color = this.GetComponent<SpriteRenderer>().color;
                    }
                }
                if(GameControl.bid > 0 || WindowsControl.bidbools[(myIndex-1), 5])
                {
                    if(WindowsControl.bidbools[(myIndex-1), 5]) // In a rare case everyone passes and no one wins the auction
                    {
                        WindowsControl.bidbools[(myIndex-1), 5] = false;
                        biddingWinner = false;
                        WindowsControl.CloseAuctionWindow();
                        Debug.Log("Player " + (myIndex) + ": nextTurn() - line: 199");
                        GameControl.nextTurn();
                    }
                    else
                    {
                        WindowsControl.biddersAndBidObjects[(myIndex-1), 6].SetActive(false);
                        WindowsControl.biddersAndBidObjects[(myIndex-1), 7].SetActive(true);
                        for(int i = 0; i < 5; i++)
                        {
                            //WindowsControl.bidButtons[(myIndex-1), i].enabled = false;
                            WindowsControl.bidButtons[(myIndex-1), i].interactable = false;
                        }

                        if(WindowsControl.bidbools[(myIndex-1), 6]) // Auction winner buys
                        {
                            biddingWinner = false;
                            WindowsControl.bidbools[(myIndex-1), 6] = false;
                            WindowsControl.biddersAndBidObjects[(myIndex-1), 6].SetActive(true);
                            WindowsControl.biddersAndBidObjects[(myIndex-1), 7].SetActive(false);
                            WindowsControl.CloseAuctionWindow();
                            // Buying
                            if (GameControl.bid > money)
                            {
                                GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().myTurn = false; // Useful in case it is not bidding winner's turn, and he run out of cash
                                shortageReason = "Auction";
                                Shortage(GameControl.bid);
                            }
                            else
                            {
                                GameControl.fieldmarkers[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().index + 1].SetActive(true);
                                GameControl.fieldmarkers[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().index + 1].GetComponent<SpriteRenderer>().color = GameControl.players[myIndex].GetComponent<SpriteRenderer>().color;
                                //Debug.Log("Player buys");
                                GameControl.titleDeedCardsOwners[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().index + 1] = myIndex;
                                capital -= GameControl.bid - GameControl.titleDeedCards[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().index + 1].buyPrice;
                                capitalWithoutSelling -= GameControl.bid - GameControl.titleDeedCards[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().index + 1].buyPrice / 2;
                                worth -= GameControl.bid - GameControl.titleDeedCards[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().index + 1].buyPrice;
                                money -= GameControl.bid;
                                SubMoney(GameControl.bid);
                                if(!(GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex == 5 || GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex == 15 || GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex == 25 || GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex == 35 || GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex == 12 || GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex == 28))
                                {
                                    GameControl.ColorUpdate("BuyNewDeed", GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex + 1, myIndex);
                                }
                                Debug.Log("Player " + (myIndex) + ": nextTurn() - line: 243");
                                GameControl.nextTurn();
                            }
                        }
                    }
                    
                }
            }
            else if(settingUpBids) // Determine how much Player can bid, enable respective buttons
            {
                WindowsControl.biddersAndBidObjects[(myIndex-1), 0].SetActive(true);
                for(int i = 0; i < 5; i++)
                {
                    if(capital - GameControl.bid >= WindowsControl.bides[i])
                    {
                        //WindowsControl.bidButtons[(myIndex-1), i].enabled = true;
                        WindowsControl.bidButtons[(myIndex-1), i].interactable = true;
                    }                    
                    //WindowsControl.bidButtons[(myIndex-1), 5].enabled = true;
                    WindowsControl.bidButtons[(myIndex-1), 5].interactable = true;
                }
                settingUpBids = false;
                stillInBidding = true;
            }
        }
        else if(povertyON)
        {
            // Selling deed (name "mortgage" (in its different forms) is borrowed for convenience's sake and will always mean "sell deed" here)
            if(WindowsControl.mortgaging)                                   // 1. A deed button clicked
            {
                for(int i = 2; i < 41; i++)                                 // 2. Find which one
                {
                    if(WindowsControl.mortgagebools[i])
                    {
                        whichDeed = i;
                        break;
                    }
                }
                WindowsControl.mortgagebools[whichDeed] = false;            // 3. Reset the button bool

                if(tempMortgage[whichDeed])
                {                          // if: Back sell a deed
                    WindowsControl.mortgageObjects[whichDeed].SetActive(true);  // 4. Reactivate mortgage graphic
                    GameControl.fieldmarkers[whichDeed].SetActive(true);        // 5. Reopen field marker
                    tempMortgage[whichDeed] = false;
                    tempSumSolgt -= GameControl.titleDeedCards[whichDeed].buyPrice / 2;
                    tempMortgaged--;
                }
                else                       // else: Sell a deed
                {
                    WindowsControl.mortgageObjects[whichDeed].SetActive(false);   // 4. Dectivate mortgage graphic
                    GameControl.fieldmarkers[whichDeed].SetActive(false);         // 5. Close field marker
                    tempMortgage[whichDeed] = true;
                    tempSumSolgt += GameControl.titleDeedCards[whichDeed].buyPrice / 2;
                    tempMortgaged++;
                }
                if(tempMortgaged > 0)
                {   // lepiej w sumie skopjować z buy houses i mieć konkretną listę
                    tempSellList = GameControl.MoneyText(tempSumSolgt) + " - " + tempMortgaged + " Title Deed Cards\n";
                }
                else
                {
                    tempSellList = "";
                }

                tempSum = capitalWithoutSelling + tempSumSolgt;
                WindowsControl.mortgaging = false;
                WindowsControl.sellList.GetComponent<TextMeshProUGUI>().text = tempSellList;
                WindowsControl.sum.GetComponent<TextMeshProUGUI>().text = GameControl.MoneyText(tempSum);
                tempRest = tempSum - tempToPay;

                if(tempRest < 0)
                {
                    WindowsControl.left.GetComponent<TextMeshProUGUI>().text = "-" + GameControl.MoneyText(-tempRest);
                    WindowsControl.confirmShortageButton.interactable = false;
                }
                else
                {
                    WindowsControl.confirmShortageButton.interactable = true;
                    WindowsControl.left.GetComponent<TextMeshProUGUI>().text = GameControl.MoneyText(tempRest);
                }
            }
            else if(WindowsControl.confirmShortage)
            {
                // Reset values
                tempSellList = "";
                WindowsControl.sellList.GetComponent<TextMeshProUGUI>().text = tempSellList;
                WindowsControl.sum.GetComponent<TextMeshProUGUI>().text = "";
                WindowsControl.confirmShortage = false;
                WindowsControl.CloseShortageWindow();

                // Payer money update
                if(money < tempRest)
                {
                    AddMoney(tempRest - money);
                }
                else if(money > tempRest)
                {
                    SubMoney(money - tempRest);
                }
                money = tempRest;
                capitalWithoutSelling = tempRest;
                if(shortageReason == "Auction") // Additional change in capitalWS and capital
                {
                    capitalWithoutSelling += GameControl.titleDeedCards[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex + 1].buyPrice/2;
                    capital = capital - tempToPay + GameControl.titleDeedCards[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex + 1].buyPrice;
                }
                else if(shortageReason == "BuyDeed") // Additional change in capitalWS
                {
                    capitalWithoutSelling += GameControl.titleDeedCards[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex + 1].buyPrice/2;
                }
                worth = capital;

                // Payee money update
                if(payee != 0)
                {
                    GameControl.players[payee].GetComponent<PlayerControl>().money += tempToPay;
                    GameControl.players[payee].GetComponent<PlayerControl>().capital += tempToPay;
                    GameControl.players[payee].GetComponent<PlayerControl>().capitalWithoutSelling += tempToPay;
                    GameControl.players[payee].GetComponent<PlayerControl>().worth += tempToPay;
                    GameControl.players[payee].GetComponent<PlayerControl>().AddMoney(tempToPay);
                }

                // Update houses and deeds
                for(int i = 0; i < 41; i++)
                {
                    // Close all of the buttons
                    WindowsControl.deedObjects[i].SetActive(false);
                    // Update solgt deeds
                    if(tempMortgage[i])
                    {
                        GameControl.titleDeedCardsOwners[i] = 0;
                        tempMortgage[i] = false;
                    }
                }

                if(shortageReason == "BuyDeed")
                {
                    GameControl.fieldmarkers[index + 1].SetActive(true);
                    GameControl.fieldmarkers[index + 1].GetComponent<SpriteRenderer>().color = GameControl.players[myIndex].GetComponent<SpriteRenderer>().color;
                    //Debug.Log("Player buys");
                    GameControl.titleDeedCardsOwners[index + 1] = GameControl.whoseTurn;
                    GameControl.ColorUpdate("BuyNewDeed", index + 1, myIndex);
                }
                if(shortageReason == "Auction")
                {
                    GameControl.fieldmarkers[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().index + 1].SetActive(true);
                    GameControl.fieldmarkers[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().index + 1].GetComponent<SpriteRenderer>().color = GameControl.players[myIndex].GetComponent<SpriteRenderer>().color;
                    //Debug.Log("Player buys");
                    GameControl.titleDeedCardsOwners[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex + 1] = myIndex;
                    GameControl.ColorUpdate("BuyNewDeed", GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex + 1, myIndex);
                    Debug.Log("Player " + (myIndex) + ": nextTurn() - line: 388");
                    GameControl.nextTurn();
                }
                else if(shortageReason != "bailOut") // In case Player goes out of jail, do not execute nextTurn()
                {
                    Debug.Log("Player " + (myIndex) + ": nextTurn() - line: 393");
                    GameControl.nextTurn();
                }
                shortageReason = "";
                povertyON = false;
            }
        }
        else if(shortageON)
        {
            // Selling houses
            if(WindowsControl.AHouse)                      // 1. A house button clicked
            {
                for(int i = 0; i < 41; i++)                    // 2. Find which one
                {
                    if(WindowsControl.housebools[i])
                    {
                        whichHouse = i;
                        break;
                    }
                }
                WindowsControl.housebools[whichHouse] = false; // 3. Reset the button bool

                if(tempSellHouses[whichHouse] == GameControl.titleDeedCards[whichHouse].numHouses)
                {                          // if: Back sell the houses
                    tempSellHouses[whichHouse] = 0; // how many of which houses has been temporarily solgt
                    tempSumSolgt -= GameControl.titleDeedCards[whichHouse].numHouses * GameControl.titleDeedCards[whichHouse].housePrice / 2;
                    tempSolgt -= GameControl.titleDeedCards[whichHouse].numHouses; // how many houses has been temporarily solgt
                    TempColorHousesUpdate(false); // Reset TempColorHouses[whichHouse] to GameControl.ColorHouses[whichHouse]
                }
                else                       // else: Sell a house
                {
                    tempSellHouses[whichHouse]++; // how many of which houses has been temporarily solgt
                    tempSumSolgt += GameControl.titleDeedCards[whichHouse].housePrice / 2;
                    tempSolgt++; // how many houses has been temporarily solgt
                    TempColorHousesUpdate(); // Substract one respective color house
                }
                if(tempSolgt > 0)
                {
                    tempSellHousesList = GameControl.MoneyText(tempSumSolgt) + " - " + tempSolgt + " Houses\n";
                }
                else
                {
                    tempSellHousesList = "";
                }

                // If all houses of a color are solgt, open the possibility of mortgaging deeds
            }
            // Mortgaging
            if(WindowsControl.mortgaging)                                   // 1. A deed button clicked
            {
                for(int i = 0; i < 41; i++)                                 // 2. Find which one
                {
                    if(WindowsControl.mortgagebools[i])
                    {
                        whichDeed = i;
                        break;
                    }
                }
                WindowsControl.mortgagebools[whichDeed] = false;            // 3. Reset the button bool

                if(tempMortgage[whichDeed])
                {                          // if: Back mortgage a deed
                    WindowsControl.mortgageObjects[whichDeed].SetActive(false);  // 4. Deactivate mortgage graphic
                    tempMortgage[whichDeed] = false;
                    tempSumMortgaged -= GameControl.titleDeedCards[whichDeed].buyPrice / 2;
                    tempMortgaged--;
                    TempSellHousesUpdate(false);
                }
                else                       // else: Mortgage a deed
                {
                    WindowsControl.mortgageObjects[whichDeed].SetActive(true);   // 4. Activate mortgage graphic
                    tempMortgage[whichDeed] = true;
                    tempSumMortgaged += GameControl.titleDeedCards[whichDeed].buyPrice / 2;
                    tempMortgaged++;
                    TempSellHousesUpdate();
                }
                if(tempMortgaged > 0)
                {
                    tempMortgageList = GameControl.MoneyText(tempSumMortgaged) + " - " + tempMortgaged + " Title Deed Cards\n";
                }
                else
                {
                    tempMortgageList = "";
                }
            }
            if(WindowsControl.AHouse || WindowsControl.mortgaging)
            {
                WindowsControl.AHouse = false;
                WindowsControl.mortgaging = false;

                tempSellList = tempMortgageList + tempSellHousesList;
                tempSum = money + tempSumSolgt + tempSumMortgaged;

                WindowsControl.sellList.GetComponent<TextMeshProUGUI>().text = tempSellList;
                WindowsControl.sum.GetComponent<TextMeshProUGUI>().text = GameControl.MoneyText(tempSum);
                tempRest = tempSum - tempToPay;

                if(tempRest < 0)
                {
                    WindowsControl.left.GetComponent<TextMeshProUGUI>().text = "-" + GameControl.MoneyText(-tempRest);
                    //WindowsControl.confirmShortageButton.enabled = false;
                    WindowsControl.confirmShortageButton.interactable = false;
                }
                else
                {
                    //WindowsControl.confirmShortageButton.enabled = true;
                    WindowsControl.confirmShortageButton.interactable = true;
                    WindowsControl.left.GetComponent<TextMeshProUGUI>().text = GameControl.MoneyText(tempRest);
                }
            }
            else if(WindowsControl.confirmShortage)
            {
                // Reset values
                tempSellList = "";
                WindowsControl.sellList.GetComponent<TextMeshProUGUI>().text = tempSellList;
                WindowsControl.sum.GetComponent<TextMeshProUGUI>().text = "";
                WindowsControl.confirmShortage = false;
                WindowsControl.CloseShortageWindow();

                // Payer money update
                if(money < tempRest)
                {
                    AddMoney(tempRest - money);
                }
                else if(money > tempRest)
                {
                    SubMoney(money - tempRest);
                }
                money = tempRest;
                if(shortageReason == "Auction") // Change in capitalWS, capital and worth (due to "Auction")
                {
                    capitalWithoutSelling = capitalWithoutSelling - tempToPay + GameControl.titleDeedCards[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex + 1].buyPrice/2;
                    capital = capital - tempToPay + GameControl.titleDeedCards[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex + 1].buyPrice;
                    worth = worth - tempToPay + GameControl.titleDeedCards[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex + 1].buyPrice;
                }
                else if(shortageReason == "BuyDeed") // Change in capitalWS (due to "BuyDeed")
                {
                    capitalWithoutSelling -= GameControl.titleDeedCards[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex + 1].buyPrice/2;
                }
                else // Change in capital in every other case
                {
                    capitalWithoutSelling -= tempToPay;
                    capital -= tempToPay;
                    worth -= tempToPay;
                }
                if(tempSumSolgt > 0) // Additional change in worth (due to selling houses)
                {
                    worth -= tempSumSolgt;
                }

                // Payee money update
                if(payee != 0)
                {
                    GameControl.players[payee].GetComponent<PlayerControl>().money += tempToPay;
                    GameControl.players[payee].GetComponent<PlayerControl>().capital += tempToPay;
                    GameControl.players[payee].GetComponent<PlayerControl>().capitalWithoutSelling += tempToPay;
                    GameControl.players[payee].GetComponent<PlayerControl>().worth += tempToPay;
                    GameControl.players[payee].GetComponent<PlayerControl>().AddMoney(tempToPay);
                }

                // Update houses and deeds
                for(int i = 0; i < 41; i++)
                {
                    // Close all of the buttons
                    WindowsControl.houseObjects[i].SetActive(false);
                    WindowsControl.deedObjects[i].SetActive(false);

                    // Update houses
                    if(tempSellHouses[i] > 0)
                    {
                        // Selling a hotel
                        if(GameControl.titleDeedCards[i].numHouses == 5)
                        {
                            WindowsControl.hotels[i].SetActive(false);
                            if(tempSellHouses[i] == 5)              // No houses left
                            {
                                GameControl.titleDeedCards[i].numHouses = 0;
                                totalNumOfHotels--;
                                continue;
                            }
                            else                                    // Some houses left
                            {
                                housesLeft = GameControl.titleDeedCards[i].numHouses - tempSellHouses[i];
                                GameControl.titleDeedCards[i].numHouses = housesLeft;
                                totalNumOfHouses += housesLeft;
                                
                                for(int j = 0; j < housesLeft; j++) // Reactivate leftover houses
                                {
                                    if(j == 0)
                                    {
                                        WindowsControl.houses1[i].SetActive(true);
                                    }
                                    else if(j == 1)
                                    {
                                        WindowsControl.houses2[i].SetActive(true);
                                    }
                                    else if(j == 2)
                                    {
                                        WindowsControl.houses3[i].SetActive(true);
                                    }
                                    else if(j == 3)
                                    {
                                        WindowsControl.houses4[i].SetActive(true);
                                    }
                                }
                            }
                        }
                        else    // Selling only houses (there was no hotel here)
                        {
                            housesLeft = GameControl.titleDeedCards[i].numHouses - tempSellHouses[i];
                            totalNumOfHouses -= tempSellHouses[i];

                            for(int j = housesLeft; j < GameControl.titleDeedCards[i].numHouses; j++)
                            {
                                if(j == 0)
                                {
                                    WindowsControl.houses1[i].SetActive(false);
                                }
                                else if(j == 1)
                                {
                                    WindowsControl.houses2[i].SetActive(false);
                                }
                                else if(j == 2)
                                {
                                    WindowsControl.houses3[i].SetActive(false);
                                }
                                else if(j == 3)
                                {
                                    WindowsControl.houses4[i].SetActive(false);
                                }
                            }
                            GameControl.titleDeedCards[i].numHouses = housesLeft;
                        }
                    }
                    if(tempMortgage[i]) // Update mortgages
                    {
                        GameControl.titleDeedCardsOwners[i] *= -1;
                    }
                }
                if(tempMortgaged > 0)
                {
                    for(int i = 0, m = 0; m < tempMortgaged; i++)
                    {
                        if(tempMortgage[i])
                        {
                            tempMortgage[i] = false;
                            m++;
                            GameControl.ColorUpdate("Mortgage", i);
                        }
                    }
                }
                if(tempSolgt > 0)
                {
                    GameControl.ColorHousesUpdate();
                }
                if(shortageReason == "BuyDeed")
                {
                    GameControl.fieldmarkers[index + 1].SetActive(true);
                    GameControl.fieldmarkers[index + 1].GetComponent<SpriteRenderer>().color = GameControl.players[myIndex].GetComponent<SpriteRenderer>().color;
                    //Debug.Log("Player buys");
                    GameControl.titleDeedCardsOwners[index + 1] = GameControl.whoseTurn;
                    GameControl.ColorUpdate("BuyNewDeed", index + 1, myIndex);
                }
                if(shortageReason == "Auction")
                {
                    GameControl.fieldmarkers[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().index + 1].SetActive(true);
                    GameControl.fieldmarkers[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().index + 1].GetComponent<SpriteRenderer>().color = GameControl.players[myIndex].GetComponent<SpriteRenderer>().color;
                    //Debug.Log("Player buys");
                    GameControl.titleDeedCardsOwners[GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex + 1] = myIndex;
                    GameControl.ColorUpdate("BuyNewDeed", GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().waypointIndex + 1, myIndex);
                    Debug.Log("Player " + (myIndex) + ": nextTurn() - line: 642");
                    GameControl.nextTurn();
                }
                else if(shortageReason != "bailOut") // In case Player goes out of jail, do not execute nextTurn()
                {
                    Debug.Log("Player " + (myIndex) + ": nextTurn() - line: 647");
                    GameControl.nextTurn();
                }
                shortageReason = "";
                shortageON = false;
            }
        }

/********************************************************************************************************************************************************

                EVENTS ONLY IN PLAYER'S TURN

*********************************************************************************************************************************************************/

        else if(myTurn)
        {
            if(WindowsControl.gameOver)
            {
                if(WindowsControl.areYouSureWindowOpen)
                {
                    if(WindowsControl.yes)
                    {
                        if(WindowsControl.gameOverWindowOpen)
                        {
                            if(WindowsControl.gameOverOK)
                            {
                                WindowsControl.gameOverOK = false;
                                WindowsControl.gameOverWindow.SetActive(false);
                                WindowsControl.gameOverWindowOpen = false;
                                myTurn = false;
                                WindowsControl.areYouSureWindowOpen = false;
                                WindowsControl.yes = false;
                                WindowsControl.gameOver = false;
                            }
                        }
                        else
                        {
                            WindowsControl.CloseActionsWindow();
                            WindowsControl.areYouSureWindow.SetActive(false);

                            var worthOrder = GameControl.SortActivePlayers();

                            WindowsControl.winnerMessage.GetComponent<TextMeshProUGUI>().text = "The winner is Player " + worthOrder[0] + ".";
                            WindowsControl.highscore.GetComponent<TextMeshProUGUI>().text = "";
                            for(int i = 0; i < worthOrder.Count; i++)
                            {
                                WindowsControl.highscore.GetComponent<TextMeshProUGUI>().text += (i + 1) + ". Player " + worthOrder[i] + " - " + GameControl.MoneyText(GameControl.players[worthOrder[i]].GetComponent<PlayerControl>().worth) + "\n";
                            }
                            WindowsControl.gameOverWindow.SetActive(true);
                            WindowsControl.gameOverWindowOpen = true;
                        }
                    }
                    else if(WindowsControl.no)
                    {
                        WindowsControl.no = false;
                        WindowsControl.gameOver = false;
                        WindowsControl.areYouSureWindow.SetActive(false);
                        WindowsControl.areYouSureWindowOpen = false;
                    }
                }
                else
                {
                    WindowsControl.areYouSureMessage.GetComponent<TextMeshProUGUI>().text = "Do you want to end the game?\nIf so, the wealthiest player wins.";
                    WindowsControl.areYouSureWindow.SetActive(true);
                    WindowsControl.areYouSureWindowOpen = true;
                }
            }
            else if(WindowsControl.gameOverWindowOpen)
            {
                if(WindowsControl.gameOverOK)
                {
                    WindowsControl.gameOverOK = false;
                    WindowsControl.gameOverWindow.SetActive(false);
                    WindowsControl.gameOverWindowOpen = false;
                    myTurn = false;
                }
            }
            else if(WindowsControl.bankruptcyWindowOpen)
            {
                if(WindowsControl.bankruptcyOK)
                {
                    WindowsControl.bankruptcyOK = false;
                    WindowsControl.bankruptcyWindow.SetActive(false);
                    WindowsControl.bankruptcyWindowOpen = false;
                    GameControl.nextTurn(myIndex);
                }
            }
            else if(buyingHouses)
            {
                if(WindowsControl.AHouse)                       // 1. A house button clicked
                {
                    WindowsControl.AHouse = false;
                    for(int i = 0; i < 41; i++)                    // 2. Find which one
                    {
                        if(WindowsControl.housebools[i])
                        {
                            whichHouse = i;
                            break;
                        }
                    }
                    WindowsControl.housebools[whichHouse] = false; // 3. Reset the button bool

                    if(tempHouses[whichHouse]) // if: Back buy the house
                    {
                        tempBuyList = "";
                        tempHouses[whichHouse] = false;
                        tempPrice -= GameControl.titleDeedCards[whichHouse].housePrice;
                        for(int i = 0; i < 41; i++)
                        {
                            if(tempHouses[i])
                            tempBuyList += GameControl.MoneyText(GameControl.titleDeedCards[i].housePrice) + " - " + GameControl.titleDeedCards[i].name + "\n";
                        }
                    }
                    else                       // else: Buy the house
                    {
                        tempHouses[whichHouse] = true;
                        tempPrice += GameControl.titleDeedCards[whichHouse].housePrice;
                        tempBuyList += GameControl.MoneyText(GameControl.titleDeedCards[whichHouse].housePrice) + " - " + GameControl.titleDeedCards[whichHouse].name + "\n";
                    }

                    WindowsControl.buyList.GetComponent<TextMeshProUGUI>().text = tempBuyList;
                    WindowsControl.price.GetComponent<TextMeshProUGUI>().text = GameControl.MoneyText(tempPrice);
                    tempRest = money - tempPrice;
                    if(tempRest < 0)
                    {
                        WindowsControl.rest.GetComponent<TextMeshProUGUI>().text = "-" + GameControl.MoneyText(-tempRest);
                        //WindowsControl.confirmBuyHousesButton.enabled = false;
                        WindowsControl.confirmBuyHousesButton.interactable = false;
                    }
                    else
                    {
                        //WindowsControl.confirmBuyHousesButton.enabled = true;
                        WindowsControl.confirmBuyHousesButton.interactable = true;
                        WindowsControl.rest.GetComponent<TextMeshProUGUI>().text = GameControl.MoneyText(tempRest);
                    }
                }
                else if(WindowsControl.confirmBuyHouses)
                {
                    if(tempPrice > 0) // Skip "after-buying-houses-procedure" if none houses bought
                    {
                        tempBuyList = "";
                        WindowsControl.buyList.GetComponent<TextMeshProUGUI>().text = tempBuyList;
                        WindowsControl.price.GetComponent<TextMeshProUGUI>().text = "";
                        money -= tempPrice;
                        capital -= tempPrice/2;
                        capitalWithoutSelling -= tempPrice/2;
                        SubMoney(tempPrice);
                        WindowsControl.unmortgageObject.SetActive(false); // Close unmortgage button after buying houses

                        for(int i = 0; i < 41; i++)
                        {
                            if(tempHouses[i])
                            {
                                GameControl.titleDeedCards[i].numHouses++;
                                if(GameControl.titleDeedCards[i].numHouses == 1)
                                {
                                    WindowsControl.houses1[i].SetActive(true);
                                    totalNumOfHouses++;
                                }
                                else if(GameControl.titleDeedCards[i].numHouses == 2)
                                {
                                    WindowsControl.houses2[i].SetActive(true);
                                    totalNumOfHouses++;
                                }
                                else if(GameControl.titleDeedCards[i].numHouses == 3)
                                {
                                    WindowsControl.houses3[i].SetActive(true);
                                    totalNumOfHouses++;
                                }
                                else if(GameControl.titleDeedCards[i].numHouses == 4)
                                {
                                    WindowsControl.houses4[i].SetActive(true);
                                    totalNumOfHouses++;
                                }
                                else if(GameControl.titleDeedCards[i].numHouses == 5)
                                {
                                    WindowsControl.houses1[i].SetActive(false);
                                    WindowsControl.houses2[i].SetActive(false);
                                    WindowsControl.houses3[i].SetActive(false);
                                    WindowsControl.houses4[i].SetActive(false);
                                    WindowsControl.hotels[i].SetActive(true);
                                    totalNumOfHotels++;
                                    totalNumOfHouses -= 4;
                                }
                            }
                        }
                        GameControl.ColorHousesUpdate();
                    }
                    for(int i = 0; i < 41; i++)
                    {
                        WindowsControl.houseObjects[i].SetActive(false);
                    }
                    WindowsControl.confirmBuyHouses = false;
                    WindowsControl.CloseBuyHousesWindow();
                    buyingHouses = false;
                    housesBought = true;
                    WindowsControl.buyHousesObject.SetActive(false);
                    if(!deedsUnmortgaged) // In case unmortgaging has not been performed yet in this turn...
                    {
                        unmortgageConditionsChecked = false; // ... let it check unmortgage conditions once more
                    }
                }
            }
            else if(unmortgaging)
            {
                if(WindowsControl.mortgaging)                                   // 1. A deed button clicked
                {
                    WindowsControl.mortgaging = false;
                    for(int i = 0; i < 41; i++)                                 // 2. Find which one
                    {
                        if(WindowsControl.mortgagebools[i])
                        {
                            whichDeed = i;
                            break;
                        }
                    }
                    WindowsControl.mortgagebools[whichDeed] = false;            // 3. Reset the button bool

                    if(tempUnmortgage[whichDeed]) // if: Back unmortgage a deed
                    {
                        tempUnmortgageList = "";
                        WindowsControl.mortgageObjects[whichDeed].SetActive(true);    // 4. Reactivate mortgage graphic
                        tempUnmortgage[whichDeed] = false;
                        tempPrice -= GameControl.titleDeedCards[whichDeed].buyPrice / 2;
                        for(int i = 0; i < 41; i++)
                        {
                            if(tempUnmortgage[i])
                            tempUnmortgageList += GameControl.MoneyText(GameControl.titleDeedCards[i].buyPrice / 2) + " - " + GameControl.titleDeedCards[i].name + "\n";
                        }
                    }
                    else                           // else: Unmortgage a deed
                    {
                        WindowsControl.mortgageObjects[whichDeed].SetActive(false);   // 4. Dectivate mortgage graphic
                        tempUnmortgage[whichDeed] = true;
                        tempPrice += GameControl.titleDeedCards[whichDeed].buyPrice / 2;
                        tempUnmortgageList += GameControl.MoneyText(GameControl.titleDeedCards[whichDeed].buyPrice / 2) + " - " + GameControl.titleDeedCards[whichDeed].name + "\n";
                    }

                    tempPrice10 = GameControl.Price10(tempPrice);
                    WindowsControl.unmortgageList.GetComponent<TextMeshProUGUI>().text = tempUnmortgageList;
                    WindowsControl.unmPrice.GetComponent<TextMeshProUGUI>().text = GameControl.MoneyText(tempPrice10);
                    tempRest = money - tempPrice10;
                    if(tempRest < 0)
                    {
                        WindowsControl.unmRest.GetComponent<TextMeshProUGUI>().text = "-" + GameControl.MoneyText(-tempRest);
                        //WindowsControl.confirmUnmortgageButton.enabled = false;
                        WindowsControl.confirmUnmortgageButton.interactable = false;
                    }
                    else
                    {
                        //WindowsControl.confirmUnmortgageButton.enabled = true;
                        WindowsControl.confirmUnmortgageButton.interactable = true;
                        WindowsControl.unmRest.GetComponent<TextMeshProUGUI>().text = GameControl.MoneyText(tempRest);
                    }
                }
                else if(WindowsControl.confirmUnmortgage)
                {
                    if(tempPrice > 0)
                    {
                        tempUnmortgageList = "";
                        WindowsControl.unmortgageList.GetComponent<TextMeshProUGUI>().text = tempUnmortgageList;
                        WindowsControl.unmPrice.GetComponent<TextMeshProUGUI>().text = "";
                        money -= tempPrice10;
                        capital = capital - tempPrice10 + tempPrice;
                        capitalWithoutSelling = capitalWithoutSelling - tempPrice10 + tempPrice;
                        worth = worth - tempPrice10 + tempPrice;
                        SubMoney(tempPrice10);
                        WindowsControl.buyHousesObject.SetActive(false); // Close buy houses button after unmortgage

                        for(int i = 0; i < 41; i++)
                        {
                            if(tempUnmortgage[i])                           // Update unmortgaged deeds
                            {
                                GameControl.titleDeedCardsOwners[i] *= -1;
                                tempUnmortgage[i] = false;
                                GameControl.ColorUpdate("BuyNewDeed", i, myIndex);
                            }
                        }
                    }
                    for(int i = 0; i < 41; i++)
                    {
                        WindowsControl.deedObjects[i].SetActive(false); // Close all of the buttons
                    }
                    WindowsControl.CloseUnmortgageWindow();
                    WindowsControl.confirmUnmortgage = false;
                    unmortgaging = false;
                    //unmortgageObjectActivated = true;
                    deedsUnmortgaged = true;
                    WindowsControl.unmortgageObject.SetActive(false);
                    
                    if(!housesBought) // In case upgrading has not been performed yet in this turn...
                    {
                        buildingConditionsChecked = false; // ... let it check building conditions once more
                    }
                }
            }
            else if(chanceChestOpportunity) // Players stays on Chance/Chest field. Show recpective button.
            {
                if(WindowsControl.chanceReading || Input.GetKeyDown(KeyCode.Space)) // Player clicked Chance Button. Hid Chance Button, open respective Chance Window.
                {
                    WindowsControl.chanceReading = false;
                    Debug.Log("chanceread");
                    GameControl.tempIndex = GameControl.chanceIndices.Dequeue();
                    WindowsControl.chanceChestObjects[0].SetActive(false);
                    WindowsControl.chanceToTake = false;
                    WindowsControl.chanceWindows[GameControl.tempIndex].SetActive(true);
                    WindowsControl.chanceWindowOpen = true;
                    WindowsControl.chanceChestObjects[2].SetActive(true);
                    chanceChestOpportunity = false;
                }
                else if(WindowsControl.chestReading || Input.GetKeyDown(KeyCode.M)) // Player clicked Chest Button. Hid Chest Button, open restective Chest Window.
                {
                    WindowsControl.chestReading = false;
                    Debug.Log("chestread");
                    GameControl.tempIndex = GameControl.chestIndices.Dequeue();
                    WindowsControl.chanceChestObjects[1].SetActive(false);
                    WindowsControl.chestToTake = false;
                    WindowsControl.chestWindows[GameControl.tempIndex].SetActive(true);
                    WindowsControl.chestWindowOpen = true;
                    if(GameControl.tempIndex != 5)
                    {
                        WindowsControl.chanceChestObjects[3].SetActive(true);
                    }
                    chanceChestOpportunity = false;
                }
                chanceChestOK = true; // Player has OK button to click.
            }
            else if(chanceChestOK) // After clicking OK Button. Close Chance/Chest Window, lunch respective Chance/Chest method.
            {
                if(WindowsControl.chanceOK || Input.GetKeyDown(KeyCode.Space))
                {
                    WindowsControl.chanceOK = false;
                    WindowsControl.chanceWindowOpen = false;
                    WindowsControl.chanceWindows[GameControl.tempIndex].SetActive(false);
                    WindowsControl.chanceChestObjects[2].SetActive(false);
                    chanceChestOK = false;
                    GameControl.ChanceFunction(GameControl.tempIndex);
                }
                else if(WindowsControl.chestOK || Input.GetKeyDown(KeyCode.M))
                {
                    WindowsControl.chestOK = false;
                    WindowsControl.chestWindowOpen = false;
                    WindowsControl.chestWindows[GameControl.tempIndex].SetActive(false);
                    WindowsControl.chanceChestObjects[3].SetActive(false);
                    chanceChestOK = false;
                    GameControl.ChestFunction(GameControl.tempIndex);
                }
                else if(WindowsControl.takeChance || Input.GetKeyDown(KeyCode.N))
                {
                    WindowsControl.chestWindowOpen = false;
                    WindowsControl.chestWindows[5].SetActive(false);
                    chanceChestOK = false;
                    GameControl.ChestFunction(5);
                }
            }

            else if(moving)//when chance/chest finished -> moving = true;
            {
                // Move player token
                Go();
            }
            else if(buyingOpportunity)
            {
                if(capital >= GameControl.titleDeedCards[waypointIndex + 1].buyPrice)
                {
                    WindowsControl.yesBuyButton.interactable = true;
                }
                else
                {
                    WindowsControl.yesBuyButton.interactable = false;
                }
                // Open Buy Window
                WindowsControl.OpenBuyWindow();

                // Yes, buy
                if(WindowsControl.yesBuy || Input.GetKeyDown(KeyCode.Space))// || !buyingHousesObjectActivated)
                {
                    WindowsControl.yesBuy = false;
                    WindowsControl.CloseBuyWindow();
                    buyingOpportunity = false;

                    if(GameControl.titleDeedCards[waypointIndex + 1].buyPrice > money)
                    {
                        shortageReason = "BuyDeed";
                        Shortage(GameControl.titleDeedCards[waypointIndex + 1].buyPrice);
                    }
                    else
                    {
                        GameControl.fieldmarkers[index + 1].SetActive(true);
                        GameControl.fieldmarkers[waypointIndex + 1].GetComponent<SpriteRenderer>().color = GameControl.players[GameControl.whoseTurn].GetComponent<SpriteRenderer>().color;
                        //Debug.Log("Player buys");
                        GameControl.titleDeedCardsOwners[waypointIndex + 1] = GameControl.whoseTurn;
                        money -= GameControl.titleDeedCards[waypointIndex + 1].buyPrice;
                        capitalWithoutSelling -= GameControl.titleDeedCards[waypointIndex + 1].buyPrice/2;
                        SubMoney(GameControl.titleDeedCards[waypointIndex + 1].buyPrice);
                        if(!(waypointIndex == 5 || waypointIndex == 15 || waypointIndex == 25 || waypointIndex == 35 || waypointIndex == 12 || waypointIndex == 28))
                        {
                            GameControl.ColorUpdate("BuyNewDeed", waypointIndex + 1, myIndex);
                        }
                        Debug.Log("Player " + (myIndex) + ": nextTurn() - line: 959");
                        GameControl.nextTurn();
                    }
                }
                // No, do not buy - run Auction
                else if(WindowsControl.noBuy || Input.GetKeyDown(KeyCode.N))
                {
                    WindowsControl.noBuy = false;
                    // Close Buy Window
                    WindowsControl.CloseBuyWindow();

                    Debug.Log("Auction time!");
                    buyingOpportunity = false;
                    WindowsControl.OpenAuctionWindow();
                }
            }
/********************************************************************************************************************************************************

                PREPARING ACTIONS WINDOW (and then opening it)

*********************************************************************************************************************************************************/

            // Prevent next action until money moved chance/chest finished AND buying houses finished AND unmortgaging finished
            else if(!GameControl.transferingActivated && GameControl.chancefinished && GameControl.chestfinished && !buyingHouses && !unmortgaging)
            {
                // If in Jail
                if(inJailLeft > 0)
                {   
                    if(capital >= 50)
                    {
                        WindowsControl.bailObject.SetActive(true);
                    }
                    if(freeChance || freeChest)
                    {
                        if(capital < 50) // In case Player can't afford bailing out
                        {
                            WindowsControl.breakFreeObject.GetComponent<Transform>().position = WindowsControl.buyHousesObject.GetComponent<Transform>().position;
                        }
                        else
                        {
                            WindowsControl.breakFreeObject.GetComponent<Transform>().position = WindowsControl.buyHousesObject.GetComponent<Transform>().position + 2.82f * Vector3.down;
                        }
                        WindowsControl.breakFreeObject.SetActive(true);
                    }
                }
                else // Determine upgrading/unmortgaging possibility only if Player is not in Jail
                {
                    if(!buildingConditionsChecked) // Don't check building conditions multiple times, unless unmortgaging was done earlier (it is possible Player doesn't have money now)
                    {
                        if(!deedsUnmortgaged) // Prevent from buying houses on newly unmortgaged color
                        {
                            GameControl.TempColorOwners();
                        }
                        // Building conditions:  1. Players has a color 2. Color is not fully upgraded 3. Player has enough cash to build
                        if(((((((GameControl.tempColorOwners[0] == GameControl.whoseTurn) // Browns
                            && (GameControl.colorHouses[0] < 10)) // Not fully upgraded
                            || ((GameControl.tempColorOwners[1] == GameControl.whoseTurn) // Lightblues
                            && (GameControl.colorHouses[1] < 15)))
                            && (money >= 50)) // Enough money for upgrading South

                            || ((((GameControl.tempColorOwners[2] == GameControl.whoseTurn) // Roses
                            && (GameControl.colorHouses[2] < 15))
                            || ((GameControl.tempColorOwners[3] == GameControl.whoseTurn) // Oranges
                            && (GameControl.colorHouses[3] < 15)))
                            && (money >= 100)) // Enough money for upgrading West

                            || ((((GameControl.tempColorOwners[4] == GameControl.whoseTurn) // Reds
                            && (GameControl.colorHouses[4] < 15))
                            || ((GameControl.tempColorOwners[5] == GameControl.whoseTurn) // Yellows
                            && (GameControl.colorHouses[5] < 15)))
                            && (money >= 150)) // Enough money for upgrading North

                            || ((((GameControl.tempColorOwners[6] == GameControl.whoseTurn) // Greens
                            && (GameControl.colorHouses[6] < 15))
                            || ((GameControl.tempColorOwners[7] == GameControl.whoseTurn) // Blues
                            && (GameControl.colorHouses[7] < 10)))
                            && (money >= 200))))) // Enough money for upgrading East
                        {
                            // Buying houses/hotels possible
                            WindowsControl.buyHousesObject.SetActive(true);
                            buyingHousesObjectActivated = true; // Used to determine position for unmortgage button
                        }
                        buildingConditionsChecked = true;
                    }
                    if(!unmortgageConditionsChecked) // Check unmortgaging conditions for the second time only in case buyinging houses was done earlier (it is possible Player doesn't have money now)
                    {
                        //Debug.Log("Unmortgage conditions check - line 1163");
                        for(int i = 0; i < 41; i++)
                        {
                            if(GameControl.titleDeedCardsOwners[i] == -(myIndex) && money >= GameControl.titleDeedCards[i].buyPrice / 2 * 1.1)
                            {
                                // Unmortgaging possible
                                if(buyingHousesObjectActivated && !housesBought) // if buy houses button is there, move unmortgage button down
                                {
                                    WindowsControl.unmortgageObject.GetComponent<Transform>().position = WindowsControl.buyHousesObject.GetComponent<Transform>().position + 2.82f * Vector3.down;
                                }
                                else
                                {
                                    WindowsControl.unmortgageObject.GetComponent<Transform>().position = WindowsControl.buyHousesObject.GetComponent<Transform>().position;
                                }
                                WindowsControl.unmortgageObject.SetActive(true);
                            }
                        }
                        unmortgageConditionsChecked = true;
                    }
                }

                if(GameControl.freeFinished) // Do not open Action Window while choosing which Free card to use
                {
                    // Open Actions Window
                    WindowsControl.OpenActionsWindow();
                }

/********************************************************************************************************************************************************

                CHOOSING ACTION

*********************************************************************************************************************************************************/

                // Actions

                    // Throwing Dices
                if(WindowsControl.throwDices || Input.GetKeyDown(KeyCode.Space))// || !buyingHousesObjectActivated)
                {
                    WindowsControl.throwDices = false;
                    // Close Actions Window
                    WindowsControl.CloseActionsWindow();

                    if(secondRoll)
                    {
                        SecondRoll();
                    }
                    else if(inJailLeft > 0)
                    {
                        JailRoll();
                    }
                    else
                    {
                        FirstRoll();
                    }
                }

                    // Bailing out
                else if(WindowsControl.bailMeOut || Input.GetKeyDown(KeyCode.M))
                {
                    WindowsControl.bailMeOut = false;
                    WindowsControl.CloseActionsWindow();
                    WindowsControl.bailObject.SetActive(false);
                    inJailLeft = 0;
                    if(50 > money)
                    {
                        shortageReason = "bailOut";
                        Shortage(50);
                    }
                    else
                    {
                        capital -= 50;
                        capitalWithoutSelling -= 50;
                        worth -= 50;
                        money -= 50;
                        SubMoney(50);
                    }
                }

                    // Breaking free
                else if(WindowsControl.breakFree || Input.GetKeyDown(KeyCode.N))
                {
                    WindowsControl.CloseActionsWindow();
                    if(freeChance)
                    {
                        if(freeChest) // In case player has both cards let him choose which one to use
                        {
                            GameControl.freeFinished = false;
                            WindowsControl.OpenFreeCardsWindow();
                            if(WindowsControl.freeChanceB)
                            {
                                WindowsControl.freeChanceB = false;
                                WindowsControl.breakFree = false;
                                freeChance = false;
                                GameControl.chanceIndices.Enqueue(11);
                                GameControl.freeFinished = true;
                                WindowsControl.bailObject.SetActive(false);
                                WindowsControl.breakFreeObject.SetActive(false);
                                inJailLeft = 0;
                                WindowsControl.CloseFreeCardsWindow();
                            }
                            else if(WindowsControl.freeChestB)
                            {
                                WindowsControl.freeChestB = false;
                                WindowsControl.breakFree = false;
                                freeChest = false;
                                GameControl.chestIndices.Enqueue(2);
                                GameControl.freeFinished = true;
                                WindowsControl.bailObject.SetActive(false);
                                WindowsControl.breakFreeObject.SetActive(false);
                                inJailLeft = 0;
                                WindowsControl.CloseFreeCardsWindow();
                            }
                        }
                        else
                        {
                            freeChance = false;
                            WindowsControl.breakFree = false;
                            GameControl.chanceIndices.Enqueue(11);
                            WindowsControl.bailObject.SetActive(false);
                            WindowsControl.breakFreeObject.SetActive(false);
                            inJailLeft = 0;
                            WindowsControl.CloseActionsWindow();
                        }
                    }
                    else if(freeChest) // Dobblecheck if player has FreeChance card (but he has)
                    {
                        freeChest = false;
                        WindowsControl.breakFree = false;
                        GameControl.chestIndices.Enqueue(2);
                        WindowsControl.bailObject.SetActive(false);
                        WindowsControl.breakFreeObject.SetActive(false);
                        inJailLeft = 0;
                        WindowsControl.CloseActionsWindow();
                    }
                }

                    // Buying houses
                else if(WindowsControl.buyHouses)
                {
                    tempBuyList = "";
                    tempPrice = 0;
                    for(int i = 0; i < 41; i++)
                    {
                        tempHouses[i] = false;
                    }
                    WindowsControl.buyHouses = false;
                    WindowsControl.CloseActionsWindow();
                    WindowsControl.OpenBuyHousesWindow();
                    if(deedsUnmortgaged)
                    {
                        WindowsControl.OpenHouseButtons(true); // Buying after unmortgaging - prevent newly unmortgaged color from upgrading
                    }
                    else
                    {
                        WindowsControl.OpenHouseButtons(); // Buying without previous unmortgage
                    }
                    WindowsControl.cash.GetComponent<TextMeshProUGUI>().text = GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().moneyText;
                    WindowsControl.price.GetComponent<TextMeshProUGUI>().text = "0";
                    WindowsControl.rest.GetComponent<TextMeshProUGUI>().text = GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().moneyText;
                    buyingHouses = true;
                }
                    // Unmortgaging
                else if(WindowsControl.unmortgage)
                {
                    GameControl.TempColorOwners(); // Used to prevent from buying houses on newly unmortgaged color
                    tempUnmortgageList = "";
                    tempPrice = 0;
                    for(int i = 0; i < 41; i++)
                    {
                        if(GameControl.titleDeedCardsOwners[i] == -(myIndex))
                        {
                            WindowsControl.deedObjects[i].SetActive(true);
                        }
                    }
                    WindowsControl.unmortgage = false;
                    WindowsControl.CloseActionsWindow();
                    WindowsControl.OpenUnmortgageWindow();

                    WindowsControl.unmCash.GetComponent<TextMeshProUGUI>().text = GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().moneyText;
                    WindowsControl.unmPrice.GetComponent<TextMeshProUGUI>().text = "0";
                    WindowsControl.unmRest.GetComponent<TextMeshProUGUI>().text = GameControl.players[GameControl.whoseTurn].GetComponent<PlayerControl>().moneyText;
                    unmortgaging = true;
                }
            }
        }        
    }


    // Throw Dices functions
    private void JailRoll()
    {
        WindowsControl.bailObject.SetActive(false);      // In case Player is in Jail and decides to throw dices.
        WindowsControl.breakFreeObject.SetActive(false); // In case Player is in Jail and decides to throw dices.
        inJailLeft--;
        Dice1.DiceRoll();
        Debug.Log(Dice1.dice1 + " " + Dice2.dice2);
        if(Dice1.dice1 == Dice2.dice2)
        {
            inJailLeft = 0;
            Debug.Log("Go out of jail.");
        }
        else
        {
            Debug.Log("Still in jail.");
            Debug.Log("Player " + (myIndex) + ": nextTurn() - line: 1237");
            GameControl.nextTurn();
        }
    }

    private void SecondRoll()
    {
        // Reset buying houses AND unmortgaging bools
        buyingHousesObjectActivated = false;
        buildingConditionsChecked = false;
        unmortgageConditionsChecked = false;
        housesBought = false;
        deedsUnmortgaged = false;

        secondRoll = false;
        Dice1.DiceRoll();
        Debug.Log(Dice1.dice1 + " " + Dice2.dice2);
        if(Dice1.dice1 == Dice2.dice2)
        {
            previousWaypointindex = index;
            moving = true;
            index = 40;
            Debug.Log("Second doublet. Go to jail.");
            waypointIndex = 10;
            inJailLeft = 3;
            fieldOccupied = false;
            return;
        }
        move += Dice1.dice1 + Dice2.dice2;
        MakeMove();
    }

    private void FirstRoll()
    {
        // Turn off [unused] buy houses and unmortgage buttons
        WindowsControl.buyHousesObject.SetActive(false);
        WindowsControl.unmortgageObject.SetActive(false);
        
        Dice1.DiceRoll();
        move = Dice1.dice1 + Dice2.dice2;
        Debug.Log(Dice1.dice1 + " " + Dice2.dice2);
        if(Dice1.dice1 == Dice2.dice2)
        {
            secondRoll = true;
        }
        else
        {
            // Reset buying houses AND unmortgaging bools
            buyingHousesObjectActivated = false;
            buildingConditionsChecked = false;
            unmortgageConditionsChecked = false;
            housesBought = false;
            deedsUnmortgaged = false;

            MakeMove();
        }
    }
    // End of Throw Dices functions

    // Calculate where to move, enable moving and go
    public void MakeMove()
    {
        // Go passed
        if(waypointIndex + move > 39)
        {
            addStartMoney = true;
        }

        previousWaypointindex = index;
        waypointIndex = (waypointIndex + move) % 40;
        index = waypointIndex;
        moving = true;
        fieldOccupied = false;
    }

    public void Go()
    {
        if(!fieldOccupied)
        {
            Debug.Log("Previously occupied: " + previousWaypointindex + "." + previousPosition);
            GameControl.occupiedFields[previousWaypointindex, previousPosition] = 0;
            for(int i = 0; i < 6; i++) // Occupy field
            {
                if(GameControl.occupiedFields[index, i] == 0)
                {
                    GameControl.occupiedFields[index, i] = 1; // First vacant position
                    previousPosition = i;
                    fieldOccupied = true;
                    break;
                }
            }

            if(previousPosition == 0) // Move target position to first vacant position in new field
            {
                shift = Vector3.zero;
            }
            else if(index == 0)
            {
                switch(previousPosition) // Here: new position!!
                {
                    case 1:
                        shift = Vector3.up;
                        break;
                    case 2:
                        shift = 2f * Vector3.up;
                        break;
                    case 3:
                        shift = Vector3.left;
                        break;
                    case 4:
                        shift = Vector3.left + Vector3.up;
                        break;
                    case 5:
                        shift = Vector3.left + 2f * Vector3.up;
                        break;
                }
            }
            else if(index > 0 && index < 10)
            {
                switch(previousPosition)
                {
                    case 1:
                        shift = Vector3.down;
                        break;
                    case 2:
                        shift = Vector3.up;
                        break;
                    case 3:
                        shift = 0.5f * Vector3.up + 0.87f * Vector3.right;
                        break;
                    case 4:
                        shift = 0.5f * Vector3.down + 0.87f * Vector3.right;
                        break;
                    case 5:
                        shift = 0.5f * Vector3.down + 0.87f * Vector3.left;
                        break;
                }
            }
            else if(index == 10)
            {
                switch(previousPosition)
                {
                    case 1:
                        shift = Vector3.right;
                        break;
                    case 2:
                        shift = 2f * Vector3.right;
                        break;
                    case 3:
                        shift = Vector3.left + 2f * Vector3.up;
                        break;
                    case 4:
                        shift = Vector3.left + Vector3.up;
                        break;
                    case 5:
                        shift = Vector3.left;
                        break;
                }
            }
            else if(index > 10 && index < 20)
            {
                switch(previousPosition)
                {
                    case 1:
                        shift = Vector3.left;
                        break;
                    case 2:
                        shift = Vector3.right;
                        break;
                    case 3:
                        shift = 0.87f * Vector3.down + 0.5f * Vector3.right;
                        break;
                    case 4:
                        shift = 0.87f * Vector3.down + 0.5f * Vector3.left;
                        break;
                    case 5:
                        shift = 0.87f * Vector3.up + 0.5f * Vector3.left;
                        break;
                }
            }
            else if(index == 20)
            {
                switch(previousPosition)
                {
                    case 1:
                        shift = Vector3.down;
                        break;
                    case 2:
                        shift = 2f * Vector3.down;
                        break;
                    case 3:
                        shift = Vector3.right;
                        break;
                    case 4:
                        shift = Vector3.right + Vector3.down;
                        break;
                    case 5:
                        shift = Vector3.right + 2f * Vector3.down;
                        break;
                }
            }
            else if(index > 20 && index < 30)
            {
                switch(previousPosition)
                {
                    case 1:
                        shift = Vector3.up;
                        break;
                    case 2:
                        shift = Vector3.down;
                        break;
                    case 3:
                        shift = 0.5f * Vector3.down + 0.87f * Vector3.left;
                        break;
                    case 4:
                        shift = 0.5f * Vector3.up + 0.87f * Vector3.left;
                        break;
                    case 5:
                        shift = 0.5f * Vector3.up + 0.87f * Vector3.right;
                        break;
                }
            }
            else if(index > 30 && index < 40)
            {
                switch(previousPosition)
                {
                    case 1:
                        shift = Vector3.right;
                        break;
                    case 2:
                        shift = Vector3.left;
                        break;
                    case 3:
                        shift = 0.87f * Vector3.up + 0.5f * Vector3.left;
                        break;
                    case 4:
                        shift = 0.87f * Vector3.up + 0.5f * Vector3.right;
                        break;
                    case 5:
                        shift = 0.87f * Vector3.down + 0.5f * Vector3.right;
                        break;
                }
            }
            else
            {
                switch(previousPosition)
                {
                    case 1:
                        shift = 0.5f * Vector3.down + 0.87f * Vector3.left;
                        break;
                    case 2:
                        shift = 0.5f * Vector3.down + 0.87f * Vector3.right;
                        break;
                    case 3:
                        shift = 0.5f * Vector3.up + 0.87f * Vector3.left;
                        break;
                    case 4:
                        shift = 0.5f * Vector3.up + 0.87f * Vector3.right;
                        break;
                    case 5:
                        shift = Vector3.down;
                        break;
                }
            }
        }
        

        target = waypoints[index].transform.position + shift;
        transform.position = Vector2.MoveTowards(transform.position, target, GameControl.step);
        if(transform.position == target)
        {
            if(addStartMoney)
            {
                money += 200;
                capital += 200;
                capitalWithoutSelling += 200;
                worth += 200;
                addStartMoney = false;
                AddMoney(200);
            }

            // 30. Go to jail!
            if(waypointIndex == 30)
            {
                index = 40;
                Debug.Log("31 Go to jail.");
                waypointIndex = 10;
                inJailLeft = 3;
                fieldOccupied = false;
            }

            // Steping on property
            // Buy property
            else if(GameControl.titleDeedCardsOwners[waypointIndex + 1] == 0)
            {
                Debug.Log("Buying opportunity: " + (waypointIndex + 1));
                buyingOpportunity = true;
                moving = false;
            }
            else
            {
                // Already yours
                if(GameControl.titleDeedCardsOwners[waypointIndex + 1] == GameControl.whoseTurn)
                {
                    ;
                }
                // Pay rent, but not if owner is in Jail
                else if(GameControl.titleDeedCardsOwners[waypointIndex + 1] > 0 && GameControl.players[GameControl.titleDeedCardsOwners[waypointIndex + 1]].GetComponent<PlayerControl>().index != 40)
                {
                    rent = GameControl.titleDeedCards[waypointIndex + 1].Rent(move);
                    if(rent > capital)
                    {
                        Bankruptcy(rent, GameControl.titleDeedCardsOwners[waypointIndex + 1]);
                        WindowsControl.bankruptcyWindowOpen = true;
                    }
                    else if(rent > money)
                    {
                        shortageReason = "Rent";
                        Shortage(rent, GameControl.titleDeedCardsOwners[waypointIndex + 1]);
                    }
                    else
                    {
                        // Payer
                        capital -= rent;
                        capitalWithoutSelling -= rent;
                        worth -= rent;
                        money -= rent;
                        SubMoney(rent);
                        // Payee
                        GameControl.players[GameControl.titleDeedCardsOwners[waypointIndex + 1]].GetComponent<PlayerControl>().capital += rent;
                        GameControl.players[GameControl.titleDeedCardsOwners[waypointIndex + 1]].GetComponent<PlayerControl>().capitalWithoutSelling += rent;
                        GameControl.players[GameControl.titleDeedCardsOwners[waypointIndex + 1]].GetComponent<PlayerControl>().worth += rent;
                        GameControl.players[GameControl.titleDeedCardsOwners[waypointIndex + 1]].GetComponent<PlayerControl>().money += rent;
                        GameControl.players[GameControl.titleDeedCardsOwners[waypointIndex + 1]].GetComponent<PlayerControl>().AddMoney(rent);
                    }
                }
                // End of steping on property

                // 5. Pay 200
                else if(waypointIndex == 4)
                {
                    if(200 > capital)
                    {
                        Bankruptcy(200);
                        WindowsControl.bankruptcyWindowOpen = true;
                    }
                    else if(200 > money)
                    {
                        shortageReason = "200";
                        Shortage(200);
                    }
                    else
                    {
                        capital -= 200;
                        capitalWithoutSelling -= 200;
                        worth -= 200;
                        money -= 200;
                        SubMoney(200);
                    }
                }
                // 39. Pay 100
                else if(waypointIndex == 38)
                {
                    if(100 > capital)
                    {
                        Bankruptcy(100);
                        WindowsControl.bankruptcyWindowOpen = true;
                    }
                    else if(100 > money)
                    {
                        shortageReason = "100";
                        Shortage(100);
                    }
                    else
                    {
                        capital -= 100;
                        capitalWithoutSelling -= 100;
                        worth -= 100;
                        money -= 100;
                        SubMoney(100);
                    }
                }
                moving = false;

                // Chance
                if(waypointIndex == 7 || waypointIndex == 22 || waypointIndex == 36)
                {
                    WindowsControl.chanceChestObjects[0].SetActive(true);
                    chanceChestOpportunity = true;
                    GameControl.chancefinished = false;
                    WindowsControl.chanceToTake = true;
                }
                // Chest
                else if(waypointIndex == 2 || waypointIndex == 17 || waypointIndex == 33)
                {                    
                    WindowsControl.chanceChestObjects[1].SetActive(true);
                    chanceChestOpportunity = true;
                    GameControl.chestfinished = false;
                    WindowsControl.chestToTake = true;
                }
                if(GameControl.chancefinished && GameControl.chestfinished && !shortageON && !povertyON && !WindowsControl.bankruptcyWindowOpen)
                {
                    Debug.Log("Player " + (myIndex) + ": nextTurn() - line: 1619");
                    GameControl.nextTurn();
                }
            }
        }
    }
    // End of moving functions

    // Money transaction functions
    public void playerUpdate()
    {
        if(money > 0)
        {
            moneyText = GameControl.MoneyText(money);
        }
        else
        {
            moneyText = GameControl.MoneyText(-money);
        }
        NameAndMoney[4].GetComponent<TextMeshProUGUI>().text = moneyText;
    }
    public void AddMoney(int num)
    {
        GameControl.transferingActivated = true;
        moneyToAdd = GameControl.MoneyText(num);
        addMoneyText.SetActive(true);
        addMoneyText.GetComponent<TextMeshProUGUI>().text = "+" + moneyToAdd;
        addingMoneyTarget = addMoneyText.GetComponent<Transform>().position + Vector3.up * 0.5f;
        addingMoney = true;
    }
    void GoAddMoney()
    {
        addMoneyText.GetComponent<Transform>().position = Vector2.MoveTowards(addMoneyText.GetComponent<Transform>().position, addingMoneyTarget, GameControl.step/50);
        if(addMoneyText.GetComponent<Transform>().position == addingMoneyTarget)
        {
            addingMoney = false;
            addMoneyText.SetActive(false);
            addMoneyText.GetComponent<Transform>().position -= Vector3.up * 0.5f;
        }
        playerUpdate();
    }
    public void SubMoney(int num)
    {
        GameControl.transferingActivated = true;
        moneyToSub = GameControl.MoneyText(num);
        subMoneyText.SetActive(true);
        subMoneyText.GetComponent<TextMeshProUGUI>().text = "-" + moneyToSub;
        subingMoneyTarget = subMoneyText.GetComponent<Transform>().position + Vector3.down * 0.5f;
        subingMoney = true;
    }
    void GoSubMoney()
    {
        subMoneyText.GetComponent<Transform>().position = Vector2.MoveTowards(subMoneyText.GetComponent<Transform>().position, subingMoneyTarget, GameControl.step/50);
        if(subMoneyText.GetComponent<Transform>().position == subingMoneyTarget)
        {
            subingMoney = false;
            subMoneyText.SetActive(false);
            subMoneyText.GetComponent<Transform>().position -= Vector3.down * 0.5f;
        }
        playerUpdate();
    }
    // End of money transactions functions

    // Player turn marker
    public void TurnMarkerOn()
    {
        TurnMarker.SetActive(true);
    }
    public void TurnMarkerOff()
    {
        TurnMarker.SetActive(false);
    }

    public void Shortage(int toPay, int payee=0)
    {
        if(toPay > capitalWithoutSelling)
        {
            tempSellList = "";
            tempSum = capitalWithoutSelling;
            tempSumSolgt = 0;
            tempMortgaged = 0; // Here: "mortgaged" means "solgt"
            tempToPay = toPay;
            this.payee = payee;
            for(int i = 0; i < 8; i++)
            {
                if(GameControl.colorOwners[i] == myIndex) // Reset colors Player owns
                {
                    GameControl.colorOwners[i] = 0;
                    Debug.Log("Color reset: " + GameControl.colorOwners[0]+","+GameControl.colorOwners[1]+","+GameControl.colorOwners[2]+","+GameControl.colorOwners[3]+","+GameControl.colorOwners[4]+","+GameControl.colorOwners[5]+","+GameControl.colorOwners[6]+","+GameControl.colorOwners[7]);
                }
            }
            for(int i = 2; i < 41; i++)
            {
                if(GameControl.titleDeedCardsOwners[i] == myIndex)
                {
                    GameControl.titleDeedCardsOwners[i] *= -1;         // Logical mortgage
                    WindowsControl.mortgageObjects[i].SetActive(true); // Graphical mortgage
                    if(GameControl.titleDeedCards[i].numHouses > 0)    // Clear all houses Player owns
                    {
                        GameControl.titleDeedCards[i].numHouses = 0;
                        WindowsControl.houses1[i].SetActive(false);
                        WindowsControl.houses2[i].SetActive(false);
                        WindowsControl.houses3[i].SetActive(false);
                        WindowsControl.houses4[i].SetActive(false);
                        WindowsControl.hotels[i].SetActive(false);
                    }
                }
            }
            totalNumOfHouses = 0;
            totalNumOfHotels = 0;
            
            GameControl.ColorHousesUpdate();
            WindowsControl.OpenShortageWindow(true);
            WindowsControl.OpenSellDeedButtons(myIndex);

            WindowsControl.sum.GetComponent<TextMeshProUGUI>().text = GameControl.MoneyText(tempSum);
            WindowsControl.toPay.GetComponent<TextMeshProUGUI>().text = GameControl.MoneyText(tempToPay);
            WindowsControl.left.GetComponent<TextMeshProUGUI>().text = "-" + GameControl.MoneyText(tempToPay - tempSum);
            
            //WindowsControl.confirmShortageButton.enabled = false;
            WindowsControl.confirmShortageButton.interactable = false;
            povertyON = true;
        }
        else
        {
            tempMortgageList = "";
            tempSellHousesList = "";
            tempSellList = "";
            tempSum = money;
            tempSumMortgaged = 0;
            tempSumSolgt = 0;
            tempSolgt = 0;
            tempMortgaged = 0;
            tempToPay = toPay;
            this.payee = payee;
            for(int i = 0; i < 41; i++)
            {
                tempSellHouses[i] = 0;
            }
            tempColorHouses = GameControl.colorHouses;
            WindowsControl.OpenShortageWindow(false, player: myIndex);

            WindowsControl.sum.GetComponent<TextMeshProUGUI>().text = GameControl.MoneyText(money);
            WindowsControl.toPay.GetComponent<TextMeshProUGUI>().text = GameControl.MoneyText(tempToPay);
            WindowsControl.left.GetComponent<TextMeshProUGUI>().text = "-" + GameControl.MoneyText(tempToPay - money);
            
            //WindowsControl.confirmShortageButton.enabled = false;
            WindowsControl.confirmShortageButton.interactable = false;
            shortageON = true;
        }
    }
    
    void TempColorHousesUpdate(bool selling=true)
    {
        if(selling) // Temporarily selling a house, if tempColorHouses[] == 0 open mortgaging
        {
            if(whichHouse == 2 || whichHouse == 4)
            {
                tempColorHouses[0]--;
                if(tempColorHouses[0] == 0)
                {
                    WindowsControl.deedObjects[2].SetActive(true);
                    WindowsControl.deedObjects[4].SetActive(true);
                }
            }
            else if(whichHouse == 7 || whichHouse == 9 || whichHouse == 10)
            {
                tempColorHouses[1]--;
                if(tempColorHouses[1] == 0)
                {
                    WindowsControl.deedObjects[7].SetActive(true);
                    WindowsControl.deedObjects[9].SetActive(true);
                    WindowsControl.deedObjects[10].SetActive(true);
                }
            }
            else if(whichHouse == 12 || whichHouse == 14 || whichHouse == 15)
            {
                tempColorHouses[2]--;
                if(tempColorHouses[2] == 0)
                {
                    WindowsControl.deedObjects[12].SetActive(true);
                    WindowsControl.deedObjects[14].SetActive(true);
                    WindowsControl.deedObjects[15].SetActive(true);
                }
            }
            else if(whichHouse == 17 || whichHouse == 19 || whichHouse == 20)
            {
                tempColorHouses[3]--;
                if(tempColorHouses[3] == 0)
                {
                    WindowsControl.deedObjects[17].SetActive(true);
                    WindowsControl.deedObjects[19].SetActive(true);
                    WindowsControl.deedObjects[20].SetActive(true);
                }
            }
            else if(whichHouse == 22 || whichHouse == 24 || whichHouse == 25)
            {
                tempColorHouses[4]--;
                if(tempColorHouses[4] == 0)
                {
                    WindowsControl.deedObjects[22].SetActive(true);
                    WindowsControl.deedObjects[24].SetActive(true);
                    WindowsControl.deedObjects[25].SetActive(true);
                }
            }
            else if(whichHouse == 27 || whichHouse == 28 || whichHouse == 30)
            {
                tempColorHouses[5]--;
                if(tempColorHouses[5] == 0)
                {
                    WindowsControl.deedObjects[27].SetActive(true);
                    WindowsControl.deedObjects[28].SetActive(true);
                    WindowsControl.deedObjects[30].SetActive(true);
                }
            }
            else if(whichHouse == 32 || whichHouse == 33 || whichHouse == 35)
            {
                tempColorHouses[6]--;
                if(tempColorHouses[6] == 0)
                {
                    WindowsControl.deedObjects[32].SetActive(true);
                    WindowsControl.deedObjects[33].SetActive(true);
                    WindowsControl.deedObjects[35].SetActive(true);
                }
            }
            else if(whichHouse == 38 || whichHouse == 40)
            {
                tempColorHouses[7]--;
                if(tempColorHouses[7] == 0)
                {
                    WindowsControl.deedObjects[38].SetActive(true);
                    WindowsControl.deedObjects[40].SetActive(true);
                }
            }
        }
        else // Back temporarily selling houses, close mortgaging respective deeds
        {
            if(whichHouse == 2)
            {
                tempColorHouses[0] += GameControl.titleDeedCards[2].numHouses;
                WindowsControl.deedObjects[2].SetActive(false);
                WindowsControl.deedObjects[4].SetActive(false);
            }
            else if(whichHouse == 4)
            {
                tempColorHouses[0] += GameControl.titleDeedCards[4].numHouses;
                WindowsControl.deedObjects[2].SetActive(false);
                WindowsControl.deedObjects[4].SetActive(false);
            }
            else if(whichHouse == 7)
            {
                tempColorHouses[1] += GameControl.titleDeedCards[7].numHouses;
                WindowsControl.deedObjects[7].SetActive(false);
                WindowsControl.deedObjects[9].SetActive(false);
                WindowsControl.deedObjects[10].SetActive(false);
            }
            else if(whichHouse == 9)
            {
                tempColorHouses[1] += GameControl.titleDeedCards[9].numHouses;
                WindowsControl.deedObjects[7].SetActive(false);
                WindowsControl.deedObjects[9].SetActive(false);
                WindowsControl.deedObjects[10].SetActive(false);
            }
            else if(whichHouse == 10)
            {
                tempColorHouses[1] += GameControl.titleDeedCards[10].numHouses;
                WindowsControl.deedObjects[7].SetActive(false);
                WindowsControl.deedObjects[9].SetActive(false);
                WindowsControl.deedObjects[10].SetActive(false);
            }
            else if(whichHouse == 12)
            {
                tempColorHouses[2] += GameControl.titleDeedCards[12].numHouses;
                WindowsControl.deedObjects[12].SetActive(false);
                WindowsControl.deedObjects[14].SetActive(false);
                WindowsControl.deedObjects[15].SetActive(false);
            }
            else if(whichHouse == 14)
            {
                tempColorHouses[2] += GameControl.titleDeedCards[14].numHouses;
                WindowsControl.deedObjects[12].SetActive(false);
                WindowsControl.deedObjects[14].SetActive(false);
                WindowsControl.deedObjects[15].SetActive(false);
            }
            else if(whichHouse == 15)
            {
                tempColorHouses[2] += GameControl.titleDeedCards[15].numHouses;
                WindowsControl.deedObjects[12].SetActive(false);
                WindowsControl.deedObjects[14].SetActive(false);
                WindowsControl.deedObjects[15].SetActive(false);
            }
            else if(whichHouse == 17)
            {
                tempColorHouses[3] += GameControl.titleDeedCards[17].numHouses;
                WindowsControl.deedObjects[17].SetActive(false);
                WindowsControl.deedObjects[19].SetActive(false);
                WindowsControl.deedObjects[20].SetActive(false);
            }
            else if(whichHouse == 19)
            {
                tempColorHouses[3] += GameControl.titleDeedCards[19].numHouses;
                WindowsControl.deedObjects[17].SetActive(false);
                WindowsControl.deedObjects[19].SetActive(false);
                WindowsControl.deedObjects[20].SetActive(false);
            }
            else if(whichHouse == 20)
            {
                tempColorHouses[3] += GameControl.titleDeedCards[20].numHouses;
                WindowsControl.deedObjects[17].SetActive(false);
                WindowsControl.deedObjects[19].SetActive(false);
                WindowsControl.deedObjects[20].SetActive(false);
            }
            else if(whichHouse == 22)
            {
                tempColorHouses[4] += GameControl.titleDeedCards[22].numHouses;
                WindowsControl.deedObjects[22].SetActive(false);
                WindowsControl.deedObjects[24].SetActive(false);
                WindowsControl.deedObjects[25].SetActive(false);
            }
            else if(whichHouse == 24)
            {
                tempColorHouses[4] += GameControl.titleDeedCards[24].numHouses;
                WindowsControl.deedObjects[22].SetActive(false);
                WindowsControl.deedObjects[24].SetActive(false);
                WindowsControl.deedObjects[25].SetActive(false);
            }
            else if(whichHouse == 25)
            {
                tempColorHouses[4] += GameControl.titleDeedCards[25].numHouses;
                WindowsControl.deedObjects[22].SetActive(false);
                WindowsControl.deedObjects[24].SetActive(false);
                WindowsControl.deedObjects[25].SetActive(false);
            }
            else if(whichHouse == 27)
            {
                tempColorHouses[5] += GameControl.titleDeedCards[27].numHouses;
                WindowsControl.deedObjects[27].SetActive(false);
                WindowsControl.deedObjects[28].SetActive(false);
                WindowsControl.deedObjects[30].SetActive(false);
            }
            else if(whichHouse == 29)
            {
                tempColorHouses[5] += GameControl.titleDeedCards[29].numHouses;
                WindowsControl.deedObjects[27].SetActive(false);
                WindowsControl.deedObjects[28].SetActive(false);
                WindowsControl.deedObjects[30].SetActive(false);
            }
            else if(whichHouse == 30)
            {
                tempColorHouses[5] += GameControl.titleDeedCards[30].numHouses;
                WindowsControl.deedObjects[27].SetActive(false);
                WindowsControl.deedObjects[28].SetActive(false);
                WindowsControl.deedObjects[30].SetActive(false);
            }
            else if(whichHouse == 32)
            {
                tempColorHouses[6] += GameControl.titleDeedCards[32].numHouses;
                WindowsControl.deedObjects[32].SetActive(false);
                WindowsControl.deedObjects[33].SetActive(false);
                WindowsControl.deedObjects[35].SetActive(false);
            }
            else if(whichHouse == 33)
            {
                tempColorHouses[6] += GameControl.titleDeedCards[33].numHouses;
                WindowsControl.deedObjects[32].SetActive(false);
                WindowsControl.deedObjects[33].SetActive(false);
                WindowsControl.deedObjects[35].SetActive(false);
            }
            else if(whichHouse == 35)
            {
                tempColorHouses[6] += GameControl.titleDeedCards[35].numHouses;
                WindowsControl.deedObjects[32].SetActive(false);
                WindowsControl.deedObjects[33].SetActive(false);
                WindowsControl.deedObjects[35].SetActive(false);
            }
            else if(whichHouse == 38)
            {
                tempColorHouses[7] += GameControl.titleDeedCards[38].numHouses;
                WindowsControl.deedObjects[38].SetActive(false);
                WindowsControl.deedObjects[40].SetActive(false);
            }
            else if(whichHouse == 40)
            {
                tempColorHouses[7] += GameControl.titleDeedCards[40].numHouses;
                WindowsControl.deedObjects[38].SetActive(false);
                WindowsControl.deedObjects[40].SetActive(false);
            }
        }
        Debug.Log("Temp Color Houses = " + tempColorHouses[0]+","+tempColorHouses[1]+","+tempColorHouses[2]+","+tempColorHouses[3]+","+tempColorHouses[4]+","+tempColorHouses[5]+","+tempColorHouses[6]+","+tempColorHouses[7]);
    }

    void TempSellHousesUpdate(bool tempMortgaging=true)
    {
        if(tempMortgaging) // Temporarily mortgaging, if player is color owner - close back selling houses
        {
            if(GameControl.colorOwners[0] == myIndex && (whichDeed == 2 || whichDeed == 4))
            {
                WindowsControl.houseObjects[2].SetActive(false);
                WindowsControl.houseObjects[4].SetActive(false);
            }
            else if(GameControl.colorOwners[1] == myIndex && (whichDeed == 7 || whichDeed == 9 || whichDeed == 10))
            {
                WindowsControl.houseObjects[7].SetActive(false);
                WindowsControl.houseObjects[9].SetActive(false);
                WindowsControl.houseObjects[10].SetActive(false);
            }
            else if(GameControl.colorOwners[2] == myIndex && (whichDeed == 12 || whichDeed == 14 || whichDeed == 15))
            {
                WindowsControl.houseObjects[12].SetActive(false);
                WindowsControl.houseObjects[14].SetActive(false);
                WindowsControl.houseObjects[15].SetActive(false);
            }
            else if(GameControl.colorOwners[3] == myIndex && (whichDeed == 17 || whichDeed == 19 || whichDeed == 20))
            {
                WindowsControl.houseObjects[17].SetActive(false);
                WindowsControl.houseObjects[19].SetActive(false);
                WindowsControl.houseObjects[20].SetActive(false);
            }
            else if(GameControl.colorOwners[4] == myIndex && (whichDeed == 22 || whichDeed == 24 || whichDeed == 25))
            {
                WindowsControl.houseObjects[22].SetActive(false);
                WindowsControl.houseObjects[24].SetActive(false);
                WindowsControl.houseObjects[25].SetActive(false);
            }
            else if(GameControl.colorOwners[5] == myIndex && (whichDeed == 27 || whichDeed == 28 || whichDeed == 30))
            {
                WindowsControl.houseObjects[27].SetActive(false);
                WindowsControl.houseObjects[28].SetActive(false);
                WindowsControl.houseObjects[30].SetActive(false);
            }
            else if(GameControl.colorOwners[6] == myIndex && (whichDeed == 32 || whichDeed == 33 || whichDeed == 35))
            {
                WindowsControl.houseObjects[32].SetActive(false);
                WindowsControl.houseObjects[33].SetActive(false);
                WindowsControl.houseObjects[35].SetActive(false);
            }
            else if(GameControl.colorOwners[7] == myIndex && (whichDeed == 38 || whichDeed == 40))
            {
                WindowsControl.houseObjects[38].SetActive(false);
                WindowsControl.houseObjects[40].SetActive(false);
            }
        }
        else // Temporarily back mortgaging, if player is color owner, AND none of the color deeds is mortgaged, AND there is a house on the deed - open back selling houses
        {       // Player is color owner                       // Checking for Browns               // None of the color deed was previously mortgaged                                                              // Neither temporarily mortgaged
            if(GameControl.colorOwners[0] == myIndex && (whichDeed == 2 || whichDeed == 4) && GameControl.titleDeedCardsOwners[2] == myIndex && GameControl.titleDeedCardsOwners[4] == myIndex && !tempMortgage[2] && !tempMortgage[4])
            {
                if(GameControl.titleDeedCards[2].numHouses > 0) // AND there is a house on the deed
                {
                    WindowsControl.houseObjects[2].SetActive(true);
                }
                if(GameControl.titleDeedCards[4].numHouses > 0)
                {
                    WindowsControl.houseObjects[4].SetActive(true);
                }
            }
            else if(GameControl.colorOwners[1] == myIndex && (whichDeed == 7 || whichDeed == 9 || whichDeed == 10) && GameControl.titleDeedCardsOwners[7] == myIndex && GameControl.titleDeedCardsOwners[9] == myIndex && GameControl.titleDeedCardsOwners[10] == myIndex && !tempMortgage[7] && !tempMortgage[9] && !tempMortgage[10])
            {
                if(GameControl.titleDeedCards[7].numHouses > 0)
                {
                    WindowsControl.houseObjects[7].SetActive(true);
                }
                if(GameControl.titleDeedCards[9].numHouses > 0)
                {
                    WindowsControl.houseObjects[9].SetActive(true);
                }
                if(GameControl.titleDeedCards[10].numHouses > 0)
                {
                    WindowsControl.houseObjects[10].SetActive(true);
                }
            }
            else if(GameControl.colorOwners[2] == myIndex && (whichDeed == 12 || whichDeed == 14 || whichDeed == 15) && GameControl.titleDeedCardsOwners[12] == myIndex && GameControl.titleDeedCardsOwners[14] == myIndex && GameControl.titleDeedCardsOwners[15] == myIndex && !tempMortgage[12] && !tempMortgage[14] && !tempMortgage[15])
            {
                if(GameControl.titleDeedCards[12].numHouses > 0)
                {
                    WindowsControl.houseObjects[12].SetActive(true);
                }
                if(GameControl.titleDeedCards[14].numHouses > 0)
                {
                    WindowsControl.houseObjects[14].SetActive(true);
                }
                if(GameControl.titleDeedCards[15].numHouses > 0)
                {
                    WindowsControl.houseObjects[15].SetActive(true);
                }
            }
            else if(GameControl.colorOwners[3] == myIndex && (whichDeed == 17 || whichDeed == 19 || whichDeed == 20) && GameControl.titleDeedCardsOwners[17] == myIndex && GameControl.titleDeedCardsOwners[19] == myIndex && GameControl.titleDeedCardsOwners[20] == myIndex && !tempMortgage[17] && !tempMortgage[19] && !tempMortgage[20])
            {
                if(GameControl.titleDeedCards[17].numHouses > 0)
                {
                    WindowsControl.houseObjects[17].SetActive(true);
                }
                if(GameControl.titleDeedCards[19].numHouses > 0)
                {
                    WindowsControl.houseObjects[19].SetActive(true);
                }
                if(GameControl.titleDeedCards[20].numHouses > 0)
                {
                    WindowsControl.houseObjects[20].SetActive(true);
                }
            }
            else if(GameControl.colorOwners[4] == myIndex && (whichDeed == 22 || whichDeed == 24 || whichDeed == 25) && GameControl.titleDeedCardsOwners[22] == myIndex && GameControl.titleDeedCardsOwners[24] == myIndex && GameControl.titleDeedCardsOwners[25] == myIndex && !tempMortgage[22] && !tempMortgage[24] && !tempMortgage[25])
            {
                if(GameControl.titleDeedCards[22].numHouses > 0)
                {
                    WindowsControl.houseObjects[22].SetActive(true);
                }
                if(GameControl.titleDeedCards[24].numHouses > 0)
                {
                    WindowsControl.houseObjects[24].SetActive(true);
                }
                if(GameControl.titleDeedCards[25].numHouses > 0)
                {
                    WindowsControl.houseObjects[25].SetActive(true);
                }
            }
            else if(GameControl.colorOwners[5] == myIndex && (whichDeed == 27 || whichDeed == 28 || whichDeed == 30) && GameControl.titleDeedCardsOwners[27] == myIndex && GameControl.titleDeedCardsOwners[28] == myIndex && GameControl.titleDeedCardsOwners[30] == myIndex && !tempMortgage[27] && !tempMortgage[28] && !tempMortgage[30])
            {
                if(GameControl.titleDeedCards[27].numHouses > 0)
                {
                    WindowsControl.houseObjects[27].SetActive(true);
                }
                if(GameControl.titleDeedCards[28].numHouses > 0)
                {
                    WindowsControl.houseObjects[28].SetActive(true);
                }
                if(GameControl.titleDeedCards[30].numHouses > 0)
                {
                    WindowsControl.houseObjects[30].SetActive(true);
                }
            }
            else if(GameControl.colorOwners[6] == myIndex && (whichDeed == 32 || whichDeed == 33 || whichDeed == 35) && GameControl.titleDeedCardsOwners[32] == myIndex && GameControl.titleDeedCardsOwners[33] == myIndex && GameControl.titleDeedCardsOwners[35] == myIndex && !tempMortgage[32] && !tempMortgage[33] && !tempMortgage[35])
            {
                if(GameControl.titleDeedCards[32].numHouses > 0)
                {
                    WindowsControl.houseObjects[32].SetActive(true);
                }
                if(GameControl.titleDeedCards[33].numHouses > 0)
                {
                    WindowsControl.houseObjects[33].SetActive(true);
                }
                if(GameControl.titleDeedCards[35].numHouses > 0)
                {
                    WindowsControl.houseObjects[35].SetActive(true);
                }
            }
            else if(GameControl.colorOwners[7] == myIndex && (whichDeed == 38 || whichDeed == 40) && GameControl.titleDeedCardsOwners[38] == myIndex && GameControl.titleDeedCardsOwners[40] == myIndex && !tempMortgage[38] && !tempMortgage[40])
            {
                if(GameControl.titleDeedCards[38].numHouses > 0)
                {
                    WindowsControl.houseObjects[38].SetActive(true);
                }
                if(GameControl.titleDeedCards[40].numHouses > 0)
                {
                    WindowsControl.houseObjects[40].SetActive(true);
                }
            }
        }
    }

    public void Bankruptcy(int debt, int payee=0)
    {
        WindowsControl.bankruptcyWindow.SetActive(true);
        WindowsControl.bankruptcyMessage.GetComponent<TextMeshProUGUI>().text = "You owe " + GameControl.MoneyText(debt) + ", but all you can afford is " + GameControl.MoneyText(capital) + ". Now all of your belongings will be solgt, and the money will be transfered to whom you owe.";
        for(int i = 2; i < 41; i++)
        {
            if(GameControl.titleDeedCardsOwners[i] == myIndex || GameControl.titleDeedCardsOwners[i] == -myIndex)
            {
                GameControl.titleDeedCardsOwners[i] = 0;                // Deeds logical
                GameControl.fieldmarkers[i].SetActive(false);           // Deeds graphical
                WindowsControl.mortgageObjects[i].SetActive(false);     // Mortgage graphical
                if(GameControl.titleDeedCards[i].numHouses > 0)         // Houses and hotels
                {
                    if(GameControl.titleDeedCards[i].numHouses == 5)
                    {
                        WindowsControl.houses[4][i].SetActive(false);
                    }
                    else
                    {
                        for(int j = 0; j < GameControl.titleDeedCards[i].numHouses; j++)
                        {
                            WindowsControl.houses[j][i].SetActive(false);
                        }
                    }
                    GameControl.titleDeedCards[i].numHouses = 0;
                }
            }
        }
        for(int i = 0; i < 8; i++)
        {
            if(GameControl.colorOwners[i] == myIndex)
            {
                GameControl.colorOwners[i] = 0;                         // Color Owners
                GameControl.colorHouses[i] = 0;                         // Color Houses
            }
        }
        if(freeChance)
        {
            GameControl.chanceIndices.Enqueue(11);                      // Free Cards
        }
        if(freeChest)
        {
            GameControl.chestIndices.Enqueue(2);
        }

        GameControl.playerIcons[myIndex].SetActive(false);              // Icon
        if(payee > 0)
        {
            GameControl.players[payee].GetComponent<PlayerControl>().money += capital;
            GameControl.players[payee].GetComponent<PlayerControl>().AddMoney(capital);
        }
    }
}
