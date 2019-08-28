using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Business
{
    public bool enabled;

    public string name; //부서 이름
    public List<int> worker; // 직원수 { 상급, 중급, 하급 }
    public int profit; //매달 수익
    public int loss; //매달 손실

    private int devStartDay;
    private int period; //개발 기간
    public int Period //앞으로 남은 기간
    {
        get
        {
            if (devStartDay == 0)
            {
                return -DecreasedPeriodByWorker() - DecreasePeriodByExpert();
            }
            else
            {
                if(period - DecreasedPeriodByTime() - DecreasedPeriodByWorker() - DecreasePeriodByExpert() <= 0)
                    DevEnd();
                return period - DecreasedPeriodByTime() - DecreasedPeriodByWorker() - DecreasePeriodByExpert();
            }
        }
    }

    public void Initialize(string departmentName)
    {
        enabled = false;
        name = departmentName;
        worker = new List<int> { 0, 0, 0 };

        devStartDay = 0;
        period = -1;
        profit = 0;
        loss = 0;
    }

    public void Create()
    {
        enabled = true;
        devStartDay = 0;
        worker = new List<int> { 0, 1, 5 };

        period = -1;
        profit = 0;
        loss = MaintenancePay() + MonthlyPay();
    }

    private int MaintenancePay()
    {
        int pay = 10000;
        if (GameManager.instance.experts[0].isHired == true)
            pay /= 2;

        return pay;
    }

    public void HireWorker(int i)
    {
        worker[i] += 1;

        loss = MaintenancePay() + MonthlyPay();
    }

    public void FireWorker(int i)
    {
        worker[i] -= 1;
        GameManager.instance.ChangeSatisfaction(-3 * (3 - i) * (3 - i));

        loss = MaintenancePay() + MonthlyPay();
    }

    public void StartDevelop(int investMoney)
    {
        //단위 : (일)
        period = investMoney / 300;
        devStartDay = GameManager.instance.Day;
    }

    private int DecreasedPeriodByTime()
    {
        return GameManager.instance.Day - devStartDay;
    }

    private int DecreasedPeriodByWorker()
    {
        return (worker[0] * 10) + (worker[1] * 3) + worker[2];
    }

    private int DecreasePeriodByExpert()
    {
        int amount = 0;

        if (GameManager.instance.experts[1].isHired == true) amount += 60;
        if (GameManager.instance.experts[3].isHired == true) amount += 10;

        return amount;
    }

    private int MonthlyPay()
    {
        int pay = worker[0] * 6000 + worker[1] * 2100 + worker[2] * 800;

        if (GameManager.instance.experts[5].isHired == true) pay = (int)(pay * 0.6f);
        return pay;
    }

    public void DevEnd()
    {
        //개발완료 현상
        UIManager.instance.EndDevelop();
        float response = Random.Range(0, 10);
        int newPorfit = (int)((15 + response) * period * Mathf.Pow(2.718f, 0.021f * period) / 3f);
        int newRepu = (int)((7.25 + response / 5) * Mathf.Log(period) - 34.65f);

        if (response >= 3.3f && newRepu < 0) newRepu = 0;
        if (response >= 6.7f && newRepu < 3) newRepu = 3;

        profit += newPorfit;
        GameManager.instance.ChangeReputation(newRepu);

        devStartDay = 0;
        period = -1;
        UIManager.instance.DevEndEvent(name, response, newPorfit, newRepu);
    }
}