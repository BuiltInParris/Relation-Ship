using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Device : MonoBehaviour, Interactable
{
    bool isDamaged;

    double timeRepaired;

    int variableAmountOfTime;

    double lastRepaired;

    System.Random rnd;

    double randomTimeTillNextDamage;

    Color color;


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
            lastRepaired = lastRepaired + 0.02;
            if(lastRepaired > timeRepaired + randomTimeTillNextDamage){
                isDamaged = true;
                this.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
                randomTimeTillNextDamage = rnd.Next(0, variableAmountOfTime * 2) - variableAmountOfTime;
                lastRepaired = 0;
            }
        }
    }

    public void interact(){
        isDamaged = false;
        this.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 1f);
    }
}
