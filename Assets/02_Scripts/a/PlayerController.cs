using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Inventory inventory;

    public GameObject Hand;

    public HUD Hud;

    private IInventoryItem mCurrentItem = null;

    private bool mLockPickup = false;

    /*private void DropCurrentItem()
    {

        mLockPickup = true;
        GameObject goItem = (mCurrentItem as MonoBehaviour).gameObject;

        inventory.RemovedItem(mCurrentItem);

        Rigidbody rbItem = goItem.AddComponent<Rigidbody>();
        rbItem.AddForce(transform.forward * 2.0f, ForceMode.Impulse);

        Invoke("DoDropItem", 0.25f);
    }

    public void DoDropItem()
    {
        mLockPickup = false;

        Destroy((mCurrentItem as MonoBehaviour).GetComponent<Rigidbody>());

        mCurrentItem = null;
    }

    private void FixedUpdate()
    {
        if(mCurrentItem != null && Input.GetKeyDown(KeyCode.R))
        {
            DropCurrentItem();
        }
    }*/ //키 누를시 드롭 


    private IInventoryItem mItemToPickup = null;


    private void OnTriggerEnter(Collider other)
    {
        
        IInventoryItem item = other.GetComponent<IInventoryItem>();
        if (item != null)
        {
            if (mLockPickup)
                return;
            mItemToPickup = item;
            //inventory.AddItem(item);
            //item.OnPickup();
            //Hud.OpenMessagePanel("");
            MiddleToastMsg.Instance.showMessage("획득 F키", 1.5f);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInventoryItem item = other.GetComponent<IInventoryItem>();
        if (item != null)
        {
            //Hud.CloseMessagePanel();
            mItemToPickup = null;
        }


    }
    /*private void OnTriggerStay(Collider other)
    {
        IInventoryItem item = other.GetComponent<IInventoryItem>();
        if (item != null)
        {
            if (mLockPickup)
                return;
            mItemToPickup = item;
            //inventory.AddItem(item);
            //item.OnPickup();
            MiddleToastMsg.Instance.showMessage("저장되었습니다!", 0.5f);

        }

    }*/

    // Start is called before the first frame update
    void Start()
    {
        inventory.ItemUsed += Inventory_ItemUsed;
        inventory.ItemRemoved += Inventory_ItemRemoved;
    }

    private void Inventory_ItemRemoved(object sender, InventoryEventArgs e)
    {
        IInventoryItem item = e.Item;

        GameObject goItem = (item as MonoBehaviour).gameObject;
        goItem.SetActive(true);

        goItem.transform.parent = null;
    }

    private void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        IInventoryItem item = e.Item;

        GameObject goItem = (item as MonoBehaviour).gameObject;
        goItem.SetActive(true);

        goItem.transform.parent = Hand.transform;

        mCurrentItem = e.Item;

    }

    // Update is called once per frame
    void Update()
    {
        if(mItemToPickup != null && Input.GetKeyDown(KeyCode.F))
        {
            inventory.AddItem(mItemToPickup);
            mItemToPickup.OnPickup();
            //Hud.CloseMessagePanel();
        }
    }
}
