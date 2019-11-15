using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //the object pooler is used to optimize projectiles, particles and boxes in game

    public static ObjectPooler instance;

    public List<GameObject> pooledObjects;

    public GameObject objectToPool;

    public int amountToPool;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
