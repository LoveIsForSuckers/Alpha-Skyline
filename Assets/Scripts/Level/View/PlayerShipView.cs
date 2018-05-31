﻿using Assets.Scripts.Level.Data.Entity;
using UnityEngine;

namespace Assets.Scripts.Level.View
{
    public class PlayerShipView : MonoBehaviour
    {
        private FieldEntity _data;

        public void Init(FieldEntity data)
        {
            _data = data;
        }

        public void Update()
        {
            transform.localPosition = _data.Movement.Position;
        }
    }
}
