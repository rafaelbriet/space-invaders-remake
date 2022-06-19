using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvadersRemake
{
    public class InvasionRow : MonoBehaviour
    {
        public float MoveDirection { get; set; }
        //private float marchDirection = -1f;

        //public InvasionCommander InvasionCommander { get; set; }

        //private void Update()
        //{
        //    //if (transform.position.x < InvasionCommander.ScreenBounds)
        //    //{
        //    //    marchDirection = 1f;
        //    //}
        //    //else if (transform.position.x > InvasionCommander.ScreenBounds)
        //    //{
        //    //    marchDirection = -1f;
        //    //}


        //    //float movePosition = transform.position.x + (marchDirection * (invasionStartingSpeed + (1f / invasionSize.y * i)) * Time.deltaTime);
        //    float movePosition = transform.position.x + (marchDirection * 5f * Time.deltaTime);
        //    transform.position = new Vector3(movePosition, transform.position.y, transform.position.z);
        //}
    }
}
