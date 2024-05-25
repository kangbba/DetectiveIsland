using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class QuestManager
{
    private static Dictionary<QuestID, Quest> _activeQuests = new Dictionary<QuestID, Quest>();
    private static List<Quest> _loadedQuests = new List<Quest>();
    private static bool _isLoaded = false;

    public static void Load()
    {
        if (_isLoaded) return;

        _loadedQuests.Clear();
        var questPrefabs = Resources.LoadAll<GameObject>("QuestPrefabs");

        foreach (var prefab in questPrefabs)
        {
            var questComponent = prefab.GetComponent<Quest>();
            if (questComponent != null)
            {
                _loadedQuests.Add(questComponent);
            }
        }

        Debug.Log($"Loaded {_loadedQuests.Count} quests from resources.");
        _isLoaded = true;
    }
    

    public static void StartQuest(QuestID questID)
    {
        if (_activeQuests.ContainsKey(questID))
        {
            Debug.LogWarning($"Quest {questID} is already active.");
            return;
        }

        var questPrefab = _loadedQuests.Find(q => q.QuestID == questID)?.gameObject;
        if (questPrefab == null)
        {
            Debug.LogError($"Quest prefab for {questID} not found!");
            return;
        }

        var questInstance = Object.Instantiate(questPrefab);
        var quest = questInstance.GetComponent<Quest>();
        if (quest == null)
        {
            Debug.LogError($"Quest component not found on prefab {questID}!");
            Object.Destroy(questInstance);
            return;
        }

        quest.StartQuest(() => _activeQuests.Remove(questID));
        _activeQuests[questID] = quest;
    }

    public static async UniTask WaitUntilQuestComplete(QuestID questID)
    {
        if (!_activeQuests.ContainsKey(questID))
        {
            StartQuest(questID);
        }

        var quest = _activeQuests[questID];
        await quest.WaitUntilComplete();
    }
}
