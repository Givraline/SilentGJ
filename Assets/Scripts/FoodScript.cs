using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

public class FoodScript : MonoBehaviour
{
    [FormerlySerializedAs("value")] public int _value;

    public Item GetItem { get; private set; }

    public void SetItem(Item item)
    {
        GetItem = item;
    }
}
