using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputFieldHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

   

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

    private string tempHolder;
    void Start()
    {
        
        
            
            JumpKeyInput.GetComponentInChildren<TMP_Text>().text = PlayerPrefs.GetString("Jump Key", KeyCode.Space.ToString());
            CrouchKeyInput.GetComponentInChildren<TMP_Text>().text = PlayerPrefs.GetString("Crouch Key", KeyCode.C.ToString());
            SprintKeyInput.GetComponentInChildren<TMP_Text>().text = PlayerPrefs.GetString("Sprint Key", KeyCode.LeftShift.ToString());
            MeleeKeyInput.GetComponentInChildren<TMP_Text>().text = PlayerPrefs.GetString("Melee Key", KeyCode.Mouse0.ToString());
            SlashKeyInput.GetComponentInChildren<TMP_Text>().text = PlayerPrefs.GetString("Slash Key", KeyCode.Q.ToString());
            SlamKeyInput.GetComponentInChildren<TMP_Text>().text = PlayerPrefs.GetString("Slam Key", KeyCode.Mouse1.ToString());
            SpecialKeyInput.GetComponentInChildren<TMP_Text>().text = PlayerPrefs.GetString("Special Key", KeyCode.Mouse2.ToString());
        
        
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

    //bool CheckDuplicateKey(KeyCode key, string str)
    //{
    //    if (key == KeyCode.Escape)
    //    {
    //        return true;
    //    }
    //    if (str == "Jump")
    //    {
    //        if (key == GameManager.instance.playerScript.sprintKey || key == GameManager.instance.playerScript.crouchKey)
    //        {
    //            return true;
    //        }
    //    }
    //    else if (str == "Crouch") {
    //        if (key == GameManager.instance.playerScript.sprintKey || key == GameManager.instance.playerScript.jumpKey)
    //        {
    //            return true;
    //        }
    //    }
    //    else if (str == "Sprint")
    //    {
    //        if (key == GameManager.instance.playerScript.jumpKey || key == GameManager.instance.playerScript.crouchKey) {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    public void SetJumpKey ()
    {
       JumpInputReady = true;
        // tempText = JumpKeyInput.GetComponentInChildren<TMP_Text>().text;
        tempHolder = JumpKeyInput.GetComponentInChildren<TMP_Text>().text;
        JumpKeyInput.GetComponentInChildren<TMP_Text>().text = "";
        GameManager.instance.UIAudioSource.Play();
    }
    void JumpChange()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                if (vKey == KeyCode.Escape)
                {
                    
                        JumpKeyInput.GetComponentInChildren<TMP_Text>().text = tempHolder.ToString();
                    JumpInputReady = false;
                    break;
                }
                else
                {
                    if (GameManager.instance.playerScript != null)
                        GameManager.instance.playerScript.jumpKey = vKey;
                    JumpKeyInput.GetComponentInChildren<TMP_Text>().text = vKey.ToString();
                    JumpInputReady = false;
                    PlayerPrefs.SetString("Jump Key", vKey.ToString());
                    break;
                }
            }
        }
        
        
    }
    public void SetCrouchKey ()
    {
        CrouchInputReady = true;
        // tempText = JumpKeyInput.GetComponentInChildren<TMP_Text>().text;
        tempHolder = CrouchKeyInput.GetComponentInChildren<TMP_Text>().text;
        CrouchKeyInput.GetComponentInChildren<TMP_Text>().text = "";
        GameManager.instance.UIAudioSource.Play();
    }
    void CrouchChange()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                if (vKey == KeyCode.Escape)
                {
                  
                        CrouchKeyInput.GetComponentInChildren<TMP_Text>().text = tempHolder.ToString();
                    CrouchInputReady = false;
                    
                    break;
                }
                else
                {
                    if (GameManager.instance.playerScript != null)
                        GameManager.instance.playerScript.crouchKey = vKey;
                    CrouchKeyInput.GetComponentInChildren<TMP_Text>().text = vKey.ToString();
                    CrouchInputReady = false;
                    PlayerPrefs.SetString("Crouch Key", vKey.ToString());
                    break;
                }
            }
        }
      
        
        
    }
    public void SetSprintKey ()
    {
        SprintInputReady = true;
        tempHolder = SprintKeyInput.GetComponentInChildren<TMP_Text>().text;
        SprintKeyInput.GetComponentInChildren<TMP_Text>().text = "";
        GameManager.instance.UIAudioSource.Play();
    }
    void SprintChange()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                if (vKey == KeyCode.Escape)
                {
                    
                        SprintKeyInput.GetComponentInChildren<TMP_Text>().text = tempHolder.ToString();
                    SprintInputReady = false;
                   
                    break;
                }
                else
                {
                    if (GameManager.instance.playerScript != null)
                        GameManager.instance.playerScript.sprintKey = vKey;
                    SprintKeyInput.GetComponentInChildren<TMP_Text>().text = vKey.ToString();
                    SprintInputReady = false;
                    PlayerPrefs.SetString("Sprint Key", vKey.ToString());
                    break;
                }
            }
            
        }
       
    }
        
        
        

    public void SetSlamKey()
    {
        SlamInputReady = true;
        tempHolder = SlamKeyInput.GetComponentInChildren<TMP_Text>().text;
        SlamKeyInput.GetComponentInChildren<TMP_Text>().text = "";
        GameManager.instance.UIAudioSource.Play();
    }
    void SlamChange()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                if (vKey == KeyCode.Escape)
                {
                   
                        SlamKeyInput.GetComponentInChildren<TMP_Text>().text = tempHolder.ToString();
                    SlamInputReady = false;
                    
                    break;
                }
                else
                {
                    if (GameManager.instance.playerScript != null)
                        GameManager.instance.playerScript.scytheScript.slamAttackKey = vKey;
                    SlamKeyInput.GetComponentInChildren<TMP_Text>().text = vKey.ToString();
                    SlamInputReady = false;
                    PlayerPrefs.SetString("Slam Key", vKey.ToString());
                    break;
                }
            }
        }
      
       
        
    }
    public void SetMeleeKey()
    {
        MeleeInputReady = true;
        tempHolder = MeleeKeyInput.GetComponentInChildren<TMP_Text>().text;
        MeleeKeyInput.GetComponentInChildren<TMP_Text>().text = "";
        GameManager.instance.UIAudioSource.Play();
    }
    void MeleeChange()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                if (vKey == KeyCode.Escape)
                {
                    
                        MeleeKeyInput.GetComponentInChildren<TMP_Text>().text = tempHolder.ToString();
                    MeleeInputReady = false;
                   
                    break;
                }
                else
                {
                    if (GameManager.instance.playerScript != null)
                        GameManager.instance.playerScript.scytheScript.attackKey = vKey;
                    MeleeKeyInput.GetComponentInChildren<TMP_Text>().text = vKey.ToString();
                    MeleeInputReady = false;
                    PlayerPrefs.SetString("Attack Key", vKey.ToString());
                    break;
                }
            }
        }
     
        
        
    }
    public void SetSlashKey()
    {
        SlashInputReady = true;
        tempHolder = SlashKeyInput.GetComponentInChildren<TMP_Text>().text;
        SlashKeyInput.GetComponentInChildren<TMP_Text>().text = "";
        GameManager.instance.UIAudioSource.Play();
    }
    void SlashChange()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                if (vKey == KeyCode.Escape)
                {
                    
                        SlashKeyInput.GetComponentInChildren<TMP_Text>().text = tempHolder.ToString();
                    SlashInputReady = false;
                    
                    break;
                }
                else
                {
                    if (GameManager.instance.playerScript != null)
                        GameManager.instance.playerScript.scytheScript.slashKey = vKey;
                    SlashKeyInput.GetComponentInChildren<TMP_Text>().text = vKey.ToString();
                    SlashInputReady = false;
                    PlayerPrefs.SetString("Slash Key", vKey.ToString());
                    break;
                }
            }
        }
       
        
        
    }
    public void SetSpecialKey()
    {
        SpecialInputReady = true;
        tempHolder = SpecialKeyInput.GetComponentInChildren<TMP_Text>().text;
        SpecialKeyInput.GetComponentInChildren<TMP_Text>().text = "";
        GameManager.instance.UIAudioSource.Play();
    }
    void SpecialChange()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                if (vKey == KeyCode.Escape)
                {
                    
                        SpecialKeyInput.GetComponentInChildren<TMP_Text>().text = tempHolder.ToString();
                    SpecialInputReady = false;
                    
                    break;
                }
                else
                {
                    if (GameManager.instance.playerScript != null)
                        GameManager.instance.playerScript.scytheScript.specialAttackKey = vKey;
                    SpecialKeyInput.GetComponentInChildren<TMP_Text>().text = vKey.ToString();
                    SpecialInputReady = false;
                    PlayerPrefs.SetString("Special Key", vKey.ToString());
                    break;
                }
            }
        }
        
        
    }
}




