using UnityEngine;
using System.Collections;

public class UIPanelRoot : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.PushPanel(UIPanelInfo.MainMenu);
    }
}
