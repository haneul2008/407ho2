using UnityEngine;

namespace Work.HN.Code.MapMaker.UI.Position
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    
    [CreateAssetMenu(menuName = "SO/UI/PositionEditBtn")]
    public class PositionEditBtnDataSO : ScriptableObject
    {
        public string text;
        public Direction direction;
        public float multiplier = 1f;
        public float textSizeMultiplier = 1;

        public Vector2 GetMoveAmount()
        {
            return direction switch
            {
                Direction.Up => Vector2.up * multiplier,
                Direction.Down => Vector2.down * multiplier,
                Direction.Left => Vector2.left * multiplier,
                Direction.Right => Vector2.right * multiplier,
                _ => Vector2.zero
            };
        }
    }
}