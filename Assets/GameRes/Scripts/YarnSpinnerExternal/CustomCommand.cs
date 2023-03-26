using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using DG;
using DG.Tweening;

public class CustomCommand
{
    [YarnCommand("LinePunch")]
    public static void LinePunch(GameObject go)
    {
        var rt = go.GetComponent<RectTransform>();
        if (rt != null)
        {
            Debug.Log($"开始播出自定义动画");
            rt.DOPunchPosition(new Vector3(20,20,1),2.5f,20,1f);    
        }
    }
}
