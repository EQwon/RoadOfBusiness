using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WorkerText
{
    public List<Text> people;
}

public class MoneyController : MonoBehaviour
{
    int money = 450000;
    int month = 1;
    int monthlyPay = 45000;
    List<int> people = new List<int> { 0, 15, 0 };
    int okh = 0;
    int okf = 0;
    int outcomeDay;

    public Text myMoney;
    public Text date;
    public Text MonthlyPayText;
    public List<Text> peopleText;
    public Text OutcomeDayText;
    public List<WorkerText> test;

    private float time;

    private void Update()
    {
        myMoney.text = "자본 : " + money.ToString();
        MonthlyPayText.text = "지출 : " + monthlyPay.ToString();

        OutcomeDayText.text = (outcomeDay - ShortenMonth()).ToString() + "월 남음";

        time += Time.deltaTime;
        if (time >= 5f)
        {
            money -= monthlyPay;
            outcomeDay -= 1;
            money -= 10000;
            month += 1;
            date.text = month.ToString() + "월";
            time = 0;
        }
    }
    public void Hire()
    {
        okh = 1;
    }

    public void MonthPayH(int pay)
    {
        if (okh==1)
        {
            monthlyPay += pay;
        }
    }
    public void PeopleH(int peo)
    {
        if (okh == 1)
        {
            people[peo] += 1;
            peopleText[peo].text = people[peo].ToString() + " 명";
            okh = 0;
        }
    }
    public void Fire(int peo)
    {
        if(people[peo]>0)
        {
            okf = 1;
        }
    }
    public void MonthPayF(int pay)
    {
        if (okf == 1)
        {
            monthlyPay -= pay;
        }
    }
    public void PeopleF(int peo)
    {
        if (okf == 1)
        {
            people[peo] -= 1;
            peopleText[peo].text = people[peo].ToString() + " 명";
            okf = 0;
        }
    }
    public void Outcome()
    {
        outcomeDay = money / 9000;
    }

    private int ShortenMonth()
    {
        int month = (people[0] / 2) + (people[1] / 10) + (people[2] / 30);

        return month;
    }
}