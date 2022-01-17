using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHelp : MonoBehaviour
{
    public Transform canvas;
    [SerializeField]
    private Image image;
    public Sprite[] imagesToShow;
    private WeaponDetection wp;
    private Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        wp = GetComponent<WeaponDetection>();
        cam = Camera.main.transform;
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        canvas.LookAt(cam.position + (canvas.position - cam.position) * 2);
    }

    public void Show(bool usingController)
    {
        image.enabled = true;
        if (usingController)
        {
            image.sprite = imagesToShow[0];
        }
        else
        {
            image.sprite = imagesToShow[1];
        }
    }
    public void Hide()
    {
        image.enabled = false;
    }
}