using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Device : MonoBehaviour, Interactable
{
    public bool isDamaged;

    double timeRepaired;

    int variableAmountOfTime;

    double lastRepaired;

    System.Random rnd;

    double randomTimeTillNextDamage;

    Color color;

    public Sprite defaultSprite;
    public Sprite damagedSprite;


    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random();
        isDamaged = true;
        timeRepaired = 5;
        variableAmountOfTime = 3;
        lastRepaired = 0;
        randomTimeTillNextDamage = rnd.Next(0, variableAmountOfTime * 2) - variableAmountOfTime;
    }

    // Update is called once per physics loop
    void FixedUpdate()
    {
        if(!isDamaged){
            lastRepaired = lastRepaired + Time.fixedDeltaTime;
            if(lastRepaired > timeRepaired + randomTimeTillNextDamage){
                isDamaged = true;
                GetComponent<SpriteRenderer>().sprite = damagedSprite;
                randomTimeTillNextDamage = rnd.Next(0, variableAmountOfTime * 2) - variableAmountOfTime;
                lastRepaired = 0;
            }
        }
    }

    public int interact(){
        if(isDamaged)
        {
            isDamaged = false;
            GetComponent<SpriteRenderer>().sprite = defaultSprite;

            return Constants.DEVICE_POINT_VALUE;
        }
        return 0;
    }
}
