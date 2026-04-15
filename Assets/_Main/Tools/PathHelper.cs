using System;
using Constant;
using UnityEngine;

namespace Tools
{
    public static class PathHelper
    {
        public static string GetUIElementPath(Enums.UIElements elementType)
        {
            var digit = Mathf.FloorToInt((int)elementType / 10f) switch
            {
                1 => "Root",
                2 => "Screen",
                3 => "Token",
                4 => "Window",
                _ => throw new Exception($"Digit of a {elementType.ToString()} outside the permissible range")
            };
            
            return $"UIElement/{digit}/{elementType.ToString()}";
        }
    }
}