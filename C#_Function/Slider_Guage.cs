Heirarchy에 Slider를 생성하고 스페이스를 누르면 게이지가 오르고 최대 값에서 서서히 작아졌다가 다시오르는 코드
-----
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviour
{
    public Slider slider;
    public float maxPower;
    public float currentPower;
    public float fillSpeed;
    bool isFull = false;

    void Update()
    {
        
        if (Input.GetKey(KeyCode.Space))
        {
            if (isFull == false)
            {
                currentPower += fillSpeed * Time.deltaTime;
                if (currentPower >= maxPower)
                {
                    currentPower = maxPower;
                    isFull = true;
                }
            }
            else
            {
                currentPower -= fillSpeed * Time.deltaTime;
                if (currentPower <= 0)
                {
                    currentPower = 0;
                    isFull = false;
                }
            }
            slider.value = currentPower / maxPower;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            // 발사체 생성
            GameObject cannonBallInstance = Instantiate(cannonBall,
                firePoint.transform.position, Quaternion.identity);

            // 발사체에 힘 가하기
            cannonBallInstance.GetComponent<Rigidbody>()
                .AddForce(firePoint.transform.forward * currentPower, ForceMode.Impulse);

          
            // 파워 초기화
            currentPower = 0f;
            slider.value = 0f;
        }
    }
}
