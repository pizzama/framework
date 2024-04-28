using System;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.StateMachine
{
    //State切换条件，每一个状态都会持有一组切换到其它状态的条件
    public interface ISFSMTransition
    {
        bool IsValid(); //状态是否可以切换
        ISFSMState GetNextFSMState(); //通过状态检查进行条件切换
        void Transition(); //状态切换动作，做清理和准备工作
        void SetFSM(SFSM value);
    }

    public interface ISFSMState
    {
        void InitState(); //创建时执行一次
        void EnterState(); //每次进入状态执行
        void UpdateState(); //每次更新状态执行
        void ExitState(); //每次退出执行
        void HandleInput(); //处理输入操作
        void SetFSM(SFSM value);
        List<ISFSMTransition> GetTransitions();
        public T AddTransition<T>() where T : ISFSMTransition, new();
        public T GetTransition<T>() where T : ISFSMTransition;

        public bool CouldTransition(); //状态是否启用trans的开关检查

        public string ToName();
    }

    public abstract class SFSMTransition : ISFSMTransition
    {
        public SFSM Machine;
        public abstract ISFSMState GetNextFSMState();

        public abstract bool IsValid();

        public void SetFSM(SFSM value)
        {
            Machine = value;
        }

        public virtual void Transition()
        {
        }
    }

    public abstract class SFSMState : ISFSMState
    {
        protected bool couldTransition = true; //状态条件检查的开关，默认打开，每帧检查一次
        public SFSM Machine;
        private List<ISFSMTransition> _transitions = new List<ISFSMTransition>();
        public virtual void InitState()
        {

        }

        public virtual void EnterState()
        {

        }

        public virtual void UpdateState()
        {

        }

        public virtual void ExitState()
        {

        }

        public virtual void HandleInput()
        {

        }

        public void SetFSM(SFSM value)
        {
            Machine = value;
        }

        public List<ISFSMTransition> GetTransitions()
        {
            return _transitions;
        }

        public T AddTransition<T>() where T : ISFSMTransition, new()
        {
            T rt = GetTransition<T>();
            if (rt == null)
            {
                rt = new T();
                _transitions.Add(rt);
            }
            rt.SetFSM(Machine);
            return rt;
        }

        public T GetTransition<T>() where T : ISFSMTransition
        {
            for (var i = 0; i < _transitions.Count; i++)
            {
                ISFSMTransition tn = _transitions[i];
                if (tn.GetType() == typeof(T))
                {
                    return (T)tn;
                }
            }

            return default;
        }

        public virtual string ToName()
        {
            Type classType = this.GetType();
            return classType.Name;
        }

        public bool CouldTransition()
        {
            return couldTransition;
        }
    }

    public class SFSM
    {
        protected Dictionary<string, ISFSMState> mStates;
        private ISFSMState _activeState;
        public UnityEngine.Object Owner { get; set; } //状态机的使用者

        public UnityEngine.Object BlackBoard { get; set; } //状态机黑板

        public ISFSMState CurrentState => _activeState;

        public SFSM()
        {
            mStates = new Dictionary<string, ISFSMState>();
        }

        //状态id，可以定义成枚举
        public void AddState(ISFSMState state)
        {
            AddState(state.ToName(), state);
        }
        public void AddState(string id, ISFSMState state)
        {
            mStates.Add(id, state);
            state.SetFSM(this);
            //添加完管理器之后在初始化状态
            state.InitState();
        }

        public ISFSMState GetState(string id)
        {
            ISFSMState state;
            mStates.TryGetValue(id, out state);
            if (state == null)
                throw new Exception("state id is not register:" + id);
            return state;
        }

        //状态机有两种切换模式。 一种是状态机强制切换。另一种是每一个状态自己检查切换
        public void ChangeState(string id)
        {
            if (_activeState != null && id.Equals(_activeState.ToName()))
                return;
            if (mStates.TryGetValue(id, out var state))
            {
                if (_activeState != null)
                {
                    _activeState?.ExitState();
                }
                _activeState = state;
                _activeState?.EnterState();
            }
        }

        public void ChangeState(ISFSMState state)
        {
            if (_activeState != null && state.ToName().Equals(_activeState.ToName())) return;
            if (mStates.ContainsKey(state.ToName()))
            {
                mStates[state.ToName()] = state;
            }
            else
            {
                AddState(state);
            }

            ChangeState(state.ToName());
        }

        public void ChangeState<T>() where T : ISFSMState
        {
            foreach (var state in mStates)
            {
                if (state.Value.GetType() == typeof(T))
                {
                    ChangeState(state.Value.ToName());
                    return;
                }
            }

            throw new NotFoundException("not found the state:" + typeof(T));
        }

        public void HandleInput()
        {
            _activeState?.HandleInput();
        }

        public void Update()
        {
            checkState();
            _activeState?.UpdateState();
        }

        public void Clear()
        {
            _activeState = null;
            mStates.Clear();
        }

        public List<ISFSMState> GetFSMStates()
        {
            return new List<ISFSMState>(mStates.Values);
        }

        private void checkState()
        {
            if (_activeState == null)
                return;
            if (!_activeState.CouldTransition())
            {
                List<ISFSMTransition> trans = _activeState.GetTransitions();
                if (trans != null)
                {
                    for (var i = 0; i < trans.Count; i++)
                    {
                        ISFSMTransition tran = trans[i];
                        if (tran.IsValid())
                        {
                            ISFSMState state = tran.GetNextFSMState();
                            Debug.Log("current state change to:" + state.ToName());
                            _activeState?.ExitState();
                            tran.Transition();
                            _activeState = state;
                            _activeState?.EnterState();
                        }
                    }
                }
            }
        }
    }
}