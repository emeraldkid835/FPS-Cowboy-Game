using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class bullet_Visual : MonoBehaviour
{
    private LineRenderer bullet;
    public Vector3 renderPoint1;
    public Vector3 renderPoint2;
    public Vector3 gunDirection;
    [SerializeField] private float bulletSpeed = 50f; //should probably be some fast shit
    [SerializeField] private float maxRenderTime = 2f;
    private float curRenderTime = 0f;
    // Start is called before the first frame update
    void Awake()
    {
        if(bullet == null)
        {
            bullet = GetComponent<LineRenderer>();
        }
        curRenderTime = 0f;
        bullet.positionCount = 2;
        bullet.SetPosition(0, renderPoint1);
        bullet.SetPosition(1, renderPoint2);
    }

    // Update is called once per frame
    void Update()
    {
        curRenderTime += Time.deltaTime;
        renderPoint1 += gunDirection * bulletSpeed;
        renderPoint2 += gunDirection * bulletSpeed;

        bullet.SetPosition(0, renderPoint1);
        bullet.SetPosition(1, renderPoint2);

        if (curRenderTime >= maxRenderTime)
        {
            Destroy(this.gameObject);
        }
    }
}
