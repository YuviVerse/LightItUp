using LightItUp.Data;
using UnityEngine;

namespace LightItUp.Game
{
    public class MissileController : MonoBehaviour
    {
        public float speed;
        public float avoidanceDistance;
        public LayerMask obstacleLayers;

        private Transform target;
        private bool hasReachedTarget;

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        private void Update()
        {
            if (target != null && !hasReachedTarget)
            {
                var distanceToTarget = Vector3.Distance(transform.position, target.position);
                var targetThreshold = 1f;
                if (distanceToTarget <= targetThreshold)
                {
                    hasReachedTarget = true;
                    HandleImpact();
                }
                else
                {
                    var position = transform.position;
                    var moveDirection = (target.position - position).normalized;

                    var hit = Physics2D.Raycast(position, moveDirection, avoidanceDistance, obstacleLayers);
                    if (hit.collider != null)
                    {
                        moveDirection = Quaternion.Euler(0, 0, 45) * moveDirection;
                    }

                    transform.position += moveDirection * speed * Time.deltaTime;
                }
            }
        }

        private void HandleImpact()
        {
            hasReachedTarget = true;
            var block = target.GetComponent<BlockController>();
            block.OnMissileHit();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, GameSettings.CameraFocus.missilesShotFocusDuration);
        }
    }
}