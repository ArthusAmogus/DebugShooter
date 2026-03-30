using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSpan : MonoBehaviour
{
    public float lifespan=2;
    public bool destroy=true;
    float timer;
    GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        obj = this.gameObject;
    }

    private void OnEnable()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (timer>=lifespan)
        {
            if (destroy) Destroy(obj);
            else obj.SetActive(false);

        } else timer += Time.deltaTime;
    }

}
