using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Fish;
using UnityEngine;

namespace CodeBase.Data
{
    public static class DataExtensions
    {
        public static Vector3Data AsVectorData(this Vector3 vector) =>
            new Vector3Data(vector.x, vector.y, vector.z);

        public static Vector3 AsUnityVector(this Vector3Data vector3Data) =>
            new Vector3(vector3Data.X, vector3Data.Y, vector3Data.Z);

        public static Vector3 AddY(this Vector3 vector, float y)
        {
            vector.y = y;
            return vector;
        }

        public static float SqrMagnitudeTo(this Vector3 from, Vector3 to)
        {
            return Vector3.SqrMagnitude(to - from);
        }

        public static string ToJson(this object obj) =>
            JsonUtility.ToJson(obj);

        public static T ToDeserialized<T>(this string json) =>
            JsonUtility.FromJson<T>(json);

        public static Vector2 DirectionAtTarget(this Vector3 from, Vector2 to)
        {
            Vector2 directionToPlayer = new Vector2(
                    to.x - from.x,
                    to.y - from.y)
                .normalized;
            return directionToPlayer;
        }

        public static Vector2 BlockYAxis(this Vector2 direction) =>
            new Vector2(direction.x, direction.y = 0);

        public static Color SwitchColor(this ColorType colorType)
        {
            switch (colorType)
            {
                case ColorType.Red:
                    return Color.red;
                case ColorType.Green:
                    return Color.green;
                case ColorType.Blue:
                    return Color.blue;
                case ColorType.Yellow:
                    return Color.yellow;
                case ColorType.Cyan:
                    return Color.cyan;
                case ColorType.Purple:
                    return Color.magenta;
                case ColorType.Rainbow:
                    return Color.white;
                default:
                    return Color.black;
            }
        }
        public static Color SwitchColor(this ColorType colorType, int i)
        {
            switch (i)
            {
                case 1:
                    return Color.red;
                case 2:
                    return Color.green;
                case 3:
                    return Color.blue;
                case 4:
                    return Color.yellow;
                case 5:
                    return Color.cyan;
                case 6:
                    return Color.magenta;
                default:
                    return Color.white;
            }
        }
    }
}