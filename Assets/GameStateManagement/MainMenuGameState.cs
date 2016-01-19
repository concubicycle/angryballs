using System;
using GameStateManagement;
using UnityEngine;

namespace Assets.GameStateManagement
{
    class MainMenuGameState : GameStateBase<MainMenuGameState>
    {
        public override void StateStaticInitialize()
        {
            Time.timeScale = 0.0f;
        }
    }
}
