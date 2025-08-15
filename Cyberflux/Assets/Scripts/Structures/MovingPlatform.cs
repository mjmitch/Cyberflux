using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Empty objects that act as position for the object to move to
    List<Vector3> destinations = new List<Vector3>(); 


    [SerializeField] int speed;
    [SerializeField] int startDelay;
    [SerializeField] 
    float delayTimer;
    bool stopped;
    int positionNum;
    bool playerInTrigger;
    void Start()
    {
        playerInTrigger = false;
        stopped = true;
        delayTimer = 0;
        Transform[] childrendest = GetComponentsInChildren<Transform>();
        for (int i = 0; i < childrendest.Length; i++)
        {

            destinations.Add(new Vector3(childrendest[i].transform.position.x, childrendest[i].transform.position.y, childrendest[i].transform.position.z));

        }
      
    }

    // Update is called once per frame
    void Update()
    {
        if (destinations.Count > 1)
        {


            if (stopped && delayTimer > startDelay)
            {
                if (positionNum == destinations.Count - 1)
                {
                    positionNum = 0;
                }
                else
                {
                    positionNum++;
                }
                stopped = false;
                delayTimer = 0;
                //transform.position = Vector3.MoveTowards(transform.position, destinations[positionNum], speed * Time.deltaTime);

            }
            else if (stopped)
            { 
                delayTimer += Time.deltaTime;
            }
            else
            { 

                    transform.position = Vector3.MoveTowards(transform.position, destinations[positionNum], speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, destinations[positionNum]) < 0.01f)
                {
                    stopped = true;
                }
                    
            }


        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        
        if (other.transform.CompareTag("PlayerModel"))
        {
            {
                if (playerInTrigger)
                {
                    return;
                }
                Rigidbody playerRb = GameManager.instance.player.gameObject.GetComponent<Rigidbody>();
                playerRb.interpolation = RigidbodyInterpolation.None;

                GameManager.instance.player.transform.SetParent(transform, true);
                playerInTrigger = true;
            }
        }
        else if (other.transform.root.gameObject.layer == 7)
        {
            if (other.transform.root.GetComponent<Rigidbody>() != null)
            {
                other.transform.root.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
            }

            other.transform.root.SetParent(transform, true);
        }
        else if (other.gameObject.layer == 7)
        {

            if (other.GetComponent<Rigidbody>() != null)
            {
                other.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
            }
            other.transform.SetParent(transform, true);
        }

     
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        if (other.transform.CompareTag("PlayerModel"))
        {
            if (!playerInTrigger)
            {
                return;
            }
            Rigidbody playerRb = GameManager.instance.player.gameObject.GetComponent<Rigidbody>();
            playerRb.interpolation = RigidbodyInterpolation.Interpolate;
            GameManager.instance.player.gameObject.transform.parent = null;
            playerInTrigger = false;
        }
        else if (other.transform.root.gameObject.layer == 7)
        {
            if (other.transform.root.GetComponent<Rigidbody>() != null)
            {
                other.transform.root.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
            }
            other.transform.root.parent = null;
        }
        else if (other.gameObject.layer == 7)
        {

            if (other.GetComponent<Rigidbody>() != null)
            {
                other.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
            }
            other.transform.parent = null;
        }
    }
}
