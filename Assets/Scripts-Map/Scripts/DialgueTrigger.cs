using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialgueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    SphereCollider sphereCollider;
    
    private bool isDialogueComplete = false; 

    public void TriggerDialogue()
    {
        if (!isDialogueComplete) 
        {
            DialogueManager.Instance.StartDialogue(dialogue, OnDialogueComplete);
        }
    }

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    private void Update()
    {
        if (isDialogueComplete)
        {
            sphereCollider.enabled = false; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerDialogue();
    }
    
    private void OnDialogueComplete()
    {
        isDialogueComplete = true;
    }
}
