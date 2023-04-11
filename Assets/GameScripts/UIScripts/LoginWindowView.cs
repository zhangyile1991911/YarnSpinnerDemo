﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Auto Generate Class!!!
/// </summary>
[UI((int)UIEnum.LoginWindow,"Assets/GameRes/Prefabs/LoginWindow.prefab")]
public partial class LoginWindow : UIWindow
{
	public Image Img_bg;
	public InputField Input_account;
	public InputField Input_passwd;
	public Button Btn_login;

	public override void Init(GameObject go)
	{
	    uiGo = go;
	    
		Img_bg = go.transform.Find("Img_bg").GetComponent<Image>();
		Input_account = go.transform.Find("Input_account").GetComponent<InputField>();
		Input_passwd = go.transform.Find("Input_passwd").GetComponent<InputField>();
		Btn_login = go.transform.Find("Btn_login").GetComponent<Button>();

	}
}