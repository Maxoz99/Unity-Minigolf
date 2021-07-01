using System;
using System.Collections.Generic;
using Code.World;
using UnityEngine;

namespace Code.Player
{
    public class PlayerInfo : MonoBehaviour
    {

        public static PlayerInfo info;

        private Queue<Vector3> _obstaclePositions = new Queue<Vector3>();
        private Queue<Vector3> _pitPositions = new Queue<Vector3>();


        public int HitObstacles { get; private set; }
        public int HitPits { get; private set; }
        public float DistanceToNextObstacle { get; private set; }
        public float DistanceToNextPit { get; private set; }
        public float DistanceToNextObject { get; private set; }

        private void Awake()
        {
            // Destroy any other existing Player Info
            // if (info != null && info != this){
            //
            //     Destroy(gameObject);
            // }
            // else{
            //     DontDestroyOnLoad(gameObject);
            //     info = this;
            // }
        }

        private void Update()
        {
            if (_obstaclePositions.Count > 0)
            {
                DistanceToNextObstacle = Vector3.Distance(_obstaclePositions.Peek(), transform.position);
                CheckObstaclePassed();
            }
            
            if (_pitPositions.Count > 0)
            {
                DistanceToNextPit = Vector3.Distance(_pitPositions.Peek(), transform.position);
                CheckPitPassed();
            }

            // Will ich das wirklich jeden Frame updaten?
            DistanceToNextObject = Mathf.Min(DistanceToNextObstacle, DistanceToNextPit);
            
        }


        // private void FixedUpdate()
        // {
        //     if (_obstaclePositions.Count > 0)
        //     {
        //         Debug.DrawLine(transform.position, _obstaclePositions.Peek(), Color.red);
        //     }
        //
        //     if (_pitPositions.Count > 0)
        //     {
        //         Debug.DrawLine(transform.position, _pitPositions.Peek(), Color.yellow);
        //     }
        // }

        private void OnEnable()
        {
            PlayerBehavior.HitObstacle += OnObstacleHit;
            PlayerBehavior.HitPit += OnPitHit;
            WorldGenerator.ObstaclePlaced += OnNewCloseObstacle;
            WorldGenerator.PitPlaced += OnNewClosePit;
            PlayerBehavior.Reset += OnReset;
        }

        private void OnDisable()
        {
            PlayerBehavior.HitObstacle -= OnObstacleHit;
            PlayerBehavior.HitPit -= OnPitHit;
            WorldInfo.NewCloseObstacle -= OnNewCloseObstacle;
            WorldInfo.NewClosePit -= OnNewClosePit;
            PlayerBehavior.Reset -= OnReset;
        }

        private void OnObstacleHit()
        {
            HitObstacles++;
            Debug.Log("Obstacles hit: " + HitObstacles);
        }

        private void OnPitHit()
        {
            HitPits++;
            Debug.Log("Pits hit: " + HitPits);
        }

        private void OnNewCloseObstacle(Vector3 pos)
        {
            _obstaclePositions.Enqueue(pos);
        }
        
        private void OnNewClosePit(Vector3 pos)
        {
            _pitPositions.Enqueue(pos);
        }

        private void OnReset()
        {
            _obstaclePositions.Clear();
            _pitPositions.Clear();
        }

        private void CheckObstaclePassed()
        {
            if (_obstaclePositions.Peek().z - transform.position.z < 0)
            {
                _obstaclePositions.Dequeue();
            }
        }

        private void CheckPitPassed()
        {
            if (_pitPositions.Peek().z - transform.position.z < 0)
            {
                _pitPositions.Dequeue();
            }
        }
        

        // returns in which lane the player is at that moment (left=1, middle=2, right=3)
        public int GetLane()
        {
            if (transform.position.x > 0.5f )
            {
                return 3;
            }
            if (transform.position.x < -0.5f)
            {
                return 1;
            }

            return 2;
        }
    }
}
