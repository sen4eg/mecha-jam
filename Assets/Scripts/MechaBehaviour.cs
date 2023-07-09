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
	public bool intertiaCompensation = true;
	public float intertiaCompensationMultiplier = 1f;
	public GameObject cockpit;
	public new GameObject camera;
	public AnimationCurve screenShakeCurve;
	public bool doScreenShake = true;
	public float absolutlyUnaceptableAccumulatedRoll = 0f;
	[Range(0.1f, 5f)]
	public float rollIntensityMultiplier = 2.5f;
	[Range(0.1f, 5f)]                            
	public float pitchIntensityMultiplier = 2.5f; 
	
	private Coroutine screenShakeCoroutine = null;
	
	private Vector3 _direction = Vector3.zero;
	private Vector3 _angularDisplacement = Vector3.zero;
	private Rigidbody _rigidbody = null;

	public ForceMode directionalForceMode = ForceMode.Force;
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
        _angularDisplacement = new Vector3()
        {
	        y = Input.GetAxis("Mouse X"),
	        x = -Input.GetAxis("Mouse Y"), // mind how unity rotate things	
	        z = ((Input.GetKey(KeyCode.Q) ? 1 : 0) + (Input.GetKey(KeyCode.E) ? -1 : 0))
        };
        HandleAngularDisplacement();
        UpdateToggleGraviCompensator();
        UpdateScreenShake();
    }

	private void HandleAngularDisplacement() {
		//transform.Rotate(_angularDisplacement * (Time.deltaTime * angularVelocity), Space.Self);
		
		// TODO: Lerp me. Interpolate me. Smooth me.      
		Transform cachedTransform = transform; // keep link, optimizes a bit
		float rollBefore = cachedTransform.rotation.eulerAngles.z;
		transform.Rotate(Vector3.right, _angularDisplacement.x * (Time.deltaTime * angularVelocity * pitchIntensityMultiplier));
		transform.Rotate(Vector3.up, _angularDisplacement.y * (Time.deltaTime * angularVelocity));
		float rollAfter = cachedTransform.rotation.eulerAngles.z;

		absolutlyUnaceptableAccumulatedRoll += Mathf.Abs(rollAfter - rollBefore);
		float rollDifference = rollBefore - rollAfter; // Compensates unintended roll after yaw and pitch
		transform.Rotate(Vector3.forward, _angularDisplacement.z * (Time.deltaTime * angularVelocity * rollIntensityMultiplier) + rollDifference);
	}

	private void UpdateScreenShake()
	{
		if (!doScreenShake) {
			return;
		}
		if (_rigidbody.velocity.sqrMagnitude > 9) {
			screenShakeCoroutine ??= StartCoroutine(Shaking());
		}
	}

	private void UpdateToggleGraviCompensator()
	{
		if (Input.GetKeyDown(KeyCode.V))
		{
			intertiaCompensation = !intertiaCompensation;
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
	    //HandleAngularMovement();
    }

    /*
    private void HandleAngularMovement()
    {
	    //_rigidbody.AddRelativeTorque(angularVelocity * _angularDisplacement, angularForceMode);
	    
	    // Roll
	    _rigidbody.AddTorque(Vector3.forward * (angularVelocity * _angularDisplacement.z * -1), angularForceMode);
	    // Yaw
	    _rigidbody.AddTorque(Vector3.right * (angularVelocity * _angularDisplacement.x), angularForceMode);
	    // Pitch
	    _rigidbody.AddTorque(Vector3.up * (angularVelocity * _angularDisplacement.y), angularForceMode);
	    
	    #region TheVeryDebugRegion
	    if (Input.GetKey(KeyCode.Space)) {
		    _rigidbody.angularVelocity = Vector3.zero;
	    }
	    #endregion
    }
    */

    private void HandleDirectionalMovement()
    {
	    if (intertiaCompensation)
	    {
		    _rigidbody.AddRelativeForce(-_rigidbody.velocity * (intertiaCompensationMultiplier), directionalForceMode);
	    }
	    _rigidbody.AddRelativeForce(speed * _direction, directionalForceMode);
    }
}
