using System.Collections;
using System.Collections.Generic;
using Aroka.ArokaUtils;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class Picture : ArokaEffector
{
    [SerializeField] private EPictureID _id;
    public EPictureID Id { get => _id; }

}
