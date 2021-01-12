namespace Script.NetWork.Entity
{
    [System.Serializable]
    public class GameObjEntity
    {
        public bool CreateOrDestroy { get; set; }    //创建物体or销毁物体
        public string ObjName { get; set; }    //实体名字
        public string PrefabName { get; set; } //预制体名字
        public string ScriptName { get; set; } //脚本名字
        
        //position
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        
        //scale
        public float LocalScaleX { get; set; }
        public float LocalScaleY { get; set; }
        public float LocalScaleZ { get; set; }
        
        

    }
}