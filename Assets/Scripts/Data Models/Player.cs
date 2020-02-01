using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int points = 0;
    int location = 0;
    bool isStunned = false;
    double lastDamaged = 3;

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (isStunned)
        {
            lastDamaged = lastDamaged - 0.02;
            if (lastDamaged == 0)
            {
                isStunned = false;
                lastDamaged = 3;
            }
        }
    }

    void hit(Player opponent){
        opponent.isStunned = true;
    }

    void jump(){
        
    }

    void repair(Device device){
        device.interact();
    }
}
