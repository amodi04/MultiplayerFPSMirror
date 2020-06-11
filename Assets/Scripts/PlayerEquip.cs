﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerEquip : NetworkBehaviour
{
    private int selectedWeaponLocal = 1;
    public GameObject[] weaponArray;
    [SyncVar(hook = nameof(OnWeaponChanged))]
    public int activeWeaponSynced;
    void OnWeaponChanged(int _Old, int _New)
    {
        activeWeaponSynced = _New;
        foreach (var item in weaponArray)
        {
            if (item != null) { item.SetActive(false); }
        }
        if (_New < weaponArray.Length && weaponArray[_New] != null)
        {
            weaponArray[_New].SetActive(true);
        }
    }

    [Command]
    public void CmdChangeActiveWeapon(int _activeWeaponSynced)
    {
        activeWeaponSynced = _activeWeaponSynced;
    }

    void Update()
    {
        if (!isLocalPlayer) { return; }
        if (Input.GetButtonDown("Fire2")) //Fire2 is mouse 2nd click and left alt
        {
            selectedWeaponLocal += 1;
            if (selectedWeaponLocal > weaponArray.Length) { selectedWeaponLocal = 1; }
            CmdChangeActiveWeapon(selectedWeaponLocal);
        }
    }
}