﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth_Basic : MonoBehaviour
{
    public Image image_HP;

    public Color MaxColor = Color.green;
    public Color HalfColor = Color.yellow;
    public Color MinColor = Color.red;

    private Camera mainCamera;
    private PlayerHealth playerHealth;

    private RectTransform rectTransform;
    private CapsuleCollider capsuleCollider;

    private float lerp = 10f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        playerHealth = GetComponentInParent<PlayerHealth>();

        mainCamera = Camera.main;

        capsuleCollider = GetComponentInParent<CapsuleCollider>();
        rectTransform.localPosition = new Vector3(0, capsuleCollider.height, 0);

        Vector2 size = rectTransform.sizeDelta;
        if(capsuleCollider.radius >=1)
        {
            size.x *= capsuleCollider.radius;
            rectTransform.sizeDelta = size;
        }
    }

    void Update()
    {
        float percent = 0;
        if (playerHealth) percent = playerHealth.CurrentHP / playerHealth.MaxHP;

        image_HP.fillAmount = Mathf.Lerp(image_HP.fillAmount, percent, lerp * Time.deltaTime);

        if(percent >= 0.5f)
        {
            image_HP.color = Color.Lerp(HalfColor, MaxColor, (percent - 0.5f) * 2f);
        }
        else
        {
            image_HP.color = Color.Lerp(MinColor, HalfColor, percent * 2f);
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}
