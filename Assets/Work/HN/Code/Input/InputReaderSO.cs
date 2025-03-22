using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Control;

namespace Work.HN.Code.Input
{
    [Flags]
    public enum InputType
    {
        Player = 1 << 0,
        MapMaker = 1 << 1
    }

    [CreateAssetMenu(menuName = "SO/InputReader")]
    public class InputReaderSO : ScriptableObject, IPlayerActions, IMapMakerActions
    {
        private Control _control;

        #region MapMakerSection

        public event Action OnClickEvent;
        public event Action OnUndoEvent;
        public event Action OnRedoEvent;
        
        public event Action<float> OnZoomInEvent;
        public Vector2 CameraMoveAmount { get; private set; }

        #endregion

        #region PlayerAction

        public Vector2 MouseScreenPos { get; private set; }
        public Vector2 MouseWorldPos { get; private set; }
        
        public Vector2 MoveDir { get; private set; }
        public event Action OnJumpPressEvent;
        public event Action OnJumpReleaseEvent;

        #endregion

        private void OnEnable()
        {
            if (_control == null)
            {
                _control = new Control();
                _control.Player.SetCallbacks(this);
                _control.MapMaker.SetCallbacks(this);
            }

            _control.Enable();
        }

        private void OnDisable()
        {
            _control.Disable();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnClickEvent?.Invoke();
        }

        public void OnMousePos(InputAction.CallbackContext context)
        {
            MouseScreenPos = context.ReadValue<Vector2>();
            MouseWorldPos = Camera.main.ScreenToWorldPoint(MouseScreenPos);
        }

        public void OnUndo(InputAction.CallbackContext context)
        {
            if(context.performed && !Keyboard.current.leftShiftKey.isPressed)
                OnUndoEvent?.Invoke();
        }

        public void OnCameraMove(InputAction.CallbackContext context)
        {
            CameraMoveAmount = context.ReadValue<Vector2>();
        }

        public void OnZoomIn(InputAction.CallbackContext context)
        {
            float y = context.ReadValue<Vector2>().y;
            OnZoomInEvent?.Invoke(y);
        }

        public void OnRedo(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnRedoEvent?.Invoke();
        }

        public void SetEnable(InputType inputType, bool isEnable)
        {
            if ((inputType & InputType.Player) != 0)
            {
                if (isEnable) _control.Player.Enable();
                else _control.Player.Disable();
            }

            if ((inputType & InputType.MapMaker) != 0)
            {
                if (isEnable) _control.MapMaker.Enable();
                else _control.MapMaker.Disable();
            }
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MoveDir = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnJumpPressEvent?.Invoke();
            if(context.canceled)
                OnJumpReleaseEvent?.Invoke();
        }
        
        public void ClearPlayerAction()
        { 
            OnJumpPressEvent = null; 
            OnJumpReleaseEvent = null;
        }
    }
}