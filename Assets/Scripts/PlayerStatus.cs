using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    //player stats
    public float health = 100f;
    [Range(0, 40)]
    public int strength = 10;
    [Range(0, 50)]
    public int defence = 0;
    [Range(0, 50)]
    public int manaPoint = 5;
    [Range(0, 40)]
    public int weight = 5;

    private Rigidbody2D rb2d;

    //how much damage the attack will deal
    [HideInInspector]
    public int damage;

    //the list of item the player character is currently equipping
    public List<Equipment> equipments;

    
    //check if the character is attacking
    [HideInInspector]
    public bool isAttacking = false;

    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        AdjustCharacterControl();
    }

    private void Update()
    {

    }

    void AdjustCharacterControl()
    {
        //todo: adjust the values and the math to make things more smooth
        if (strength < weight)
        {
            rb2d.mass += (weight - strength) / 40;
        }
        else
        {
            rb2d.mass -= (strength - weight) / 40;
        }

        rb2d.gravityScale += weight / 50;
    }

    //todo: add equipment visualization

}
