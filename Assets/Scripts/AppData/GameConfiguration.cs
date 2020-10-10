using System;
using System.Collections.Generic;
using UnityEngine;

namespace WGA.AppData
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "WGA/GameConfig", order = 0)]
    public class GameConfiguration : ScriptableObject
    {
        public string enemyBlueprintsPath = "Blueprints/Enemies/";
        public string towerBlueprintsPath = "Blueprints/Towers/";

        [Header("Context Start Settings")]
        public int CardTypeCount = 5;

        [Header("Camera Settings")]
        public float CameraSpeed = 10f;
        public int ScreenBorderInPx = 40;
    }
}