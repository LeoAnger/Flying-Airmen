using System.Collections.Generic;

namespace Script.netWork.UserInfo
{
    public class RoleInfo
    {
        // 玩家选定的角色Id
        public static int RoleId1 = 1;
        public static int RoleId2 = 2;
        
        // 角色信息表
        public SortedList<int, string> RoleNickNameSortedList = new SortedList<int, string>()
        {
            {1, "Tsunade"},
            {2, "manjimaru"}
            
        };
        
        // 角色头像路径
       public SortedDictionary<int, string> RoleIconSortedList = new SortedDictionary<int, string>()
       {
           {1,"Role/tsunade/icon"},
           {2,"Role/manjimaru/icon"}
           
       };
    }
}