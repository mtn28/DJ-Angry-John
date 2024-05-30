using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickable : MonoBehaviour, IPickable
{
    public WeaponSO itemScriptableObject;

    public void PickItem()
    {
        Destroy(gameObject);
    }
}
