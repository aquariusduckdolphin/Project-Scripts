#if UNITY_EDITOR
using UnityEngine;

namespace SceneManagerToolkit.Attribute
{
    public class ParsedColor : MonoBehaviour
    {

        public static Color SetColor(string _color)
        {

            if (ColorUtility.TryParseHtmlString(_color, out Color parsedColor))
            {

                return parsedColor;

            }

            return Color.white;

        }

    }

}
#endif