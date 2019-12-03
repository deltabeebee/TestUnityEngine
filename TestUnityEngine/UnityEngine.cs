using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace TestUnityEngine
{
    public class Debug
    {
        public static void Log(object obj)
        {
            Console.WriteLine(string.Format("[{0}][{1}] {2}", DateTime.Now.ToString(), "INFO", obj.ToString()));
        }
    }
    public class Time
    {
        public static float deltaTime = 0.016f;
        //运行的fps总计
        public static ulong fps = 0;
    }
    public class UnityEngine
    {
        //存所有的脚本
        private List<MonoBehaviour> monoBehaviours = new List<MonoBehaviour>();
        public UnityEngine() { }
        public UnityEngine(MonoBehaviour[] monos)
        {
            foreach (var item in monos)
            {
                this.Add(item);
            }
        }
        //添加脚本
        public void Add(MonoBehaviour mono)
        {
            mono.Start();
            monoBehaviours.Add(mono);
        }
        //移除脚本
        public void Remove(MonoBehaviour mono)
        {
            mono.OnDestroy();
            monoBehaviours.Remove(mono);
        }

        //引擎启动
        public void Start()
        {
            while (true)
            {
                Time.fps++;
                Thread.Sleep((int)(Time.deltaTime * 1000));
                It();
            }
        }
        //每帧执行
        public void It()
        {
            foreach (MonoBehaviour mono in monoBehaviours)
            {
                //每帧执行Update
                mono.Update();
                //协程
                mono.__coroutine();
            }
        }
    }

    public abstract class MonoBehaviour
    {
        public IEnumerator enumerator;
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void OnDestroy() { }

        public void StartCoroutine(IEnumerator enumerator)
        {
            this.enumerator = enumerator;
        }

        private int count = 0;
        private int delay = 0;

        public void __coroutine()
        {
            if (enumerator != null)
            {
                if (delay == 0)
                {
                    count = 0;
                    //不需要等待 直接执行
                    if (enumerator.MoveNext())
                    {
                        object obj = enumerator.Current;
                        if (obj == null)
                        {
                            //返回了个null,丢弃
                            return;
                        }
                        Type type = obj.GetType();
                        if (type == typeof(Int32))
                        {
                            //返回帧数
                            this.delay = (int)obj;
                        }
                        else if (type == typeof(WaitForSeconds))
                        {
                            //返回时间 * 帧数
                            this.delay = (int)(((WaitForSeconds)obj).Time * 60);
                        }
                        count++;
                    }
                    else
                    {
                        //到头了，删除协程
                        this.enumerator = null;
                    }
                }
                else
                {
                    //需要等待
                    count++;
                    if (count > delay)
                    {
                        //等待完毕，清空等待时间
                        delay = 0;
                    }
                }

            }
        }
    }

    //等待类
    public class WaitForSeconds
    {
        private float time;
        public float Time { get => time; }
        public WaitForSeconds(float time)
        {
            this.time = time;
        }
    }
}
