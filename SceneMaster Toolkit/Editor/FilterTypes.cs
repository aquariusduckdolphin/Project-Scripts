#if UNITY_EDITOR

namespace SceneManagerToolkit.Editor
{

    public enum FilterTypes
    {

        GameObjectName,
        Tag,
        Layer,
        Component

    }

    public enum MaterialType
    {

        Standard,
        Universal_Renderer_Pipeline,
        High_Definition_Renderer_Pipeline,
        Object_Shader,
        Transparent_Object_Shader

    }

    public enum IDStyle
    {

        Parentheses,
        Underscore,
        Period,

    }

}
#endif