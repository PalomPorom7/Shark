using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : Singleton<FishSpawner>
{
    [SerializeField]
    private float offscreenRadius = 10;

    [SerializeField]
    private int howManyFishAtOnce = 100;
    private int currentFishCount = 0;

    [SerializeField]
    private ObjectPool pool;

    private Fish newFish;
    private Vector2 randomDirection;

    private void Start()
    {
        StartCoroutine(AutoSpawn());
    }
    private IEnumerator AutoSpawn()
    {
        while(true)
        {
            yield return null;

            if(currentFishCount < howManyFishAtOnce)
            {
                SpawnFish(GameManager.Instance.GetRandomColor(true));
                ++currentFishCount;
            }
        }
    }

    public void SpawnFish(Color fishColor)
    {
        newFish = pool.GetPooledObject().GetComponent<Fish>();
        newFish.FishColor = fishColor;

        randomDirection = Random.insideUnitCircle;
        randomDirection.Normalize();
        randomDirection *= offscreenRadius;

        newFish.transform.position = transform.position + (Vector3)randomDirection;

        newFish.StartSwimming();
    }

    public void DespawnFish(Fish toDespawn)
    {
        pool.ReturnObjectToPool(toDespawn.gameObject);
        --currentFishCount;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Fish")
            DespawnFish(other.gameObject.GetComponent<Fish>());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, offscreenRadius);
    }
}
