using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SOItemDropTable : ScriptableObject
{
    [Serializable]
    public class DropItems
    {
        public Item item;
        public float weight;
    }

    public List<DropItems> listDropItems = new List<DropItems>();

    public Item PickItem()
    {
        if(listDropItems.Count == 0) return null;

        float sum = 0; 
        foreach(DropItems dropItem in listDropItems)
        {
            sum += dropItem.weight;
        }

        float randomValue = UnityEngine.Random.value * sum;

        foreach(DropItems dropItem in listDropItems)
        {
            randomValue -= dropItem.weight;

            if(randomValue <= 0) return dropItem.item;
        }

        return listDropItems[listDropItems.Count - 1].item;
    }
}
