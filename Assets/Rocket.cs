using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] public float rcsThrust = 100f;
    [SerializeField] public float mainThrust = 1.0f;
    [SerializeField] public AudioClip mainEngine;
    [SerializeField] public AudioClip deathExplosion;
    [SerializeField] public AudioClip successLanding;

    Rigidbody rigidBody;
    AudioSource audioSource;

    public enum State { Alive, Dying, Trancending }

    public State currentState = State.Alive;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
        if ( currentState == State.Alive )
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
	}

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void RespondToRotateInput()
    {

        // take manual control of rotation so that any force applied
        // does not dominate the movement, thus allowing user controlled 
        // recovery
        rigidBody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if ( currentState != State.Alive ) { return; }

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        currentState = State.Trancending;
        audioSource.Stop();
        audioSource.PlayOneShot(successLanding);
        Invoke("LoadNextScene", 1.5f);
    }

    private void StartDeathSequence()
    {
        // kaboom
        currentState = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathExplosion);
        Invoke("LoadFirstScene", 2f);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }
}
