using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moneyadd : MonoBehaviour
{
    int money = 0;
    public void Money()
    {
        money = money + 1000;
        GetComponent<Text>().text = money.ToString();
    }
    public void Moneymi()
    {
        if (money >= 1000)
        {
            money = money - 1000;
            GetComponent<Text>().text = money.ToString();

        }
    }
    public void Buyg()
    {
        if (money >= 10000)
        {
            money = money - 10000;
            GetComponent<Text>().text = money.ToString();

        }
    }
}
