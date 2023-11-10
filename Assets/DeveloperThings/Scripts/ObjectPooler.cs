using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : UnitySingleton<ObjectPooler>
{
    private List<GameObject> level1Equipments = new List<GameObject>();
    private List<GameObject> level2Equipments = new List<GameObject>();
    private List<GameObject> level3Equipments = new List<GameObject>();
    private List<GameObject> level4Equipments = new List<GameObject>();
    private List<GameObject> level5Equipments = new List<GameObject>();
    private List<GameObject> level6Equipments = new List<GameObject>();
    private int amountOfEachEquipment = 10;
    [SerializeField] private GameObject level1EquipmentPrefab;
    [SerializeField] private GameObject level2EquipmentPrefab;
    [SerializeField] private GameObject level3EquipmentPrefab;
    [SerializeField] private GameObject level4EquipmentPrefab;
    [SerializeField] private GameObject level5EquipmentPrefab;
    [SerializeField] private GameObject level6EquipmentPrefab;
    /////////////////////////////////////
    private List<GameObject> level1Tablets = new List<GameObject>();
    private List<GameObject> level2Tablets = new List<GameObject>();
    private List<GameObject> level3Tablets = new List<GameObject>();
    private List<GameObject> level4Tablets = new List<GameObject>();
    private List<GameObject> level5Tablets = new List<GameObject>();
    private List<GameObject> level6Tablets = new List<GameObject>();
    private int amountOfEachTablet = 10;
    [SerializeField] private GameObject level1TabletPrefab;
    [SerializeField] private GameObject level2TabletPrefab;
    [SerializeField] private GameObject level3TabletPrefab;
    [SerializeField] private GameObject level4TabletPrefab;
    [SerializeField] private GameObject level5TabletPrefab;
    [SerializeField] private GameObject level6TabletPrefab;
    /////////////////////////////////////
   
    private List<GameObject> moneyPopUps = new List<GameObject>();
    private List<GameObject> fortUpgradePopUps = new List<GameObject>();
    private int amountOfPopUps = 5;
    [SerializeField] private GameObject animatedMoneyPrefab;
    [SerializeField] private GameObject fortUpgradePopUPrefab;

    /////////////////////////////////////
    private List<GameObject> buyEquipmentParticles = new List<GameObject>();
    private List<GameObject> mergeEquipmentLvl2Particles = new List<GameObject>();
    private List<GameObject> mergeEquipmentLvl3Particles = new List<GameObject>();
    private List<GameObject> mergeEquipmentLvl4Particles = new List<GameObject>();
    private List<GameObject> mergeEquipmentLvl5Particles = new List<GameObject>();
    private List<GameObject> mergeEquipmentLvl6Particles = new List<GameObject>();
    private List<GameObject> equipParticles = new List<GameObject>();
    private List<GameObject> hitParticles = new List<GameObject>();
    private List<GameObject> upgradeFortParticles = new List<GameObject>();
    private List<GameObject> deadEffectParticles = new List<GameObject>();
    private int amountOfEachParticle = 5;
    [SerializeField] private GameObject buyEquipmentParticlePrefab;
    [SerializeField] private GameObject mergeEquipmentLvl2ParticlePrefab;
    [SerializeField] private GameObject mergeEquipmentLvl3ParticlePrefab;
    [SerializeField] private GameObject mergeEquipmentLvl4ParticlePrefab;
    [SerializeField] private GameObject mergeEquipmentLvl5ParticlePrefab;
    [SerializeField] private GameObject mergeEquipmentLvl6ParticlePrefab;
    [SerializeField] private GameObject hitParticlePrefab;
    [SerializeField] private GameObject equipParticlePrefab;
    [SerializeField] private GameObject upgradeFortParticlePrefab;
    [SerializeField] private GameObject deadEffectParticlePrefab;
    void Awake()
    {
        CreateAllEquipmentPool();
        CreateTabletsPool();
        CreateAllPopUpPool();
        CreateAllParticlePool();
    }

    #region Equipment Pool
    private void CreateAllEquipmentPool()
    {
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            var level1Equipment = Instantiate(level1EquipmentPrefab);
            level1Equipment.transform.SetParent(transform);
            level1Equipment.SetActive(false);
            level1Equipments.Add(level1Equipment);
        }
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            var level2Equipment = Instantiate(level2EquipmentPrefab);
            level2Equipment.transform.SetParent(transform);
            level2Equipment.SetActive(false);
            level2Equipments.Add(level2Equipment);
        }
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            var level3Equipment = Instantiate(level3EquipmentPrefab);
            level3Equipment.transform.SetParent(transform);
            level3Equipment.SetActive(false);
            level3Equipments.Add(level3Equipment);
        }
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            var level4Equipment = Instantiate(level4EquipmentPrefab);
            level4Equipment.transform.SetParent(transform);
            level4Equipment.SetActive(false);
            level4Equipments.Add(level4Equipment);
        }
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            var level5Equipment = Instantiate(level5EquipmentPrefab);
            level5Equipment.transform.SetParent(transform);
            level5Equipment.SetActive(false);
            level5Equipments.Add(level5Equipment);
        }
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            var level6Equipment = Instantiate(level6EquipmentPrefab);
            level6Equipment.transform.SetParent(transform);
            level6Equipment.SetActive(false);
            level6Equipments.Add(level6Equipment);
        }
    }

    public GameObject GetEquipmentFromPool(int level)
    {
        switch (level)
        {
            case 1:
                return Getlevel1EquipmentFromPool();
            case 2:
                return Getlevel2EquipmentFromPool();
            case 3:
                return Getlevel3EquipmentFromPool();
            case 4:
                return Getlevel4EquipmentFromPool();
            case 5:
                return Getlevel5EquipmentFromPool();
            case 6:
                return Getlevel6EquipmentFromPool();
        }
        return null;

    }
    public GameObject Getlevel1EquipmentFromPool()
    {
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            if (!level1Equipments[i].activeSelf) return level1Equipments[i];
        }
        return null;
    }
    public GameObject Getlevel2EquipmentFromPool()
    {
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            if (!level2Equipments[i].activeSelf) return level2Equipments[i];
        }
        return null;
    }
    public GameObject Getlevel3EquipmentFromPool()
    {
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            if (!level3Equipments[i].activeSelf) return level3Equipments[i];
        }
        return null;
    }
    public GameObject Getlevel4EquipmentFromPool()
    {
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            if (!level4Equipments[i].activeSelf) return level4Equipments[i];
        }
        return null;
    }
    public GameObject Getlevel5EquipmentFromPool()
    {
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            if (!level5Equipments[i].activeSelf) return level5Equipments[i];
        }
        return null;
    }
    public GameObject Getlevel6EquipmentFromPool()
    {
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            if (!level6Equipments[i].activeSelf) return level6Equipments[i];
        }
        return null;
    }
    #endregion
    #region  LevelTablet Pool
    private void CreateTabletsPool()
    {
        for (int i = 0; i < amountOfEachTablet; i++)
        {
            var level1Tablet = Instantiate(level1TabletPrefab);
            level1Tablet.transform.SetParent(transform);
            level1Tablet.SetActive(false);
            level1Tablets.Add(level1Tablet);
        }
        for (int i = 0; i < amountOfEachTablet; i++)
        {
            var level2Tablet = Instantiate(level2TabletPrefab);
            level2Tablet.transform.SetParent(transform);
            level2Tablet.SetActive(false);
            level2Tablets.Add(level2Tablet);
        }
        for (int i = 0; i < amountOfEachTablet; i++)
        {
            var level3Tablet = Instantiate(level3TabletPrefab);
            level3Tablet.transform.SetParent(transform);
            level3Tablet.SetActive(false);
            level3Tablets.Add(level3Tablet);
        }
        for (int i = 0; i < amountOfEachTablet; i++)
        {
            var level4Tablet = Instantiate(level4TabletPrefab);
            level4Tablet.transform.SetParent(transform);
            level4Tablet.SetActive(false);
            level4Tablets.Add(level4Tablet);
        }
        for (int i = 0; i < amountOfEachTablet; i++)
        {
            var level5Tablet = Instantiate(level5TabletPrefab);
            level5Tablet.transform.SetParent(transform);
            level5Tablet.SetActive(false);
            level5Tablets.Add(level5Tablet);
        }
        for (int i = 0; i < amountOfEachTablet; i++)
        {
            var level6Tablet = Instantiate(level6TabletPrefab);
            level6Tablet.transform.SetParent(transform);
            level6Tablet.SetActive(false);
            level6Tablets.Add(level6Tablet);
        }
    }
    public GameObject GetTabletFromPool(int level)
    {
        switch (level)
        {
            case 1:
                return Getlevel1TabletFromPool();
            case 2:
                return Getlevel2TabletFromPool();
            case 3:
                return Getlevel3TabletFromPool();
            case 4:
                return Getlevel4TabletFromPool();
            case 5:
                return Getlevel5TabletFromPool();
            case 6:
                return Getlevel6TabletFromPool();
        }
        return null;

    }
    public GameObject Getlevel1TabletFromPool()
    {
        for (int i = 0; i < amountOfEachTablet; i++)
        {
            if (!level1Tablets[i].activeSelf) return level1Tablets[i];
        }
        return null;
    }
    public GameObject Getlevel2TabletFromPool()
    {
        for (int i = 0; i < amountOfEachTablet; i++)
        {
            if (!level2Tablets[i].activeSelf) return level2Tablets[i];
        }
        return null;
    }
    public GameObject Getlevel3TabletFromPool()
    {
        for (int i = 0; i < amountOfEachTablet; i++)
        {
            if (!level3Tablets[i].activeSelf) return level3Tablets[i];
        }
        return null;
    }
    public GameObject Getlevel4TabletFromPool()
    {
        for (int i = 0; i < amountOfEachTablet; i++)
        {
            if (!level4Tablets[i].activeSelf) return level4Tablets[i];
        }
        return null;
    }
    public GameObject Getlevel5TabletFromPool()
    {
        for (int i = 0; i < amountOfEachTablet; i++)
        {
            if (!level5Tablets[i].activeSelf) return level5Tablets[i];
        }
        return null;
    }
    public GameObject Getlevel6TabletFromPool()
    {
        for (int i = 0; i < amountOfEachTablet; i++)
        {
            if (!level6Tablets[i].activeSelf) return level6Tablets[i];
        }
        return null;
    }

    #endregion
    #region  PopUp Pool
    private void CreateAllPopUpPool()
    {
        for (int i = 0; i < amountOfPopUps; i++)
        {
            var popUp = Instantiate(animatedMoneyPrefab);
            popUp.transform.SetParent(transform);
            popUp.SetActive(false);
            moneyPopUps.Add(popUp);
        }
        for (int i = 0; i < amountOfPopUps; i++)
        {
            var popUp = Instantiate(fortUpgradePopUPrefab);
            popUp.transform.SetParent(transform);
            popUp.SetActive(false);
            fortUpgradePopUps.Add(popUp);
        }
    }

    public GameObject GetMoneyPopUp()
    {
        for (int i = 0; i < amountOfPopUps; i++)
        {
            if (!moneyPopUps[i].activeSelf) return moneyPopUps[i];
        }
        return null;
    }
    public GameObject GetFortUpgradePopUp()
    {
        for (int i = 0; i < amountOfPopUps; i++)
        {
            if (!fortUpgradePopUps[i].activeSelf) return fortUpgradePopUps[i];
        }
        return null;
    }


    #endregion
    #region  Particles Pool
    private void CreateAllParticlePool()
    {
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            var particle = Instantiate(buyEquipmentParticlePrefab);
            particle.transform.SetParent(transform);
            particle.SetActive(false);
            buyEquipmentParticles.Add(particle);
        }
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            var particle = Instantiate(hitParticlePrefab);
            particle.transform.SetParent(transform);
            particle.SetActive(false);
            hitParticles.Add(particle);
        }
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            var particle = Instantiate(mergeEquipmentLvl2ParticlePrefab);
            particle.transform.SetParent(transform);
            particle.SetActive(false);
            mergeEquipmentLvl2Particles.Add(particle);
        }
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            var particle = Instantiate(mergeEquipmentLvl3ParticlePrefab);
            particle.transform.SetParent(transform);
            particle.SetActive(false);
            mergeEquipmentLvl3Particles.Add(particle);
        }
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            var particle = Instantiate(mergeEquipmentLvl4ParticlePrefab);
            particle.transform.SetParent(transform);
            particle.SetActive(false);
            mergeEquipmentLvl4Particles.Add(particle);
        }
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            var particle = Instantiate(mergeEquipmentLvl5ParticlePrefab);
            particle.transform.SetParent(transform);
            particle.SetActive(false);
            mergeEquipmentLvl5Particles.Add(particle);
        }
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            var particle = Instantiate(mergeEquipmentLvl6ParticlePrefab);
            particle.transform.SetParent(transform);
            particle.SetActive(false);
            mergeEquipmentLvl6Particles.Add(particle);
        }
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            var particle = Instantiate(equipParticlePrefab);
            particle.transform.SetParent(transform);
            particle.SetActive(false);
            equipParticles.Add(particle);
        }
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            var particle = Instantiate(upgradeFortParticlePrefab);
            particle.transform.SetParent(transform);
            particle.SetActive(false);
            upgradeFortParticles.Add(particle);
        }
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            var particle = Instantiate(deadEffectParticlePrefab);
            particle.transform.SetParent(transform);
            particle.SetActive(false);
            deadEffectParticles.Add(particle);
        }
    }

    public GameObject GetMergeEffects(int lvl)
    {
        switch (lvl)
        {
            case 2:
                return GetMergeLvl2EquipmentParticlesFromPool();
            case 3:
                return GetMergeLvl3EquipmentParticlesFromPool();
            case 4:
                return GetMergeLvl4EquipmentParticlesFromPool();
            case 5:
                return GetMergeLvl5EquipmentParticlesFromPool();
            case 6:
                return GetMergeLvl6EquipmentParticlesFromPool();
        }
        return null;
    }
    public GameObject GetMergeLvl2EquipmentParticlesFromPool()
    {
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            if (!mergeEquipmentLvl2Particles[i].activeSelf) return mergeEquipmentLvl2Particles[i];
        }
        return null;
    }

    public GameObject GetMergeLvl3EquipmentParticlesFromPool()
    {
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            if (!mergeEquipmentLvl3Particles[i].activeSelf) return mergeEquipmentLvl3Particles[i];
        }
        return null;
    }
    public GameObject GetMergeLvl4EquipmentParticlesFromPool()
    {
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            if (!mergeEquipmentLvl4Particles[i].activeSelf) return mergeEquipmentLvl4Particles[i];
        }
        return null;
    }
    public GameObject GetMergeLvl5EquipmentParticlesFromPool()
    {
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            if (!mergeEquipmentLvl5Particles[i].activeSelf) return mergeEquipmentLvl5Particles[i];
        }
        return null;
    }
    public GameObject GetMergeLvl6EquipmentParticlesFromPool()
    {
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            if (!mergeEquipmentLvl6Particles[i].activeSelf) return mergeEquipmentLvl6Particles[i];
        }
        return null;
    }
    public GameObject GetBuyEquipmentParticlesFromPool()
    {
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            if (!buyEquipmentParticles[i].activeSelf) return buyEquipmentParticles[i];
        }
        return null;
    }
    public GameObject GetHitParticlesFromPool()
    {
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            if (!hitParticles[i].activeSelf) return hitParticles[i];
        }
        return null;
    }
    public GameObject GetEquipParticlesFromPool()
    {
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            if (!equipParticles[i].activeSelf) return equipParticles[i];
        }
        return null;
    }
    public GameObject GetUpgradeFortParticlesFromPool()
    {
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            if (!upgradeFortParticles[i].activeSelf) return upgradeFortParticles[i];
        }
        return null;
    }
    public GameObject GetDeadEffectParticlesFromPool()
    {
        for (int i = 0; i < amountOfEachParticle; i++)
        {
            if (!deadEffectParticles[i].activeSelf) return deadEffectParticles[i];
        }
        return null;
    }
    #endregion
}

