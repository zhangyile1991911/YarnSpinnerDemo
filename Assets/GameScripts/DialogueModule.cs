using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class DialogueModule : MonoBehaviour
{
    public DialogueRunner runner;

    public Button DialogueStartBtn;

    [Range(0,100f)]
    public float like;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        DialogueStartBtn.onClick.AddListener(StartDialogue);
        runner.VariableStorage.SetValue("$like",like);
        runner.AddCommandHandler<RectTransform>("LineShake",LineShake);
    }

    void LineShake(RectTransform rectTransform)
    {
        
    }

    void StartDialogue()
    {
        runner.StartDialogue("Beginner");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
