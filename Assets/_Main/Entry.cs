using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entry : MonoBehaviour
{
    public IEnumerator LoadScene()
    {
        LoadManagers();
        yield break;
    }

    private void LoadManagers()
    {
        var managerNames = Core.Data.Get.preferences.game.managers;
        var managers = new List<Manager>();
        foreach (var manager in managerNames)
        {
            var type = Type.GetType(manager);
            if (type == null) throw new Exception("Manager not found: " + manager);
            var instance = (Manager)Activator.CreateInstance(type);
            if (instance == null) throw new Exception("Manager can't be created: " + manager);
            managers.Add(instance);
            instance.Init();
        }

        foreach (var manager in managers) { manager.Run(); }
    }
}