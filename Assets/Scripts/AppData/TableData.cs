using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WGA.AppData
{
    public class TableData : MonoBehaviour
    {
        public Tile Empty;
        //public Tile Freezy;

        public Tile CardPlace;
        public List<Tile> Cards;

        public Tile Select;
        public Tile Move;
    }
}