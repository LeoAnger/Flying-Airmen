using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using NetWork.RoomInfo;
using Script.Enum;
using Script.NetWork.UserInfo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class JoinRoomBtn : MonoBehaviour
    {
        public Text errorMsgText;
        private Thread childThread;
        public InputField InputField_Uname;    // 用户名
        public InputField InputField_RoomID;    // 房间ID
    
        // 密码
        public GameObject PwdPanel;
        public InputField PwdInputField; 
        private string Password = "";
        public bool IsInput = false;
        public bool IsShowPanel = false;

        private bool isSelectRole = false;
        private bool isStandalone = false;
        public static bool isShowErrorMsgText = false;
        private bool isJoinRoomBtn = true;


        public static string url_room1 = "http://localhost:8080/room1";
        public static string url_room2 = "http://localhost:8080/room2";
        static string url_return_content = "";    //网络返回内容
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (isSelectRole)
            {
                isSelectRole = false;
                // 跳转场景必须是主线程
                SceneManager.LoadScene(SceneName.RoleSel);
            }
            
            if (isStandalone)
            {
                isStandalone = false;
                // 单机模式
                SceneManager.LoadScene(SceneName.GamePlay);
            }

            if (isShowErrorMsgText)
            {
                isShowErrorMsgText = false;
                print("显示错误信息..." + errorMsgText.isActiveAndEnabled);
            
                errorMsgText.gameObject.SetActive(true);
            }

            PwdPanel.active = IsShowPanel;
        
        }


        public void BtnClick()
        {
            // 隐藏ErrorMsgText
            errorMsgText.enabled = false;
            // 按钮可用
            if (!isJoinRoomBtn)
            {
                return;
            }

            // 取消按钮可用状态
            isJoinRoomBtn = false;
        
            // 开启接收服务器消息的sub线程
            ThreadStart childref = new ThreadStart(joinRoom);
            childThread = new Thread(childref);
            childThread.Start();
        }

        /*
     * 获得IP后跳转Game场景
     * 1.http请求发送房间id (http://www.米游.com/?room_id=9595)
     * 2.http请求发送密码pw (http://www.米游.com/?room_num=977927&pw=123)
     */
        public void joinRoom()
        {
            int num = 6;
            while (true)
            {
                print(num);
                num--;
                string s = url_room1 + "?room_id=" + InputField_RoomID.text;
                print("第一次请求：" + s);
                string doGetRequestSendData = DoGetRequestSendData(s);
                print(doGetRequestSendData);    // RoomIdTemp:4EoLwjomrm
                if (doGetRequestSendData.Contains("RoomIdTemp"))
                {
                    //显示密码输入框
                    IsShowPanel = true;
                    while (true)
                    {
                        if (IsInput)
                        {
                            IsInput = false;
                            break;
                        }
                    }
                
                    //2.http请求发送密码pw (http://www.米游.com/?RoomIdTemp=977927&pw=123)
                    string s1 = url_room2 + "?RoomIdTemp=" + doGetRequestSendData.Split(':')[1] 
                                + "&pw=" + Password;
                    print("第二次请求：" + s1);
                    string Data2 = DoGetRequestSendData(s1);
                    print(Data2);
                
                    //3.处理结果
                    if (Data2.EndsWith("}"))
                    {
                        // 判断UserInfo是管理员还是玩家
                        // 房间权限
                        if (Data2.StartsWith("{Admin"))
                        {
                            RoomInfo.RoomPermissions = RoomPermissions.Admin;
                        } else if (Data2.StartsWith("{Player"))
                        {
                            RoomInfo.RoomPermissions = RoomPermissions.Player;
                        } else if (Data2.StartsWith("{Viewer"))
                        {
                            RoomInfo.RoomPermissions = RoomPermissions.Viewer;
                        }
                    
                        // 判断玩家角色
                        if (Data2.Contains("Player1"))
                        {
                            UserInfo.PlayerRoleType = PlayerRoleType.Player1;
                        } else if (Data2.Contains("Player2"))
                        {
                            UserInfo.PlayerRoleType = PlayerRoleType.Player2;
                        }

                        // 保存房间ID
                        RoomInfo.RoomId = InputField_RoomID.text;
                        // 保存用户名
                        UserInfo.UserName = InputField_Uname.text;
                        url_return_content = Data2;
                        // 跳转选人界面
                        //成功
                        isSelectRole = true;
                        print("跳转选人界面" + "\n房间ID: " + RoomInfo.RoomId +
                              "\n用户名：" + UserInfo.UserName);
                        print(url_return_content);
                        print("RoomRoleType：" + UserInfo.RoomRoleType + "\nPlayerRoleType: " + UserInfo.PlayerRoleType);
                    }
                    break;
                }

                if (num<0)
                {
                    print("加入房间失败...");
                    print(doGetRequestSendData);
                    break;
                }
                Thread.Sleep(100);
            }
            childThread.Abort();
        }
    
        // 用户输入密码按钮事件
        public void PwdButtionClick()
        {
            // 1.获取pwd
            string text = PwdInputField.text;
            print("用户输入的密码：" + text);
            // 2.赋值
            Password = text;
            // 3.通知
            IsInput = true;
            // 4.隐藏Panel
            IsShowPanel = false;

        }

        public void Standalone()
        {
            isStandalone = true;
        }
    
        /// <summary>
        /// GET方式发送得结果
        /// </summary>
        /// <param name="url">请求的url</param>
        public static string DoGetRequestSendData(string url)
        {
            HttpWebRequest hwRequest = null;
            HttpWebResponse hwResponse = null;

            string strResult = string.Empty;
            try
            {
                hwRequest = (System.Net.HttpWebRequest)WebRequest.Create(url);
                //hwRequest.Timeout = 30000;
                hwRequest.Method = "GET";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
            }
            catch (System.Exception err)
            {

            }
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.ASCII);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch (System.Exception err)
            {
            }
            return strResult;
        }
    
    
    }
}
