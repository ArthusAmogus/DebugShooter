using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{   
    //Properties
    float posx; float posy;

    //Options
    [Header("-----X Axis-----")]
    public bool do_x = true;
    public bool xmove = false;
    public float xmove_speed = 0.1f;
    public bool limit_x = false;
    public float xlimit = 2;

    [Header("-----Y Axis-----")]
    public bool do_y = true;
    public bool ymove = false;
    public float ymove_speed = 0.1f;
    public bool limit_y = false;
    public float ylimit = 2;


    //Drag Initialization
    private Vector3 offset;
    private float originalZ;


    //Drag
    void OnMouseDown()
    {
        if (posx < 0) { pos(posx - 0.01f, posy); }
        if (posx > 0) { pos(posx + 0.01f, posy); }

        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = this.transform.position - new Vector3(mousepos.x, mousepos.y, transform.position.z);
        originalZ = transform.position.z;
    }

    void OnMouseUp()
    {
        if (posx < 0) { pos(posx + 0.01f, posy); }
        if (posx > 0) { pos(posx - 0.01f, posy); }
    }

    private void Update()
    {
        // Position
        posx = this.transform.localPosition.x;
        posy = this.transform.localPosition.y;

        // AutoMove
        if (xmove)
        {
            if (posx < xlimit && posx > -xlimit) { posx += xmove_speed;}
            else if (posx >=  xlimit) { posx =  xlimit; }
            else if (posx <= -xlimit) { posx = -xlimit; }
        }

        if (ymove)
        {
            if (posy < ylimit && posy > -ylimit) {posy += ymove_speed;}
            else if (posy >=  ylimit) {posy =  ylimit; }
            else if (posy <= -ylimit) {posy = -ylimit; }
        }

        // Update position
        transform.localPosition = new Vector2(posx, posy);
    }

    private void OnMouseDrag()
    {

        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (do_x) { posx = mousepos.x + offset.x; };
        if (do_y) { posy = mousepos.y + offset.y; };

        // Check limits
        if (limit_x)
        {
            if (posx >=  xlimit) { posx =  xlimit; }
            if (posx <= -xlimit) { posx = -xlimit; }
        }

        if (limit_y)
        {
            if (posy >=  ylimit) { posy =  ylimit; }
            if (posy <= -ylimit) { posy = -ylimit; }
        }

        transform.position = new Vector3(posx, posy, originalZ);
    }

    private void pos(float posx, float posy)
    {
        this.transform.localPosition = new Vector2(posx, posy);
    }
}