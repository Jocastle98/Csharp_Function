using System.Collections;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed =3f;

    
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); //W, S 방향키 위 아래
        float v = Input.GetAxis("Vertical"); // A, D 방향키 왼쪽 오른쪽

        Vector3 dir = new Vector3(h, 0, v);
        transform.position += dir * moveSpeed * Time.deltaTime;
       
    }
    
}
