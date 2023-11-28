using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabSelector : MonoBehaviour
{
    [SerializeField] private List<GameObject> tabs;

    public void ActivateTab(int index)
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            tabs[i].SetActive(i==index);
        }
    }
}
