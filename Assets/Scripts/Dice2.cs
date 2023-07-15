using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice2 : MonoBehaviour
{
    [SerializeField]
    private Sprite[] diceSides;

    public static int dice2;


    void Start()
    {
        dice2 = 1;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = diceSides[dice2 - 1];
    }
}
