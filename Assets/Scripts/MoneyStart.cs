using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyStart : MonoBehaviour
{
    [SerializeField] public GameObject MoneyStartObject;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 1; i <= GameStart.numberOfPlayers; i++)
        {
            GameControl.playerUpdate(i);
        }
        MoneyStartObject.SetActive(false);
    }
}
