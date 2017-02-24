using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Framework
{
    public static class FrameworkStart
    {
        static Timer mGcCollectionTimer;
        /// <summary>
        /// 初始化框架中的内容
        /// </summary>
        public static void Start()
        {
            mGcCollectionTimer = new Timer(FrameworkConstant.GcConlectionTimer);
            mGcCollectionTimer.Elapsed += mGcCollectionTimer_Elapsed;
            mGcCollectionTimer.Start();
        }

        /// <summary>
        /// 强制进行垃圾回收操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void mGcCollectionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            GC.Collect();
        }

        public static void Stop()
        {
            mGcCollectionTimer.Stop();
        }
    }
}
