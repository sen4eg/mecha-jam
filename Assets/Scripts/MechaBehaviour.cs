using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaBehaviour : MonoBehaviour
{
	public float speed = 10f;
	public float angularVelocity = 90f;
	public float boostMultiplier = 2f;
	public float screenShakeOnMove = 0.1f;
	public GameObject cockpit;
	public GameObject camera;
	public AnimationCurve screenShakeCurve;
	private Coroutine screenShakeCoroutine = null;
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    handleMouseControl();
        Vector3 direction = getMovementDirection();
        if (Input.GetKey(KeyCode.LeftShift)){
	        direction *= boostMultiplier;
		}
        gameObject.transform.Translate(direction * (speed * Time.deltaTime));

        if (Input.GetKey(KeyCode.Q)){
			gameObject.transform.Rotate(Vector3.forward, angularVelocity * Time.deltaTime);
		}
        if (Input.GetKey(KeyCode.E))
        {
	        gameObject.transform.Rotate(Vector3.forward, -angularVelocity* Time.deltaTime);
        }
        
        if (direction.magnitude > 0.1f && screenShakeCoroutine == null)
		{
	        StartCoroutine(Shaking());
		}

        
    }

    private void handleMouseControl()
    {
	    float mouseX = Input.GetAxis("Mouse X");
	    float mouseY = Input.GetAxis("Mouse Y");
	    gameObject.transform.Rotate(Vector3.up, mouseX * angularVelocity * Time.deltaTime);
	    gameObject.transform.Rotate(Vector3.left, mouseY * angularVelocity * Time.deltaTime);
    }


    Vector3 getMovementDirection(){
		Vector3 direction = Vector3.zero;
		if (Input.GetKey(KeyCode.W)){
            direction += Vector3.forward;
        }
		if (Input.GetKey(KeyCode.S)){
            direction += Vector3.back;
        }
		if (Input.GetKey(KeyCode.A)){
            direction += Vector3.left;
        }
		if (Input.GetKey(KeyCode.D)){
            direction += Vector3.right;
        }
		if (Input.GetKey(KeyCode.Space)){
            direction += Vector3.up;
        }
		if (Input.GetKey(KeyCode.LeftControl)){
            direction += Vector3.down;
        }
		return direction.normalized;
	}

    IEnumerator Shaking()
    {
	    Vector3 originalPosition = camera.transform.localPosition;
	    float time = 0f;
	    while (time < screenShakeCurve.length)
	    {
		    time += Time.deltaTime;
		    float displacement = screenShakeCurve.Evaluate(time);
		    camera.transform.localPosition = originalPosition + Random.insideUnitSphere * displacement;
		    yield return null;
	    }
	    camera.transform.localPosition = originalPosition;
    }
}
