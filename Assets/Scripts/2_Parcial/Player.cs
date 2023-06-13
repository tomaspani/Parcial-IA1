using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 5f;


    private void Update()
    {
        transform.position = new Vector3(transform.position.x + Input.GetAxisRaw("Horizontal") * _speed * Time.deltaTime,
                                transform.position.y, transform.position.z + Input.GetAxisRaw("Vertical") * _speed * Time.deltaTime);
    }
}
