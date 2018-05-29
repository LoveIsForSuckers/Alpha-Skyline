using Assets.Scripts.Level.Data;
using Assets.Scripts.Level.Data.Entity;
using Assets.Scripts.Level.Logic;
using Assets.Scripts.Level.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Level.Controller
{
    public class LevelMainController
    {
        public static LevelMainController Instance { get; private set; }

        public MovementController movementController;

        public LevelMainController()
        {
            if (Instance != null)
                throw new InvalidOperationException("LevelMainController already exists");

            Instance = this;

            InitControllers();
            InitPlayer();
        }

        private void InitControllers()
        {
            movementController = new MovementController();
        }

        private void InitPlayer()
        {
            var entity = new InputControlledEntity();
            var moveData = movementController.RegisterMovable(entity);

            var view = Transform.FindObjectOfType<PlayerShipView>();
            view.Init(entity);
            moveData.Speed = 4.0f;

            PlayerInputLogic.Instance.entity = entity;
        }
    }
}
