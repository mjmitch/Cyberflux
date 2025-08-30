using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Door : MonoBehaviour
{
    [SerializeField] bool stayOpen;
    [SerializeField] bool playerTriggered;
    [SerializeField] bool keyRequired;
    [SerializeField] FixedSpawner enemiesToKill;
    
    [SerializeField] Transform moveToWhenOpened;

    [Header("Do not check")]
    public bool playerInTrigger = false;
    Vector3 startPos;
    Vector3 endPos;
    private bool dontClose = false;
    private bool keyGiven = false;
    private bool moveToStart = false;
    private bool moveToEnd = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        GetComponentInChildren<DoorTrigger>().transform.parent = null;
        startPos = new Vector3 (transform.position.x,transform.position.y,transform.position.z);
        endPos = new Vector3(moveToWhenOpened.transform.position.x, moveToWhenOpened.transform.position.y, moveToWhenOpened.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (CheckForSuccess() && !moveToStart && !moveToEnd)
        {
            if (dontClose && stayOpen)
            {
                return;
            }
            
            {
                if (keyRequired && !keyGiven)
                {
                    GameManager.instance.playerScript.keys--;
                    if (GameManager.instance.playerScript.keys == 0)
                    {
                        GameManager.instance.keyImage.gameObject.SetActive(false);
                    }
                    keyGiven = true;
                }
                {

                }
                if (Vector3.Distance(transform.position, endPos) < 0.01f)
                {
                    moveToStart = true;
                }
                else
                {
                    moveToEnd = true;
                }
                if (stayOpen)
                {
                    dontClose = true;
                }
            }
        }
        else if (playerTriggered)
        {
            if (moveToStart && !playerInTrigger)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, 100 * Time.deltaTime);

                if (Vector3.Distance(transform.position, startPos) < 0.01f)
                {
                    moveToStart = false;
                }
            }
            else if (moveToEnd && playerInTrigger)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPos, 100 * Time.deltaTime);

                if (Vector3.Distance(transform.position, endPos) < 0.01f)
                {
                    moveToEnd = false;
                }
            }
        }
        else
        {
            if (moveToStart)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, 100 * Time.deltaTime);

                if (Vector3.Distance(transform.position, startPos) < 0.01f)
                {
                    moveToStart = false;
                }
            }
            else if (moveToEnd)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPos, 100 * Time.deltaTime);

                if (Vector3.Distance(transform.position, endPos) < 0.01f)
                {
                    moveToEnd = false;
                }
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    bool CheckForSuccess()
    {
        if (playerTriggered && !playerInTrigger)
        {
            return false;
        }
        if (keyRequired && GameManager.instance.playerScript.keys <= 0 && !keyGiven)
        {
            return false;
        }
        if (enemiesToKill != null && enemiesToKill.IsDoneSpawning() && enemiesToKill.transform.childCount == 0)
        {
            if (!enemiesToKill.IsDoneSpawning() || enemiesToKill.transform.childCount != 0)
            {
                return false;
            }
            
        }
        return true;
    }
       
   
}

