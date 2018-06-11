using Assets.Scripts.Level.Controller;
using Assets.Scripts.Level.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Level.View
{
    public class FieldView : MonoBehaviour, ILevelListener
    {
        [SerializeField]
        private GameObject projectilePrefab;
        [SerializeField]
        private GameObject collisionMarkerPrefab;
        [SerializeField]
        private Transform projectileLayer;
        [SerializeField]
        private Transform overlayLayer;

        private IDictionary<FieldEntity, ProjectileView> _projectiles = new Dictionary<FieldEntity, ProjectileView>();

        public void Start()
        {
            // TODO: HACK
            LevelController.Instance.Listener = this;
            LevelController.Instance.StartPlay();
        }

        public void OnProjectileCreated(FieldEntity projectileEntity)
        {
            var projectileGO = Instantiate(projectilePrefab, projectileLayer);
            projectileGO.transform.localPosition = projectileEntity.Movement.Position;

            var projectile = projectileGO.GetComponent<ProjectileView>();
            projectile.Init(projectileEntity);
            _projectiles.Add(projectileEntity, projectile);
        }

        public void OnProjectileDestroying(FieldEntity projectileEntity)
        {
            var projectile = _projectiles[projectileEntity];
            _projectiles.Remove(projectileEntity);

            projectile.Dispose();
        }

        public void OnCollisionDetected(FieldEntity entity1, FieldEntity entity2, Vector2 position)
        {
            var marker = Instantiate(collisionMarkerPrefab, overlayLayer);
            marker.transform.localPosition = position;
            GameObject.Destroy(marker, 0.5f);
        }
    }
}
