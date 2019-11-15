using UnityEngine;

[CreateAssetMenu(fileName = "New Epuipment", menuName = "Item/Equipment")]
public class Equipment : ScriptableObject
{
    public Sprite icon = null;
    public new string name = "Equipment Name";
    public string description = "Description of this item";

    public int addHealth = 0;

    [Range(0, 40)]
    public int strength;
    [Range(0, 50)]
    public int defence;
    [Range(0, 50)]
    public int manaPoint;
    [Range(0, 40)]
    public int weight;

    public Sprite equipedSprite = null;

    [Range(1, 5)]
    public int rarity = 1;

    public ParticleSystem passiveParticle = null;
    public ParticleSystem activeParticle = null;
    
}
