using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard : MonoBehaviour
{
    public Dictionary<string, object> Variables = new Dictionary<string, object>();

    public object GetValues<T>(string _key)
    {
        if (Variables.ContainsKey(name))
        {
            return (T)Variables[name];
        }
        return default(T);
    }
    public void SetValue<T>(string _key, T _value)
    {
        if (Variables.ContainsKey(_key))
        {
            Variables[_key] = _value;
        }
        else
        {
            Variables[_key] = _value;
        }
    }
}
