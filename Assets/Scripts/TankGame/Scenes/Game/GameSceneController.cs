﻿using Game.States;
using Game.Tank;
using System;
using System.Collections.Generic;
using SHLibrary.StateMachine;
using UnityEngine;
using Game.States.Online;
using Game.States.Offline;
using TankGame.GameClient;
using TankGame.Network.Client;

namespace Game
{
    public class GameSceneController : StateMachineBase<GameView>
    {
        public Action BackClicked;

        [SerializeField]
        public GameContext Context;
        
        public ITank Player;
        public List<ITank> Opponents = new List<ITank>();
        
        public IGameClient Client { private set; get; }
        public GameModel Model { get; private set; }

        public void Run(GameMode mode, IGameClient client)
        {
            Context.Run();
            Model = new GameModel(mode);
            View.ApplyModel(Model);
            Client = client;
            ApplyInitState();
        }

        public void RestartGame()
        {
            ApplyInitState();
        }

        public void Shutdown()
        {
            ApplyState<GameClearResourceBeforeExitState>();
            Context.Shutdown();
        }

        private void ApplyInitState()
        {
            if (Model.Mode == GameMode.Offline)
            {
                ApplyState<GameInitOfflineState>();
            }
            else
            {
                ApplyState<GameInitOnlineState>();
            }
        }
    }
}