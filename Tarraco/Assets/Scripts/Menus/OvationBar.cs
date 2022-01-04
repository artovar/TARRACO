using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OvationBar : MonoBehaviour
{
    private float ovationMeter = 0;
    [SerializeField]
    private float ovationMax = 240;
    [SerializeField]
    private RectTransform meter;
    [SerializeField]
    private Characters owner;
    [SerializeField]
    private TextMeshProUGUI score;
    private int scoreInt = 0;

    private bool coloring;

    // Start is called before the first frame update
    void Start()
    {
        ovationMeter = 0;
        OvationSingleton.Instance.AddBar(this, owner);
    }

    // Update is called once per frame
    void Update()
    {
        ovationMeter -= Time.deltaTime * 1.5f;
        if(!coloring) {
            meter.sizeDelta = Vector2.right * ovationMeter + Vector2.up * meter.sizeDelta.y;
        }
        if (ovationMeter <= 0)
        {
            ovationMeter = 0;
            if(!coloring) meter.sizeDelta -= Vector2.right * meter.sizeDelta.x;
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
            coloring = true;
            StartCoroutine(PrepareColor());
            OvationSingleton.Instance.BarAccomplished();
            scoreInt++;
            score.text = "" + scoreInt;
            if (scoreInt == 1) OvationSingleton.Instance.Win(owner, score);
            ovationMeter = 0;
        }
    }

    public void ResetBar()
    {
        score.text = "0";
        scoreInt = 0;
        ovationMeter = 0f;
    }
    private IEnumerator PrepareColor()
    {
        //GetComponent<Animator>();
        yield return new WaitForSeconds(1.5f);
        foreach (Image i in GetComponentsInChildren<Image>())
        {
            if (!i.gameObject.Equals(this.gameObject)) i.color = new Color(0.3738351f, 0.8584f, 0.28888f, 1);
        }
        coloring = false;
    }
}
