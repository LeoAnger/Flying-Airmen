namespace NetWork.Entity
{
    [System.Serializable]
    public class EnemyEntity
    {
        public string ObjName { get; set; }
        
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        
        public float LocalScaleX { get; set; }
        public float LocalScaleY { get; set; }
        public float LocalScaleZ { get; set; }     
    }
}