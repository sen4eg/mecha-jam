using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MechaBehaviour : MonoBehaviour
{
	public float speed = 10f;
	public float angularVelocity = 90f;
	public float boostMultiplier = 2f;
	public float screenShakeOnMove = 0.1f;
	public bool intertiaCompensation = true;
	public GameObject cockpit;
	public GameObject camera;
	public AnimationCurve screenShakeCurve;
	private Coroutine screenShakeCoroutine = null;
	
	private Vector3 _direction;
	private Vector3 _angularDisplacement;
	private Rigidbody _rigidbody;

	public Vector2 DragToSpeedCoefOffOn = new Vector2(0.1f, 20);
	// Start is called before the first frame update
    public void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
	public void Update()
    {
	    _direction = GetMovementDirection();
        if (Input.GetKey(KeyCode.LeftShift)){
	        _direction *= boostMultiplier;
		}
        gameObject.transform.Translate(_direction * (speed * Time.deltaTime));

        _angularDisplacement = new Vector3()
        {
	        y = Input.GetAxis("Mouse X"),
	        x = -Input.GetAxis("Mouse Y"), // mind how unity rotate things
	        z = (Input.GetKey(KeyCode.Q) ? 1 : 0) + (Input.GetKey(KeyCode.E) ? -1 : 0)
        };
        UpdateToggleGraviCompensator();
        UpdateScreenShake();
        Debug.Log(_rigidbody.velocity.sqrMagnitude);
    }

	private void UpdateScreenShake()
	{
		if (_rigidbody.velocity.sqrMagnitude > 9)
		{
			screenShakeCoroutine ??= StartCoroutine(Shaking());
		}
	}

	private void UpdateToggleGraviCompensator()
	{
		if (Input.GetKeyDown(KeyCode.V)) {
			intertiaCompensation = !intertiaCompensation;
		}
		if (intertiaCompensation)
		{
			_rigidbody.angularDrag = DragToSpeedCoefOffOn.y;
			_rigidbody.drag = DragToSpeedCoefOffOn.y;
		}else {
			_rigidbody.angularDrag = DragToSpeedCoefOffOn.x;
			_rigidbody.drag = DragToSpeedCoefOffOn.x;
		}
	}


	private Vector3 GetMovementDirection(){
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

    private IEnumerator Shaking()
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
	    screenShakeCoroutine = null;
    }

    public void FixedUpdate()
    {
	    UpdateMovement();
    }

    private void UpdateMovement()
    {
	    HandleDirectionalMovement();
	    HandleAngularMovement();
    }

    private void HandleAngularMovement()
    {
	    _rigidbody.AddRelativeTorque(angularVelocity * Time.deltaTime * _angularDisplacement, ForceMode.Impulse);
    }

    private void HandleDirectionalMovement()
    {
	    _rigidbody.AddForce(speed * Time.deltaTime * _direction, ForceMode.Impulse);
    }
}
