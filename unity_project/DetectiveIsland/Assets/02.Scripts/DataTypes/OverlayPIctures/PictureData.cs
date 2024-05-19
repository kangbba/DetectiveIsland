using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PictureData : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _SpriteRenderer;
    [SerializeField] private EPictureID _id;
    public EPictureID Id { get => _id; }
    public SpriteRenderer SpriteRenderer { get => _SpriteRenderer; }
}
