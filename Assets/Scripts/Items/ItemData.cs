using UnityEngine;

public enum ItemRarity { Common, Uncommon, Legendary, Chronos }

public enum ItemID
{
    //Common
    HardBoiledEgg, WornSneakers, BrokenTimeWatch, BadgersHoney, SecondHand, BrokenSpearhead,
    //Uncommon
    GunslingersHolster, BrokenWing, Stopwatch, FoxShrine, MinuteHand, RabbitsFoot,
    //Legendary
    PristineTimeWatch, HourHand, SpiritOfTheDragon, FruitOffering, RavenFeather,
    //Chronos
    GlassShard, SwirlyGlasses, MagicKarp, UShapedBarrel, CrackedTimeWatch
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item Data")]
public class ItemData : ScriptableObject
{
    public ItemID id;
    public string itemName;
    [TextArea] public string description;
    public ItemRarity rarity;

    [Header("Visuals (leave empty to use placeholder color)")]
    public Color placeholderColor = Color.white;
    public GameObject worldModelPrefab; // drag real 3D model here later
    public Sprite icon;                  // drag real UI icon here later
}