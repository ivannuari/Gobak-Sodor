using UnityEngine;

[CreateAssetMenu(menuName = "Game/Character Definition", fileName = "Char_")]
public class CharacterDefinition : ScriptableObject
{
    [Header("Identity")]
    public string characterId;       // unik, mis. "runner_red"
    public string displayName;       // nama di UI
    public Sprite icon;              // gambar di UI

    [Header("Prefab")]
    public GameObject playerPrefab;  // prefab siap main (Animator, collider, controller)

    [Header("Tuning (opsional)")]
    public float moveSpeed = 4f;     // jika controller membaca nilai ini
}
