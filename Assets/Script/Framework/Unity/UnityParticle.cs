using UnityEngine;
using System.Collections;

namespace Framework.Unity
{
    /// <summary>
    /// 通过该脚本，将美术提供的多层级粒子系统播放封装
    /// </summary>
    public class UnityParticle : MonoBehaviour
    {
        // 粒子发射器数组中发射一次粒子的最长持续时间
        private float destroyTime;

        // 粒子数组
        private ParticleSystem[] particles;

        void Awake()
        {
            particles = this.GetComponentsInChildren<ParticleSystem>();
            if (null != particles)
            {
                foreach (var item in particles)
                {
                    item.Stop();

                    // 此粒子系统的延时时间 + 持续时间
                    float totalTime = item.startDelay + item.duration;
                    if (totalTime > destroyTime)
                    {
                        // 设置最长持续时间
                        destroyTime = totalTime;
                    }
                }
            }
        }

        /// <summary>
        /// 播放一次后销毁
        /// </summary>
        public void PlayOnceThenDestroy()
        {
            if (particles != null)
            {
                CancelInvoke();

                foreach (var item in particles)
                {
                    item.Play();
                }

                Invoke("DestroyParticle", destroyTime);
            }
        }

        /// <summary>
        /// 播放一次后停止粒子系统
        /// </summary>
        public void PlayOnceStopParticle()
        {
            if (particles != null)
            {
                CancelInvoke();

                foreach (var item in particles)
                {
                    item.Play();
                }

                Invoke("StopParticle", destroyTime);
            }
        }

        /// <summary>
        /// 固定时间循环播放
        /// </summary>
        public void PlayLoop(float intervaltime)
        {
            CancelInvoke();

            InvokeRepeating("PlayParticle", 0f, intervaltime + destroyTime);
        }

        /// <summary>
        /// 播放粒子系统
        /// </summary>
        public void PlayParticle()
        {
            if (particles != null)
            {
                foreach (var item in particles)
                {
                    item.Stop();
                    item.Play();
                }
            }
        }

        /// <summary>
        /// 停止粒子系统
        /// </summary>
        public void StopParticle()
        {
            CancelInvoke();

            foreach (var item in particles)
            {
                item.Stop();
            }
        }

        /// <summary>
        /// 删除粒子系统
        /// </summary>
        public void DestroyParticle()
        {
            DestroyImmediate(this.gameObject);
        }
    }
}
