using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FoodScript : MonoBehaviour
{
    [FormerlySerializedAs("value")] public int _value;

    public Item GetItem { get; private set; }

    public void SetItem(Item item)
    {
        GetItem = item;
    }

    private void Awake()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-8, 8), Random.Range(0f, 4f));
    }
}
