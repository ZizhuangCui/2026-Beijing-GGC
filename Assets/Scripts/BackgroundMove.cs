using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    [SerializeField] private Vector2 moveSpeed;
    private Vector2 offset;
    private Material material;

    private void Awake() {
        material = GetComponent<Renderer>().material;
    }

    private void Update() {
        offset = Time.deltaTime * moveSpeed;
        material.mainTextureOffset += offset;
    }
}
