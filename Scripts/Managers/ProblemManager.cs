using UnityEngine;
using UnityEngine.SceneManagement;

public class ProblemManager : MonoBehaviour
{
    private Vector3 playerPos;
    [SerializeField] private GameObject helperAI;
    [SerializeField] private Transform jumpDestination;
    [SerializeField] private Transform platformDestination;

    [SerializeField] private GameObject quitMenu;

    [Header("Jump Problem")] 
    [SerializeField] private int maxNumOfJumpFails;
    private int currentJumpFails = 0;
    
    [SerializeField] private GameObject jumpHelpUI;

    private bool bCanShowPrompt = false;
    
    [Header("Moving Platform Problem")]
    [SerializeField] private int maxNumOfPlatformFails;
    private int currentPlatformFails;

    [SerializeField] private float waveDecreaser;
    
    [SerializeField] private GameObject platformHelpUI;

    private void ToggleJumpUI(bool state)
    {
        if (state)
        {
            CustomEvents.Scripts.OnDisableMovement?.Invoke(false);
            CustomEvents.Scripts.OnDisableCamera?.Invoke(false);
            Cursor.lockState = CursorLockMode.None;
            jumpHelpUI.SetActive(true);
        }
        else
        {
            CustomEvents.Scripts.OnDisableCamera?.Invoke(true);
            CustomEvents.Scripts.OnDisableMovement?.Invoke(true);
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }
    
    private void TogglePlatformUI(bool state)
    {
        if (state)
        {
            CustomEvents.Scripts.OnDisableMovement?.Invoke(false);
            CustomEvents.Scripts.OnDisableCamera?.Invoke(false);
            Cursor.lockState = CursorLockMode.None;
            platformHelpUI.SetActive(true);
        }
        else
        {
            CustomEvents.Scripts.OnDisableCamera?.Invoke(true);
            Cursor.lockState = CursorLockMode.Locked;
            platformHelpUI.SetActive(false);
        }
        
    }

    private void UpdateJumpFails()
    {
        if (currentJumpFails < maxNumOfJumpFails)
        {
            currentJumpFails++;
        }
        else
        {
            ToggleJumpUI(true);
            currentJumpFails = 0;
        }
    }
    
    private void UpdatePlatformFails()
    {
        if (currentPlatformFails < maxNumOfPlatformFails)
        {
            currentPlatformFails++;
        }
        else
        {
            TogglePlatformUI(true);
            currentPlatformFails = 0;
        }
    }

    public void SpawnHelperAI()
    {
        playerPos = (Vector3)CustomEvents.Player.OnGetPlayerPos?.Invoke();
        GameObject temp = Instantiate(helperAI, playerPos, Quaternion.identity);
        AIHelper script = temp.GetComponent<AIHelper>();
        
        switch (CustomEvents.Player.OnGetProblemType?.Invoke())
        {
            case ProblemType.None:
                script.SetDestination(Vector3.zero);
                break;
            case ProblemType.Jumping:
                script.SetDestination(jumpDestination.position);
                break;
            case ProblemType.Platforms:
                script.SetDestination(platformDestination.position);
                break;
            default:
                Debug.LogWarning("Unhandled problem type");
                break;
        }
        CustomEvents.Scripts.OnDisableMovement?.Invoke(false);
    }

    public void SetAllowPrompts(bool state)
    {
        bCanShowPrompt = state;
        CustomEvents.Scripts.OnDisableMovement?.Invoke(true);
        CustomEvents.Scripts.OnDisableCamera?.Invoke(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private bool GetPromptState()
    {
        return bCanShowPrompt;
    }

    public void ChangePlatformSpeeds()
    {
        CustomEvents.Problems.Platforms.OnChangeWaveStrength?.Invoke(waveDecreaser);
    }

    private void ToggleQuitMenu(bool state)
    {
        if (state)
        {
            CustomEvents.Scripts.OnDisableMovement?.Invoke(false);
            CustomEvents.Scripts.OnDisableCamera?.Invoke(false);
            Cursor.lockState = CursorLockMode.None;
            quitMenu.SetActive(true);
        }
        else
        {
            CustomEvents.Scripts.OnDisableCamera?.Invoke(true);
            CustomEvents.Scripts.OnDisableMovement?.Invoke(true);
            Cursor.lockState = CursorLockMode.Locked;
            quitMenu.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void CloseMenu()
    {
        ToggleQuitMenu(false);
    }
    private void OnEnable()
    {
        CustomEvents.Problems.Jumping.OnAddJumpFail += UpdateJumpFails;
        CustomEvents.Problems.Jumping.OnToggleUI += ToggleJumpUI;
        CustomEvents.Problems.Platforms.OnAddPlatformFail += UpdatePlatformFails;
        CustomEvents.Problems.Platforms.OnToggleUI += TogglePlatformUI;
        CustomEvents.Problems.OnAllowPrompts += SetAllowPrompts;
        CustomEvents.Problems.OnGetPrompt += GetPromptState;
        CustomEvents.Options.OnToggleQuitMenu += ToggleQuitMenu;
    }

    private void OnDisable()
    {
        CustomEvents.Problems.Jumping.OnAddJumpFail -= UpdateJumpFails;
        CustomEvents.Problems.Jumping.OnToggleUI -= ToggleJumpUI;
        CustomEvents.Problems.Platforms.OnAddPlatformFail -= UpdatePlatformFails;
        CustomEvents.Problems.Platforms.OnToggleUI -= TogglePlatformUI;
        CustomEvents.Problems.OnAllowPrompts -= SetAllowPrompts;
        CustomEvents.Problems.OnGetPrompt -= GetPromptState;
        CustomEvents.Options.OnToggleQuitMenu -= ToggleQuitMenu;
    }
}
