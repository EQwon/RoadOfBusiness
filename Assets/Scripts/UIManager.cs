using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Basic UI Holder")]
    public Text dateText;
    public Text creditRateText;
    public Text moneyText;
    public Text satisfactionText;
    public Text reputationText;

    [Header("Department Status UI Holder")]
    public List<Text> workerAmountText;

    /// <summary>
    /// 현재 살펴보고 있는 사업부의 현황판이 몇 번째 사업부인지를 저장해둡니다.
    /// </summary>
    private int nowDepartment;

    private void Update()
    {
        ShowDepartmentStatus();
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
    }

    public void HireWorker(int type)
    {
        GameManager.instance.department[nowDepartment].worker[type] += 1;
    }

    public void FireWorker(int type)
    {
        if(GameManager.instance.department[nowDepartment].worker[type] > 0)
            GameManager.instance.department[nowDepartment].worker[type] -= 1;
    }
}
