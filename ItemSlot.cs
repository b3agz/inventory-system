using UnityEngine;

// This class represents one inventory/container/toolbelt/whatever slot. An item cannot exist outside of
// a "slot". Every in-game object that contains an item is just storing a collection (or one) of these slots.
public class ItemSlot {

    // The item stored here.
    public Item item;

     // The amount of the above item this slot is holding.
    private int _amount;
    public int amount {

        get { return _amount; }
        set {

            if (item == null) _amount = 0; // Can't have an amount of no item.
            else if (value > item.maxStack) _amount = item.maxStack; // Ensure we don't end up with more items than can stack.
            else if (value < 1) _amount = 0; // Can't have a minus amount of something.
            else _amount = value;
            RefreshUISlot();

        }
    }

    // The condition of the item(s). A slot can only stack identical items so items with
    // different conditions will be treated as different items.
    private int _condition;
    public int condition {

        get { return _condition; }
        set {

            if (item == null) _condition = 0; // No item = no condition.
            else if (value > item.maxCondition) _condition = item.maxCondition;
            else _condition = value;
            RefreshUISlot();

        }
    }

    private UIItemSlot uiItemSlot;

    // Quick bool to check if this ItemSlot is occupied.
    public bool hasItem { get { return (item != null); } }

    // Return an Item (or null) from a name.
    private Item FindByName (string itemName) {

        itemName = itemName.ToLower(); // Make sure string is lower case (item files need to be lower case as well).
        Item _item = Resources.Load<Item>(string.Format("Items/{0}", itemName)); // Load item from resources folder.

        if (_item == null) // If null, throw up a warning to say we couldn't find the item.
            Debug.LogWarning(string.Format("Could not find item \"{0}\". Item slot is empty.", itemName));

        return _item;

    }

    // Returns true the two ItemSlots are the same.
    public static bool Compare (ItemSlot slotA, ItemSlot slotB) {

        // If the items or condition are different--or one item is null--this ItemSlot is treated as different.
        if (slotA.item != slotB.item || slotA.condition != slotB.condition)
            return false;

        return true;

    }

    // Swaps the contents of two ItemSlots.
    public static void Swap (ItemSlot slotA, ItemSlot slotB) {

        // Cache slotA's values.
        Item _item = slotA.item;
        int _amount = slotA.amount;
        int _condition = slotA.condition;

        // Copy slotB's values to slotA.
        slotA.item = slotB.item;
        slotA.amount = slotB.amount;
        slotA.condition = slotB.condition;

        // Copy the cached slotA values to slotB.
        slotB.item = _item;
        slotB.amount = _amount;
        slotB.condition = _condition;

        // Refresh both.
        slotA.RefreshUISlot();
        slotB.RefreshUISlot();

    }

    public void AttachUI (UIItemSlot uiSlot) {

        uiItemSlot = uiSlot;
        uiItemSlot.itemSlot = this;
        RefreshUISlot();

    }

    public void DetachUI () {

        uiItemSlot.ClearSlot();
        uiItemSlot = null;

    }

    // Bool to quickly check if ItemSlot is currently attached to a UIItemSlot.
    private bool isAttachedToUI { get { return (uiItemSlot != null); } }

    public void RefreshUISlot () {

        // If we're not attached to a UIItemSlot, there's nothing to refresh.
        if (!isAttachedToUI)
            return;

        uiItemSlot.RefreshSlot();

    }

    // Clears this ItemSlot of its contents.
    public void Clear() {

        item = null;
        amount = 0;
        condition = 0;
        RefreshUISlot();

    }

    // Constructor. Putting default values in the parameters makes them optional, so
    // we can just pass in an item name if we want.
    public ItemSlot (string itemName, int _amount = 1, int _condition = 0) {

        Item _item = FindByName(itemName); // Get the item.

        if (_item == null) { // If item is null for whatever reason, slot is empty.

            item = null;
            amount = 0;
            condition = 0;
            return;

        } else { // Else set our values.

            item = _item;
            amount = _amount;
            condition = _condition;

        }
    }

    // Constructor for empty slots.
    public ItemSlot () {

        item = null;
        amount = 0;
        condition = 0;

    }
}
