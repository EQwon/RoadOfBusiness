using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class level : MonoBehaviour
{
    int levela = 1;
    public void Level1()
    {
        levela += 1;
        GetComponent<Text>().text = levela.ToString();
    }
    public void Level2()
    {
        levela += 2;
        GetComponent<Text>().text = levela.ToString();
    }
    public void Level3()
    {
        levela += 3;
        GetComponent<Text>().text = levela.ToString();
    }
    public void Level4()
    {
        levela += 4;
        GetComponent<Text>().text = levela.ToString();
    }
    
}