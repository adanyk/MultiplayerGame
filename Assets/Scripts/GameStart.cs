using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    // number of players
    [SerializeField] public Button[] NumberOfPlayersButtons;
    [SerializeField] public GameObject StartWindow;
    [SerializeField] public GameObject GameControlObject;
    public static bool numberOfPlayersChosen;
    public static int numberOfPlayers;


    // Start is called before the first frame update
    void Start()
    {
        // choose number of players
        numberOfPlayersChosen = false;
        StartWindow.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // choose number of players
        if(!numberOfPlayersChosen)
        {
            NumberOfPlayersButtons[0].onClick.AddListener(Button1Clicked);
            NumberOfPlayersButtons[1].onClick.AddListener(Button2Clicked);
            NumberOfPlayersButtons[2].onClick.AddListener(Button3Clicked);
            NumberOfPlayersButtons[3].onClick.AddListener(Button4Clicked);
            NumberOfPlayersButtons[4].onClick.AddListener(Button5Clicked);
            NumberOfPlayersButtons[5].onClick.AddListener(Button6Clicked);
        }
        // begin game
        else
        {
            GameControlObject.SetActive(true);
            StartWindow.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

    // number of players
    public static void Button1Clicked()
    {
        numberOfPlayers = 1;
        numberOfPlayersChosen = true;
    }
    public static void Button2Clicked()
    {
        numberOfPlayers = 2;
        numberOfPlayersChosen = true;
    }
    public static void Button3Clicked()
    {
        numberOfPlayers = 3;
        numberOfPlayersChosen = true;
    }
    public static void Button4Clicked()
    {
        numberOfPlayers = 4;
        numberOfPlayersChosen = true;
    }
    public static void Button5Clicked()
    {
        numberOfPlayers = 5;
        numberOfPlayersChosen = true;
    }
    public static void Button6Clicked()
    {
        numberOfPlayers = 6;
        numberOfPlayersChosen = true;
    }
}
