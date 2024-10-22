using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI interactionText;
    public float dialogueDuration = 3f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowDialogue(string message)
    {
        dialogueText.text = message;
        StopAllCoroutines();
        StartCoroutine(HideDialogueAfterTime());
    }

    private IEnumerator HideDialogueAfterTime()
    {
        yield return new WaitForSeconds(dialogueDuration);
        dialogueText.text = "";
    }

    public void ShowInteractionText(string message)
    {
        interactionText.text = message;
    }

    public void ClearInteractionText()
    {
        interactionText.text = "";
    }
}
