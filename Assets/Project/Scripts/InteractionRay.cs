﻿using UnityEngine;

public class InteractionRay : MonoBehaviour
{
    public float length = 4f;

    private Interactable hover;
    private GameObject parent;
    private Controller controller;

	// Use this for initialization
	void Start ()
    {
        hover = null;
        parent = gameObject.transform.parent.gameObject;
        controller = parent.GetComponent<Controller>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!controller) {
            controller = parent.GetComponent<Controller>();
        }
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Interactable objectHit = null;

        if (Physics.Raycast(ray, out hit, length)) {
            objectHit = hit.collider.gameObject.GetComponent<Interactable>();
        }

        if(objectHit != hover) {
            if (hover != null && hover.HoverHUD != null) {
                hover.HoverHUD.GetComponent<CanvasGroup>().alpha = 0f;
            }
            if (objectHit != null && objectHit.HoverHUD != null) {
                objectHit.HoverHUD.GetComponent<CanvasGroup>().alpha = 1f;
            }
            hover = objectHit;
        } 

        if(controller.GetButtonDown("LeftHand") && hover != null) {
            hover.interact(parent);
        }
    }
}
