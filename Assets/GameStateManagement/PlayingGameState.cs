using GameStateManagement;
using System;
using UnityEngine;

namespace Assets.GameStateManagement
{
    public class PlayingGameState : GameStateBase<PlayingGameState>
    {
        public override void StateStaticInitialize()
        {
            Time.timeScale = 1.0f;
        }
    }
}
