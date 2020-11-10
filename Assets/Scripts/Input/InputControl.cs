using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using System.Linq;

public class InputControl : MonoBehaviour, ILoopable
{
    private class ActionInfo {
        public bool WasPressed;
        public bool IsKeyDown;
        public bool IsKeyUp;
        public PlayerAction Actions;
        public ActionInfo(PlayerAction action) {
            WasPressed = false;
            IsKeyUp = false;
            IsKeyDown = false;
            Actions = action;
        }
    }

    private PlayerControl _model;
    //private PlayerAnimator _animator;
    private InputActions _actions;
    private Dictionary<string, ActionInfo> _wasPressedAtLastFrame;
    private bool _isJumpPressed;
    //private PlayerAnimator.States[] _inputIgnoreStates, _moveIgnoreStates;

    public void Initalize(PlayerControl character)
    {
        _model = character;
        //_animator = _model.GetComponent<PlayerAnimator>();

        _actions = new InputActions();

        _actions.Left.AddDefaultBinding(Key.LeftArrow);
        _actions.Left.AddDefaultBinding(InputControlType.DPadLeft);
        _actions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);

        _actions.Right.AddDefaultBinding(Key.RightArrow);
        _actions.Right.AddDefaultBinding(InputControlType.DPadRight);
        _actions.Right.AddDefaultBinding(InputControlType.LeftStickRight);

        _actions.Up.AddDefaultBinding(Key.UpArrow);
        _actions.Up.AddDefaultBinding(InputControlType.DPadUp);
        _actions.Up.AddDefaultBinding(InputControlType.LeftStickUp);

        _actions.Down.AddDefaultBinding(Key.DownArrow);
        _actions.Down.AddDefaultBinding(InputControlType.DPadDown);
        _actions.Down.AddDefaultBinding(InputControlType.LeftStickDown);

        _actions.Jump.AddDefaultBinding(Key.Z);
        _actions.Jump.AddDefaultBinding(InputControlType.Action1);

        _actions.Attack.AddDefaultBinding(Key.X);
        _actions.Attack.AddDefaultBinding(InputControlType.Action3);

        _actions.Throw.AddDefaultBinding(Key.A);
        _actions.Throw.AddDefaultBinding(InputControlType.Action2);

        _wasPressedAtLastFrame = new Dictionary<string, ActionInfo>();
        _wasPressedAtLastFrame.Add(InputActions.JumpActionName, new ActionInfo(_actions.Jump));
        _wasPressedAtLastFrame.Add(InputActions.AttackActionName, new ActionInfo(_actions.Attack));
        _wasPressedAtLastFrame.Add(InputActions.ThrowActionName, new ActionInfo(_actions.Throw));
    }

    public bool GetKeyDown(string actionName) {
        ActionInfo info;
        if (_wasPressedAtLastFrame.TryGetValue(actionName, out info)) {
            return info.IsKeyDown;
        }
        return false;
    }

    public bool GetKeyUp(string actionName) {
        ActionInfo info;
        if (_wasPressedAtLastFrame.TryGetValue(actionName, out info)) {
            return info.IsKeyUp;
        }
        return false;
    }
    public void Progress() {
        SetupActionInfo();
        InputKeys();
    }

    public void FixedProgress() { }

    void InputKeys() {
        //if (CheckCannotInput()) return;

        float horizontal = 0f, vertical = 0f;
        //if (!CheckCannotMove()) {
            horizontal = IgnoreSmallValue(_actions.Horizontal.Value);
            vertical = IgnoreSmallValue(_actions.Vertical.Value);
        //}
        
        _model.SetInputX(horizontal);
        _model.SetInputY(vertical);

        if (GetKeyDown(InputActions.JumpActionName))
            _model.OnJumpInputDown();
        else if (GetKeyUp(InputActions.JumpActionName))
            _model.OnJumpInputUp();

        /*
        _model.SetAttack(GetKeyDown(InputActions.AttackActionName));
        
        _model.AddCharge(_actions.Throw.IsPressed);
        _model.OnChargeEnd(GetKeyUp(InputActions.ThrowActionName));*/

        _isJumpPressed = _actions.Jump.IsPressed;
    }

    void SetupActionInfo() {
        foreach (var pair in _wasPressedAtLastFrame) {
            var action = pair.Value;

            action.IsKeyDown = !action.WasPressed && action.Actions.IsPressed;
            action.IsKeyUp = action.WasPressed && !action.Actions.IsPressed;
            
            action.WasPressed = action.Actions.IsPressed;
        }
    }

    float IgnoreSmallValue(float value) {
        value = (Mathf.Abs(value) > 0.5f) ? value : 0f;
        value = (value == 0f) ? value : Mathf.Sign(value);
        return value;
    }
/*
    bool CheckCannotInput() {
        var state = _animator.State;

        if (_inputIgnoreStates == null) {
            _inputIgnoreStates = new PlayerAnimator.States[] {
                PlayerAnimator.States.Hit,
                PlayerAnimator.States.Dead,
            };
        }

        return _inputIgnoreStates.Contains(state);
    }

    bool CheckCannotMove() {
        var state = _animator.State;

        if (_moveIgnoreStates == null) {
            _moveIgnoreStates = new PlayerAnimator.States[] {
                PlayerAnimator.States.AttackA,
                PlayerAnimator.States.AttackB,
                PlayerAnimator.States.Shoot,
            };
        }

        return _moveIgnoreStates.Contains(state);
    }*/
}
