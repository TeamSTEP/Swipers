using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundEffects : MonoBehaviour
{
    [Range(0.01f, 5f)]
    public float scrollSpeed = 0.5f;
    
    public float scrollOffset;

    private Vector2 startPos;

    private float newPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //move the background image
        newPos = Mathf.Repeat(Time.time * scrollSpeed, scrollOffset);
        
        transform.position = startPos + Vector2.up * newPos;
        
    }
}
