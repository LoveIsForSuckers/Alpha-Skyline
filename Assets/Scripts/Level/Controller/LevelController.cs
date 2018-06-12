using Assets.Scripts.Level.Controller.Enemies;
using Assets.Scripts.Level.Controller.Internal;
using Assets.Scripts.Level.Data.Components;
using Assets.Scripts.Level.Data.Entity;
using Assets.Scripts.Level.Logic;
using Assets.Scripts.Level.View;
using System;
using System.Collections.Generic;
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

        private List<EnemyController> _enemies;

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

            _enemies = new List<EnemyController>();
        }

        private void InitEnemy()
        {
            var entity = new FieldEntity();
            entity.Name = "Enemy";

            var moveData = Movement.Register(entity);
            moveData.Position = new Vector2(4, 4);
            moveData.Speed = 3.0f;
            var collision = Collision.Register(entity);
            collision.Radius = 0.5f;
            collision.IsPlayerOwned = false;

            var view = Transform.FindObjectOfType<EnemyShipView>();
            view.Init(entity);

            var enemy = new EnemyController();
            enemy.Load(entity);
            _enemies.Add(enemy);
        }

        private void InitPlayer()
        {
            var entity = new InputControlledEntity();
            entity.Name = "Player";

            var moveData = Movement.Register(entity);
            moveData.Position = new Vector2(0, -7);
            moveData.Speed = 20.0f;

            var collision = Collision.Register(entity);
            collision.IsPlayerOwned = true;
            collision.Radius = 0.5f;

            var projectile = new ProjectileData()
            {
                BaseDamage = 3, DamageVariance = 1, DestroyOnCollision = false, MaxLifetime = 4.0f
            };

            var weapon = new WeaponData()
            {
                Projectile = projectile, FireProjectileSpeed = 6f, RateOfFire = 4f,
                FirePositionOffset = new Vector2(0f, 0.6f),
                FireDirection = new Vector2(0f, 1f)
            };
            Weapon.RegisterWeapon(entity, weapon);

            var weapon2 = new WeaponData().Load(weapon);
            weapon2.FirePositionOffset = new Vector2(0.4f, 0.2f);
            weapon2.FireDirection = new Vector2(0.12f, 1f);
            Weapon.RegisterWeapon(entity, weapon2);

            var weapon3 = new WeaponData().Load(weapon);
            weapon3.FirePositionOffset = new Vector2(-0.4f, 0.2f);
            weapon3.FireDirection = new Vector2(-0.12f, 1f);
            Weapon.RegisterWeapon(entity, weapon3);

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
            Collision.CollisionDetected += OnCollisionDetected;

            _isPlaying = true;
        }

        public void StopPlay()
        {
            if (Listener == null)
                return;

            Projectile.EntityCreated -= Listener.OnProjectileCreated;
            Projectile.EntityDestroying -= Listener.OnProjectileDestroying;
            Collision.CollisionDetected -= OnCollisionDetected;

            _isPlaying = false;
        }

        public void Update(float deltaTime)
        {
            if (!_isPlaying || Time.timeScale <= 0)
                return;
            
            Movement.Update(deltaTime);
            Weapon.Update(deltaTime);
            Projectile.Update(deltaTime);
            Collision.Update(deltaTime);

            foreach (var enemy in _enemies)
                enemy.Update(deltaTime);
        }

        private void OnCollisionDetected(FieldEntity entity1, FieldEntity entity2, Vector2 position)
        {
            if (Listener != null)
                Listener.OnCollisionDetected(entity1, entity2, position);

            if (entity1.Projectile != null)
                entity1.Projectile.HadCollision = true;
            else if (entity2.Projectile != null)
                entity2.Projectile.HadCollision = true;
        }
    }
}
