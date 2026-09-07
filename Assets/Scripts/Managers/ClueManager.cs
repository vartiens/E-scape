using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueManager : MonoBehaviour
{
    string[] clueContent = new string[10];

    public void Init()
    {
        for (int i = 0; i < clueContent.Length; i++)
        {
            clueContent[i] = "Clue " + (i + 1);
        }
    }
    void Start()
    {
        for (int i = 0; i < clueContent.Length; i++)
        {
            clueContent[i] = "Clue " + (i + 1);
        }

        for (int i = 0; i < clueContent.Length; i++)
        {
            Debug.Log(clueContent[i]);
        }
    }

    void Update()
    {
        
    }

    public string GetClue(int idx)
    {
        if (clueContent[idx] == null)
            Init();
        return clueContent[idx];
    }

}
