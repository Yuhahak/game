
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealPack : InventoryItemBase
{
    public int HealthPoints = 20;

    public Inventory _Inventory;

    public override void OnUse()
    {

        Player_Health.instance.IncDegHp("Hungry", HealthPoints);

        _Inventory.RemovedItemm(this);

        Destroy(this.gameObject);

    }
}
