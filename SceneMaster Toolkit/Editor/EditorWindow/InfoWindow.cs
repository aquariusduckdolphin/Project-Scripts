using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneManagerToolkit
{

    public class InfoWindow : ScriptableObject
    {

        public string[] attributeTabs = { "Getting Started", "ReadOnly", "Progress Bar", "Required", "Separator", "Title" };

        public int attributeIndex = 0;

        public string readOnlyText = "The ReadOnly attribute is used to make a field viewable" +
            " but no editable in the inspector. This is perfect for monitoring value changes" +
            " without allowing modifications. It ensures that other project members can see" +
            " the value but cannot accidentally change it. This attribute works only with" +
            " public or serialized fields. By using the ReadOnly attribute, you can maintain" +
            " control over important values while still making them visible for debugging and" +
            " monitoring purpose.";

        public string progressBarText = "";

        public string requiredText = "";

        public string separatorText = "";

        public string titleText = "";

    }

}
