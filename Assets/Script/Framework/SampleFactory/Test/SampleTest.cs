using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Framework;

public class SampleTest : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        Debug.Log("\n初始化-------------------------------------------------");
        SampleFactory.Instance.InitSample<TxtData>("TxtList");

        Debug.Log("\n打印所有工厂数据---------------------------------------------");
        SampleFactory.Instance.DebugAllSample();

        Debug.Log("\n从工厂取出所有TxtData-------------------------------------");
        List<TxtData> txtlist = SampleFactory.Instance.GetSamples<TxtData>();
        foreach (var item in txtlist)
        {
            item.DoDebug();
        }
    }
}
