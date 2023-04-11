using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class NewGameButton : MonoBehaviour
{
	
	public string path;
	
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (IsTouchingMouse(gameObject) && Input.GetMouseButtonDown(0)) {
        	//SceneManager.LoadScene("Assets/Scenes/Prototype Scene.unity");
			TransitionManager.instance.LoadSelectedScene(0, Vector3.zero);
        }
    }
	
	public bool IsTouchingMouse(GameObject g) {
	    Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	    return g.GetComponent<Collider2D>().OverlapPoint(point);
	}
	
}
