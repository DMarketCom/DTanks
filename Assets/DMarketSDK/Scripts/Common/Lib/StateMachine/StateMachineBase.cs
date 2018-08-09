using System;
using System.Collections.Generic;
using SHLibrary.ObserverView;
using UnityEngine;

namespace SHLibrary.StateMachine
{
    public abstract class StateMachineBase : MonoBehaviour, IStateMachine
    {
        private readonly List<CommandBase> _lstCommands = new List<CommandBase>();
        private readonly Dictionary<Type, StateBase> _mapStates = new Dictionary<Type, StateBase>();

        #if UNITY_EDITOR
        private string _basicName;
        #endif

        public Type PreviousState { get; private set; }

        public StateBase State { get; private set; }

        public virtual void BindState(StateBase state)
        {
            state.Binding(this, null);
        }

        public event Action<Type, Type> StateChanged;

        public virtual void Stop()
        {
            _lstCommands.ForEach(command => command.Terminate());
        }

        private void Awake()
        {
            #if UNITY_EDITOR
            _basicName = gameObject.name;
            #endif
        }

        protected virtual void Start()
        {
        }

        protected void AddStates(params StateBase[] state)
        {
            for (var i = 0; i < state.Length; i++)
            {
                _mapStates.Add(state[i].GetType(), state[i]);
                BindState(state[i]);
            }
        }

        protected T ApplyState<T>(params object[] args)
            where T : StateBase
        {
            ApplyState(typeof(T), args);
            return State as T;
        }

        protected void ApplyCommand(CommandBase command)
        {
            _lstCommands.Add(command);
            command.NeedApplyCommand += ApplyCommand;
            command.CommandFinished += OnCommandFinished;
            command.StateMachine = this;
            command.Start();
        }

        protected virtual void OnStateStarted()
        {
        }

        protected virtual void OnStateFinished()
        {
        }

        private void ApplyPreviousState(object[] args = null)
        {
            ApplyState(PreviousState, args);
        }

        private void ApplyState(Type type, object[] args = null)
        {
            if (State != null)
            {
                PreviousState = State.GetType();
                State.NeedApplyState -= ApplyState;
                State.NeedApplyPreviousState -= ApplyPreviousState;
                State.NeedApplyCommand -= ApplyCommand;
                _lstCommands.ForEach(command =>
                    {
                        if (!command.IsIndependentFromState)
                        {
                            command.Terminate();
                        }
                    });
                State.Finish();
                OnStateFinished();
            }
            if (!_mapStates.ContainsKey(type))
            {
                _mapStates.Add(type, Activator.CreateInstance(type) as StateBase);
                BindState(_mapStates[type]);
            }
            State = _mapStates[type];
            State.NeedApplyState += ApplyState;
            State.NeedApplyPreviousState += ApplyPreviousState;
            State.NeedApplyCommand += ApplyCommand;
            #if UNITY_EDITOR
            gameObject.name = _basicName + " -> " + State;
            #endif
            OnStateStarted();
            StateChanged.SafeRaise(PreviousState, type);
            State.Start(args);
        }

        private void Update()
        {
            if (State != null)
            {
                State.Update();
            }
            _lstCommands.ForEach(element => element.Update());
        }

        private void FixedUpdate()
        {
            if (State != null)
            {
                State.FixedUpdate();
            }
            _lstCommands.ForEach(element => element.FixedUpdate());
        }

        private void OnCommandFinished(CommandBase command)
        {
            command.CommandFinished -= OnCommandFinished;
            command.NeedApplyCommand -= ApplyCommand;
            _lstCommands.Remove(command);
        }
    }

    public abstract class StateMachineBase<T> : StateMachineBase
        where T : ViewBase
    {
        public T View;

        public override void BindState(StateBase state)
        {
            state.Binding(this, View);
        }
    }
}