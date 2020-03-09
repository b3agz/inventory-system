using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

// This class represents an inventory/container/etc slot in the UI. This could be an inventory window, a
// container window, a toolbelt. UIItemSlots are "attached" to an ItemSlot when used, and the information
// from that ItemSlot is show in the UI Elements.
public class UIItemSlot : MonoBehaviour {

    public bool isCursor = false;

    public ItemSlot itemSlot;

    RectTransform slotRect;
    public Image icon;
    public TextMeshProUGUI amount;
    public Image condition;

    private void Awake() {
        
        slotRect = GetComponent<RectTransform>();

        // Make sure we never have a null ItemSlot by creating an empty one.
        itemSlot = new ItemSlot();

    }


    private void Update() {

        // If we're not the mouse cursor, we don't need to Update() so just return.
        if (!isCursor) return;

        transform.position = Input.mousePosition;

    }

    public void RefreshSlot () {

        UpdateAmount();
        UpdateIcon();

        // Dragged items don't show the condition bar so check that we're not the cursor
        // before updating it.
        if (!isCursor)
            UpdateConditionBar();

    }

    public void ClearSlot () {

        itemSlot = null;
        RefreshSlot();

    }

    public void UpdateIcon () {

        if (itemSlot == null || !itemSlot.hasItem)
            icon.enabled = false;
        else {

            icon.enabled = true;
            icon.sprite = itemSlot.item.icon;

        }
    }

    public void UpdateAmount () {

        // Cases where amount is not needed at all.
        if (itemSlot == null || !itemSlot.hasItem || itemSlot.amount < 2)
            amount.enabled = false;
        // Else we can just display the amount.
        else {

            amount.enabled = true;
            amount.text = itemSlot.amount.ToString();

        }
    }

    public void UpdateConditionBar () {

        // Cases where condition bar is not needed at all.
        if (itemSlot == null || !itemSlot.hasItem || !itemSlot.item.isDegradable)
            condition.enabled = false;
        // Else work out how much of our condition bar we should be showing.
        else {

            condition.enabled = true;

            // Get the normalised percentage of condition (0 - 1).
            float conditionPercent = (float)itemSlot.condition / (float)itemSlot.item.maxCondition;

            // Multiply max width by that normalised percentage to get width.
            float barWidth = slotRect.rect.width * conditionPercent;

            // Set width. We have to pass in a Vector2 so keep same value for the y variable.
            condition.rectTransform.sizeDelta = new Vector2(barWidth, condition.rectTransform.sizeDelta.y);

            // Lerp colour from green to red as it becomes more degraded.
            condition.color = Color.Lerp(Color.red, Color.green, conditionPercent);
            
        }
    }

}
