using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OvationBar : MonoBehaviour
{
    private float ovationMeter = 0;
    [SerializeField]
    private float ovationMax = 240;
    [SerializeField]
    private RectTransform meter;
    [SerializeField]
    private Characters owner;

    // Start is called before the first frame update
    void Start()
    {
        ovationMeter = 0;
        OvationSingleton.Instance.AddBar(this, owner);
    }

    // Update is called once per frame
    void Update()
    {
        if (ovationMeter == ovationMax) return;
        ovationMeter -= Time.deltaTime * 3.5f;
        meter.sizeDelta = Vector2.right * ovationMeter + Vector2.up * meter.sizeDelta.y;
        if (ovationMeter <= 0)
        {
            ovationMeter = 0;
            meter.sizeDelta -= Vector2.right * meter.sizeDelta.x;
        }
    }

    public void IncreaseMeter(float incrPerc)
    {
        ovationMeter += (ovationMax) * (incrPerc/100f);
        if(ovationMeter >= ovationMax)
        {
            ovationMeter = ovationMax;
            meter.sizeDelta = Vector2.right * ovationMeter + Vector2.up * meter.sizeDelta.y;
            foreach (Image i in GetComponentsInChildren<Image>())
            { 
                if(!i.gameObject.Equals(this.gameObject)) i.color = Color.yellow;
            }
            OvationSingleton.Instance.BarAccomplished();
        }
    }
}
