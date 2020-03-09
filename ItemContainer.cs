using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemContainer : MonoBehaviour {

    [Header("Container UI Elements")]
    public GameObject windowParent;
    public TextMeshProUGUI title;
    public Transform window; // The GridLayoutWindow we're using to display our UIItemSlots.

    [Header("Container Details")]
    public string containerName;
    GameObject SlotPrefab; // The prefab of the UIItemSlots.

    // In real usage, this list would be stored with game objects (chests, lootable bodies, inventory, etc)
    // and passed to this script when the player "opens" that container. For now we'll just put it here.
    List<ItemSlot> items = new List<ItemSlot>();

    private void Start() {

        SlotPrefab = Resources.Load<GameObject>("Prefabs/Inventory Slot");

        #region Demonstration Code

        Item[] tempItems = new Item[3];
        tempItems[0] = Resources.Load<Item>("Items/Sword");
        tempItems[1] = Resources.Load<Item>("Items/Ring");
        tempItems[2] = Resources.Load<Item>("Items/Coin");


        for (int i = 0; i < 14; i++) {

            int index = Random.Range(0, 3);
            int amount = Random.Range(1, tempItems[index].maxStack);
            int condition = tempItems[index].maxCondition;

            items.Add(new ItemSlot(tempItems[index].name, amount, condition));

        }

        #endregion

    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Q))
            CloseContainer();
        if (Input.GetKeyDown(KeyCode.W))
            OpenContainer(items);

    }

    List<UIItemSlot> UISlots = new List<UIItemSlot>();

    public void OpenContainer (List<ItemSlot> slots) {

        windowParent.SetActive(true);

        title.text = containerName.ToUpper(); // Set the name of the container.

        // Loop through each item in the given items list and instantiate a new UIItemSlot prefab for each one.
        for (int i = 0; i < slots.Count; i++) {

            // Make sure our GridLayoutWindow is set as the parent of the new UIItemSlot object.
            GameObject newSlot = Instantiate(SlotPrefab, window);

            // Name the new slot with its index in the list so we have a way of identifying it.
            newSlot.name = i.ToString();

            // Add the new slot to our UISlots list so we can find it later.
            UISlots.Add(newSlot.GetComponent<UIItemSlot>());

            // Attach the UIItemSlot to the ItemSlot it corresponds to.
            slots[i].AttachUI(UISlots[i]);

        }
    }

    public void CloseContainer () {

        // Loop through each slot, detatch it from its ItemSlot and delete the GameObject.
        foreach (UIItemSlot slot in UISlots) {

            if (slot.itemSlot != null)
                slot.itemSlot.DetachUI();
            Destroy(slot.gameObject);

        }

        // Clear the list and deactivate the container window.
        UISlots.Clear();
        windowParent.SetActive(false);

    }
}
