using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    public class PlayerMoveFSM : StateMachine<Character, MoveState, IPlayerMove>
    {
        public static void Init()
        {
            StateList.Add(MoveState.Idle, new PlayerMoveIdleState());
            StateList.Add(MoveState.MoveStart, new PlayerMoveStartState());
            StateList.Add(MoveState.MoveStop, new PlayerMoveStopState());
            StateList.Add(MoveState.Moving, new PlayerMoveMovingState());
            isInit = true;
        }
    }

    

    public interface IPlayerMove : IMove
    {
        float CurrentMoveStateSpeed { get; }
        float JiaSudu { get; }
    }

    public class PlayerMoveIdleState : BaseState<MoveState, IPlayerMove>
    {
        public override MoveState State => MoveState.Idle;

        public override bool CheckChangedState(IPlayerMove agent, float deltaTime)
        {
            if (agent.JiaSudu == 0)
            {
                ///没有目标保持Idle
                return false;
            }
            else
            {
                agent.CurrentState = MoveState.MoveStart;
                return true;
            }
        }
    }

    public class PlayerMoveStartState : BaseState<MoveState, IPlayerMove>
    {
        public override MoveState State => MoveState.MoveStart;

        public override void OnEnter(IPlayerMove agent, MoveState lastState)
        {
            base.OnEnter(agent, lastState);
            agent.PlayAnim(State, lastState);
        }

        public override bool CheckChangedState(IPlayerMove agent, float deltaTime)
        {
            const float V = 0.5f;

            if (agent.JiaSudu < 0)
            {
                if (agent.CurrentMoveStateSpeed < V)
                {
                    agent.CurrentState = MoveState.MoveStop;
                    return true;
                }
            }
            else
            {
                if (agent.CurrentMoveStateSpeed > V)
                {
                    agent.CurrentState = MoveState.Moving;
                    return true;
                }
            }

            return false;
        }

        public override void DoUpdate(IPlayerMove agent, float deltaTime)
        {
            agent.MoveStart(deltaTime);
        }
    }

    public class PlayerMoveMovingState : BaseState<MoveState, IPlayerMove>
    {
        public override MoveState State => MoveState.Moving;

        public override void OnEnter(IPlayerMove agent, MoveState lastState)
        {
            base.OnEnter(agent, lastState);
            agent.PlayAnim(State, lastState);
        }

        public override bool CheckChangedState(IPlayerMove agent, float deltaTime)
        {

            if (agent.JiaSudu < 0 && agent.CurrentMoveStateSpeed < 0.5f)
            {
                agent.CurrentState = MoveState.MoveStop;
                return true;
            }
            return false;
        }

        public override void DoUpdate(IPlayerMove agent, float deltaTime)
        {
            agent.Moving(deltaTime);
        }
        
    }

    public class PlayerMoveStopState : BaseState<MoveState, IPlayerMove>
    {
        public override MoveState State => MoveState.MoveStop;

        public override void OnEnter(IPlayerMove agent, MoveState lastState)
        {
            base.OnEnter(agent, lastState);
            agent.PlayAnim(State, lastState);
        }

        public override bool CheckChangedState(IPlayerMove agent, float deltaTime)
        {
            if (agent.CurrentMoveStateSpeed < 0.02f)
            {
                agent.CurrentState = MoveState.MoveStop;
                return true;
            }
            return false;
        }

        public override void DoUpdate(IPlayerMove agent, float deltaTime)
        {
            agent.MoveStop(deltaTime);
        }
    }
}
