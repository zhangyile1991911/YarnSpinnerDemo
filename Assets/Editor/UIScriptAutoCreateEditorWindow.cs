using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.DemiEditor;
using UnityEditor;
using UnityEngine;

public class UIScriptAutoCreateEditorWindow : EditorWindow
{
    private string newUIName;
    private GameObject uiRootGo;
    
    [MenuItem("自定义工具/UI绑定生成工具", false, 10)]
    static void ShowEditor()
    {
        UIScriptAutoCreateEditorWindow window = GetWindow<UIScriptAutoCreateEditorWindow>();
        window.minSize = new Vector2(400, 250);
        window.maxSize = new Vector2(400, 250);
        window.titleContent.text = "UI绑定生成工具";

        // var readMe = AssetDatabase.LoadAssetAtPath<TextAsset>("UIAutoCreatePathSetting.ReadMeFilePath");
        // readMeText = readMe.text;
    }

    private void OnGUI()
    {
        #region GUIStyle设置
        Color fontColor = Color.white;
        GUIStyle titleStyle = new GUIStyle() { fontSize = 18, alignment = TextAnchor.MiddleCenter };
        titleStyle.normal.textColor = fontColor;

        GUIStyle sonTittleStyle = new GUIStyle() { fontSize = 15, alignment = TextAnchor.MiddleCenter };
        sonTittleStyle.normal.textColor = fontColor;

        GUIStyle leftStyle = new GUIStyle() { fontSize = 15, alignment = TextAnchor.MiddleLeft };
        leftStyle.normal.textColor = fontColor;

        GUIStyle littoleStyle = new GUIStyle() { fontSize = 13, alignment = TextAnchor.MiddleCenter };
        littoleStyle.normal.textColor = fontColor;
        
        #endregion
        
        GUILayout.BeginArea(new Rect(0,0,400,600));
        GUILayout.BeginVertical();
        
        GUILayout.BeginHorizontal();
        EditorGUILayout.TextArea("注意事项:\n"+ 
                                 "新UI的名字需要以UI作为前缀\n"+
                                 "例如: UITest\n"+
                                 "UI组件命名规范请查看 UIViewAutoCreateConfig 文件", leftStyle, GUILayout.Width(600));
        GUILayout.EndHorizontal();
        
        // GUILayout.Space(50);
        
        // GUILayout.BeginHorizontal();
        // EditorGUILayout.LabelField("Prefab自动生成绑定",titleStyle,GUILayout.Width(600));
        // GUILayout.EndHorizontal();
        // GUILayout.Space(10);
        
        // GUILayout.BeginHorizontal();
        // GUILayout.FlexibleSpace();
        // EditorGUILayout.LabelField("UI类名",leftStyle,GUILayout.Width(50));
        // newUIName = EditorGUILayout.TextField(newUIName, GUILayout.Width(150));
        // GUILayout.FlexibleSpace();
        // GUILayout.EndHorizontal();

        GUILayout.Space(10);
        
        GUI.skin.button.wordWrap = true;

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("代码自动生成设置",leftStyle,GUILayout.Width(600));
        GUILayout.EndHorizontal();
        
        GUILayout.Space(10);
        
        GUILayout.BeginHorizontal();
        // GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Prefab根节点",leftStyle,GUILayout.Width(100));
        uiRootGo = (GameObject)EditorGUILayout.ObjectField(uiRootGo, typeof(GameObject), true);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.Space(10);
        
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("生成", GUILayout.Width(150), GUILayout.Height(30)))
        {
            CreateUIClass();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    private void CreateUIClass()
    {
        if (uiRootGo == null) throw new System.Exception("请拖入需要生成的预制体节点");
        string uiName = GetUIName();

        var config = AssetDatabase.LoadAssetAtPath<UIAutoCreateInfoConfig>("Assets/Editor/UIAutoCreateInfoConfig.asset");
        var targetPath = config.ScriptPath;
        CheckTargetPath(targetPath);
        new UIClassAutoCreate().Create(uiName,uiRootGo,config);
    }

    private string GetUIName()
    {
        string uiName = uiRootGo.name.Replace("UI", "");
        return uiName;
    }
    
    private void CheckTargetPath(string targetPath)
    {
        if (!Directory.Exists(targetPath))
        {
            Directory.CreateDirectory(targetPath);
        }
    }
}
