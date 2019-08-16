using System.Collections;
using System.Collections.Generic;

public class Business
{
    public string name; //부서 이름
    public List<int> worker; // 직원수 { 상급, 중급, 하급 }

    private int devStartDay;
    private int period; //개발 기간
    public int Period
    {
        get
        {
            if (period - DecreasedPeriodByTime() - DecreasedPeriodByWorker() < 0)
                return -1;
            else
                return period - DecreasedPeriodByTime() - DecreasedPeriodByWorker();
        }
    }

    public void Initialize(string departmentName)
    {
        name = departmentName;
        worker = new List<int> { 0, 15, 0 };

        period = -1;
    }

    public void HireWorker(int i)
    {
        worker[i] += 1;
    }

    public void FireWorker(int i)
    {
        worker[i] -= 1;
        //직원 만족도를 깍아야함.
    }

    public void StartDevelop()
    {
        int nowMoney = GameManager.instance.Money;

        GameManager.instance.UseMoney(nowMoney / 12);
        period = nowMoney / 9000;
    }

    private int DecreasedPeriodByTime()
    {
        return (GameManager.instance.Day - devStartDay) / 30;
    }

    private int DecreasedPeriodByWorker()
    {
        return (worker[0] / 7) + (worker[1] / 15) + (worker[2] / 30);
    }

    public int MonthlyPay()
    {
        return worker[0] * 6000 + worker[1] * 3000 + worker[2] * 2000;
    }

    public void SetPeriod(int initialPeriod)
    {
        devStartDay = GameManager.instance.Day;
        period = initialPeriod;
    }
}