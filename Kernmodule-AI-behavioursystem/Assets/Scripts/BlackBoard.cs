using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard : MonoBehaviour
{
    private Dictionary<string, object> Variables = new Dictionary<string, object>();

    public object GetValues<T>(string _key)
    {
        return Variables[_key];
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
