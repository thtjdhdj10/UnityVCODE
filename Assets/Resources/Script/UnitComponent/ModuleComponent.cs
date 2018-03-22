using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleComponent : UnitComponent
{

    public override void Init()
    {
        base.Init();


    }

    [SerializeField]
    public class ModuleSpawnProperty
    {
        public Module module;

        public float respawnDelay;
        private float remainRespawnDelay;
        public float RemainRespawnDelay
        {
            get
            {
                return remainRespawnDelay;
            }
            set
            {
                if(remainRespawnDelay < 0f)
                {
                    GameObject _module = VEasyPoolerManager.GetObjectRequest("Player Module 1");
                    module = _module.GetComponent<Module>();
                    if(module == null)
                    {
                        VEasyPoolerManager.ReleaseObjectRequest(_module);
                    }
                }

                remainRespawnDelay = Mathf.Min(value, respawnDelay);
            }
        }
    }

    public ModuleSpawnProperty[] moduleSpawnProperty;

    //

    private void FixedUpdate()
    {
        if (IsActivated() == false)
            return;

        for (int i = 0; i < moduleSpawnProperty.Length; ++i)
        {
            ModuleSpawnProperty targetProperty = moduleSpawnProperty[i];
            if (targetProperty.module == null)
            {
                targetProperty.RemainRespawnDelay -= Time.fixedDeltaTime;
            }
        }
    }

}
