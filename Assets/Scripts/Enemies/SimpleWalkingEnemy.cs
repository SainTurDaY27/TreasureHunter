using UnityEngine;

namespace Enemies
{
    public class SimpleWalkingEnemy : MonoBehaviour
    {
        public float walkSpeed = 3f;

        public enum WalkableDirection
        {
            Left,
            Right
        }

        private WalkableDirection _walkDirection;

        public WalkableDirection WalkDirection
        {
            get => _walkDirection;
            set
            {
                if (_walkDirection != value)
                {
                    // Flip scale
                    var o = gameObject;
                    var oldLocaleScale = o.transform.localScale;
                    o.transform.localScale = new Vector2(oldLocaleScale.x * -1, oldLocaleScale.y);
                }

                _walkDirection = value;
            }
        }

        private Vector2 WalkDirectionVector
        {
            get
            {
                if (WalkDirection == WalkableDirection.Left)
                {
                    return Vector2.left;
                }
                else
                {
                    return Vector2.right;
                }
            }
        }
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
