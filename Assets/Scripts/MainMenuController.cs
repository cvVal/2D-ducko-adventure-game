using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Management")]
    public string gameSceneName = "MainScene"; // Your main game scene name
    
    [Header("UI Document")]
    public UIDocument uiDocument;
    
    private Button startButton;
    private Button controlsButton;
    private Button exitButton;
    
    private VisualElement mainMenuPanel;
    private VisualElement controlsPanel;
    private Button backButton;

    void Start()
    {
        // Get UI Document if not assigned
        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();
            
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not found! Please assign it in the inspector or add UIDocument component.");
            return;
        }
        
        SetupUI();
    }
    
    void SetupUI()
    {
        var root = uiDocument.rootVisualElement;
        
        // Get panel references
        mainMenuPanel = root.Q<VisualElement>("MainMenuPanel");
        controlsPanel = root.Q<VisualElement>("ControlsPanel");
        
        // Get button references
        startButton = root.Q<Button>("StartButton");
        controlsButton = root.Q<Button>("ControlsButton");
        exitButton = root.Q<Button>("ExitButton");
        backButton = root.Q<Button>("BackButton");
        
        // Verify all elements were found
        if (startButton == null) Debug.LogError("StartButton not found in UXML!");
        if (controlsButton == null) Debug.LogError("ControlsButton not found in UXML!");
        if (exitButton == null) Debug.LogError("ExitButton not found in UXML!");
        if (controlsPanel == null) Debug.LogError("ControlsPanel not found in UXML!");
        
        // Hide controls panel initially
        if (controlsPanel != null)
            controlsPanel.style.display = DisplayStyle.None;
            
        // Show main menu panel
        if (mainMenuPanel != null)
            mainMenuPanel.style.display = DisplayStyle.Flex;
        
        // Bind button events
        BindEvents();
    }
    
    void BindEvents()
    {
        if (startButton != null)
            startButton.clicked += StartGame;
            
        if (controlsButton != null)
            controlsButton.clicked += ShowControls;
            
        if (exitButton != null)
            exitButton.clicked += ExitGame;
            
        if (backButton != null)
            backButton.clicked += HideControls;
    }

    void StartGame()
    {
        Debug.Log("Starting game...");
        
        // Load the main game scene
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("Game scene name is not set!");
        }
    }

    void ShowControls()
    {
        Debug.Log("Showing controls...");
        
        if (mainMenuPanel != null)
            mainMenuPanel.style.display = DisplayStyle.None;
            
        if (controlsPanel != null)
            controlsPanel.style.display = DisplayStyle.Flex;
    }
    
    void HideControls()
    {
        Debug.Log("Hiding controls...");
        
        if (controlsPanel != null)
            controlsPanel.style.display = DisplayStyle.None;
            
        if (mainMenuPanel != null)
            mainMenuPanel.style.display = DisplayStyle.Flex;
    }

    void ExitGame()
    {
        Debug.Log("Exiting game...");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    void OnDestroy()
    {
        // Clean up event subscriptions
        UnbindEvents();
    }
    
    void UnbindEvents()
    {
        if (startButton != null)
            startButton.clicked -= StartGame;
        if (controlsButton != null)
            controlsButton.clicked -= ShowControls;
        if (exitButton != null)
            exitButton.clicked -= ExitGame;
        if (backButton != null)
            backButton.clicked -= HideControls;
    }
}
