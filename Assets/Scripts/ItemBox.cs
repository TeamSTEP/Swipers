using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public float health = 15f;

    // Start is called before the first frame update
    void Start()
    {
        //get random item that will be spawned once this item is destroyed
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0f)
        {
            //destroy this object when the hp is lower than 0
            //Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        //show object destroy effect
    }
}
