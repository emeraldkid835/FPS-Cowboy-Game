using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class victorytext : MonoBehaviour
{
    private TMP_Text me;
    [SerializeField] private float duration;
    [SerializeField] private Color c1;
    [SerializeField] private Color c2;
    private float curT;
    private bool up;
    // Start is called before the first frame update
    void Start()
    {
        me = this.GetComponent<TMP_Text>();
        curT = 0;
        up = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if(up == true)
        {
            curT += Time.deltaTime;
        }
        else
        {
            curT -= Time.deltaTime;
        }
        
        if(curT <= 0)
        {
            up = true;
        }else if(curT > duration)
        {
            up = false;
        }


        me.color = Color.Lerp(c1, c2, curT / duration);
    }
}
