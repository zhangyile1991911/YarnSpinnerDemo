using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Yarn.Unity;

public class DialogueModule : MonoBehaviour
{
    
    public DialogueRunner runner;

    public Button DialogueStartBtn;

    public List<Transform> CharacterNodeList;

    public Dictionary<string, GameObject> peoples;
    [Range(0,100f)]
    public float like;

    private static DialogueModule _module;
    public static DialogueModule Instance => _module;
    private void Awake()
    {
        _module = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        DialogueStartBtn.onClick.AddListener(StartDialogue);
        runner.VariableStorage.SetValue("$like",like);
        // runner.AddCommandHandler<RectTransform>("LineShake",LineShake);
    }

    // void LineShake(RectTransform rectTransform)
    // {
    //     
    // }

    void StartDialogue()
    {
        runner.StartDialogue("Beginner");
    }

    Transform findNode(string name)
    {
        foreach (var one in CharacterNodeList)
        {
            if (one.name.Equals(name))
            {
                return one;
            }
        }

        return null;
    }

    #region 自定义Command
    //加载Actor是异步的 可能图片还没加载完 执行了后面的命令
    //所以加一个计数器 等到人物都加载完后 在执行后面的演出命令
    //暂时还没想到更好的解决方式
    private static uint LoadActorCounter = 0;
    [YarnCommand("LoadActor")]
    public static async void LoadActor(string nodeName,string characterName,bool show)
    {
        var trans = Instance.findNode(nodeName);
        if (trans == null)
        {
            Debug.LogError($"找不到{nodeName}节点");
            return;
        }

        LoadActorCounter++;
        var spName = string.Concat("Assets/Art/Character/Picture/", characterName,".png");
        var characterSp = await Addressables.LoadAssetAsync<Sprite>(spName).ToUniTask();
        LoadActorCounter--;
        
        var spriteRenderer = trans.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = characterSp;
        trans.gameObject.SetActive(show);
        Debug.Log($"LoadActor {show}");
    }

    [YarnCommand("ActorFadeIn")]
    public static async void ActorFadeIn(string actorName,float x,float y,float duration)
    {
        await UniTask.WaitUntil(()=>LoadActorCounter == 0);
        var actor = Instance.findNode(actorName);
        actor.transform.position = new Vector3(x, y,0);
        var spriteRenderer = actor.GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1,1,1,0);
        spriteRenderer.DOFade(1, duration);
        actor.gameObject.SetActive(true);
        Debug.Log($"ActorFadeIn");
    }
    
    [YarnCommand("CharacterFadeOut")]
    public static void ActorFadeOut(string actorName,float x,float y,float duration)
    {
        var actor = Instance.findNode(actorName);
        actor.transform.position = new Vector3(x, y,0);
        var spriteRenderer = actor.GetComponent<SpriteRenderer>();
        spriteRenderer.DOFade(0, duration);
    }
    
    #endregion
    
    // [YarnCommand("CharacterMoveIn")]
    // public static void CharacterMoveInFrom(RectTransform character,string direction,float duration)
    // {
    //     switch (direction)
    //     {
    //         case "left":
    //             break;
    //         case "right":
    //             break;
    //         case "up":
    //             break;
    //         case "bottom":
    //             break;
    //     }
    // }
    
    // GameObject findPeople(string CharacterName)
    // {
    //     if (peoples.ContainsKey(CharacterName))
    //     {
    //         return peoples[CharacterName];
    //     }
    //
    //     return null;
    // }
}
