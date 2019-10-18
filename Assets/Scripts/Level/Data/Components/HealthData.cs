using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Level.Data.Components
{
    public class HealthData {
        private uint _totalHealth = 0;
        private uint _currentHealth = 0;

        public HealthData Load(uint totalValue) {
            _totalHealth = _currentHealth = totalValue;
            return this;
        }

        public HealthData Load(HealthData other) {
            _totalHealth = other._totalHealth;
            _currentHealth = other._currentHealth;
            return this;
        }

        public void TakeDamage(uint damageValue) {
            if ( damageValue > _currentHealth ) {
                _currentHealth = 0;
            } else {
                _currentHealth -= damageValue;
            }
        }

        public void Heal(uint healValue) {
            _currentHealth += healValue;
            if ( _currentHealth > _totalHealth ) {
                _currentHealth = _totalHealth;
            }
        }

        public void Clear() {
            _totalHealth = _currentHealth = 0;
        }

        public uint TotalHealth { get { return _totalHealth; } }
        public uint CurrentHealth { get { return _currentHealth; } }
        public float Ratio { get { return (float)_currentHealth / _totalHealth; } }
    }
}
