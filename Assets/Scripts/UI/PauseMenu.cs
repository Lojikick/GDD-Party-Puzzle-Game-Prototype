using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	private bool open = true;
	private GameObject window;
    // Start is called before the first frame update
    void Start() {
        window = this.transform.GetChild(0).gameObject;
		menuLogic();
    }

    // Update is called once per frame
    void Update() {
		if (Input.GetKeyDown("escape")) {
			menuLogic();
		}
    }
	
	void menuLogic() {
		if (open) {
			open = false;
		} else {
			open = true;
		}
		window.SetActive(open);
	}
}
