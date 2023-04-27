using System;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework
{
    //State切换条件，每一个状态都会持有一组切换到其它状态的条件
    public interface IFSMTransition<TBlackBoard> where TBlackBoard : new()
    {
        bool IsValid(); //状态是否可以切换
        IFSMState<TBlackBoard> GetNextFSMState(); //通过状态检查进行条件切换
        void Transition(); //状态切换动作，做清理和准备工作
        void SetFSM(FSM<TBlackBoard> value);
    }

    public interface IFSMState<TBlackBoard> where TBlackBoard : new()
    {
        void Init(); //创建时执行一次
        void Enter(); //每次进入状态执行
        void Update(); //每次更新状态执行
        void Exit(); //每次退出执行
        void SetFSM(FSM<TBlackBoard> value);
        List<IFSMTransition<TBlackBoard>> GetTransitions();
        public T AddTransition<T>() where T : IFSMTransition<TBlackBoard>, new();
        public T GetTransition<T>() where T : IFSMTransition<TBlackBoard>;

        public bool CouldTransition(); //状态是否启用trans的开关检查

        public string ToName();
    }

    public abstract class FSMTransition<TBlackBoard> : IFSMTransition<TBlackBoard> where TBlackBoard : new()
    {
        public FSM<TBlackBoard> Machine;
        public abstract IFSMState<TBlackBoard> GetNextFSMState();

        public abstract bool IsValid();

        public void SetFSM(FSM<TBlackBoard> value)
        {
            Machine = value;
        }

        public virtual void Transition()
        {
        }
    }

    public abstract class FSMState<TBlackBoard> : IFSMState<TBlackBoard> where TBlackBoard : new()
    {
        protected bool couldTransition = true; //是否可以开始转换检查的开关，默认打开，每帧检查一次
        public FSM<TBlackBoard> Machine;
        private List<IFSMTransition<TBlackBoard>> _transitions = new List<IFSMTransition<TBlackBoard>>();
        public virtual void Init()
        {

        }

        public virtual void Enter()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Exit()
        {

        }

        public void SetFSM(FSM<TBlackBoard> value)
        {
            Machine = value;
        }

        public List<IFSMTransition<TBlackBoard>> GetTransitions()
        {
            return _transitions;
        }

        public T AddTransition<T>() where T : IFSMTransition<TBlackBoard>, new()
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

        public T GetTransition<T>() where T : IFSMTransition<TBlackBoard>
        {
            for (var i = 0; i < _transitions.Count; i++)
            {
                IFSMTransition<TBlackBoard> tn = _transitions[i];
                if (tn.GetType() == typeof(T))
                {
                    return (T)tn;
                }
            }

            return default;
        }

        public abstract string ToName();

        public bool CouldTransition()
        {
            return couldTransition;
        }
    }

    public class FSM<TBlackBoard> where TBlackBoard : new()
    {
        protected Dictionary<string, IFSMState<TBlackBoard>> mStates = new Dictionary<string, IFSMState<TBlackBoard>>();
        //状态id，可以定义成枚举
        public void AddState(IFSMState<TBlackBoard> state)
        {
            AddState(state.ToName(), state);
        }
        public void AddState(string id, IFSMState<TBlackBoard> state)
        {
            mStates.Add(id, state);
            state.SetFSM(this);
            //添加完管理器之后在初始化状态
            state.Init();
        }

        public IFSMState<TBlackBoard> GetState(string id)
        {
            IFSMState<TBlackBoard> state;
            mStates.TryGetValue(id, out state);
            if (state == null)
                throw new Exception("state id is not register:" + id);
            return state;
        }

        private IFSMState<TBlackBoard> _activeState;
        private string _currentStateId;

        public TBlackBoard BlackBoard; //状态机黑板

        public IFSMState<TBlackBoard> CurrentState => _activeState;
        public string CurrentStateId => _currentStateId;

        public FSM()
        {
            BlackBoard = new TBlackBoard();
        }

        //状态机有两种切换模式。 一种是状态机强制切换。另一种是每一个状态自己检查切换
        public void ChangeState(string id)
        {
            if (id.Equals(CurrentStateId)) return;
            if (mStates.TryGetValue(id, out var state))
            {
                if (_activeState != null)
                {
                    _activeState.Exit();
                }
                _activeState = state;
                _currentStateId = id;
                _activeState.Enter();
            }
        }

        public void ChangeState(IFSMState<TBlackBoard> state)
        {
            if (state.ToName().Equals(CurrentStateId)) return;
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

        public void Update()
        {
            // Debug.Log(_activeState.ToName());
            checkState();
            _activeState?.Update();
        }

        public void Clear()
        {
            _activeState = null;
            _currentStateId = default;
            mStates.Clear();
        }

        private void checkState()
        {
            if (!_activeState.CouldTransition())
                return;
            List<IFSMTransition<TBlackBoard>> trans = _activeState.GetTransitions();
            if (trans != null)
            {
                for (var i = 0; i < trans.Count; i++)
                {
                    IFSMTransition<TBlackBoard> tran = trans[i];
                    if (tran.IsValid())
                    {
                        IFSMState<TBlackBoard> state = tran.GetNextFSMState();
                        Debug.Log("current state change to:" + state.ToName());
                        _activeState.Exit();
                        tran.Transition();
                        _activeState = state;
                        _activeState.Enter();
                    }
                }
            }
        }
    }
}