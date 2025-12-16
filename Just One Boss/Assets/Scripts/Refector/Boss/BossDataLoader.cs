using UnityEngine;

public class BossDataLoader : MonoBehaviour
{
    public TextAsset bossJson;

    public BossData Load()
    {
        if (bossJson == null)
        {
            Debug.LogError("Boss JSON이 지정되지 않았습니다.");
            return null;
        }

        BossData data = JsonUtility.FromJson<BossData>(bossJson.text);
        return data;
    }
}
