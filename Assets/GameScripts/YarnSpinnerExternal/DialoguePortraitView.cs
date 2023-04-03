using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Yarn.Unity;
using UnityEngine.UI;
using Yarn.Markup;

public class DialoguePortraitView : DialogueViewBase
{
    [SerializeField]
    public Image CharacterPortrait;

    // private AsyncOperationHandle loadSpriteHandle;
    // private CancellationToken cts;
    
    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        HandleMarkup(dialogueLine.Text.Attributes,onDialogueLineFinished);
    }
    
    private void HandleMarkup(List<MarkupAttribute> attributes,Action onDialogueLineFinish)
    {
        foreach (var markupAttribute in attributes)
        {
            if(markupAttribute.Name != "character")
            {
                continue;
            }

            var properties = markupAttribute.Properties;
            if (!properties.ContainsKey("name"))
            {
                continue;
            }
            
            var characterName = properties["name"].StringValue;
            var spName = string.Concat("Assets/Art/Character/Portrait/",characterName, "Portrait.png");
            var loadSpriteHandle = Addressables.LoadAssetAsync<Sprite>(spName);
            loadSpriteHandle.Completed += sp =>
            {
                CharacterPortrait.sprite = sp.Result;
                CharacterPortrait.gameObject.SetActive(true);
                onDialogueLineFinish();
            };
        }
    }

    // public override void DismissLine(Action onDismissalComplete)
    // {
    //     CharacterPortrait.sprite = null;
    //     CharacterPortrait.gameObject.SetActive(false);
    //     onDismissalComplete();
    // }
    
    // public override void InterruptLine(LocalizedLine dialogueLine, Action onDialogueLineFinish)
    // {
    //     CharacterPortrait.sprite = null;
    //     CharacterPortrait.gameObject.SetActive(false);
    //     onDialogueLineFinish();
    // }
}
