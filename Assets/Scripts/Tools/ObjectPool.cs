using System;
using System.Collections.Generic;
using UnityEngine;

public enum WhatToDoWhenThePoolIsEmpty
{
    returnNull,
    add1,
    doubleAmount
}
/*
    This class will allow us to instantiate as many objects as we will ever need
    while the scene is being set up so that we don't slow down the game by calling
    Instantiate during gameplay
*/
public class ObjectPool : MonoBehaviour
{
    //  Where to store the pool in the heirarchy
    [SerializeField]
    private Transform parentFolder;

    //  This is a prefab of the object of which we will be making lots of copies
    [SerializeField]
    private GameObject prefab;

    //  A dropdown selectable option of what to do when the object pool is empty
    [SerializeField]
    private WhatToDoWhenThePoolIsEmpty whatToDo;

    //  How many copies?
    [SerializeField]
    private int amount = 0;

    //  A list of all the copies we made
    private Queue<GameObject> pooledGameObjects;

    //  Have the objects been pooled yet?
    private bool ready;

    //  This function instantiates the specified amount of objects
    //  It should be called only once during the setup of the scene
    public void PoolObjects()
    {
        //  Check if the pool is already ready
        if (ready)
        {
            Debug.LogWarning("The pool is already ready.");
            return;
        }

        //  Make sure the size of the pool is not a negative number
        if (amount < 0)
            throw new ArgumentOutOfRangeException("Amount to pool must be non-negative.");

        //  If no parent folder is specified, use this transform
        if (parentFolder == null)
            parentFolder = transform;

        //  Initialize the list
        pooledGameObjects = new Queue<GameObject>(amount);

        AddNewObjectsToPool(amount);

        //  The object pool is now ready
        ready = true;
    }
    public void AddNewObjectsToPool(int amount)
    {
        //  A reference to our newly instantiated object
        GameObject newObject;

        //  Make x amount of copies
        for (int i = 0; i != amount; ++i)
        {
            //  Instantiate and set the parent as this transform
            //  Then add it to the list by getting the T Component
            newObject = Instantiate(prefab, parentFolder);
            newObject.SetActive(false);
            pooledGameObjects.Enqueue(newObject);
        }
    }
    //  Pool a specified amount of copies
    public void PoolObjects(int amount)
    {
        //  Save the amount in this object
        this.amount = amount;

        //  Call the function above
        PoolObjects();
    }
    //  Retrieve an object from the pool
    //  Any object retrieved from the pool must be set to active
    //  before getting another or it will return the same one
    public GameObject GetPooledObject()
    {
        //  Make sure the pool is ready.
        //  If not, get it ready
        if(!ready)
            PoolObjects();

        //  Check if the pool is empty, do something about it
        if(pooledGameObjects.Count == 0)
            return PoolIsEmpty();

        //  Get next object from the pool, activate it, and return it
        GameObject toReturn = pooledGameObjects.Dequeue();
        toReturn.SetActive(true);
        return toReturn;
    }
    //  Return an object back to the pool
    public void ReturnObjectToPool(GameObject toReturn)
    {
        //  Make sure its not null
        if(toReturn == null)
            return;

        //  Make sure the pool is ready
        //  If not, make it ready
        if(!ready)
            PoolObjects();

        //  Deactivate the game object and add it back into the pool
        toReturn.SetActive(false);
        pooledGameObjects.Enqueue(toReturn);
    }
    public GameObject PoolIsEmpty()
    {
        GameObject toReturn = null;

        switch(whatToDo)
        {
            case WhatToDoWhenThePoolIsEmpty.add1:
                AddNewObjectsToPool(1);
                ++amount;
                toReturn = GetPooledObject();
                break;

            case WhatToDoWhenThePoolIsEmpty.doubleAmount:
                AddNewObjectsToPool(amount);
                amount *= 2;
                toReturn = GetPooledObject();
                break;
        }
        return toReturn;
    }
}
