using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    Augment Item1;
    Augment Item2;

    [SerializeField] Image Item1Image;
    [SerializeField] Image Item2Image;

    [SerializeField] TMP_Text Item1Desc;
    [SerializeField] TMP_Text Item2Desc;

    [SerializeField] TMP_Text Item1Stats;
    [SerializeField] TMP_Text Item2Stats;

    public Button selectItem1;
    public Button selectItem2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        if (other.transform.CompareTag("PlayerModel"))
        {
            
                GameManager.instance.itemSelectionObject.UpdateItems();
                GameManager.instance.SelectItem();
            
            
                Destroy(gameObject);
        }
    }

    

    void UpdateItems()
    {
        int temp = Random.Range(0, GameManager.instance.itemPool.Count);
        Item1 = GameManager.instance.itemPool[temp];
        int temp2 = Random.Range(0, GameManager.instance.itemPool.Count);
        while (temp == temp2)
        {
            temp2 = Random.Range(0, GameManager.instance.itemPool.Count);
        }
        Item1 = GameManager.instance.itemPool[temp];
        Item2 = GameManager.instance.itemPool[temp2];

        Item1Image.sprite = Item1.icon;
        Item1Desc.text = Item1.itemDescription;
        Item1Stats.text = Item1.itemStats;

        Item2Image.sprite = Item2.icon;
        Item2Desc.text = Item2.itemDescription;
        Item2Stats.text = Item2.itemStats;

    }

    public void Item1Select()
    {
        
        GameManager.instance.playerScript.AddItem(Item1);
       
        GameManager.instance.GameStateResume();
        GameManager.instance.YouWin();
        Cursor.visible = true;
    }
    public void Item2Select()
    {
       
        GameManager.instance.playerScript.AddItem(Item2);
        
        GameManager.instance.GameStateResume();
        GameManager.instance.YouWin();
        Cursor.visible = true;
    }
}
