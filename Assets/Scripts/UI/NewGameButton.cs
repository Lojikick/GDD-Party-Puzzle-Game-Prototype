using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTouchingMouse(this)) {
        	this.GetParent().hide();
        }
    }
	
	public bool IsTouchingMouse(GameObject g)
	{
	    Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	    return g.GetComponent<Collider2D>().OverlapPoint(point);
	}
	
}