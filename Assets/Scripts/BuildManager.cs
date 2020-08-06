﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public TurretData laserTurretData;
    public Text laserMoneyText;
    public TurretData missileTurretData;
    public Text missileMoneyText;
    public TurretData standardTurretData;
    public Text standardMoneyText;
    public Text moneyText;
    public Animator moneyAnimator;
    public GameObject upgradeCanvas;
    public Button buttonUpgrade;

    private int money = 1000;
    private static BuildManager _instance;
    private TurretData selectedTurretData;
    private GameObject selectedTurretGo;

    public static BuildManager Ins()
    {
        return _instance;
    }

    protected virtual void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        laserMoneyText.text = laserTurretData.cost.ToString();
        missileMoneyText.text = missileTurretData.cost.ToString();
        standardMoneyText.text = standardTurretData.cost.ToString();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube"));
                if (isCollider)
                {
                    MapCube mapCube = hit.collider.GetComponent<MapCube>();
                    if (selectedTurretData != null && mapCube.turretGo == null)
                    {
                        if (money >= selectedTurretData.cost)
                        {
                            ChangeMoney(-selectedTurretData.cost);
                            mapCube.BuildTurret(selectedTurretData.turretPrefab);
                        }
                        else
                        {
                            moneyAnimator.SetTrigger("Flicker");
                        }
                    }
                    else if (mapCube.turretGo != null)
                    {
                        if (mapCube.turretGo == selectedTurretGo && upgradeCanvas.activeInHierarchy)
                        {
                            HideUpgradeUI();
                        }
                        else
                        {
                            selectedTurretGo = mapCube.turretGo;
                            ShowUpgradeUI(mapCube.transform.position, mapCube.isUpgraded);
                        }
                    }
                }
            }
        }
    }

    public void OnLaserSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = laserTurretData;
        }
    }

    public void OnMissileSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = missileTurretData;
        }
    }

    public void OnStandardSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = standardTurretData;
        }
    }

    public void ChangeMoney(int change = 0)
    {
        money += change;
        moneyText.text = "￥" + money;
    }

    void ShowUpgradeUI(Vector3 pos, bool isDisableUpgrade = false)
    {
        upgradeCanvas.SetActive(true);
        upgradeCanvas.transform.position = pos;
        buttonUpgrade.interactable = !isDisableUpgrade;
    }

    void HideUpgradeUI()
    {
        upgradeCanvas.SetActive(false);
    }

    public void OnUpgradeButtonDown()
    {

    }

    public void OnDestoryButtonDown()
    {

    }
}
