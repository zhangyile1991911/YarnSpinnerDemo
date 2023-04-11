using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorUISceneTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenUI()
    {
        UIManager.Instance.OpenUI(UIEnum.LoginWindow,null,null);
    }

    public void CloseUI()
    {
        UIManager.Instance.CloseUI(UIEnum.LoginWindow);
    }

    public void DestroyUI()
    {
        
    }
}
