using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("기본 UI Holder")]
    public Text companyNameText;
    public Text dateText;
    public Text moneyText;
    public Text satisfactionText;
    public Text reputationText;
    
    [Header("사업부 현황판 UI Holder")]
    public List<Text> workerAmountText;
    public Text remainPeriodText;
    public GameObject beforeDev;
    public GameObject duringDev;

    [Header("회사 현황판 UI Holder")]
    public Text companyName;
    public Text totalExpenditure;
    public Text totalSales;
    public Text netRevenue;
    public Text totalWorkers;

    [Header("식단표 UI Holder")]
    public GameObject reputationIncreasedText;
    public GameObject satisfactionIncreasedText;

    /// <summary>
    /// 현재 살펴보고 있는 사업부의 현황판이 몇 번째 사업부인지를 저장해둡니다.
    /// </summary>
    private int nowDepartment;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        companyNameText.text = GameManager.instance.companyName;
    }

    private void Update()
    {
        //기본 정보
        ShowMoney();
        ShowDate();
        ShowRepuation();
        ShowSatisfaction();

        //사업부 현황판 정보
        ShowDepartmentStatus();

        //회사 전체 정보
        ShowCompanyStatus();
    }

    public void WhichDepartment(int num)
    {
        nowDepartment = num;
    }

    /// <summary>
    /// 현재 사업부에 해당하는 내용으로 텍스트를 교체합니다.
    /// </summary>
    private void ShowDepartmentStatus()
    {
        for (int i = 0; i < 3; i++)
        {
            workerAmountText[i].text = GameManager.instance.department[nowDepartment].worker[i].ToString() + " 명";
        }

        remainPeriodText.text = "남은 개발 기간 : " + GameManager.instance.department[nowDepartment].Period.ToString() + " 개월";
    }

    private void ShowMoney()
    {
        moneyText.text = GameManager.instance.Money.ToString() + " ZS";
    }

    private void ShowDate()
    {
        int day = GameManager.instance.Day;
        dateText.text = (day / 30) + "개월 " + (day % 30) + "일 째";
    }

    private void ShowRepuation()
    {
        reputationText.text = GameManager.instance.repuation.ToString();
    }

    private void ShowSatisfaction()
    {
        satisfactionText.text = GameManager.instance.Satisfaction.ToString();
    }
    public void HireWorker(int type)
    {
        GameManager.instance.department[nowDepartment].worker[type] += 1;
    }

    public void FireWorker(int type)
    {
        if (GameManager.instance.department[nowDepartment].worker[type] > 0)
        {
            GameManager.instance.department[nowDepartment].worker[type] -= 1;
            GameManager.instance.ChangeSatisfaction(-0.3f);
        }
    }

    public void StartDevelop()
    {
        //신제품 개발에 착수
        GameManager.instance.StartNewDevelop(nowDepartment);
    }

    public void EndDevelop()
    {
        duringDev.SetActive(false);
        beforeDev.SetActive(true);
    }

    public void ShowCompanyStatus()
    {
        companyName.text = GameManager.instance.companyName;
        totalExpenditure.text = "" + " zs";
        totalSales.text = "" + "  zs";
        netRevenue.text = GameManager.instance.TotalNeRevenue() + " zs";
        totalWorkers.text = GameManager.instance.TotalWorkerAmount().ToString() + " 명";
    }

    public IEnumerator ShowReputationIncrease(int amount)
    {
        reputationIncreasedText.GetComponent<Text>().text = "인식 +" + amount;
        reputationIncreasedText.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        reputationIncreasedText.SetActive(false);
    }

    public IEnumerator ShowSatisfactionIncrease(int amount)
    {
        satisfactionIncreasedText.GetComponent<Text>().text = "직원만족도 +" + amount;
        satisfactionIncreasedText.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        satisfactionIncreasedText.SetActive(false);
    }
}
