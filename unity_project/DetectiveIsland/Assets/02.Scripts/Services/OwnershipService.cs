using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OwnershipService
{
    private const string PlacePrefix = "Place_";
    private const string ItemPrefix = "Item_";
    private const string BackupSuffix = "BackUp_";

    // 플레이어가 특정 장소를 소유하게 하거나 잃게 만드는 메서드
    public static void SetPlaceOwnership(EPlaceID placeID, bool ownsPlace)
    {
        int placeIDValue = (int)placeID;
        string key = $"{PlacePrefix}{placeIDValue}";
        PlayerPrefs.SetInt(key, ownsPlace ? 1 : 0);
        PlayerPrefs.SetInt($"{key}{BackupSuffix}", ownsPlace ? 1 : 0);
        PlayerPrefs.Save();
    }

    // 플레이어가 특정 장소를 알고 있는지 확인하는 메서드
    public static bool GetPlaceOwnership(EPlaceID placeID)
    {
        int placeIDValue = (int)placeID;
        string key = $"{PlacePrefix}{placeIDValue}";
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    // 플레이어가 특정 아이템을 소유하게 하거나 잃게 만드는 메서드
    public static void SetHasItem(EItemID itemID, bool ownsItem)
    {
        int itemIDValue = (int)itemID;
        string key = $"{ItemPrefix}{itemIDValue}";
        PlayerPrefs.SetInt(key, ownsItem ? 1 : 0);
        PlayerPrefs.SetInt($"{key}{BackupSuffix}", ownsItem ? 1 : 0);
        PlayerPrefs.Save();
    }

    // 플레이어가 특정 아이템을 소유하고 있는지 확인하는 메서드
    public static bool HasItem(EItemID itemID)
    {
        int itemIDValue = (int)itemID;
        string key = $"{ItemPrefix}{itemIDValue}";
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    // 업데이트 시 백업 데이터 복원
    public static void RestoreBackupData()
    {
        // 장소 데이터 복원
        foreach (EPlaceID placeID in System.Enum.GetValues(typeof(EPlaceID)))
        {
            int placeIDValue = (int)placeID;
            string key = $"{PlacePrefix}{placeIDValue}";
            if (PlayerPrefs.HasKey($"{key}{BackupSuffix}"))
            {
                int backupValue = PlayerPrefs.GetInt($"{key}{BackupSuffix}");
                PlayerPrefs.SetInt(key, backupValue);
            }
        }

        // 아이템 데이터 복원
        foreach (EItemID itemID in System.Enum.GetValues(typeof(EItemID)))
        {
            int itemIDValue = (int)itemID;
            string key = $"{ItemPrefix}{itemIDValue}";
            if (PlayerPrefs.HasKey($"{key}{BackupSuffix}"))
            {
                int backupValue = PlayerPrefs.GetInt($"{key}{BackupSuffix}");
                PlayerPrefs.SetInt(key, backupValue);
            }
        }

        PlayerPrefs.Save();
    }
}