using System.Collections.Generic;
using SHLibrary.ObserverView;
using UnityEngine;

namespace Game.Tank
{
    public class TankView : ObserverViewBase<TankModel>
    {
        [SerializeField]
        private Transform _bodyTransform;

        [SerializeField]
        private Transform _helmetPoint;

        [SerializeField]
        private List<MeshRenderer> _bodyRenderers;

        [SerializeField]
        private Rigidbody _rigidbody;

        public Transform HelmetPoint { get { return _helmetPoint; } }

        public Transform Body { get { return _bodyTransform; } }

        public List<MeshRenderer> BodyRenderers { get { return _bodyRenderers; } }

        public Rigidbody Rigidbody { get { return _rigidbody;} }

        public GameObject Helmet { get; set; }

        protected override void OnModelChanged()
        {
        }
    }
}