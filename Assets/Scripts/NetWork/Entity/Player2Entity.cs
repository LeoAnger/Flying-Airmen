/**
     1.position
     2.localScale
     3.animation
     */
namespace Script.NetWork.Entity
{
    [System.Serializable]
    public class Player2Entity
    {
//        public Vector3 position { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
//        public Vector3 localScale { get; set; }
        public float LocalScaleX { get; set; }
        public float LocalScaleY { get; set; }
        public float LocalScaleZ { get; set; }
        
        // Animator参数
        public float running { get; set; }
        public bool jumping { get; set; }
        public bool falling { get; set; }
        public bool idle { get; set; }
    }
}