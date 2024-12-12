using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;

public class ColorChangeMaterial : MonoBehaviour
{
    public GameObject cube;
    private BoxCollider boxCollider;
    private MeshRenderer meshRenderer;
    void Start()
    {
        boxCollider = cube.GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();   
        meshRenderer.material.color = Color.black;
    }

    void Update()
    {                
        //게임 오브젝트는  큐브 오브젝트의 박스 콜라이더 범위를 벗어날 수 없다    
        //Mathf.Clamp는 (제한하려는 값, 허용 가능한 최소 값, 허용가능한 최대값)
        // boxCollider.bounds.size는 컴포넌트의 스케일까지 고려된 월드 좌표계에서의 크기입니다.
        // 참고로 월드 좌표계에서의 크기는 자신부터 루트 부모,
        // 즉 최상위 부모까지의 스케일을 모두 적용했을 때의 크기입니다
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x,
                boxCollider.bounds.min.x,
                boxCollider.bounds.max.x),0, 
            Mathf.Clamp(transform.position.z,
                boxCollider.bounds.min.z, 
                boxCollider.bounds.max.z));
        
        //게임 오브젝트는 큐브 오브젝트의 박스 콜라이더 내에서 위치를 이동할 때마다 색상이 변화한다.
        meshRenderer.material.color =
            new Color((transform.position.x + boxCollider.bounds.size.x) 
                      / boxCollider.bounds.size.x,
                0,
                (transform.position.z + boxCollider.bounds.size.z) 
                / boxCollider.bounds.size.z);
        
    }
}
