using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        //check if there is another instance of this object
        if (instance != null)
        {
            Debug.Log("More than one instance of inventory! Destroying one");
            Destroy(gameObject);
            return;
        }
        //by assigning only one instance, this will make sure that only one inventory is selected
        instance = this;
    }
    #endregion

    public List<Equipment> equipments = new List<Equipment>();

    /// <summary>
    /// Add the equipment to the inventory list
    /// </summary>
    /// <param name="equipment">Equipment.</param>
    public void Add(Equipment equipment)
    {
        equipments.Add(equipment);
        Debug.Log("Added " + equipment.name);
    }

    /// <summary>
    /// Remove the specified equipment.
    /// </summary>
    /// <param name="equipment">Equipment.</param>
    public void Remove(Equipment equipment)
    {
        equipments.Remove(equipment);
    }
    
}
