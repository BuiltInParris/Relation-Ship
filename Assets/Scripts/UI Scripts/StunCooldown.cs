using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StunCooldown : MonoBehaviour
{
    public Player player;
    public Text text;

    void FixedUpdate()
    {
        if(player.stunCooldownCounter > 0)
        {
            text.transform.gameObject.SetActive(true);
            text.text = player.stunCooldownDisplay.ToString();
        }
        else
        {
            text.transform.gameObject.SetActive(false);
        }

    }
}
