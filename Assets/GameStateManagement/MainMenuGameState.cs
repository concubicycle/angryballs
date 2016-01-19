using System;
using GameStateManagement;
using UnityEngine;

namespace Assets.GameStateManagement
{
    class MainMenuGameState : GameStateBase<MainMenuGameState>
    {
        public override void EnterState()
        {
            Time.timeScale = 0;
        }
    }
}
