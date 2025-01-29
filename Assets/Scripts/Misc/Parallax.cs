using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour{

    [SerializeField] private float parallaxOffset = -0.15f;

    private Camera cam;
    private Vector2 startingPosition;
    private Vector2 travel => (Vector2)cam.transform.position - startingPosition;

    private void Start(){
        cam = Camera.main;
        startingPosition = transform.position;
    }

    private void LateUpdate(){
        transform.position = startingPosition + travel * parallaxOffset;
    }

}
