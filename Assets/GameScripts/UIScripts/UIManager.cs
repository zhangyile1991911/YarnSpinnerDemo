using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.PlayerLoop;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance => _instance;
    private static UIManager _instance;

    private LRUCache<UIEnum, IUIBase> _uiCachedDic;

    private Transform _bottom;

    private Transform _center;

    private Transform _top;

    private Transform _guide;
    // private Dictionary<UIEnum, IUIBase> uiCachedDic;
    // private Stack<IUIBase> uiStack;
    // private List<IUIBase> uiList;
    
    private void Awake()
    {
        _uiCachedDic = new LRUCache<UIEnum, IUIBase>(10);
        _uiCachedDic.OnRemove += (ui) =>
        {
            ui.OnHide();
            ui.OnDestroy();
        };
        // uiStack = new Stack<IUIBase>();
        // uiList = new List<IUIBase>();
        _bottom = transform.Find("Bottom");
        _center = transform.Find("Center");
        _top = transform.Find("Top");
        _guide = transform.Find("Guide");
        
        _instance = this;
    }

    public IUIBase Get(UIEnum uiName)
    {
        IUIBase ui = null;
        if (!_uiCachedDic.TryGetValue(uiName, out ui))
        {
            return null;
        }

        return ui;
    }

    private void OnOpenUI(IUIBase ui,Action<IUIBase> onComplete,UIOpenParam openParam,UILayer layer)
    {
        ui.OnShow(openParam);
        onComplete?.Invoke(ui);
    }
    
    public void OpenUI(UIEnum uiName,Action<IUIBase> onComplete,UIOpenParam openParam,UILayer layer = UILayer.Bottom)
    {
        IUIBase ui = null;
        if (_uiCachedDic.TryGetValue(uiName, out ui))
        {
            OnOpenUI(ui, onComplete, openParam,layer);
        }
        else
        {
            LoadUI(uiName,(loadUi)=>
            {
                loadUi.OnCreate();
                OnOpenUI(loadUi,onComplete,openParam,layer);
            },layer);
        }
    }

    public void CloseUI(UIEnum uiName)
    {
        IUIBase ui = null;
        if (_uiCachedDic.TryGetValue(uiName, out ui))
        {
            ui.OnHide();
        }
    }

    private Transform getParentNode(UILayer layer)
    {
        switch (layer)
        {
            case UILayer.Bottom:
                return _bottom;
            case UILayer.Center:
                return _center;
            case UILayer.Top:
                return _top;
            case UILayer.Guide:
                return _guide;
        }

        return null;
    }
    private void LoadUI(UIEnum uiName, Action<IUIBase> onComplete,UILayer layer)
    {
        Type uiType = Type.GetType(uiName.ToString());
        var attributes = uiType.GetCustomAttributes(false);
        var uiPath = attributes
            .Where(one => one is UIAttribute)
            .Select(tmp=> (tmp as UIAttribute).ResPath).FirstOrDefault();
        
        var handle = Addressables.LoadAssetAsync<GameObject>(uiPath);
        handle.Completed += (result) =>
        {
            IUIBase ui = Activator.CreateInstance(uiType) as IUIBase;
            var uiPrefab = result.Result;
            
            var parentNode = getParentNode(layer);
            var uiGameObject = GameObject.Instantiate(uiPrefab,parentNode);
            // uiGameObject.transform.position = Vector3.zero;
            // uiGameObject.transform.localScale = Vector3.one;
            uiGameObject.transform.SetParent(parentNode,false);
            ui.Init(uiGameObject);
            onComplete?.Invoke(ui);
            _uiCachedDic.Add(uiName,ui);
        };
    }
}
