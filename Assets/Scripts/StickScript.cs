using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

public class StickScript : MonoBehaviour
{
    [FormerlySerializedAs("value")] public int _value;
    public Item GetItem;
}
