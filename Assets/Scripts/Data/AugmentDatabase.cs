using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "AugmentDatabase",
    menuName = "Bungeoppang/Augment Database"
)]
public class AugmentDatabase : ScriptableObject
{
    public List<AugmentData> augments = new();
}