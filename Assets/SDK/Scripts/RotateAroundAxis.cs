using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public class RotateAroundAxis : MonoBehaviour
    {
        [SerializeField] private float zSpeed;
        [SerializeField] private float xSpeed;
        [SerializeField] private float ySpeed;

        private void Update()
        {
            transform.Rotate(Time.deltaTime * xSpeed, Time.deltaTime * ySpeed, Time.deltaTime * zSpeed);
        }
    }
}