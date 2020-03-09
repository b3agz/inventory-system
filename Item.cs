using UnityEngine;

[CreateAssetMenu(fileName="New Item", menuName ="Inventory/New Item")]
public class Item : ScriptableObject {

    // Item name, can't be called "name" as clashes with built in Object.name variable.
    public string itemName;

    // Description of item, uses TextArea tag to give us more space in the inspector to write in.
    [TextArea]
    public string itemDescription;

    // The image that will show up in inventory slots.
    public Sprite icon;

    // The maximum of this item that can be placed in one slot.
    public int maxStack;

    // The maximum condition this item can be in. If the item does not degrade, set this value to -1.
    public int maxCondition;

    // Quick ways of checking if the item is stackable or degradable.
    public bool isStackable { get { return (maxStack > 1); } }
    public bool isDegradable { get { return (maxCondition > -1); } }

}
