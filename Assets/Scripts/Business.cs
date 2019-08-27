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
        worker = new List<int> { 0, 15, 0 };

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
        return ((worker[0] / 7) + (worker[1] / 15) + (worker[2] / 30)) * 30;
    }

    private int MonthlyPay()
    {
        return worker[0] * 6000 + worker[1] * 3000 + worker[2] * 2000;
    }

    public void DevEnd()
    {
        //개발완료 현상
        UIManager.instance.EndDevelop();
        int response = Random.Range(0,7);
        profit += 10 * period * (int)Mathf.Pow(Mathf.Pow(2, 1 / 6), 2 - response);
        GameManager.instance.repuation += 4 - response;

        devStartDay = 0;

        //기간이 짧을 때는 이득이 적게
        //30일 동안 개발 시 24개월간 원금회수
        //120일 동안 개발 시 4개월간 원금회수
        //150일 동안 개발 시 2개월간 원금회수
        //120일 동안 개발에 필요한 원금 = 36000
        //120일 개발 시 월 수익 = 9000
        //profit += 
    }
}