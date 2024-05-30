using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("General")]
    public List<itemType> inventoryList;
    public int selectedItem;
    public float playerReach;
    [SerializeField] GameObject throwItem_gameobject;

    [Space(20)]
    [Header("Keys")]
    [SerializeField] KeyCode throwItemKey;
    [SerializeField] KeyCode pickItemKey;

    [Space(20)]
    [Header("Item gameobjects")]
    [SerializeField] GameObject zeus_item;
    [SerializeField] GameObject lancacocos_item;
    [SerializeField] GameObject rigon_item;
    [SerializeField] GameObject health_item;


    [Space(20)]
    [Header("Item prefabs")]
    [SerializeField] GameObject zeus_prefab;
    [SerializeField] GameObject lancacocos_prefab;
    [SerializeField] GameObject rigon_prefab;
    [SerializeField] GameObject health_prefab;

    [SerializeField] Camera cam;


    [SerializeField] GameObject pickUpItem_gameobjet;


    [Space(20)]
    [Header("UI")]
    [SerializeField] Image[] inventorySlotImage = new Image[4];
    [SerializeField] Image[] inventoryBackgroundImage = new Image[4];
    [SerializeField] Sprite emptySlotSprite;


    private Dictionary<itemType, GameObject> itemSetActive = new Dictionary<itemType, GameObject>() { };
    private Dictionary<itemType, GameObject> itemInstantiate = new Dictionary<itemType, GameObject>() { };


    private IPickable nearbyItem;



    [SerializeField] Sprite lancacocos;
    [SerializeField] Sprite zeus;
    [SerializeField] Sprite health;
    [SerializeField] Sprite rigon;



    void Start()
    {
        itemSetActive.Add(itemType.Zeus, zeus_item);
        itemSetActive.Add(itemType.LancaCocos, lancacocos_item);
        itemSetActive.Add(itemType.Rigon, rigon_item);
        itemSetActive.Add(itemType.Health, health_item);

        itemInstantiate.Add(itemType.Zeus, zeus_prefab);
        itemInstantiate.Add(itemType.LancaCocos, lancacocos_prefab);
        itemInstantiate.Add(itemType.Rigon, rigon_prefab);
        itemInstantiate.Add(itemType.Health, health_prefab);

        NewItemSelected();

    }

    void Update()
    {
        if (Input.GetKeyDown(pickItemKey) && nearbyItem != null)
        {

            ItemPickable itemPickable = nearbyItem as ItemPickable;
            if (itemPickable != null)
            {

                inventoryList.Add(itemPickable.itemScriptableObject.item_type);
                nearbyItem.PickItem();


                nearbyItem = null; // Clear the reference after picking up the item
                pickUpItem_gameobjet.SetActive(false); // Deactivate the pickup UI
            }
        }


        //Items
        if (Input.GetKeyDown(throwItemKey) && inventoryList.Count > 1)
        {
            Instantiate(itemInstantiate[inventoryList[selectedItem]], position: throwItem_gameobject.transform.position, new Quaternion());
            inventoryList.RemoveAt(selectedItem);

            if (selectedItem != 0)
            {
                selectedItem -= 1;
            }
            NewItemSelected();
        }


        //UI
        for (int i = 0; i < 4; i++)
        {
            if (i < inventoryList.Count)
            {

                //inventorySlotImage[i].sprite = itemSetActive[inventoryList[i]].GetComponent<Item>().itemScriptableObject.item_sprite;


                //Debug.Log("dasd" + itemSetActive[inventoryList[i]].ToString()+ " asdads");
                if (itemSetActive[inventoryList[i]].ToString() == "LancaCocos (UnityEngine.GameObject)")
                {
                    inventorySlotImage[i].sprite = lancacocos;
                }
                else if (itemSetActive[inventoryList[i]].ToString() == "Zeus (UnityEngine.GameObject)")
                {
                    inventorySlotImage[i].sprite = zeus;
                }
                else if (itemSetActive[inventoryList[i]].ToString() == "Health (UnityEngine.GameObject)")
                {
                    inventorySlotImage[i].sprite = health;
                }
                else if (itemSetActive[inventoryList[i]].ToString() == "Rigon (UnityEngine.GameObject)")
                {
                    inventorySlotImage[i].sprite = rigon;
                }
            }
            else
            {
                inventorySlotImage[i].sprite = emptySlotSprite;
            }
        }

        int a = 0;
        foreach (Image image in inventoryBackgroundImage)
        {
            if (a == selectedItem)
            {
                image.color = new Color32(145, 255, 126, 255);
            }
            else
            {
                image.color = new Color32(219, 219, 219, 255);
            }
            a++;
        }


        if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 0)
        {
            selectedItem = 0;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && inventoryList.Count > 1)
        {
            selectedItem = 1;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && inventoryList.Count > 2)
        {
            selectedItem = 2;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && inventoryList.Count > 3)
        {
            selectedItem = 3;
            NewItemSelected();
        }

    }


    private void NewItemSelected()
    {
        zeus_item.SetActive(false);
        lancacocos_item.SetActive(false);
        rigon_item.SetActive(false);
        health_item.SetActive(false);

        GameObject selectedItemGameObject = itemSetActive[inventoryList[selectedItem]];
        selectedItemGameObject.SetActive(true);

    }

    private void OnTriggerEnter(Collider other)
    {
        IPickable item = other.GetComponent<IPickable>();
        pickUpItem_gameobjet.SetActive(true);
        if (item != null)
        {
            nearbyItem = item;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IPickable item = other.GetComponent<IPickable>();
        pickUpItem_gameobjet.SetActive(false);
        if (item == nearbyItem)
        {
            nearbyItem = null;
        }
    }






}


public interface IPickable
{
    void PickItem();
}