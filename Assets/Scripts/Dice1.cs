using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice1 : MonoBehaviour
{
    [SerializeField]
    private Sprite[] diceSides;

    public static int dice1;


    void Start()
    {
        dice1 = 1;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = diceSides[dice1 - 1];
    }

    public static void DiceRoll()
    {
        dice1 = Random.Range(1,7);
        Dice2.dice2 = Random.Range(1,7);
    }
}
