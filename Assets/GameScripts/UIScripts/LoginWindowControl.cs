using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Auto Generate Class!!!
/// </summary>
public partial class LoginWindow : UIWindow
{
    public override UILayer GetLayer()
    {
        return UILayer.Bottom;
    }

    public override void OnCreate()
    {
        Debug.Log("LoginWindow::OnCreate");
    }
    
    public override void OnDestroy()
    {
        Debug.Log("LoginWindow::OnDestroy");
    }
    
    public override void OnShow(UIOpenParam openParam)
    {
        base.OnShow(openParam);
        Debug.Log("LoginWindow::OnShow");
    }

    public override void OnHide()
    {
        base.OnHide();
        Debug.Log("LoginWindow::OnHide");
    }

    public override void OnUpdate()
    {
        Debug.Log("LoginWindow::OnUpdate");
    }
}