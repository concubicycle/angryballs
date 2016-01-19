using GameStateManagement;
using System;
using UnityEngine;

namespace Assets.GameStateManagement
{
    public class PlayingGameState : GameStateBase<PlayingGameState>
    {
        public override void EnterState()
        {
            Time.timeScale = 1.0f;
        }
    }
}
