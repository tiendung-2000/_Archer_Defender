using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    Common,
    Medium,
    Rare,
    Epic,
    Legendary
}

[System.Serializable]
public class DropItem {
    public ItemType itemType;

    [Tooltip("Base rate before applying any modifiers")]
    public float baseRate;

    [Tooltip("Adjusted rate after applying modifiers")]
    public float currentRate;
}

public class DropSystem : MonoBehaviour {
    [Tooltip("List of items and their drop rates")]
    public List<DropItem> items = new List<DropItem>();

    Dictionary<ItemType, DropItem> _itemDictionary;
    readonly Dictionary<ItemType, float> _activeModifiers = new Dictionary<ItemType, float>(); // Track active modifiers

    void Awake() {
        InitializeDropSystem();
    }

    // Initialize item dictionary and set current rates to base rates
    void InitializeDropSystem() {
        _itemDictionary = new Dictionary<ItemType, DropItem>();
        foreach (var item in items) {
            item.currentRate = item.baseRate;
            _itemDictionary[item.itemType] = item;
        }
    }

    // Equip an item that modifies the drop rate of a specific item type
    public void EquipItemModifier(ItemType type, float percentIncrease) {
        if (_itemDictionary.ContainsKey(type)) {
            // Apply the modifier only if not already applied
            if (!_activeModifiers.ContainsKey(type)) {
                _activeModifiers[type] = percentIncrease; // Store the modifier
                float baseRate = _itemDictionary[type].baseRate;
                _itemDictionary[type].currentRate = baseRate * (1 + percentIncrease / 100f); // Apply percentage increase
            }
        }
    }

    // Unequip the item, reverting the drop rate back to the base rate
    public void UnequipItemModifier(ItemType type) {
        if (_activeModifiers.ContainsKey(type)) {
            _activeModifiers.Remove(type); // Remove the modifier
            _itemDictionary[type].currentRate = _itemDictionary[type].baseRate; // Reset to base rate
        }
    }

    // Roll for a drop based on the current rates of all items
    public ItemType RollForDrop() {
        float totalRate = 0f;

        // Calculate the total rate across all items
        foreach (var item in items) {
            totalRate += item.currentRate;
        }

        // Generate a random number within the total rate range
        float roll = Random.Range(0, totalRate);
        float cumulativeRate = 0f;

        // Find which item corresponds to the rolled value
        foreach (var item in items) {
            cumulativeRate += item.currentRate;
            if (roll <= cumulativeRate) {
                return item.itemType;
            }
        }

        // Default to Common if something unexpected happens
        return ItemType.Common;
    }

    // Example method to test the drop system
    public void TestDropSystem(int trials) {
        Dictionary<ItemType, int> dropCounts = new Dictionary<ItemType, int>();

        // Initialize drop counts
        foreach (ItemType type in System.Enum.GetValues(typeof(ItemType))) {
            dropCounts[type] = 0;
        }

        // Perform drop trials
        for (int i = 0; i < trials; i++) {
            ItemType droppedItem = RollForDrop();
            dropCounts[droppedItem]++;
        }

        // Print results
        foreach (var pair in dropCounts) {
            Debug.Log($"{pair.Key}: {pair.Value} drops ({(pair.Value / (float)trials) * 100}% chance)");
        }
    }

    // Simulates catching a fish once, using current drop rates
    public ItemType OnCatchFish() {
        float totalRate = 0f;

        // Calculate the total rate across all items
        foreach (var item in items) {
            totalRate += item.currentRate;
        }

        // Generate a random number within the total rate range
        float roll = Random.Range(0, totalRate);
        float cumulativeRate = 0f;

        // Find which item corresponds to the rolled value
        foreach (var item in items) {
            cumulativeRate += item.currentRate;
            if (roll <= cumulativeRate) {
                Debug.Log($"Caught a {item.itemType} fish!");
                return item.itemType;
            }
        }

        // Default to Common if something unexpected happens
        Debug.Log("Caught a Common fish!");
        return ItemType.Common;
    }
}