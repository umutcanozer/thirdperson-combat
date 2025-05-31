using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;
        public bool isDialogueActive = false;

        [SerializeField] private GameObject _dialogueCanvas;
        [SerializeField] private GameObject _inputManager;


        [SerializeField] private TextMeshProUGUI characterName;
        [SerializeField] private TextMeshProUGUI dialogueArea;


        [SerializeField] private bool isDialogueEnd = false;
        [SerializeField] private float typingSpeed = 0.2f;

        private Queue<DialogueLine> _lines = new Queue<DialogueLine>();
        private System.Action onDialogueCompleteCallback;
        public bool IsDialogueEnd => isDialogueEnd;
        private void Start()
        {
            if (Instance == null) Instance = this;
            _dialogueCanvas.SetActive(false);
        }
        public void StartDialogue(Dialogue dialogue, System.Action onComplete)
        {
            onDialogueCompleteCallback = onComplete;
            
            isDialogueActive = true;
            _dialogueCanvas.SetActive(true);
            _inputManager.SetActive(false);
            _lines.Clear();

            foreach(DialogueLine dialogueLine in dialogue.dialogueLines)
            {
                _lines.Enqueue(dialogueLine);
            }

            DisplayNextDialogueLine();
        }

        public void DisplayNextDialogueLine()
        {
            if(_lines.Count == 0)
            {
                EndDialogue();
                return;
            }

            DialogueLine currentLine = _lines.Dequeue();

            characterName.text = currentLine.character.name;
            dialogueArea.text = currentLine.line;

        }

        
        private void EndDialogue()
        {
            isDialogueActive = false;
            isDialogueEnd = true;
            _dialogueCanvas.SetActive(false);
            _inputManager.SetActive(true);
            
            onDialogueCompleteCallback?.Invoke();
            onDialogueCompleteCallback = null;
        }
    }
