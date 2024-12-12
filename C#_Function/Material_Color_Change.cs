using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeMaterial : MonoBehaviour
{
    public GameObject cube;

    void Update()
    {
        GetComponent<MeshRenderer>()
            .material.color = Color.black;
        
        
        BoxCollider boxCollider = cube.GetComponent<BoxCollider>();
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x,
                boxCollider.bounds.min.x,
                boxCollider.bounds.max.x),0, 
            Mathf.Clamp(transform.position.z,
                boxCollider.bounds.min.z, 
                boxCollider.bounds.max.z));
        
        GetComponent<MeshRenderer>().material.color =
            new Color((transform.position.x + boxCollider.size.x) / boxCollider.size.x, 0,
                (transform.position.z + boxCollider.size.z) / boxCollider.size.z);
        
    }
    
}

--
큐브 바닥의 지정 범위 내에서 자신의 게임오브젝트를 움직이면 위치에 따라 색상이 변화한다.
