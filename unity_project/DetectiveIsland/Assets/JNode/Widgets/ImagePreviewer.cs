using UnityEngine;
using System.IO;

public class ImagePreviewer
{
    private string currentFilePath = string.Empty;
    private Texture2D cachedTexture = null;

    public void ImagePreview(string filePath, float width, float height, Vector2 worldPos)
    {
        if (currentFilePath == filePath)
        {
            // 이미 로드된 텍스처가 있다면 그것을 사용
            if (cachedTexture != null)
            {
                DrawTexture(cachedTexture, width, height, worldPos);
            }
            return;
        }

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("File not found: " + filePath);
            return;
        }

        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData))
        {
            currentFilePath = filePath;
            cachedTexture = texture;
            DrawTexture(texture, width, height, worldPos);
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
