using DamageSystem;
using System.Collections;
using UnityEngine;
using VRBeats.ScriptableEvents;

namespace VRBeats
{
    public class VR_BeatCube : MonoBehaviour
    {
        [SerializeField] private float minCutSpeed = 0.5f;
        [SerializeField] private OnSliceAction sliceAction = null;
        public GameEvent onCorrectSlice = null;
        [SerializeField] private GameEvent onIncorrectSlice = null;
        [SerializeField] private GameEvent onPlayerMiss = null;
        [SerializeField] Rigidbody rbLeft;
        [SerializeField] Rigidbody rbRight;

        public GameObject particles;
        public bool canMove = true;

        private MaterialBindings materialBindings = null;
        private ColorSide thisColorSide = ColorSide.Right;
        private Transform player = null;
        private VR_BeatCubeSpawneable thisSpawneable = null;

        private bool canBeKilled = true;
        private bool spawnComplete = false;
        private bool destroyed = false;

        public float MinCutSpeed { get { return minCutSpeed; } }
        public Direction HitDirection { get { return thisSpawneable.HitDirection; } }
        public ColorSide ThisColorSide { get { return thisColorSide; } }

        private void Awake()
        {
            player = VR_BeatManager.instance.Player.transform;
        }

        public void Start()
        {
            thisSpawneable = GetComponent<VR_BeatCubeSpawneable>();
            thisSpawneable.onSpawnComplete += delegate { spawnComplete = true; };

            materialBindings = GetComponent<MaterialBindings>();

            thisColorSide = thisSpawneable.ColorSide;
            Color color = VR_BeatManager.instance.GetColorFromColorSide(thisColorSide);
            materialBindings.SetEmmisiveColor(color);

            //rbLeft.AddForce(new Vector3(1, 1, 0) * 100f);
            //rbRight.AddForce(new Vector3(-1, -1, 0) * 100f);
        }

        private void OnDestroy()
        {
            destroyed = true;
        }

        public void OnCut(DamageInfo info)
        {
            StartCoroutine(StopShowText());
            //if (int.Parse(FindObjectOfType<ScoreManager>().scoreLabel.text) <= 300)
            //{
            //    //GameEventManager.instance.etkisizHaleGetirir.enabled = true;
            //}
            //else
            //{
            //    //GameEventManager.instance.etkisizHaleGetirir.enabled = false;
            //}
            canBeKilled = false;

            //notify to whoever is listening that the player did a correct/incorrect slice
            if (IsCutIntentValid(info as BeatDamageInfo))
            {
               
                if (GameEventManager.instance.canSpawnWall)
                {
                    
                    GameEventManager.instance.SpawnWallForCube();
                    GameEventManager.instance.canSpawnWall = false;
                }
                onCorrectSlice.Invoke();
                //rbLeft.isKinematic = false;
                //rbRight.isKinematic = false;
                //rbLeft.AddForce(new Vector3(0, 0, -1) * 1f);
                //rbRight.AddForce(new Vector3(0, 0, -1) * 1f);
                //rbLeft.AddForce(new Vector3(1, 1, 0) * 100f);
                //rbRight.AddForce(new Vector3(-1, -1, 0) * 100f);
            }
            else
            {
                onIncorrectSlice.Invoke();
            }

        }
        IEnumerator StopShowText()
        {
            yield return new WaitForSeconds(1.5f);
            GameEventManager.instance.etkisizHaleGetirir.enabled = false;
        }
        private bool IsCutIntentValid(BeatDamageInfo info)
        {
            if (info == null) return false;

            if (info.velocity < minCutSpeed) return false;

            //no matter the hit direction as soon as we have the right velocity for a cube that has a dot
            if (HitDirection == Direction.Center)
                return true;
            else
            {
                return true;
            }
            //float cutAngle = Vector2.Angle(transform.up, info.hitDir);
            //return info.colorSide == ThisColorSide && cutAngle < 80.0f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Shield"))
            {
                KillShield();
            }
        }

        private void Update()
        {
            if (spawnComplete)
            {
                if (canMove)
                {
                    transform.position += Vector3.forward * thisSpawneable.Speed * Time.deltaTime;
                }
            }

            if (ShouldKillCube())
            {
                KillShield();
            }
        }

        private bool ShouldKillCube()
        {
            return canBeKilled && transform.position.z < player.position.z - 2.0f;
        }

        public void Kill()
        {
            onPlayerMiss.Invoke();
            canBeKilled = false;
            transform.ScaleTween(Vector3.zero, 20f).SetEase(Ease.EaseOutExpo).SetOnComplete(delegate
           {
               if (!destroyed)
                   Destroy(gameObject, 9f);
           });
        }

        public void KillShield()
        {
            onPlayerMiss.Invoke();
            canBeKilled = false;
            transform.ScaleTween(Vector3.zero, 1.0f).SetEase(Ease.EaseOutExpo).SetOnComplete(delegate
            {
                if (!destroyed)
                    Destroy(gameObject);
            });
        }
    }

}

