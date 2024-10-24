using System;
using Ink.Runtime;
public class DialogueVariableSetter
{
    public static Ink.Runtime.Object SetVariable<T>(T value)
    {
        return ConvertToInkObject(value);
    }

    public static Ink.Runtime.Object ConvertToInkObject<T>(T value)
    {
        if(typeof(T) == typeof(bool))
        {
            return new Ink.Runtime.BoolValue(Convert.ToBoolean(value));
        }
        else
        {
            throw new InvalidCastException($"Type {typeof(T).Name} not supported in Ink");
        }
    }
}
