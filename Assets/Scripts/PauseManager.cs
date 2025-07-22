using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    
    [Header("UI References")]
    public UIDocument pauseUIDocument;
    
    [Header("Scene Management")]
    public string mainMenuSceneName = "MainMenu";
    
    private bool isPaused = false;
    
    // UI Elements
    private VisualElement pauseContainer;
    private VisualElement pausePanel;
    private VisualElement pauseControlsPanel;
    
    // Buttons
    private Button resumeButton;
    private Button pauseControlsButton;
    private Button mainMenuButton;
    private Button pauseControlsBackButton;

    void Start()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        SetupPauseUI();
    }

    void Update()
    {
        // Check for ESC key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    
    void SetupPauseUI()
    {
        if (pauseUIDocument == null)
        {
            Debug.LogError("Pause UI Document not assigned!");
            return;
        }
        
        var root = pauseUIDocument.rootVisualElement;
        
        // Get UI element references
        pauseContainer = root.Q<VisualElement>("PauseContainer");
        pausePanel = root.Q<VisualElement>("PausePanel");
        pauseControlsPanel = root.Q<VisualElement>("PauseControlsPanel");
        
        // Get button references
        resumeButton = root.Q<Button>("ResumeButton");
        pauseControlsButton = root.Q<Button>("PauseControlsButton");
        mainMenuButton = root.Q<Button>("MainMenuButton");
        pauseControlsBackButton = root.Q<Button>("PauseControlsBackButton");
        
        // Verify elements were found
        if (pauseContainer == null) Debug.LogError("PauseContainer not found in UXML!");
        if (resumeButton == null) Debug.LogError("ResumeButton not found in UXML!");
        
        // Initially hide pause menu
        if (pauseContainer != null)
            pauseContainer.style.display = DisplayStyle.None;
            
        // Bind button events
        BindPauseEvents();
    }
    
    void BindPauseEvents()
    {
        if (resumeButton != null)
            resumeButton.clicked += ResumeGame;
            
        if (pauseControlsButton != null)
            pauseControlsButton.clicked += ShowPauseControls;
            
        if (mainMenuButton != null)
            mainMenuButton.clicked += ReturnToMainMenu;
            
        if (pauseControlsBackButton != null)
            pauseControlsBackButton.clicked += HidePauseControls;
    }
    
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Stop game time
        
        if (pauseContainer != null)
            pauseContainer.style.display = DisplayStyle.Flex;
            
        if (pausePanel != null)
            pausePanel.style.display = DisplayStyle.Flex;
            
        if (pauseControlsPanel != null)
            pauseControlsPanel.style.display = DisplayStyle.None;
        
        Debug.Log("Game Paused");
    }
    
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume game time
        
        if (pauseContainer != null)
            pauseContainer.style.display = DisplayStyle.None;
        
        Debug.Log("Game Resumed");
    }
    
    void ShowPauseControls()
    {
        if (pausePanel != null)
            pausePanel.style.display = DisplayStyle.None;
            
        if (pauseControlsPanel != null)
            pauseControlsPanel.style.display = DisplayStyle.Flex;
    }
    
    void HidePauseControls()
    {
        if (pauseControlsPanel != null)
            pauseControlsPanel.style.display = DisplayStyle.None;
            
        if (pausePanel != null)
            pausePanel.style.display = DisplayStyle.Flex;
    }
    
    void ReturnToMainMenu()
    {
        // Resume time before changing scenes
        Time.timeScale = 1f;
        isPaused = false;
        
        Debug.Log("Returning to Main Menu...");
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    // Public method to check if game is paused (useful for other scripts)
    public static bool IsGamePaused()
    {
        return Instance != null && Instance.isPaused;
    }
    
    void OnDestroy()
    {
        // Clean up event subscriptions
        UnbindPauseEvents();
        
        // Ensure time scale is reset if this object is destroyed
        Time.timeScale = 1f;
    }
    
    void UnbindPauseEvents()
    {
        if (resumeButton != null)
            resumeButton.clicked -= ResumeGame;
        if (pauseControlsButton != null)
            pauseControlsButton.clicked -= ShowPauseControls;
        if (mainMenuButton != null)
            mainMenuButton.clicked -= ReturnToMainMenu;
        if (pauseControlsBackButton != null)
            pauseControlsBackButton.clicked -= HidePauseControls;
    }
}
