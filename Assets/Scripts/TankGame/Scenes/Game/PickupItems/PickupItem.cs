using System;
using SHLibrary;
using TankGame.Domain.GameItem;
using UnityEngine;

namespace Game.PickupItems
{
    public class PickupItem : UnityBehaviourBase
    {
        public GameItemType ItemType { get; private set; }

        public long WorldId { get; private set; }

        public event Action<PickupItem, Collider> Interacted;

        public void Initialize(long worldId, GameItemType itemType)
        {
            WorldId = worldId;
            ItemType = itemType;

            gameObject.SetActive(true);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Interacted.SafeRaise(this, collision.collider);
        }
    }
}