using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class day : MonoBehaviour
{
    int daya = 1;
    int month = 1;
    public void Day()
    {
        daya += 1;
        if(daya==31)
        {
            month += 1;
            daya -= 31;
        }
        GetComponent<Text>().text = month.ToString() + "월" + daya.ToString() + "일";
    }
}
