using UnityEngine;
//using Foodrush;

namespace Foodrush
{

    public class Board : MonoBehaviour
    {
        [SerializeField] public int boardValue = 0;
        [SerializeField] public BoardType boardType;


    }

    public enum BoardType
    {
        Addition,
        Subtraction,
        Multiply,
        None
    }

}