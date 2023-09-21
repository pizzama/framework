using System;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework
{
    //State切换条件，每一个状态都会持有一组切换到其它状态的条件
    public interface IFSMTransition
    {
        bool IsValid(); //状态是否可以切换
        IFSMState GetNextFSMState(); //通过状态检查进行条件切换
        void Transition(); //状态切换动作，做清理和准备工作
        void SetFSM(FSM value);
    }

    public interface IFSMState
    {
        void InitState(); //创建时执行一次
        void EnterState(); //每次进入状态执行
        void UpdateState(); //每次更新状态执行
        void ExitState(); //每次退出执行
        void HandleInput(); //处理输入操作
        void SetFSM(FSM value);
        List<IFSMTransition> GetTransitions();
        public T AddTransition<T>() where T : IFSMTransition, new();
        public T GetTransition<T>() where T : IFSMTransition;

        public bool CouldTransition(); //状态是否启用trans的开关检查

        public string ToName();
    }

    public abstract class FSMTransition : IFSMTransition
    {
        public FSM Machine;
        public abstract IFSMState GetNextFSMState();

        public abstract bool IsValid();

        public void SetFSM(FSM value)
        {
            Machine = value;
        }

        public virtual void Transition()
        {
        }
    }

    public abstract class FSMState : IFSMState
    {
        protected bool couldTransition = true; //是否可以开始转换检查的开关，默认打开，每帧检查一次
        public FSM Machine;
        private List<IFSMTransition> _transitions = new List<IFSMTransition>();
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

        public void SetFSM(FSM value)
        {
            Machine = value;
        }

        public List<IFSMTransition> GetTransitions()
        {
            return _transitions;
        }

        public T AddTransition<T>() where T : IFSMTransition, new()
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

        public T GetTransition<T>() where T : IFSMTransition
        {
            for (var i = 0; i < _transitions.Count; i++)
            {
                IFSMTransition tn = _transitions[i];
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

    public class FSM
    {
        protected Dictionary<string, IFSMState> mStates = new Dictionary<string, IFSMState>();
        //状态id，可以定义成枚举
        public void AddState(IFSMState state)
        {
            AddState(state.ToName(), state);
        }
        public void AddState(string id, IFSMState state)
        {
            mStates.Add(id, state);
            state.SetFSM(this);
            //添加完管理器之后在初始化状态
            state.InitState();
        }

        public IFSMState GetState(string id)
        {
            IFSMState state;
            mStates.TryGetValue(id, out state);
            if (state == null)
                throw new Exception("state id is not register:" + id);
            return state;
        }

        private IFSMState _activeState;

        public UnityEngine.Object BlackBoard { get; set; } //状态机黑板

        public IFSMState CurrentState => _activeState;

        public FSM()
        {}

        //状态机有两种切换模式。 一种是状态机强制切换。另一种是每一个状态自己检查切换
        public void ChangeState(string id)
        {
            if (id.Equals(CurrentState.ToName())) return;
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

        public void ChangeState(IFSMState state)
        {
            if (state.ToName().Equals(_activeState.ToName())) return;
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

        public void HandleInput()
        {
            _activeState?.HandleInput();
        }

        public void Update()
        {
            // Debug.Log(_activeState.ToName());
            checkState();
            _activeState?.UpdateState();
        }

        public void Clear()
        {
            _activeState = null;
            mStates.Clear();
        }

        private void checkState()
        {
            if (_activeState == null || !_activeState.CouldTransition())
                return;
            List<IFSMTransition> trans = _activeState.GetTransitions();
            if (trans != null)
            {
                for (var i = 0; i < trans.Count; i++)
                {
                    IFSMTransition tran = trans[i];
                    if (tran.IsValid())
                    {
                        IFSMState state = tran.GetNextFSMState();
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