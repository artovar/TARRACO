using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animator))]
public class OvationBar : MonoBehaviour
{
    [SerializeField]
    private GameObject bckgrnd;
    private float ovationMeter = 0;
    [SerializeField]
    private float ovationMax = 240;
    [SerializeField]
    private RectTransform meter;
    [SerializeField]
    private Characters owner;
    [SerializeField]
    private TextMeshProUGUI score;
    [SerializeField]
    private Animator animator;
    private int scoreInt = 0;
    private float reductionRate = 1.5f;

    private bool coloring;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
    public void SetReductionRate(float red)
    {
        reductionRate = red;
    }

    public void IncreaseMeter(float incrPerc)
    {
        if (!gameObject.activeSelf) return;
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
            scoreInt++;
            score.text = "" + scoreInt;
            ovationMeter = 0;
        }
    }
    public void IncreaseScore(int scorePoints)
    {
        if (!gameObject.activeSelf) return;
        for(int i = 1; i <= scorePoints; i++)
        {
            ovationMeter += (ovationMax);
            if (ovationMeter >= ovationMax)
            {
                ovationMeter = ovationMax;
                meter.sizeDelta = Vector2.right * ovationMeter + Vector2.up * meter.sizeDelta.y;
                foreach (Image im in GetComponentsInChildren<Image>())
                {
                    if (!im.gameObject.Equals(this.gameObject)) im.color = Color.yellow;
                }
                coloring = true;
                StartCoroutine(PrepareColor());
                scoreInt++;
                score.text = "" + scoreInt;
                ovationMeter = 0;
            }
        }
    }
    private IEnumerator PrepareColor()
    {
        animator.SetBool("Accomplished", true);
        //GetComponent<Animator>();
        yield return new WaitForSeconds(1.5f);
        foreach (Image i in GetComponentsInChildren<Image>())
        {
            if (!i.gameObject.Equals(this.gameObject)) i.color = new Color(0.3738351f, 0.8584f, 0.28888f, 1);
        }
        animator.SetBool("Accomplished", false);
        OvationSingleton.Instance.BarAccomplished(scoreInt, owner, score);
        coloring = false;
    }

    public void ResetBar()
    {
        score.text = "0";
        scoreInt = 0;
        ovationMeter = 0f;
    }
    public void DisableBar()
    {
        score.gameObject.SetActive(false);
        bckgrnd.SetActive(false);
        gameObject.SetActive(false);
    }
    public void EnableBar()
    {
        score.gameObject.SetActive(true);
        score.text = "";
        bckgrnd.SetActive(true);
    }
    public TextMeshProUGUI GetText()
    {
        return score;
    }

    public float GetValue()
    {
        return meter.sizeDelta.x;
    }
    public float GetMax()
    {
        return ovationMax;
    }
    public int GetScore()
    {
        return scoreInt;
    }
}
