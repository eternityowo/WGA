using Leopotam.Ecs;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using WGA.AppData;

namespace WGA.Components
{
    internal struct ChangeGameStateRequest
    {
        public GameStates State;
    }

    internal struct IsCard
    {
    }

    internal struct IsEmpty
    {
    }

    //internal struct Position
    //{
    //    public Vector3Int Value;
    //}

    internal struct Selected
    {
        public Vector3Int Value;
    }

    internal struct MoveEvent
    {
        public Vector3Int From;
        public Vector3Int To;
    }

    internal struct DrawOutlineEvent
    {
        public Vector3Int Value;
    }

    internal struct EmptyNeighbors
    {
        public EcsEntity[] Value;
    }

    internal struct ClearOutlineEvent
    {
    }
}