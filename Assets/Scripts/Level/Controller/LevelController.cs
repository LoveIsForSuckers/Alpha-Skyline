﻿using Assets.Scripts.Level.Controller.Enemies;
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
        public HealthController Health { get; private set; }

        public ILevelListener Listener { get; set; }

        private List<EnemyController> _enemies;

        private List<FieldEntity> _shipsToRemove;

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
            Health = new HealthController();

            _enemies = new List<EnemyController>();
            _shipsToRemove = new List<FieldEntity>();
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

            var health = Health.Register(entity);
            health.Load(30);

            var projectile = new ProjectileData() {
                BaseDamage = 3,
                DamageVariance = 1,
                DestroyOnCollision = true,
                MaxLifetime = 4.0f
            };

            var weapon = new WeaponData() {
                Projectile = projectile,
                FireProjectileSpeed = 8f,
                RateOfFire = 2f,
                FirePositionOffset = new Vector2(0f, -0.6f),
                FireDirection = new Vector2(0f, -1f)
            };
            Weapon.RegisterWeapon(entity, weapon);

            // TODO: view should be not made here but in fieldView
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

            var health = Health.Register(entity);
            health.Load(30);

            var projectile = new ProjectileData()
            {
                BaseDamage = 3, DamageVariance = 1, DestroyOnCollision = true, MaxLifetime = 4.0f
            };

            var weapon = new WeaponData()
            {
                Projectile = projectile, FireProjectileSpeed = 19f, RateOfFire = 4f,
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

            // TODO: view should be not made here but in fieldView
            var view = Transform.FindObjectOfType<PlayerShipView>();
            view.Init(entity);

            PlayerInputLogic.Instance.entity = entity;
        }

        // TODO: ship controller ? (this and init)
        public void RemoveShip(FieldEntity entity) {
            if ( entity == PlayerInputLogic.Instance.entity ) {
                PlayerInputLogic.Instance.entity = null;
                var view = Transform.FindObjectOfType<PlayerShipView>();
                view.Clear();
            } else {
                var enemy = _enemies.Find(x => x.IsThis(entity));
                if ( enemy != null ) {
                    enemy.Clear();
                    _enemies.Remove(enemy);
                }
                var view = Transform.FindObjectOfType<EnemyShipView>();
                view.Clear();
            }

            if ( entity.Movement != null ) {
                Movement.Unregister(entity);
            }
            if ( entity.Collision != null ) {
                Collision.Unregister(entity);
            }
            if ( entity.Weapons != null ) {
                Weapon.UnregisterAllWeapons(entity);
            }
            if ( entity.Health != null ) {
                Health.Unregister(entity);
            }
        }

        public void StartPlay()
        {
            if (Listener == null)
                return;

            Projectile.EntityCreated += Listener.OnProjectileCreated;
            Projectile.EntityDestroying += Listener.OnProjectileDestroying;
            Collision.CollisionDetected += OnCollisionDetected;
            Health.EntityShouldDie += OnEntityShouldDie;

            _isPlaying = true;
        }

        public void StopPlay()
        {
            if (Listener == null)
                return;

            Projectile.EntityCreated -= Listener.OnProjectileCreated;
            Projectile.EntityDestroying -= Listener.OnProjectileDestroying;
            Collision.CollisionDetected -= OnCollisionDetected;
            Health.EntityShouldDie -= OnEntityShouldDie;

            _isPlaying = false;
        }

        public void Update(float deltaTime)
        {
            if (!_isPlaying || Time.timeScale <= 0)
                return;
            
            foreach ( var ship in _shipsToRemove ) {
                RemoveShip(ship);
            }
            _shipsToRemove.Clear();

            Movement.Update(deltaTime);
            Weapon.Update(deltaTime);
            Projectile.Update(deltaTime);
            Collision.Update(deltaTime);

            foreach (var enemy in _enemies)
                enemy.Update(deltaTime);
        }

        private void OnCollisionDetected(FieldEntity entity1, FieldEntity entity2, Vector2 position)
        {
            // TODO: stop communicating game logic through events, use update cycle
            // events should only be used for viewer
            Projectile.HandleCollision(entity1, entity2);
            Health.HandleCollision(entity1, entity2);

            if ( Listener != null )
                Listener.OnCollisionDetected(entity1, entity2, position);
        }

        private void OnEntityShouldDie(FieldEntity entity) {
            _shipsToRemove.Add(entity);
        }
    }
}
