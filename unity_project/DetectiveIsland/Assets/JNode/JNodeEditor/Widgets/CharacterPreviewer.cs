using UnityEngine;
using System.IO;

public class CharacterPreviewer
{
    private string currentFilePath = string.Empty;
    private Texture2D cachedTexture = null;

    public void CharacterPreview(string characterID, Vector2 size, Vector2 worldPos)
    {
        string filePath = $"Assets/JNode/Textures/Characters/{characterID}.png";
        if (!File.Exists(filePath))
        {
            return;
        }
        if (currentFilePath == filePath)
        {
            // 이미 로드된 텍스처가 있다면 그것을 사용
            if (cachedTexture != null)
            {
                DrawTexture(cachedTexture, size.x, size.y, worldPos);
            }
            return;
        }

        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData))
        {
            currentFilePath = filePath;
            cachedTexture = texture;
            DrawTexture(texture, size.x, size.y, worldPos);
        }
        else
        {
            Debug.LogError("Failed to load image: " + filePath);
        }
    }

    private void DrawTexture(Texture2D texture, float width, float height, Vector2 worldPos)
    {
        // Calculate the aspect ratio
        float aspectRatio = (float)texture.width / texture.height;
        float drawWidth = width;
        float drawHeight = height;

        // Adjust width and height to maintain aspect ratio
        if (width / height > aspectRatio)
        {
            drawWidth = height * aspectRatio;
        }
        else
        {
            drawHeight = width / aspectRatio;
        }

        // Use worldPos directly as GUI position
        Rect rect = new Rect(worldPos.x, worldPos.y, drawWidth, drawHeight);
        GUI.DrawTexture(rect, texture, ScaleMode.ScaleToFit, true);
    }
}
