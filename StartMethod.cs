using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMethod : MonoBehaviour
{
    void Start()
    {
        AdjustTheFrameRate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AdjustTheFrameRate()
    {
        QualitySettings.SetQualityLevel(0);//设定质量等级->预设序列号是游戏里面的预设。
        QualitySettings.vSyncCount = 0;//=>设置同步类型为不同步
        Application.targetFrameRate = 60;//手动设定帧率
    }
}
