using System.Collections;
using System.Collections.Generic;

public class Business
{
    public string name; //부서 이름
    public List<int> worker; // 직원수 { 상급, 중급, 하급 }

    private int period; //개발 기간
    public int Period { get { return period < 0 ? 0 : period - DecreasedPeriodByWorker(); } }

    public void Initialize(DepartmentName departmentName)
    {
        name = departmentName.ToString();
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
    }

    public void StartDevelop()
    {
        int nowMoney = GameManager.instance.Money;

        GameManager.instance.UseMoney(nowMoney / 12);
        period = nowMoney / 9000;
    }

    private int DecreasedPeriodByWorker()
    {
        return (worker[0] / 7) + (worker[1] / 15) + (worker[2] / 30);
    }

    public int MonthlyPay()
    {
        return worker[0] * 6000 + worker[1] * 3000 + worker[2] * 2000;
    }
}