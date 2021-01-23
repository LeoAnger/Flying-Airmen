using Script.Enum;

namespace NetWork.Entity
{
    [System.Serializable]
    public class SourceDataEntity
    {
        public SourceDataType SourceDataType { get; set; }
        public string Content { get; set; }
        public int DataSerialNumber { get; set; }    // 默认值：0
    }
}