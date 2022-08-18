using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    public class CameraMovement : MonoBehaviour
    {
        public GameObject Target;
        public float FollowSpeed = 2f;
        public float OffsetY = -10f;
        public float OffsetZ = -15f;

        private IPlayerController playerController;

        // Start is called before the first frame update
        void Start()
        {
            playerController = Target.GetComponent<IPlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            var newPositionZ = OffsetZ;
            var newPositionY = Target.transform.position.y + OffsetY;
            if(Target.transform.position.y >= 5.0f)
            {
                newPositionZ += -(Target.transform.position.y / 2);
                newPositionY -= OffsetY;
            }

            Vector3 newPos = new Vector3(Target.transform.position.x, Target.transform.position.y + OffsetY, newPositionZ);
            transform.position =  Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
        }
    }
}
