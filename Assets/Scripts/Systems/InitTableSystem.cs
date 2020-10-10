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
        private readonly GameConfiguration _config = null;
        private readonly SceneData _sceneData = null;
        private readonly TableData _tableData = null;
        private readonly GameContext _context = null;

        private readonly EcsWorld _world;

        void IEcsInitSystem.Init()
        {
            var coreTilemap = _sceneData.CoreTilemap;

            BoundsInt bounds = coreTilemap.cellBounds;
            TileBase[] allTiles = coreTilemap.GetTilesBlock(bounds);
            _context.Table = new TableModel<EcsEntity>(bounds.size.x, bounds.size.y);

            var cards = _tableData.Cards;
            var totalCardCount = allTiles.Count(t => t == _tableData.CardPlace);
            var typeCardCount = totalCardCount / cards.Count();

            Dictionary<Tile, int> cardOfTypeCount = cards.ToDictionary(k => k, v => typeCardCount);

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    TileBase tile = allTiles[x + y * bounds.size.x];

                    if (tile == _tableData.CardPlace)
                    {
                        var rndCard = cards.Random();
                        if (--cardOfTypeCount[rndCard] == 0)
                        {
                            cards.Remove(rndCard);
                        }
                        var v3int = new Vector3Int(x, y, 0);

                        var card = _world.NewEntity();
                        card.Get<IsCard>();
                        //card.Get<Position>().Value = v3int;

                        coreTilemap.SetTile(v3int, rndCard);
                        _context.Tiles.Add(v3int, card);
                    }

                    if (tile == _tableData.Empty)
                    {
                        var v3int = new Vector3Int(x, y, 0);

                        var empty = _world.NewEntity();
                        empty.Get<IsEmpty>();
                        //empty.Get<Position>().Value = v3int;

                        coreTilemap.SetTile(v3int, null);
                        _context.Tiles.Add(v3int, empty);
                    }
                }
            }
        }


    }

    internal sealed class ClickSystem : IEcsRunSystem
    {
        private readonly SceneData _sceneData = null;
        private readonly GameContext _context = null;

        private readonly EcsFilter<IsCard> cardFilter = null;
        //private readonly EcsFilter<IsCard, Selected, Position> _selectedFilter = null;
        private readonly EcsFilter<IsCard, Selected> _selectedFilter = null;

        private readonly EcsWorld _world = null;

        public void Run()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            _world.SendMessage(new ClearOutlineEvent());

            Vector3 sreenPos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(sreenPos);
            Vector3Int tileGridPos = _sceneData.CoreTilemap.WorldToCell(worldPos);

            foreach (var i in _selectedFilter)
            {
                var selectedCard = _selectedFilter.GetEntity(i);
                ref var fromPos = ref selectedCard.Get<Selected>();
                if (fromPos.Value != tileGridPos)
                {
                    selectedCard.Get<MoveEvent>() = new MoveEvent() { From = fromPos.Value, To = tileGridPos };
                    selectedCard.Del<Selected>();
                    return;
                }
            }

            if(_context.Tiles.TryGetValue(tileGridPos, out EcsEntity card))
            {
                if(card.Has<IsCard>())
                {
                    card.Get<Selected>();
                    _world.SendMessage(new DrawOutlineEvent() { Value = tileGridPos });
                }
            }

            if (_context.Tiles.TryGetValue(tileGridPos, out EcsEntity card))
            {
                if (card.Has<IsCard>())
                {
                    card.Get<Selected>();
                    _world.SendMessage(new DrawOutlineEvent() { Value = tileGridPos });
                }
            }
        }
    }

    internal sealed class CheckNeighborsSystem : IEcsRunSystem
    {
        private readonly GameContext _context = null;
        private readonly SceneData _sceneData = null;
        private readonly TableData _tableData = null;

        private readonly EcsWorld _world;

        private readonly EcsFilter<IsCard, Selected> _selectedFilter = null;

        void IEcsRunSystem.Run()
        {

            foreach (var i in _selectedFilter)
            {
                ref var position = ref _selectedFilter.Get2(i);
                // TODO Get position of tile
                _selectedFilter.GetEntity(i).Get<EmptyNeighbors>().Value = _context.Tiles.CheckEmptyNeighbors<IsEmpty>(position.Value, 1).ToArray();
            }
        }
    }


    // TODO Divide on Check Empty and Draw Outline
    internal sealed class DrawOutlineSystem : IEcsRunSystem
    {
        private readonly GameContext _context = null;
        private readonly SceneData _sceneData = null;
        private readonly TableData _tableData = null;

        private readonly EcsWorld _world;

        private readonly EcsFilter<DrawOutlineEvent> drawFilter = null;
        private readonly EcsFilter<ClearOutlineEvent> clearFilter = null;
        void IEcsRunSystem.Run()
        {
            if(!clearFilter.IsEmpty())
            {
                _sceneData.SelectTilemap.ClearAllTiles();
            }

            foreach(var i in drawFilter)
            {
                ref var position = ref drawFilter.Get1(i);
                _sceneData.SelectTilemap.SetTile(position.Value, _tableData.Select);
                foreach(var n in _context.Tiles.CheckEmptyNeighbors<IsEmpty>(position.Value, 1))
                {
                    _sceneData.SelectTilemap.SetTile(n.Get<Selected>().Value, _tableData.Move);
                }
            }
        }
    }

    internal sealed class MoveSystem : IEcsRunSystem
    {
        private readonly GameContext _context = null;
        private readonly SceneData _sceneData = null;
        private readonly TableData _tableData = null;

        private readonly EcsWorld _world;

        private readonly EcsFilter<IsCard, MoveEvent> moveFilter = null;

        void IEcsRunSystem.Run()
        {
            foreach (var i in moveFilter)
            {
                ref var move = ref moveFilter.Get2(i);

                var entityFrom = _context.Tiles[move.From];
                var entityTo = _context.Tiles[move.To];

                //entityFrom.Get<Position>().Value = move.To;
                //entityTo.Get<Position>().Value = move.From;

                _context.Tiles[move.From] = entityTo;
                _context.Tiles[move.To] = entityFrom;

                var tileFrom = _sceneData.CoreTilemap.GetTile(move.From);
                var tileTo = _sceneData.CoreTilemap.GetTile(move.To);

                _sceneData.CoreTilemap.SetTile(move.From, tileTo);
                _sceneData.CoreTilemap.SetTile(move.To, tileFrom);
            }
        }

        //[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public void SwapTile(TileBase start, TileBase end)
        //{
        //    var temp = this[start];
        //    this[start] = this[end];
        //    this[end] = temp;
        //}
    }
}
