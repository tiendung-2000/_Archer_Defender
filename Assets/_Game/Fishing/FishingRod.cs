using UnityEngine;

public class FishingRod : MonoBehaviour {
    [Tooltip("Percentage increase multipliers for each item type")]
    public float percentCommonIncrease = 1.0f;
    public float percentMediumIncrease = 1.0f;
    public float percentRareIncrease = 1.0f;
    public float percentEpicIncrease = 1.0f;
    public float percentLegendaryIncrease = 1.0f;

    [SerializeField] DropSystem _dropSystem;

    void Start() {
        // Find the DropSystem in the scene
        _dropSystem = FindObjectOfType<DropSystem>();

        if (_dropSystem == null) {
            Debug.LogError("DropSystem not found in the scene!");
        }
    }

    // Equip the fishing rod and adjust drop rates accordingly
    [Sirenix.OdinInspector.Button]
    public void Equip() {
        if (_dropSystem == null) return;

        // Apply custom drop rate increases for each item type
        _dropSystem.EquipItemModifier(ItemType.Common, percentCommonIncrease);
        _dropSystem.EquipItemModifier(ItemType.Medium, percentMediumIncrease);
        _dropSystem.EquipItemModifier(ItemType.Rare, percentRareIncrease);
        _dropSystem.EquipItemModifier(ItemType.Epic, percentEpicIncrease);
        _dropSystem.EquipItemModifier(ItemType.Legendary, percentLegendaryIncrease);

        Debug.Log("Fishing rod equipped with custom drop rate increases.");
    }

    // Unequipped the fishing rod and reset all drop rates
    [Sirenix.OdinInspector.Button]
    public void Unequip() {
        if (_dropSystem == null) return;

        // Reset drop rates for each item type
        _dropSystem.UnequipItemModifier(ItemType.Common);
        _dropSystem.UnequipItemModifier(ItemType.Medium);
        _dropSystem.UnequipItemModifier(ItemType.Rare);
        _dropSystem.UnequipItemModifier(ItemType.Epic);
        _dropSystem.UnequipItemModifier(ItemType.Legendary);

        Debug.Log("Fishing rod unequipped. All drop rates reset.");
    }

    [Sirenix.OdinInspector.Button]
    public void OnCatchFish() {
        if (_dropSystem == null) return;

        // Call DropSystem's OnCatchFish to simulate a one-time catch
        ItemType caughtFish = _dropSystem.OnCatchFish();

        Debug.Log($"You caught a {caughtFish} fish!");
    }
}