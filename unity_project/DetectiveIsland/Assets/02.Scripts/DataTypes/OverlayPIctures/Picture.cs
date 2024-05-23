using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Picture : ArokaSpriteEffector
{
    [SerializeField] private EPictureID _id;
    public EPictureID Id { get => _id; }

    private void Start(){
        SpriteRenderer.sortingOrder = 1;
    }
}
