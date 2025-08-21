using UnityEngine;

public class HubLevel : MonoBehaviour
{

    [SerializeField] GameObject Level1Trigger;
    [SerializeField] GameObject Level2Trigger;
    [SerializeField] GameObject Level3Trigger;
    [SerializeField] GameObject Level4Trigger;
    [SerializeField] GameObject Level5Trigger;
    [SerializeField] GameObject Level6Trigger;

    [SerializeField] Material activeLevelMaterial;
    [SerializeField] Material inactiveLevelMaterial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        CheckLevelProgress();
    }

    void CheckLevelProgress()
    {
        Level1Trigger.GetComponentInChildren<StartLevel>().gameObject.SetActive(true);
        Renderer[] lvl1renders = Level1Trigger.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < lvl1renders.Length; i++)
        {
            lvl1renders[i].material = activeLevelMaterial;
        }

        if (PlayerPrefs.GetInt("Level 1 Completed", 0) == 1)
        {
            Level2Trigger.GetComponentInChildren<StartLevel>().gameObject.SetActive(true);
            Renderer[] renders = Level2Trigger.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                renders[i].material = activeLevelMaterial;
            }
        }
        else
        {
            Level2Trigger.GetComponentInChildren<StartLevel>().gameObject.SetActive(false);
            Renderer[] renders = Level2Trigger.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                renders[i].material = inactiveLevelMaterial;
            }
        }

        if (PlayerPrefs.GetInt("Level 2 Completed", 0) == 1)
        {
            Level3Trigger.GetComponentInChildren<StartLevel>().gameObject.SetActive(true);
            Renderer[] renders = Level3Trigger.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                renders[i].material = activeLevelMaterial;
            }
        }
        else
        {
             Level3Trigger.GetComponentInChildren<StartLevel>().gameObject.SetActive(false);
            Renderer[] renders = Level3Trigger.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                renders[i].material = inactiveLevelMaterial;
            }
        }

        if (PlayerPrefs.GetInt("Level 3 Completed", 0) == 1)
        {
            Level4Trigger.GetComponentInChildren<StartLevel>().gameObject.SetActive(true);
            Renderer[] renders = Level4Trigger.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                renders[i].material = activeLevelMaterial;
            }
        }
        else
        {
              Level4Trigger.GetComponentInChildren<StartLevel>().gameObject.SetActive(false);
            Renderer[] renders = Level4Trigger.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                renders[i].material = inactiveLevelMaterial;
            }
        }

        if (PlayerPrefs.GetInt("Level 4 Completed", 0) == 1)
        {
            Level5Trigger.GetComponentInChildren<StartLevel>().gameObject.SetActive(true);
            Renderer[] renders = Level5Trigger.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                renders[i].material = activeLevelMaterial;
            }
        }
        else
        {
            Level5Trigger.GetComponentInChildren<StartLevel>().gameObject.SetActive(false);
            Renderer[] renders = Level5Trigger.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                renders[i].material = inactiveLevelMaterial;
            }
        }

        if (PlayerPrefs.GetInt("Level 5 Completed", 0) == 1)
        {
            Level6Trigger.GetComponentInChildren<StartLevel>().gameObject.SetActive(true);
            

            Renderer[] renders = Level6Trigger.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                renders[i].material = activeLevelMaterial;
            }
        }
        else
        {
            Level6Trigger.GetComponentInChildren<StartLevel>().gameObject.SetActive(false);
       
        Renderer[] renders = Level6Trigger.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            renders[i].material = inactiveLevelMaterial;
        }
        }
    }
}
