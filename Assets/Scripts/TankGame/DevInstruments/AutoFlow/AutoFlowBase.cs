using SHLibrary;
using SHLibrary.Logging;
using System.Collections;
using UnityEngine;

namespace DevInstruments.AutoFlow
{
    public abstract class AutoFlowBase<T> : UnityBehaviourBase
        where T : MonoBehaviour
    {
        protected T SceneController { get; private set; }

        protected virtual float DelayTime { get { return 0.1f; } }
        protected AutoFlowSettings Settings { get { return AutoFlowDefining.Settings; } }

        protected override IEnumerator Start()
        { 
            if (AutoFlowDefining.IsUseAutoFlow)
            {
                SceneController = GameObject.FindObjectOfType<T>();
                if (SceneController != null)
                {
                    yield return new WaitForSeconds(DelayTime);
                    DevLogger.Log("start auto flow " + GetType(), TankGameLogChannel.DevInstruments);
                    ApplyFlowOperation();
                }
            }
            yield return null;
        }

        protected abstract void ApplyFlowOperation();
    }
}
