using Assets.Scripts.Level.Controller.Internal;
using Assets.Scripts.Level.Data.Components;
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
        public CollisionController Collision { get; private set; }

        public ILevelListener Listener { get; set; }

        private bool _isPlaying;

        public LevelController()
        {
            if (Instance != null)
                throw new InvalidOperationException("LevelController already exists");

            Instance = this;

            InitControllers();
            InitEnemy();
            InitPlayer();
        }

        private void InitControllers()
        {
            Movement = new MovementController();
            Weapon = new WeaponController();
            Projectile = new ProjectileController();
            Collision = new CollisionController();
        }

        private void InitEnemy()
        {
            var entity = new FieldEntity();

            var moveData = Movement.Register(entity);
            moveData.Position = new Vector2(4, 4);
            var collision = Collision.Register(entity);
            collision.Radius = 0.5f;
            collision.IsPlayerOwned = false;

            var view = Transform.FindObjectOfType<EnemyShipView>();
            view.Init(entity);
        }

        private void InitPlayer()
        {
            var entity = new InputControlledEntity();

            var moveData = Movement.Register(entity);
            moveData.Position = new Vector2(0, -7);
            moveData.Speed = 4.0f;

            var collision = Collision.Register(entity);
            collision.IsPlayerOwned = true;
            collision.Radius = 0.5f;

            var projectile = new ProjectileData()
            {
                BaseDamage = 3, DamageVariance = 1, DestroyOnImpact = true, MaxLifetime = 4.0f
            };
            var weapon = new WeaponData()
            {
                Projectile = projectile, FireProjectileSpeed = 7.0f, RateOfFire = 1.0f,
                FirePositionOffset = new Vector2(0f, 0.3f),
                FireDirection = new Vector2(0f, 1f)
            };
            Weapon.RegisterWeapon(entity, weapon);

            var view = Transform.FindObjectOfType<PlayerShipView>();
            view.Init(entity);

            PlayerInputLogic.Instance.entity = entity;
        }

        public void StartPlay()
        {
            if (Listener == null)
                return;

            Projectile.EntityCreated += Listener.OnProjectileCreated;
            Projectile.EntityDestroying += Listener.OnProjectileDestroying;
            Collision.CollisionDetected += Listener.OnCollisionDetected;

            _isPlaying = true;
        }

        public void StopPlay()
        {
            if (Listener == null)
                return;

            Projectile.EntityCreated -= Listener.OnProjectileCreated;
            Projectile.EntityDestroying -= Listener.OnProjectileDestroying;
            Collision.CollisionDetected -= Listener.OnCollisionDetected;

            _isPlaying = false;
        }

        public void Update(float deltaTime)
        {
            if (!_isPlaying)
                return;

            Collision.Update(deltaTime);
            Movement.Update(deltaTime);
            Weapon.Update(deltaTime);
            Projectile.Update(deltaTime);
        }
    }
}
