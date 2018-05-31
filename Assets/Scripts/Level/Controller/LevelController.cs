using Assets.Scripts.Level.Controller.Internal;
using Assets.Scripts.Level.Data.Entity;
using Assets.Scripts.Level.Logic;
using Assets.Scripts.Level.View;
using System;
using UnityEngine;

namespace Assets.Scripts.Level.Controller
{
    public class LevelController
    {
        public static LevelController Instance { get; private set; }

        public MovementController Movement { get; private set; }
        public WeaponController Weapon { get; private set; }
        public ProjectileController Projectile { get; private set; }

        public LevelController()
        {
            if (Instance != null)
                throw new InvalidOperationException("LevelController already exists");

            Instance = this;

            InitControllers();
            InitPlayer();
        }
        
        private void InitControllers()
        {
            Movement = new MovementController();
            Weapon = new WeaponController();
            Projectile = new ProjectileController();
        }

        private void InitPlayer()
        {
            var entity = new InputControlledEntity();
            var moveData = Movement.Register(entity);

            var view = Transform.FindObjectOfType<PlayerShipView>();
            view.Init(entity);
            moveData.Speed = 4.0f;

            PlayerInputLogic.Instance.entity = entity;
        }
    }
}
