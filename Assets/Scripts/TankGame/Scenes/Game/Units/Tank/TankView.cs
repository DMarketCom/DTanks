using System.Collections.Generic;
using SHLibrary.ObserverView;
using UnityEngine;

namespace Game.Tank
{
    public class TankView : ObserverViewBase<TankModel>
    {
        [SerializeField]
        public Transform Body;
        [SerializeField]
        public Transform GunPoint;
        [SerializeField]
        public List<MeshRenderer> BodуRenders;

        [SerializeField]
        private Transform _helmetPoint;

        public Transform HelmetPoint { get { return _helmetPoint; } }

        public GameObject Helmet { get; set; }

        protected override void OnModelChanged()
        {
        }
    }
}