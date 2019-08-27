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
                return -1;
            else
            {
                if (period - DecreasedPeriodByTime() - DecreasedPeriodByWorker() <= 0)
                    DevEnd();
                return period - DecreasedPeriodByTime() - DecreasedPeriodByWorker();
            }
        }
    }

    public void Initialize(string departmentName)
    {
        enabled = false;
        devStartDay = 0;
        name = departmentName;
        worker = new List<int> { 0, 0, 0 };

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
        loss = 10000 + MonthlyPay();
    }

    public void HireWorker(int i)
    {
        worker[i] += 1;

        loss = 10000 + MonthlyPay();
    }

    public void FireWorker(int i)
    {
        worker[i] -= 1;
        GameManager.instance.ChangeSatisfaction(-3f);

        loss = 10000 + MonthlyPay();
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

    private int MonthlyPay()
    {
        return worker[0] * 6000 + worker[1] * 2100 + worker[2] * 800;
    }

    public void DevEnd()
    {
        //개발완료 현상
        UIManager.instance.EndDevelop();
        int response = Random.Range(0, 10);
        int newPorfit = (int)(20f * period * Mathf.Pow(2.718f, 0.021f * period) / 3f);
        int newRepu = 2 - response;
        profit += newPorfit;
        GameManager.instance.repuation += newRepu;

        devStartDay = 0;
        UIManager.instance.DevEndEvent(name, newPorfit, newRepu);
    }
}