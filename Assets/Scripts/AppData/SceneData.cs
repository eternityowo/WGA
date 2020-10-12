using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace WGA.AppData
{
    public class SceneData : MonoBehaviour
    {
        [Header("Camera")]
        public Camera Camera = default;

        [Header("UI Elements")]
        public Canvas Canvas = default;
        public RectTransform PauseScreen = default;
        public Text Text = default;

        [Header("Scene object")]
        [Header("Tilemap&Tiles")]
        public Tilemap IndicatorTilemap = default;
        public Tilemap CoreTilemap = default;
        public Tilemap SelectTilemap = default;

        public Tile Empty;

        public Tile CardPlace;
        public List<Tile> Cards;

        public Tile Select;
        public Tile Move;
    }
}