using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OvationBar : MonoBehaviour
{
    private float ovationMeter = 0;
    [SerializeField]
    private float ovationMax = 180;
    [SerializeField]
    private RectTransform meter;
    [SerializeField]
    private Characters owner;

    // Start is called before the first frame update
    void Start()
    {
        ovationMeter = 5;
        OvationSingleton.Instance.AddBar(this, owner);
    }

    // Update is called once per frame
    void Update()
    {
        ovationMeter -= Time.deltaTime * 4;
        meter.sizeDelta = Vector2.right * ovationMeter + Vector2.up * meter.sizeDelta;
        if (ovationMeter <= 0)
        {
            ovationMeter = 0;
            meter.sizeDelta -= Vector2.right * meter.sizeDelta.x;
        }
    }

    public void IncreaseMeter(float incr)
    {
        ovationMeter += incr;
        if(ovationMeter >= ovationMax)
        {
            ovationMeter = ovationMax;
            foreach(Image i in GetComponentsInChildren<Image>())
            { 
                if(!i.gameObject.Equals(this.gameObject)) i.color = Color.green;
            }
            OvationSingleton.Instance.BarAccomplished();
        }
    }
}
