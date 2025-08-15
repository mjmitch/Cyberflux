using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUnlock : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Augment item;

    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text itemText;

    public Button confirm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        if (other.transform.CompareTag("PlayerModel"))
        {

        }
    }

    public void UpdateItem()
    {
        itemImage.sprite = item.icon;
        itemText.text = item.itemDescription;
    }
}
