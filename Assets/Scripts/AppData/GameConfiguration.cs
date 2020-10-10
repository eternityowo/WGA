using System;
using System.Collections.Generic;
using UnityEngine;

namespace WGA.AppData
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "WGA/GameConfig", order = 0)]
    public class GameConfiguration : ScriptableObject
    {
        [Header("Context Start Settings")]
        public int CardTypeCount = 5;

        //[Header("Camera Settings")]
    }
}