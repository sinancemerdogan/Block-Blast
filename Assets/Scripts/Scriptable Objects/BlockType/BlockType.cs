using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Block Type", menuName = "Scriptable Objects/Block Type")]
public class BlockType : ScriptableObject {
    [Tooltip("Prefab of the block.")]
    [SerializeField] private Block prefab;

    [SerializeField] bool damageReceiver;
    [SerializeField] bool damageDealer;
    [SerializeField] bool creator;


    [Tooltip("The blocks that can damage this block.")]
    [SerializeField] private List<BlockType> blockTypesCanDamageThisBlock = new List<BlockType>();

    [Tooltip("The block's health.")]
    [SerializeField] private int health;

    [Tooltip("The block's damage.")]
    [SerializeField] private int damage;

    [Tooltip("The blocks that this block can create.")]
    [SerializeField] private List<BlockType> blockTypesThisBlockCanCreate = new List<BlockType>();

    [SerializeField] private GameObject blockBlastEffect;
    [SerializeField] private List<int> creationThresholds = new();
    [SerializeField] private List<Sprite> sprites = new();
    [SerializeField] private AudioClip blastSFX;

    [SerializeField] private string id;

    // Getters for fields
    public Block Prefab { get { return prefab; } }
    
    public int Health { get { return health; } }
    public List<BlockType> BlockTypesCanDamageThisBlock { get { return blockTypesCanDamageThisBlock; } }
    public GameObject BlockBlastEffect { get { return blockBlastEffect; } }
    public AudioClip BlastSFX { get { return blastSFX; } }
    public int Damage { get { return damage; } }
    public List<BlockType> BlockTypesThisBlockCanCreate { get { return blockTypesThisBlockCanCreate; } }
    public List<int> CreationThresholds { get { return creationThresholds; } }
    public List<Sprite> Sprites { get { return sprites; } }
    public string ID { get { return id; } }
}
