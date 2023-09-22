using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Names : MonoBehaviour
{
    [SerializeField] public GameObject GameControlObject;
    [SerializeField] public GameObject NamesWindow;
    public TMP_InputField[] inputs;
    
    // Store input texts into a list
    public static List<string> playerNames;


    private void Start()
    {
        playerNames = new();

        // Deactivate all input fields first
        foreach (var input in inputs) input.gameObject.SetActive(false);
        // Activate the appropriate number of input fields based on the value of numberOfPlayers
        for (int i = 0; i < GameStart.numberOfPlayers; i++) inputs[i].gameObject.SetActive(true);

        NamesWindow.SetActive(true);
    }

    public void NamesOKButton()
    {
        for (int i = 0; i < GameStart.numberOfPlayers; i++)
        {
            var name = inputs[i].text;
            if (name.ToLower().Contains("iza") || name.ToLower().Contains("izoz")) name = "Izabelsko";
            playerNames.Add(name);
        }

        NamesWindow.SetActive(false);
        GameControlObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
