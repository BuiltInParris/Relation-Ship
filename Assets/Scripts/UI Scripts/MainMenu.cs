using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Canvas canvas;

    public void Start()
    {
        for (var i = 0; i < Gamepad.all.Count; i++)
        {
            GetComponentInChildren<Dropdown>().options.Add(new Dropdown.OptionData(NumberToWords(i + 5)));
        }

    }
    
    public void PlayGame ()
    {
        SceneManager.LoadScene("Scenes/DynamicallyGeneratedLevel");

    }

    public void QuitGame()
    {
      Debug.Log("Quit");
      Application.Quit();
    }

    public void SetPlayerCount (int pcIndex)
    {
        GameSettings.playerCount = pcIndex + 2;
    }
    
    public static string NumberToWords(int number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Negative " + NumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 1000000) > 0)
        {
            words += NumberToWords(number / 1000000) + " Million ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            words += NumberToWords(number / 1000) + " Thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += NumberToWords(number / 100) + " Hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += "-" + unitsMap[number % 10];
            }
        }

        return words;
    }
}
