using UnityEngine;

namespace Script.MainMenu
{
    public class HelpMenu : MonoBehaviour
    {
        public static string url_help = "http://www.ilxdh.com/";
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void helpBtn()
        {
            WWW www = new WWW(url_help);
            Application.OpenURL(www.url);
        }
    }
}
