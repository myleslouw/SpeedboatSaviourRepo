using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TextMeshProUGUI currentSpeaker;
    [SerializeField] TextMeshProUGUI currentText;
    bool dialogueActive = false;

    private string playerName = "Player";

    int dialogueIndex;      //the index in the array of sentences for a conversation  (0- NPC speaking, 1 - Player speaking, 2 - NPC speaking etc)

    NPC NPCinRange;         //the current NPC the player is talking to 

    int numSentences;   //the number of sentences in the dialogue piece


    // Start is called before the first frame update
    void Start()
    {

        dialogueBox.SetActive(false);
        EventManager.OnDelegateEvent StartDialogueDelegate = StartDialogue;
        EventManager.OnDelegateEvent EndDialogeDelegate = EndDialogue;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.NPC_TALK, StartDialogueDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.NPC_LEAVE, EndDialogeDelegate);
    }

    // Update is called once per frame
    void Update()
    {
        if (NPCinRange != null)
        {
            //if there is an NPC in range

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                dialogueIndex++;

                if (numSentences == dialogueIndex)
                {
                    //DIALOGUE FINISHED
                    //if all the sentences have been said
                    if (NPCinRange.dialogue.rewardAfter)
                    {
                        //if there is a reward from completing the dialogue
                        //rewards will be in a dictionary and the a key will be used from the OBj to the dictionary
                        Destroy(NPCinRange.gameObject);
                        EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.UPGRADE_BOAT, this, null);
                    }

                    //stop the dialogue and undo 
                    EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.NPC_LEAVE, this, null);
                }
                else
                {
                    //say the dialogue
                    NextSentence(dialogueIndex);
                }
            }
        }
    }

    private void StartDialogue(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //shows dialogue box
        dialogueActive = true;
        dialogueBox.SetActive(true);

        //starts the dialogue from zero
        dialogueIndex = 0;
        //gets NPC details from the Event
        NPCinRange = (NPC)Params;

        //gets the number of sentences in the dialogue piece
        numSentences = NPCinRange.dialogue.sentences.Length;

        //NPC ALWAYS Speaks first
        SaySentence(NPCinRange.npcName, NPCinRange.dialogue.sentences[0]);
    }

    private void EndDialogue(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //Hide dialogue box
        dialogueActive = false;
        dialogueBox.SetActive(false);

        currentSpeaker.text = null;
        currentText.text = null;

        dialogueIndex = 0;
    }

    private void SaySentence(string speakerName, string sentence)
    {
        //prints a sentence and the speaker name
        currentSpeaker.text = speakerName;
        currentText.text = sentence;
    }

    private void NextSentence(int currentIndex)
    {
        if (NPCinRange != null)
        {
            //if the index is odd, the player must speak
            string speaker = currentIndex % 2 == 0 ? NPCinRange.npcName : playerName;
            SaySentence(speaker, NPCinRange.dialogue.sentences[currentIndex]);
        }
    }
}
