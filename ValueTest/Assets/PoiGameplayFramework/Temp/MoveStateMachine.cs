using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    /// <summary>
    /// （人为约束）子类需要实现初始化StateList方法并isInit置为true，并且OnUpdate前必须初始化
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <typeparam name="IAgent"></typeparam>
    public abstract class StateMachine<T,TState, IAgent> where TState : struct where IAgent : IStateMachineAgent<TState>
    {
        protected static readonly Dictionary<TState, IState<TState, IAgent>> StateList
            = new Dictionary<TState, IState<TState, IAgent>>();

        /// <summary>
        /// 是否实例化
        /// </summary>
        protected static bool isInit = false;

        /// <summary>
        /// 隐藏构造方法
        /// </summary>
        protected StateMachine() { }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="agent">要更新的实例</param>
        /// <param name="deltaTime"></param>
        public static void OnUpdate(IAgent agent, float deltaTime)
        {
            if (!isInit)
            {
                throw new NotyetInitException();
            }
            ///取得当前状态
            TState currentState = agent.CurrentState;
            ///取得当前状态实例
            IState<TState, IAgent> state = StateList[currentState];

            ///更新状态机
            state.OnUpdate(agent, deltaTime);
        }

        public static void ChangeState(IAgent agent, TState state, TState nextState, float deltaTime)
        {
            StateList[state].OnExit(agent, nextState);
            var next = StateList[nextState];
            next.OnEnter(agent, state);
            agent.CurrentState = nextState;

            ///????暂定 切换状态所消耗的时间不计
            next.OnUpdate(agent, deltaTime);
        }
    }

    public interface IState<TState, IAgent> where IAgent : IStateMachineAgent<TState>
    {
        TState State { get; }
        void OnEnter(IAgent agent, TState lastState);
        void OnUpdate(IAgent agent, float deltaTime);
        void OnExit(IAgent agent, TState nextState);
    }

    public interface IStateMachineAgent<T>
    {
        /// <summary>
        /// 当前状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T CurrentState { get; set; }

        /// <summary>
        /// 当前状态持续时间
        /// </summary>
        float DurationTimeInCurrentState { get; set; }
    }

    /// <summary>
    /// 状态的抽象类 OnEnter提供重置时间方法
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <typeparam name="IAgent"></typeparam>
    public abstract class BaseState<TState, IAgent> : IState<TState, IAgent>
        where IAgent : IStateMachineAgent<TState>
    {
        public abstract TState State { get; }

        public virtual void OnEnter(IAgent agent, TState lastState)
        {
            agent.DurationTimeInCurrentState = 0f;
            agent.CurrentState = State;
        }

        public virtual void OnExit(IAgent agent, TState nextState) { }

        public virtual void OnUpdate(IAgent agent, float deltaTime)
        {
            agent.DurationTimeInCurrentState += deltaTime;
        }

    }



    public class PawnMoveFSM : StateMachine<Pawn,MoveState, IMove>
    {
        public static void Init()
        {
            StateList.Add(MoveState.Idle, new MoveIdleState());
            StateList.Add(MoveState.MoveStart, new MoveStartState());
            StateList.Add(MoveState.MoveStop, new MoveStopState());
            StateList.Add(MoveState.Moving, new MoveMovingState());
            isInit = true;
        }
    }

    public class MoveIdleState : BaseState<MoveState, IMove>
    {
        public override MoveState State => MoveState.Idle;

        public override void OnUpdate(IMove agent, float deltaTime)
        {
            if (agent.NextMoveDistance.Count == 0)
            {
                ///没有目标保持Idle
                return;
            }
            else
            {
                PawnMoveFSM.ChangeState(agent, State, MoveState.MoveStart,deltaTime);
            }
        }
    }

    public class MoveStartState : BaseState<MoveState, IMove>
    {
        public override MoveState State => MoveState.MoveStart;

        public override void OnEnter(IMove agent, MoveState lastState)
        {
            base.OnEnter(agent, lastState);
            agent.PlayAnim(State, lastState);
        }

        public override void OnUpdate(IMove agent, float deltaTime)
        {
            base.OnUpdate(agent, deltaTime);

            ///至少持续的时间
            const float ShortestTime = 0.1f;

            if (agent.NextMoveDistance.Count == 0)
            {
                PawnMoveFSM.ChangeState(agent, State, MoveState.Idle, deltaTime);
                return;
            }

            if (agent.NextMoveDistance.Count == 1)
            {
                ///检测持续时间

                if (agent.DurationTimeInCurrentState > ShortestTime)
                {
                    ///最后一个位移
                    var next = agent.NextMoveDistance.Peek();

                    float length = (next - agent.transform.position).sqrMagnitude;

                    if (length > 0.5f)
                    {
                        agent.MoveStart(deltaTime);
                    }
                    else
                    {
                        PawnMoveFSM.ChangeState(agent, State, MoveState.MoveStop, deltaTime);
                    }
                }
                else
                {
                    agent.MoveStart(deltaTime);
                }
            }
            else
            {
                ///中间位移
                
                if (agent.DurationTimeInCurrentState > ShortestTime)
                {
                    PawnMoveFSM.ChangeState(agent, State, MoveState.Moving, deltaTime);
                }
                else
                {
                    agent.MoveStart(deltaTime);
                }
                
            }
        }
    }

    public class MoveStopState : BaseState<MoveState, IMove>
    {
        public override MoveState State => MoveState.MoveStop;

        public override void OnEnter(IMove agent, MoveState lastState)
        {
            base.OnEnter(agent, lastState);
            agent.PlayAnim(State, lastState);
        }

        public override void OnUpdate(IMove agent, float deltaTime)
        {
            if (agent.NextMoveDistance.Count == 0)
            {
                PawnMoveFSM.ChangeState(agent, State, MoveState.Idle, deltaTime);
                return;
            }

            if (agent.NextMoveDistance.Count == 1)
            {
                ///检测持续时间

                ///至少持续的时间
                const float ShortestTime = 0.3f;

                if (agent.DurationTimeInCurrentState > ShortestTime)
                {
                    ///最后一个位移
                    var next = agent.NextMoveDistance.Peek();

                    float length = (next - agent.transform.position).sqrMagnitude;

                    if (length > 0.5f)
                    {
                        agent.MoveStart(deltaTime);
                    }
                    else
                    {
                        PawnMoveFSM.ChangeState(agent, State, MoveState.MoveStop, deltaTime);
                    }
                }
                else
                {
                    agent.MoveStop(deltaTime);
                }
            }


        }
    }

    public class MoveMovingState : BaseState<MoveState, IMove>
    {
        public override MoveState State => MoveState.Moving;

        public override void OnEnter(IMove agent, MoveState lastState)
        {
            base.OnEnter(agent, lastState);
            agent.PlayAnim(State, lastState);
        }

        public override void OnUpdate(IMove agent, float deltaTime)
        {
            base.OnUpdate(agent, deltaTime);

            if (agent.NextMoveDistance.Count == 0)
            {
                PawnMoveFSM.ChangeState(agent, State, MoveState.Idle, deltaTime);
                return;
            }

            if (agent.NextMoveDistance.Count == 1)
            {
                ///最后一个位移
                var next = agent.NextMoveDistance.Peek();

                float length = (next - agent.transform.position).sqrMagnitude;

                if (length > 0.5f)
                {
                    agent.Moving(deltaTime);
                }
                else
                {
                    PawnMoveFSM.ChangeState(agent, State, MoveState.MoveStop, deltaTime);
                }
            }
            else
            {
                agent.Moving(deltaTime);
            }
        }
    }


    public enum MoveState
    {
        Idle,
        MoveStart,
        MoveStop,
        Moving,
    }

    public interface IMove : IStateMachineAgent<MoveState>
    {
        Stack<Vector3> NextMoveDistance { get; }
        Transform transform { get; }

        void MoveStart(float deltaTime);
        void MoveStop(float deltaTime);
        void Moving(float deltaTime);
        void PlayAnim(MoveState state, MoveState lastState);
    }


}
