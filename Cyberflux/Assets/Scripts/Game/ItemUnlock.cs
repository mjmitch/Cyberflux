using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUnlock : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public Augment item;

    [SerializeField] public Image itemImage;
    [SerializeField] public TMP_Text itemText;

    public Button confirm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        if (other.transform.CompareTag("PlayerModel"))
        {
            UpdateItem();
            GameManager.instance.UnlockItem();

            Destroy(gameObject);
        }
    }

    public void UpdateItem()
    {
        GameManager.instance.itemUnlockObject.item = item;
        GameManager.instance.itemUnlockObject.itemImage.sprite = item.icon;
        GameManager.instance.itemUnlockObject.itemText.text = item.itemDescription;
    }

    public void Continue()
    {
        
        GameManager.instance.playerScript.AddItem(item);
        
        GameManager.instance.GameStateResume();
    }
}
