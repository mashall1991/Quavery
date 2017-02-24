using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SoundVolum : MonoBehaviour {

    public AudioSource audios;

    public Text ValueText;

    private float TimeCount = 0.2f;
	// Use this for initialization
	void Start () {
        if(Microphone.devices.Length > 0)
        {
			Debug.Log ("Setup microphone");
            audios = base.GetComponent<AudioSource>();
            audios.clip = Microphone.Start(null, true, 0x3e7, 0xac44);
            audios.loop = true;
            while (Microphone.GetPosition(string.Empty) <= 0)
            {
            }
            this.audios.Play();
        }
    }
	
	// Update is called once per frame
	void FixedUpdate()
    {
        TimeCount -= Time.deltaTime;
        if(TimeCount < 0)
        {
            float value = GetAveragedVolume();
            VolumToMoveManager.Instance.VolumToMove(value);
            if (ValueText) ValueText.text = string.Format("{0:0.00}", value);
            TimeCount = 0.2f;
        }
        
    }
    private float GetAveragedVolume()
    {
        float[] samples = new float[0x100];
        float num = 0f;
        this.audios.GetOutputData(samples, 0);
        foreach (float num2 in samples)
        {
            num += Mathf.Abs(num2);
        }
        return (num / 25.6f);
    }
}
