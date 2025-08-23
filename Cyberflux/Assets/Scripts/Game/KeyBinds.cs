using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputFieldHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //string tempText;

    public Button JumpKeyInput;
    bool JumpInputReady = false;
    public Button CrouchKeyInput;
    bool CrouchInputReady = false;
    public Button SprintKeyInput;
    bool SprintInputReady = false;
    public Button MeleeKeyInput;
    bool MeleeInputReady = false;
    public Button SlashKeyInput;
    bool SlashInputReady = false;
    public Button SlamKeyInput;
    bool SlamInputReady = false;
    public Button SpecialKeyInput;
    bool SpecialInputReady = false;
    
    void Start()
    {
        if (GameManager.instance.playerScript != null)
        {
            
            JumpKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.jumpKey.ToString();
            CrouchKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.crouchKey.ToString();
            SprintKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.sprintKey.ToString();
            MeleeKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.scytheScript.attackKey.ToString();
            SlashKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.scytheScript.slashKey.ToString();
            SlamKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.scytheScript.slamAttackKey.ToString();
            SpecialKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.scytheScript.specialAttackKey.ToString();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (JumpInputReady && Input.anyKeyDown)
        {
            JumpChange();
        }
        else if (CrouchInputReady && Input.anyKeyDown)
        {
            CrouchChange();
        }
        else if (SprintInputReady && Input.anyKeyDown)
        {
            SprintChange();
        }
        else if (MeleeInputReady && Input.anyKeyDown)
        {
            MeleeChange();
        }
        else if (SlamInputReady && Input.anyKeyDown)
        {
            SlamChange();
        }
        else if (SpecialInputReady && Input.anyKeyDown)
        {
            SpecialChange();
        }
        else if (SlashInputReady && Input.anyKeyDown)
        {
            SlashChange();
        }
    }

    bool CheckDuplicateKey(KeyCode key, string str)
    {
        if (key == KeyCode.Escape)
        {
            return true;
        }
        if (str == "Jump")
        {
            if (key == GameManager.instance.playerScript.sprintKey || key == GameManager.instance.playerScript.crouchKey)
            {
                return true;
            }
        }
        else if (str == "Crouch") {
            if (key == GameManager.instance.playerScript.sprintKey || key == GameManager.instance.playerScript.jumpKey)
            {
                return true;
            }
        }
        else if (str == "Sprint")
        {
            if (key == GameManager.instance.playerScript.jumpKey || key == GameManager.instance.playerScript.crouchKey) {
                return true;
            }
        }
        return false;
    }

    public void SetJumpKey ()
    {
       JumpInputReady = true;
       // tempText = JumpKeyInput.GetComponentInChildren<TMP_Text>().text;
        JumpKeyInput.GetComponentInChildren<TMP_Text>().text = "";
        GameManager.instance.UIAudioSource.Play();
    }
    void JumpChange()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                if (CheckDuplicateKey(vKey, "Jump"))
                {
                    JumpKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.jumpKey.ToString();
                    JumpInputReady = false;
                    break;
                }
                else
                {
                    GameManager.instance.playerScript.jumpKey = vKey;
                    JumpKeyInput.GetComponentInChildren<TMP_Text>().text = vKey.ToString();
                    JumpInputReady = false;
                    break;
                }
            }
        }
        PlayerPrefs.SetString("Jump Key", GameManager.instance.playerScript.jumpKey.ToString());
        
    }
    public void SetCrouchKey ()
    {
        CrouchInputReady = true;
       // tempText = JumpKeyInput.GetComponentInChildren<TMP_Text>().text;
        CrouchKeyInput.GetComponentInChildren<TMP_Text>().text = "";
        GameManager.instance.UIAudioSource.Play();
    }
    void CrouchChange()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                if (CheckDuplicateKey(vKey, "Crouch"))
                {
                    CrouchKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.crouchKey.ToString();
                    CrouchInputReady = false;
                    break;
                }
                else
                {
                    GameManager.instance.playerScript.crouchKey = vKey;
                    CrouchKeyInput.GetComponentInChildren<TMP_Text>().text = vKey.ToString();
                    CrouchInputReady = false;
                    break;
                }
            }
        }
      
        PlayerPrefs.SetString("Crouch Key", GameManager.instance.playerScript.crouchKey.ToString());
        
    }
    public void SetSprintKey ()
    {
        SprintInputReady = true;
        // tempText = JumpKeyInput.GetComponentInChildren<TMP_Text>().text;
        SprintKeyInput.GetComponentInChildren<TMP_Text>().text = "";
        GameManager.instance.UIAudioSource.Play();
    }
    void SprintChange()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                if (CheckDuplicateKey(vKey, "Sprint"))
                {
                    SprintKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.sprintKey.ToString();
                    SprintInputReady = false;
                    break;
                }
                else
                {
                    GameManager.instance.playerScript.sprintKey = vKey;
                    SprintKeyInput.GetComponentInChildren<TMP_Text>().text = vKey.ToString();
                    SprintInputReady = false;
                    break;
                }
            }
            
        }
        PlayerPrefs.SetString("Sprint Key", GameManager.instance.playerScript.sprintKey.ToString());
    }
        
        
        

    public void SetSlamKey()
    {
        SlamInputReady = true;
        // tempText = JumpKeyInput.GetComponentInChildren<TMP_Text>().text;
        SlamKeyInput.GetComponentInChildren<TMP_Text>().text = "";
        GameManager.instance.UIAudioSource.Play();
    }
    void SlamChange()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                if (CheckDuplicateKey(vKey, ""))
                {
                    SlamKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.scytheScript.slamAttackKey.ToString();
                    SlamInputReady = false;
                    break;
                }
                else
                {
                    GameManager.instance.playerScript.scytheScript.slamAttackKey = vKey;
                    SlamKeyInput.GetComponentInChildren<TMP_Text>().text = vKey.ToString();
                    SlamInputReady = false;
                    break;
                }
            }
        }
      
        PlayerPrefs.SetString("Slam Key", GameManager.instance.playerScript.scytheScript.slamAttackKey.ToString());
        
    }
    public void SetMeleeKey()
    {
        MeleeInputReady = true;
        // tempText = JumpKeyInput.GetComponentInChildren<TMP_Text>().text;
        MeleeKeyInput.GetComponentInChildren<TMP_Text>().text = "";
        GameManager.instance.UIAudioSource.Play();
    }
    void MeleeChange()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                if (CheckDuplicateKey(vKey, "Sprint"))
                {
                    MeleeKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.scytheScript.attackKey.ToString();
                    MeleeInputReady = false;
                    break;
                }
                else
                {
                    GameManager.instance.playerScript.scytheScript.attackKey = vKey;
                    MeleeKeyInput.GetComponentInChildren<TMP_Text>().text = vKey.ToString();
                    MeleeInputReady = false;
                    break;
                }
            }
        }
     
        PlayerPrefs.SetString("Attack Key", GameManager.instance.playerScript.scytheScript.attackKey.ToString());
        
    }
    public void SetSlashKey()
    {
        SlashInputReady = true;
        // tempText = JumpKeyInput.GetComponentInChildren<TMP_Text>().text;
        SlashKeyInput.GetComponentInChildren<TMP_Text>().text = "";
        GameManager.instance.UIAudioSource.Play();
    }
    void SlashChange()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                if (CheckDuplicateKey(vKey, ""))
                {
                    SlashKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.scytheScript.slashKey.ToString();
                    SlashInputReady = false;
                    break;
                }
                else
                {
                    GameManager.instance.playerScript.scytheScript.slashKey = vKey;
                    SlashKeyInput.GetComponentInChildren<TMP_Text>().text = vKey.ToString();
                    SlashInputReady = false;
                    break;
                }
            }
        }
       
        PlayerPrefs.SetString("Slash Key", GameManager.instance.playerScript.scytheScript.slashKey.ToString());
        
    }
    public void SetSpecialKey()
    {
        SpecialInputReady = true;
        // tempText = JumpKeyInput.GetComponentInChildren<TMP_Text>().text;
        SpecialKeyInput.GetComponentInChildren<TMP_Text>().text = "";
        GameManager.instance.UIAudioSource.Play();
    }
    void SpecialChange()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                if (CheckDuplicateKey(vKey, ""))
                {
                    SpecialKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.scytheScript.specialAttackKey.ToString();
                    SpecialInputReady = false;
                    break;
                }
                else
                {
                    GameManager.instance.playerScript.scytheScript.specialAttackKey = vKey;
                    SpecialKeyInput.GetComponentInChildren<TMP_Text>().text = vKey.ToString();
                    SpecialInputReady = false;
                    break;
                }
            }
        }
        
        PlayerPrefs.SetString("Special Key", GameManager.instance.playerScript.scytheScript.specialAttackKey.ToString());
    }
}




