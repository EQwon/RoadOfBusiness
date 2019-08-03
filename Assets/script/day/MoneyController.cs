using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyController : MonoBehaviour
{
    int money = 5000;
    int month = 1;
    int monthlyPay = 0;
    int monthlyGain = 0;

    public Text myMoney;
    public Text date;
    public Text MonthlyPayText;
    public Text MonthlyGainText;

    private void Update()
    {
        myMoney.text = "자본 : " + money.ToString();
        MonthlyPayText.text = "지출 : " + monthlyPay.ToString();
        MonthlyGainText.text = "수입 : " + monthlyGain.ToString();
    }

    public void NextDay()
    {
        /*money += Random.Range(400, 601);*/
        money += monthlyGain;
        money -= monthlyPay;
        month += 1;
        date.text = month.ToString() + "월";
    }

    public void Hire(int price)
    {
        if (money >= price)
        {
            money -= price;
        }
    }

    public void MonthPay(int pay)
    {
        monthlyPay += pay;
    }
    public void MonthGain(int gain)
    {
        monthlyGain += gain;
    }
}