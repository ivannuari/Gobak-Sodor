using UnityEngine;

[CreateAssetMenu(menuName = "Game/Character Database", fileName = "CharacterDatabase")]
public class CharacterDatabase : ScriptableObject
{
    public CharacterDefinition[] characters;

    public int Count => characters != null ? characters.Length : 0;

    public CharacterDefinition GetByIndex(int index)
    {
        if (characters == null || characters.Length == 0) return null;
        index = Mathf.Clamp(index, 0, characters.Length - 1);
        return characters[index];
    }

    public int FindIndexById(string id)
    {
        if (characters == null) return -1;
        for (int i = 0; i < characters.Length; i++)
            if (characters[i] != null && characters[i].characterId == id)
                return i;
        return -1;
    }
}
