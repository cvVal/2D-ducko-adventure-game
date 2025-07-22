using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private VisualElement m_HealthBar;

    // NPC Dialogue
    public float displayTime = 6.0f;
    private VisualElement m_NonPlayerDialogue;
    private float m_TimerDisplay;
    private Label m_NPCDialogueText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize health bar element
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_HealthBar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        SetHealthValue(1.0f); // Initialize health value

        // Initialize NPC dialogue element
        m_NonPlayerDialogue = uiDocument.rootVisualElement.Q<VisualElement>("NPCDialogue");
        m_NonPlayerDialogue.style.display = DisplayStyle.None;
        m_TimerDisplay = -1.0f;

        m_NPCDialogueText = m_NonPlayerDialogue.Q<Label>("DialogueText");
    }

    void Update()
    {
        if (m_TimerDisplay > 0f)
        {
            m_TimerDisplay -= Time.deltaTime;
            if (m_TimerDisplay < 0f)
            {
                m_NonPlayerDialogue.style.display = DisplayStyle.None; // Hide dialogue after time expires
            }
        }
    }

    public void SetHealthValue(float percentage)
    {
        m_HealthBar.style.width = Length.Percent(percentage * 100);
    }

    public void DisplayNPCDialogue(NPC npc)
    {
        if (npc != null)
        {
            m_NPCDialogueText.text = npc.dialogueText; // Set the specific NPC's dialogue text
            m_NonPlayerDialogue.style.display = DisplayStyle.Flex;
            m_TimerDisplay = displayTime;
        }
        else
        {
            Debug.LogWarning("No NPC provided to DisplayNPCDialogue!");
        }
    }
}
