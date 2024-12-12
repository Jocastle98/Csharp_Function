using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeRestriction : MonoBehaviour
{
    public GameObject cube;

    void Start()
    {
      //큐브 오브젝트의 콜라이더를 가져오기
      BoxCollider boxCollider = cube.GetComponent<BoxCollider>();
    }
    
    void Update()
    {
        //큐브오브젝트 범위 내에서 나갈 수 없도록 범위 제한
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x,
                boxCollider.bounds.min.x,
                boxCollider.bounds.max.x),0, 
            Mathf.Clamp(transform.position.z,
                boxCollider.bounds.min.z, 
                boxCollider.bounds.max.z));
        
    }
    
}
