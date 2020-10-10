using Assets.Scripts.Extensions;
using Leopotam.Ecs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using WGA.AppData;
using WGA.Components;
using WGA.Extensions;

namespace WGA.Systems
{
    internal sealed class InitTableSystem : IEcsInitSystem
    {
        private readonly SceneData _sceneData = null;
        private readonly TableData _tableData = null;
        private readonly GameContext _context = null;

        private readonly EcsWorld _world;

        void IEcsInitSystem.Init()
        {
            var indTilemap = _sceneData.IndicatorTilemap;
            var boundsInd = indTilemap.cellBounds;
            TileBase[] indicatorTiles = indTilemap.GetTilesBlock(boundsInd);

            _context.WinColumns = new Dictionary<int, TileBase>()
            {
                { 0, indicatorTiles[0]},
                { 2, indicatorTiles[2]},
                { 4, indicatorTiles[4]},
            };


            var coreTilemap = _sceneData.CoreTilemap;
            var bounds = coreTilemap.cellBounds;
            TileBase[] coreTiles = coreTilemap.GetTilesBlock(bounds);

            _context.Table = new TableModel(bounds.size.x, bounds.size.y);

            var cards = _tableData.Cards.Select(x => x).ToList();

            var totalCardCount = coreTiles.Count(t => t == _tableData.CardPlace);
            var typeCardCount = totalCardCount / cards.Count();

            Dictionary<Tile, int> cardOfTypeCount = cards.ToDictionary(k => k, v => typeCardCount);

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    TileBase tile = coreTiles[x + y * bounds.size.x];
                    var v3int = new Vector3Int(x, y, 0);

                    if (tile == _tableData.CardPlace)
                    {
                        var rndCard = cards.Random();
                        if (--cardOfTypeCount[rndCard] == 0)
                        {
                            cards.Remove(rndCard);
                        }

                        var card = _world.NewEntity();
                        card.Get<IsCard>().Type = rndCard;

                        coreTilemap.SetTile(v3int, rndCard);
                        _context.Table[v3int] = card;
                        continue;
                    }

                    if (tile == _tableData.Empty)
                    {
                        var empty = _world.NewEntity();
                        empty.Get<IsEmpty>();

                        coreTilemap.SetTile(v3int, null);
                        _context.Table[v3int] = empty;
                        continue;
                    }

                    _context.Table[v3int] = EcsEntity.Null;
                }
            }
        }
    }
}
