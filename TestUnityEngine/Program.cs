using System.Collections;

namespace TestUnityEngine
{
    public class Program
    {
        static void Main(string[] args)
        {
            UnityEngine unityEngine = new UnityEngine(new MonoBehaviour[]
            {
                //在这里添加运行的类，用逗号分隔，同时可以在Start前使用Add方法添加，对象必须继承于MonoBehaviour
                new TestMono()
            });
            unityEngine.Start();
        }
    }

    public class TestMono : MonoBehaviour
    {
        public override void Start()
        {
            Debug.Log("脚本开始了");
            StartCoroutine(cor());
        }
        IEnumerator cor()
        {
            yield return 120;
            Debug.Log("我等了120帧");
            yield return new WaitForSeconds(3f);
            Debug.Log("稍等三秒...");
            yield return new WaitForSeconds(5f);
            Debug.Log("稍等五秒...");
        }
        public override void Update()
        {

        }

    }

}
