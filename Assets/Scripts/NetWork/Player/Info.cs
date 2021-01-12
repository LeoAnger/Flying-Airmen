namespace Script.NetWork.Player
{
    /*
     * 1.仅仅对C#脚本（Player1、Player2...）使用
     * 2.不需要Json映射
     * 3.Player创建的初始值 --> 每个英雄的移速、跳跃高度、跳跃段数等不同
     */
    public class Info
    {
        // Player1信息
        public static float  speed, jumpForce;
    }
}