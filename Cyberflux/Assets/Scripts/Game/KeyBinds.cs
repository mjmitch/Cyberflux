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
    
    void Start()
    {
        JumpKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.jumpKey.ToString();
        CrouchKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.crouchKey.ToString();
        SprintKeyInput.GetComponentInChildren<TMP_Text>().text = GameManager.instance.playerScript.sprintKey.ToString();
        
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
    }
    public void SetCrouchKey ()
    {
        CrouchInputReady = true;
       // tempText = JumpKeyInput.GetComponentInChildren<TMP_Text>().text;
        CrouchKeyInput.GetComponentInChildren<TMP_Text>().text = "";
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
    }
    public void SetSprintKey ()
    {
        SprintInputReady = true;
        // tempText = JumpKeyInput.GetComponentInChildren<TMP_Text>().text;
        SprintKeyInput.GetComponentInChildren<TMP_Text>().text = "";
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
    }

}
