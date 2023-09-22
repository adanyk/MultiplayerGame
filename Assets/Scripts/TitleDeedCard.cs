using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleDeedCard
{
    public int cardIndex, buyPrice, basicRent, housePrice, oneHouse, twoHouses, threeHouses, fourHouses, hotel;
    public int numHouses = 0;
    public string name, type, color;
    int transportProperties;

    // Creator
    public TitleDeedCard(int cardIndex, int buyPrice, int basicRent, int housePrice, int oneHouse, int twoHouses, int threeHouses, int fourHouses, int hotel, string name, string type, string color=null)
    {
        this.cardIndex = cardIndex;
        this.buyPrice = buyPrice;
        this.basicRent = basicRent;
        this.housePrice = housePrice;
        this.oneHouse = oneHouse;
        this.twoHouses = twoHouses;
        this.threeHouses = threeHouses;
        this.fourHouses = fourHouses;
        this.hotel = hotel;
        this.name = name;
        this.type = type;
        this.color = color;
    }


    public int Rent(int move)
    {
        switch(type)
        {
            case "city":
                switch(numHouses)
                {
                    case 1:
                        return oneHouse;
                    case 2:
                        return twoHouses;
                    case 3:
                        return threeHouses;
                    case 4:
                        return fourHouses;
                    case 5:
                        return hotel;
                    case 0:
                        switch(color)
                        {
                            case "brown":
                                if(GameControl.colorOwners[0] > 0)
                                {
                                    return 2 * basicRent;
                                }
                                else
                                {
                                    return basicRent;
                                }
                            case "lightblue":
                                if(GameControl.colorOwners[1] > 0)
                                {
                                    return 2 * basicRent;
                                }
                                else
                                {
                                    return basicRent;
                                }
                            case "rose":
                                if(GameControl.colorOwners[2] > 0)
                                {
                                    return 2 * basicRent;
                                }
                                else
                                {
                                    return basicRent;
                                }
                            case "orange":
                                if(GameControl.colorOwners[3] > 0)
                                {
                                    return 2 * basicRent;
                                }
                                else
                                {
                                    return basicRent;
                                }
                            case "red":
                                if(GameControl.colorOwners[4] > 0)
                                {
                                    return 2 * basicRent;
                                }
                                else
                                {
                                    return basicRent;
                                }
                            case "yellow":
                                if(GameControl.colorOwners[5] > 0)
                                {
                                    return 2 * basicRent;
                                }
                                else
                                {
                                    return basicRent;
                                }
                            case "green":
                                if(GameControl.colorOwners[6] > 0)
                                {
                                    return 2 * basicRent;
                                }
                                else
                                {
                                    return basicRent;
                                }
                            case "blue":
                                if(GameControl.colorOwners[7] > 0)
                                {
                                    return 2 * basicRent;
                                }
                                else
                                {
                                    return basicRent;
                                }
                        }
                        break;
                }
                break;
            case "transport":
                transportProperties = 0;
                for(int i = 6; i < 37; i += 10)
                {
                    if(GameControl.titleDeedCardsOwners[cardIndex] == GameControl.titleDeedCardsOwners[i])
                    {
                        transportProperties++;
                    }
                }
                if(transportProperties == 4)
                {
                    return 8 * basicRent;
                }
                else if(transportProperties == 3)
                {
                    return 4 * basicRent;
                }
                else
                {
                    return transportProperties * basicRent;
                }
            case "utility":
                if(GameControl.titleDeedCardsOwners[13] == GameControl.titleDeedCardsOwners[29])
                {
                    return 10 * move;
                }
                else
                {
                    return 4 * move;
                }
            }
        return 0;
    }
}
