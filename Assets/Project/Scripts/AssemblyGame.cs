﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssemblyGame : Minigame {
    public Vector3 dropLocation = new Vector3(0.2f, 0.5f, 0f);
    public Transform tomatoPrefab;
    public Transform lettucePrefab;
    public Transform topBunPrefab;
    public Transform bottomBunPrefab;
    public Transform pattyPrefab;
    public Transform cheesePrefab;
    public Text buildText;

    private string[] ingredientSlot;
    private string[] selectedSlots;
    private List<string> currentBuild;
    private GameObject buildSpace;

    // Use this for initialization
    new void Start () {
        base.Start();

        ingredientSlot = new string[8];

        ingredientSlot[0] = "TopBun";
        ingredientSlot[1] = "Patty";
        ingredientSlot[2] = "BottomBun";
        ingredientSlot[3] = "Cheese";
        ingredientSlot[4] = "Lettuce";
        ingredientSlot[5] = "Tomato";
        ingredientSlot[6] = "";
        ingredientSlot[7] = "";

        selectedSlots = new string[4];
        for (int i = 0; i < selectedSlots.Length; i++)
            selectedSlots[i] = "";

        currentBuild = new List<string>();
        newBuildSpace();
	}
	
    public override void complete() {
        Destroy(buildSpace.gameObject);
        newBuildSpace();

        currentBuild = new List<string>();
        updateHUD();
    }

    protected override void activeUpdate() {
        if(Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.LeftControl)) {
            if(Input.GetKey(KeyCode.LeftAlt)) {
                selectSide(1);
            } else if(Input.GetKey(KeyCode.LeftControl)) {
                selectSide(0);
            } else {
                selectSide(-1);
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftControl)) {
            selectSide(0);
        } else if(Input.GetKeyDown(KeyCode.LeftAlt)) {
            selectSide(1);
        }
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            addIngredient(0);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            addIngredient(1);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)) {
            addIngredient(2);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            addIngredient(3);
        }
        if(Input.GetKeyDown(KeyCode.Return)) {
            complete();
        }
    }

    private void selectSide(int side) {
        if(side >= 0) {
            int off = side * selectedSlots.Length;
            for (int x = 0; x < selectedSlots.Length; x++)
                selectedSlots[x] = ingredientSlot[x + off];
            string panelName = side == 0 ? "LeftItems" : "RightItems";
            string otherPanel = side == 1 ? "LeftItems" : "RightItems";
            HUD.transform.Find(panelName).transform.Find("BGPanel").GetComponent<Image>().color = new Color(0.55f, 1.0f, 0.5f, 0.35f);
            HUD.transform.Find(otherPanel).transform.Find("BGPanel").GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
        } else {
            for(int x = 0; x < selectedSlots.Length; x++) {
                selectedSlots[x] = "";
            }
            HUD.transform.Find("LeftItems").transform.Find("BGPanel").GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
            HUD.transform.Find("RightItems").transform.Find("BGPanel").GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
        }
    }

    private void addIngredient(int slot) {
        if(selectedSlots[slot] != "") {
            currentBuild.Add(selectedSlots[slot]);
            Transform ingredient;
            switch (selectedSlots[slot]) {
                case "Tomato":
                    ingredient = Instantiate(tomatoPrefab);
                    break;
                case "Lettuce":
                    ingredient = Instantiate(lettucePrefab);
                    break;
                case "TopBun":
                    ingredient = Instantiate(topBunPrefab);
                    break;
                case "BottomBun":
                    ingredient = Instantiate(bottomBunPrefab);
                    break;
                case "Patty":
                    ingredient = Instantiate(pattyPrefab);
                    break;
                case "Cheese":
                    ingredient = Instantiate(cheesePrefab);
                    break;
                default:
                    ingredient = Instantiate(pattyPrefab);
                    break;
            }
            ingredient.parent = buildSpace.transform;
            ingredient.localPosition = dropLocation;
            ingredient.rotation = Quaternion.identity;
            updateHUD();
        }
    }

    private void updateHUD() {
        string text = "Build:\n";
        foreach(string ingredient in currentBuild) {
            text += ingredient + "\n";
        }
        buildText.text = text;
    }

    private void newBuildSpace() {
        buildSpace = new GameObject();
        buildSpace.transform.parent = transform.parent.transform.Find("AssemblyTop").transform;
        buildSpace.transform.localPosition = Vector3.zero;
    }
}